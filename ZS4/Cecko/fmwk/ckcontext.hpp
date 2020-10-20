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

	class context : public CKContext {
	public:
		context(CKTablesObs tables) : CKContext(tables), line_(1) {}

		void message(errors::err_s err, loc_t loc, std::string_view msg);
		void message(errors::err_n err, loc_t loc);

		static std::string escape(std::string_view s);

		loc_t line() const { return line_; }
		loc_t incline() { return line_++; }		// returns line value before increment
	private:
		loc_t	line_;
	};

	using context_obs = context*;
}

#endif // CECKO_CONTEXT_GUARD__
