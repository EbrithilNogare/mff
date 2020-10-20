/*

bisonflex.hpp

common interfaces for Flex and Bison

*/

#ifndef CECKO_BISONFLEX_GUARD__
#define CECKO_BISONFLEX_GUARD__

#include <cstdio>

#include "ckcontext.hpp"

// define the macro for Flex ...
#define YY_DECL \
	cecko::parser::symbol_type yylex(yyscan_t yyscanner, cecko::context * ctx)
// ... and declare the function

/* An opaque pointer.  - taken from generated Flex file */
#ifndef YY_TYPEDEF_YY_SCANNER_T
#define YY_TYPEDEF_YY_SCANNER_T
typedef void* yyscan_t;
#endif

#define STRINGIZE(n) #n
#define INCLUDE_WRAP(n) STRINGIZE(n)

namespace cecko {
	yyscan_t lexer_init(FILE * iff);
	void lexer_shutdown(yyscan_t scanner);
}

#endif
