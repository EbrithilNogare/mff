/** @file

ckcontext.hpp

State-aware upper layer of lexer/parser context.

*/

#ifndef ckcontext_hpp__
#define ckcontext_hpp__

#include "cktables.hpp"

#include <string_view>
#include <functional>
#include <array>
#include <ostream>

namespace cecko {

	namespace errors {
		// messages

		/// @cond INTERNAL
		template< std::size_t N>
		using err_object_base = std::array<const char*, N>;
		/// @endcond 

		/// Error message with a string parameter
		class err_object_s : public err_object_base<2> {
		public:
			/// @cond INTERNAL
			err_object_s(const char* e0, const char* e1)
				: err_object_base<2>{ e0, e1 }
			{}
			/// @endcond 
		};

		/// Error message without parameters
		class err_object_n : public err_object_base<1> {
		public:
			/// @cond INTERNAL
			err_object_n(const char* e0)
				: err_object_base<1>{ e0 }
			{}
			/// @endcond 
		};

		/// Error message with a string parameter
		using err_def_s = const err_object_s;
		/// Error message with a string parameter
		using err_s = std::reference_wrapper<err_def_s>;

		/// Error message without parameters
		using err_def_n = const err_object_n;
		/// Error message without parameters
		using err_n = std::reference_wrapper<err_def_n>;

		extern err_def_s SYNTAX;	///< Syntax error
		extern err_def_s INTOUTRANGE;	///< Integer literal out of range
		extern err_def_s BADINT;	///< Malformed integer literal
		extern err_def_s BADESCAPE;	///< Malformed escape sequence
		extern err_def_s UNCHAR;	///< Invalid character
		extern err_def_s UNDEF_IDF;	///< Undefined identifier of constant/variable/function
		//extern err_def_s UNDEF_TYPEIDF;	///< Undefined type identifier
		/// @cond INTERNAL
		extern err_def_s NOFILE;	///< Cannot open input file (reported by the framework)
		extern err_def_s DUPLICATE_IDF;	///< Duplicate identifier (reported by the framework)
		extern err_def_s DUPLICATE_TAG; ///< Duplicate tag (reported by the framework)
		extern err_def_s DUPLICATE_FUNCTION_DEFINITION; ///< Duplicate function definition (reported by the framework)
		extern err_def_s DUPLICATE_STRUCT_DEFINITION; ///< Duplicate function definition (reported by the framework)
		extern err_def_s DUPLICATE_ENUM_DEFINITION; ///< Duplicate function definition (reported by the framework)
		/// @endcond
		
		extern err_def_n INTERNAL;	///< Internal error
		extern err_def_n EMPTYCHAR;	///< Empty character literal
		extern err_def_n MULTICHAR_LONG;	///< Too long character literal
		extern err_def_n EOLINSTRCHR;	///< End of line in string/character literal
		extern err_def_n EOFINSTRCHR;	///< End of file in string/character literal
		extern err_def_n EOFINCMT;	///< End of file in comment
		extern err_def_n UNEXPENDCMT;	///< Unexpected end of comment
		extern err_def_n VOIDEXPR;	///< Expression is of void type
		//extern err_def_n ARRAY_NOT_LVALUE;	///< Array is not lvalue?
		//extern err_def_n NAME_NOT_VALUE;
		extern err_def_n NOT_NUMBER;	///< Expression is not a number
		extern err_def_n NOT_POINTER;	///< Expression is not a pointer
		extern err_def_n NOT_NUMBER_OR_POINTER; 	///< Expression is not a number or pointer
		extern err_def_n INCOMPATIBLE;		///< Incompatible operands
		extern err_def_n INVALID_FUNCTION_TYPE;		///< Invalid function type constructed
		extern err_def_n INVALID_ARRAY_TYPE;		///< Invalid array type constructed
		extern err_def_n INVALID_SPECIFIERS;	///< Conflicting declaration specifiers

		/// @cond INTERNAL
		extern err_def_n INVALID_VARIABLE_TYPE;		///< Invalid variable type (reported by the framework)
		/// @endcond
	}

	/// @cond COVERAGE
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
	/// @endcond

	/// Lexical level of compiler context + error messaging
	class context : public CKContext {
	public:
		/// @cond INTERNAL
		context(CKTablesObs tables, std::ostream* outp, coverage::coverage_data * cd) : CKContext(tables), line_(1), outp_(outp), cd_(cd) {}

		std::ostream& out() { return *outp_; }
		/// @endcond

		/// @name Generating error messages
		/// @{
		
		/// <param name="err">Error descriptor</param>
		/// <param name="loc">Line number</param>
		/// <param name="msg">A string argument</param>
		void message(errors::err_s err, loc_t loc, std::string_view msg);

		/// <param name="err">Error descriptor</param>
		/// <param name="loc">Line number</param>
		void message(errors::err_n err, loc_t loc);
		/// @}
		
		/// @cond INTERNAL
		static std::string escape(std::string_view s);
		/// @endcond
		
		/// @name Lexer source-line counting
		/// @{
		
		loc_t line() const { return line_; }	///< Get current line
		loc_t incline() { return line_++; }		///< Increment current line
		/// @}
		
		/// @cond COVERAGE
		void cov(std::string n)
		{
			cd_->inc(line(), std::move(n));
		}
		/// @endcond
	private:
		loc_t	line_;

		std::ostream * outp_;

		coverage::coverage_data * cd_;
	};

	/// Pointer to compiler context
	using context_obs = context*;
}

#endif // CECKO_CONTEXT_GUARD__
