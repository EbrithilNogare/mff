/*

context.hpp

context for the compiler

*/

#ifndef ckcontext_hpp__
#define ckcontext_hpp__

#include "cktables.hpp"

#include <string_view>
#include <functional>
#include <array>
#include <ostream>

namespace cecko {

	using loc_t = unsigned;

	namespace errors {
		// messages

		template< std::size_t N>
		using err_object_base = std::array<const char*, N>;

		class err_object_s : public err_object_base<2> {
		public:
			err_object_s(const char* e0, const char* e1)
				: err_object_base<2>{ e0, e1 }
			{}
		};

		class err_object_n : public err_object_base<1> {
		public:
			err_object_n(const char* e0)
				: err_object_base<1>{ e0 }
			{}
		};

		using err_def_s = const err_object_s;
		using err_s = std::reference_wrapper<err_def_s>;

		using err_def_n = const err_object_n;
		using err_n = std::reference_wrapper<err_def_n>;

		extern err_def_s SYNTAX;
		extern err_def_s INTOUTRANGE;
		extern err_def_s BADINT;
		extern err_def_s BADESCAPE;
		extern err_def_s UNCHAR;
		extern err_def_s NOFILE;
		extern err_def_s UNDEF_IDF;

		extern err_def_n INTERNAL;
		extern err_def_n EMPTYCHAR;
		extern err_def_n MULTICHAR_LONG;
		extern err_def_n EOLINSTRCHR;
		extern err_def_n EOFINSTRCHR;
		extern err_def_n EOFINCMT;
		extern err_def_n UNEXPENDCMT;
		extern err_def_n VOIDEXPR;
		extern err_def_n ARRAY_NOT_LVALUE;
		extern err_def_n NAME_NOT_VALUE;
		extern err_def_n NOT_NUMBER;
		extern err_def_n NOT_POINTER;
		extern err_def_n NOT_NUMBER_OR_POINTER;
		extern err_def_n INCOMPATIBLE;
	}

	namespace coverage {

		struct coverage_counter {
		public:
			coverage_counter()
				: num(0)
			{}

			void inc()
			{
				++num;
			}

			int get() const
			{
				return num;
			}
		private:
			int num;
		};

		using map_t = std::map<std::string, coverage_counter>;
		using map_element_t = std::pair<const std::string, coverage_counter>;
		using map_element_obs = const map_element_t*;

		struct line_coverage_data {
		public:
			void push(map_element_obs p)
			{
				v_.push_back(p);
			}

			template< typename F>
			void for_each(F && f) const
			{
				for (auto&& a : v_)
				{
					f(a->first);
				}
			}
		private:
			std::vector< map_element_obs> v_;
		};

		using line_map_t = std::map<loc_t, line_coverage_data>;

		struct coverage_data {
		public:
			void inc(loc_t line, std::string n)
			{
				auto rv = map_.try_emplace(std::move(n));
				rv.first->second.inc();
				auto rvl = line_map_.try_emplace(line);
				rvl.first->second.push(&*rv.first);
			}

			template< typename F>
			void for_each(F&& f) const
			{
				for (auto&& a : map_)
				{
					f(a.first, a.second);
				}
			}

			template< typename F>
			void for_each_line(F&& f) const
			{
				for (auto&& a : line_map_)
				{
					f(a.first, a.second);
				}
			}
		private:
			map_t map_;
			line_map_t line_map_;
		};
	}

	class context : public CKContext {
	public:
		context(CKTablesObs tables, std::ostream* outp, coverage::coverage_data * cd) : CKContext(tables), line_(1), outp_(outp), cd_(cd) {}

		std::ostream& out() { return *outp_; }

		void message(errors::err_s err, loc_t loc, std::string_view msg);
		void message(errors::err_n err, loc_t loc);

		static std::string escape(std::string_view s);

		loc_t line() const { return line_; }
		loc_t incline() { return line_++; }		// returns line value before increment

		void cov(std::string n)
		{
			cd_->inc(line(), std::move(n));
		}

	private:
		loc_t	line_;

		std::ostream * outp_;

		coverage::coverage_data * cd_;
	};

	using context_obs = context*;
}

#endif // CECKO_CONTEXT_GUARD__
