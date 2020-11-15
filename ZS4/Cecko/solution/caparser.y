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
%token						RETURN		"return"
%token						SIZEOF		"sizeof"

%token<CIName>				IDF			"identifier"
%token<CIName>				TYPEIDF		"type identifier"
%token<int>					INTLIT		"integer literal"
%token<CIName>				STRLIT		"string literal"

/////////////////////////////////

%%

/////////////////////////////////


/////////////// Expressions

primary_expression:
	IDF
	| INTLIT
	| STRLIT
	| LPAR expression RPAR
	;

postfix_expression:
	primary_expression
	| postfix_expression LBRA expression RBRA
	| postfix_expression LPAR argument_expression_list RPAR
	| postfix_expression DOT IDF
	| postfix_expression ARROW IDF
	| postfix_expression INCDEC
	;

argument_expression_list:
	assignment_expression
	| argument_expression_list COMMA assignment_expression
	;

unary_expression:
	postfix_expression
	| INCDEC unary_expression
	| unary_operator cast_expression
	| SIZEOF LPAR type_name RPAR
	;

unary_operator:
	AMP
	| STAR
	| ADDOP
	| EMPH
	;

cast_expression:
	unary_expression
	;

multiplicative_expression:
	cast_expression
	| multiplicative_expression STAR cast_expression
	| multiplicative_expression DIVOP cast_expression
	;

additive_expression:
	multiplicative_expression
	| additive_expression ADDOP multiplicative_expression
	;

relational_expression:
	additive_expression
	| relational_expression CMPO additive_expression
	;

equality_expression:
	relational_expression
	| equality_expression CMPE relational_expression
	;

logical_AND_expression:
	equality_expression
	| logical_AND_expression DAMP equality_expression
	;

logical_OR_expression:
	logical_AND_expression
	| logical_OR_expression DVERT logical_AND_expression
	;

assignment_expression:
	logical_OR_expression
	| unary_expression assignment_operator assignment_expression
	;

assignment_operator:
	ASGN 
	| CASS 
	;

expression:
	assignment_expression
	;

constant_expression:
	logical_OR_expression
	;



/////////////// Declarations

declaration:
	no_leading_attribute_declaration
	;

no_leading_attribute_declaration:
	declaration_specifiers init_declarator_list SEMIC
	;

declaration_specifiers:
	declaration_specifier
	| declaration_specifier declaration_specifiers
	;

declaration_specifier:
	storage_class_specifier
	| type_specifier_qualifier
	;

init_declarator_list:
	init_declarator
	| init_declarator_list COMMA init_declarator
	;

init_declarator:
	declarator
	;

storage_class_specifier:
	TYPEDEF
	;

type_specifier:
	VOID
	| ETYPE
	| struct_or_union_specifier
	| enum_specifier
	| typedef_name
	;

struct_or_union_specifier:
	struct_or_union IDF LCUR member_declaration_list RCUR
	| struct_or_union IDF
	;

struct_or_union:
	STRUCT
	;

member_declaration_list:
	member_declaration
	| member_declaration_list member_declaration
	;

member_declaration:
	specifier_qualifier_list member_declarator_list SEMIC
	;

specifier_qualifier_list:
	type_specifier_qualifier
	| type_specifier_qualifier specifier_qualifier_list
	;

type_specifier_qualifier:
	type_specifier
	| type_qualifier
	;

member_declarator_list:
	member_declarator
	| member_declarator_list COMMA member_declarator
	;

member_declarator:
	declarator
	;

enum_specifier:
	ENUM IDF LCUR enumerator_list RCUR
	| ENUM IDF LCUR enumerator_list COMMA RCUR
	| ENUM IDF
	;

enumerator_list:
	enumerator
	| enumerator_list COMMA enumerator
	;

enumerator:
	IDF
	| IDF ASGN constant_expression
	;

type_qualifier:
	CONST

declarator:
	pointer direct_declarator
	;

direct_declarator:
	IDF
	| LPAR declarator RPAR
	| array_declarator
	| function_declarator
	;

array_declarator:
	direct_declarator LBRA assignment_expression RBRA
	;

function_declarator:
	direct_declarator LPAR parameter_type_list RPAR
	;

pointer:
	STAR type_qualifier_list
	| STAR type_qualifier_list pointer
	;

type_qualifier_list:
	type_qualifier
	| type_qualifier_list type_qualifier
	;

parameter_type_list:
	parameter_list
	;

parameter_list:
	parameter_declaration
	| parameter_list COMMA parameter_declaration
	;

parameter_declaration:
	declaration_specifiers declarator
	| declaration_specifiers abstract_declarator
	;

type_name:
	specifier_qualifier_list abstract_declarator
	;

abstract_declarator:
	pointer
	| pointer direct_abstract_declarator
	;

direct_abstract_declarator:
	LPAR abstract_declarator RPAR
	| array_abstract_declarator
	| function_abstract_declarator
	;

array_abstract_declarator:
	direct_abstract_declarator LBRA assignment_expression RBRA
	;

function_abstract_declarator:
	direct_abstract_declarator LPAR parameter_type_list RPAR
	;

typedef_name:
	TYPEIDF
	;




/////////////// Statements

statement:
	statement_m
	| statement_u
	;

statement_m:
	selection_statement_m
	| jump_statement
	;

statement_u:
	expression_statement
	| compound_statement
	| selection_statement_u
	| iteration_statement
	;

compound_statement:
	LCUR block_item_list RCUR
	;

block_item_list:
	block_item
	| block_item_list block_item
	;

block_item:
	declaration
	| statement
	;

expression_statement:
	expression SEMIC
	;


selection_statement_m:
	IF LPAR expression RPAR statement_m ELSE statement_m
	;

selection_statement_u:
	IF LPAR expression RPAR statement
	| IF LPAR expression RPAR statement_m ELSE statement_u
	;
	
iteration_statement:
	WHILE LPAR expression RPAR statement
	| DO statement WHILE LPAR expression RPAR SEMIC
	| FOR LPAR expression SEMIC expression SEMIC expression RPAR statement
	;

jump_statement:
	RETURN expression SEMIC
	;



/////////////// External definitions

translation_unit:
	external_declaration
	| translation_unit external_declaration
	;

external_declaration:
	function_definition
	| declaration
	;

function_definition:
	declaration_specifiers declarator compound_statement
	;









/////////////////////////////////

%%

namespace cecko {

	void parser::error(const location_type& l, const std::string& m)
	{
		ctx->message(cecko::errors::SYNTAX, l, m);
	}

}