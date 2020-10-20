/*

context.cpp

context for the compiler

*/

#include "ckcontext.hpp"

#include <sstream>
#include <iomanip>

namespace cecko {

	namespace errors {

		err_def_s SYNTAX{ "Syntax error: ", "" };
		err_def_s INTOUTRANGE{ "Integer literal \"", "\" out of range" };
		err_def_s BADINT{ "Malformed integer literal \"", "\"" };
		err_def_s BADESCAPE{ "Malformed escape sequence \"", "\"" };
		err_def_s UNCHAR{ "Unknown character '", "'" };
		err_def_s NOFILE{ "Unable to open input file \"", "\"" };
		err_def_s UNDEF_IDF{ "Undefined identifier \"", "\"" };

		err_def_n INTERNAL{ "INTERNAL ERROR" };
		err_def_n EMPTYCHAR{ "Empty character" };
		err_def_n EOLINSTRCHR{ "End of line in string or character literal" };
		err_def_n EOFINSTRCHR{ "End of file in string or character literal" };
		err_def_n EOFINCMT{ "End of file in comment" };
		err_def_n UNEXPENDCMT{ "End of comment outside comment" };
		err_def_n VOIDEXPR{ "Expression is void" };
		err_def_n ARRAY_NOT_LVALUE{ "Array expression is not an lvalue"};
		err_def_n NAME_NOT_VALUE{ "Name does not denote a value" };
		err_def_n NOT_NUMBER{ "Expression is not a number" };
		err_def_n NOT_POINTER{ "Expression is not a pointer" };
		err_def_n NOT_NUMBER_OR_POINTER{ "Expression is not a number or pointer" };
		err_def_n INCOMPATIBLE{ "Incompatible operand(s)" };
	}

	std::string context::escape(std::string_view s)
	{
		std::ostringstream r;
		for (std::uint8_t ch : s)
		{
			if (ch < 32 || ch > 126 || ch == '\'' || ch == '"')
				r << "\\x" << std::hex << std::setfill('0') << std::setw(2) << (int)ch;
			else
				r.put(ch);
		}
		return r.str();
	}

	void context::message(errors::err_s err, loc_t loc, std::string_view msg)
	{
		errors::err_def_s& e = err;
		std::cout << "Error (line " << loc << "): " << e[0] << escape(msg) << e[1] << std::endl;
	}

	void context::message(errors::err_n err, loc_t loc)
	{
		errors::err_def_n & e = err;
		std::cout << "Error (line " << loc << "): " << e[0] << std::endl;
	}
}