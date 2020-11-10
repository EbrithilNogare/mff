%language "c++"
%require "3.4"
// NEVER SET THIS INTERNALLY - SHALL BE SET BY CMAKE: %defines "../private/caparser.hpp"
// NEVER SET THIS INTERNALLY - SHALL BE SET BY CMAKE: %output "../private/caparser.cpp"
%locations
%define api.location.type {cecko::loc_t}
%define api.namespace {cecko}
%define api.value.type variant
%define api.token.constructor
%define api.parser.class {parser}
%define api.token.prefix {TOK_}
//%define parse.trace
%define parse.assert
%define parse.error verbose

%code requires
{
// this code is emitted to caparser.hpp

#include "ckbisonflex.hpp"

// adjust YYLLOC_DEFAULT macro for our api.location.type
#define YYLLOC_DEFAULT(res,rhs,N)	(res = (N)?YYRHSLOC(rhs, 1):YYRHSLOC(rhs, 0))

#include "ckgrptokens.hpp"
#include "ckcontext.hpp"
#include "casem.hpp"
}

%code
{
// this code is emitted to caparser.cpp

YY_DECL;

using namespace casem;
}

%param {yyscan_t yyscanner}		// the name yyscanner is enforced by Flex
%param {cecko::context * ctx}

%start translation_unit

%token EOF		0				"end of file"

%token						LBRA		"["
%token						RBRA		"]"
%token						LPAR		"("
%token						RPAR		")"
%token						DOT			"."
%token						ARROW		"->"
%token<cecko::gt_incdec>	INCDEC		"++ or --"
%token						COMMA		","
%token						AMP			"&"
%token						STAR		"*"
%token<cecko::gt_addop>		ADDOP		"+ or -"
%token						EMPH		"!"
%token<cecko::gt_divop>		DIVOP		"/ or %"
%token<cecko::gt_cmpo>		CMPO		"<, >, <=, or >="
%token<cecko::gt_cmpe>		CMPE		"== or !="
%token						DAMP		"&&"
%token						DVERT		"||"
%token						ASGN		"="
%token<cecko::gt_cass>		CASS		"*=, /=, %=, +=, or -="
%token						SEMIC		";"
%token						LCUR		"{"
%token						RCUR		"}"
%token						COLON		":"

%token						TYPEDEF		"typedef"
%token						VOID		"void"
%token<cecko::gt_etype>		ETYPE		"_Bool, char, or int"
%token						STRUCT		"struct"
%token						ENUM		"enum"
%token						CONST		"const"
%token						IF			"if"
%token						ELSE		"else"
%token						DO			"do"
%token						WHILE		"while"
%token						FOR			"for"
%token						GOTO		"goto"
%token						CONTINUE	"continue"
%token						BREAK		"break"
%token						RETURN		"return"
%token						SIZEOF		"sizeof"

%token<CIName>				IDF			"identifier"
%token<CIName>				TYPEIDF		"type identifier"
%token<int>					INTLIT		"integer literal"
%token<CIName>				STRLIT		"string literal"

/////////////////////////////////

%%

/////////////////////////////////


translation_unit:
	ETYPE IDF LPAR ETYPE IDF COMMA ETYPE STAR STAR IDF RPAR
	LCUR RETURN INTLIT SEMIC RCUR
	| 
	;

/////////////// Expressions

primary-expression:
	IDF
	| INTLIT
	| STRLIT
	| LPAR expression RPAR
	;

postfix-expression:
	| primary-expression
	| postfix-expression LBRA expression RBRA
//	| postfix-expression LPAR argument-expression-list_opt RPAR
	| postfix-expression DOT IDF
	| postfix-expression ARROW IDF
	| postfix-expression INCDEC
	| postfix-expression INCDEC
	;

argument-expression-list:
	| assignment-expression
	| argument-expression-list
	| assignment-expression
	;

unary-expression:
	| postfix-expression
	| INCDEC unary-expression
	| INCDEC unary-expression
	| unary-operator cast-expression
	| SIZEOF LPAR TYPEIDF RPAR
	;

unary-operator:
	| AMP
	| STAR
	| ADDOP
	| ADDOP
	| EMPH
	;

cast-expression:
	| unary-expression
	;

multiplicative-expression:
	| cast-expression
	| multiplicative-expression STAR cast-expression
	| multiplicative-expression DIVOP cast-expression
	| multiplicative-expression DIVOP cast-expression
	;

additive-expression:
	| multiplicative-expression
	| additive-expression ADDOP multiplicative-expression
	| additive-expression ADDOP multiplicative-expression
	;

relational-expression:
	| additive-expression
	| relational-expression CMPO additive-expression
	| relational-expression CMPO additive-expression
	| relational-expression CMPO additive-expression
	| relational-expression CMPO additive-expression
	;

equality-expression:
	| relational-expression
	| equality-expression CMPE relational-expression
	| equality-expression CMPE relational-expression
	;

logical-AND-expression:
	| equality-expression
	| logical-AND-expression DAMP equality-expression
	;

logical-OR-expression:
	| logical-AND-expression
	| logical-OR-expression DVERT logical-AND-expression
	;

assignment-expression:
	| logical-OR-expression
	| unary-expression assignment-operator assignment-expression
	;

assignment-operator:
	| ASGN 
	| CASS 
	| CASS 
	| CASS 
	| CASS 
	| CASS 
	;

expression:
	| assignment-expression
	;

constant-expression:
	| logical-OR-expression
	;





/////////////////////////////////

%%

namespace cecko {

	void parser::error(const location_type& l, const std::string& m)
	{
		ctx->message(cecko::errors::SYNTAX, l, m);
	}

}