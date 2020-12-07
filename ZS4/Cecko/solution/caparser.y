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

/////////////// Declarations
%type<cecko::CKTypeObs> type_specifier typedef_name
%type<casem::DeclarationSpecifierDto> declaration_specifier type_qualifier
%type<casem::DeclarationSpecifiersDto> declaration_specifiers specifier_qualifier_list
%type<casem::DeclaratorDto> declarator direct_declarator
%type<casem::DeclaratorsDto> init_declarator_list member_declarator_list
%type<casem::PointersDto> pointer type_qualifier_list
%type<casem::DeclaratorsDto> init_declarator_list_opt
%type<casem::DeclarationSpecifierDto> type_specifier_qualifier







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
	| postfix_expression LPAR argument_expression_list_opt RPAR
	| postfix_expression DOT IDF
	| postfix_expression ARROW IDF
	| postfix_expression INCDEC
	;

argument_expression_list_opt:
	%empty
	| argument_expression_list
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

expression_opt:
	%empty
	| expression
	;

expression:
	assignment_expression
	;

constant_expression:
	logical_OR_expression
	;




/////////////// Declarations

declaration:
	declaration_body SEMIC
	;

declaration_body:
	declaration_specifiers init_declarator_list_opt { casem::DefineVariables(ctx, $1, $2); }
	;

declaration_specifiers:
	declaration_specifier { 
		auto vec = casem::DeclarationSpecifiersDto();
		vec.push_back($1);
		$$ = vec;
	}
	| declaration_specifier declaration_specifiers {
		$2.push_back($1);
		$$ = $2;
	}
	;

declaration_specifier:
	TYPEDEF { $$ = casem::DeclarationSpecifierDto{true, false}; }
	| type_specifier_qualifier { $$ = $1; }
	;

init_declarator_list_opt:
	%empty { $$ = casem::DeclaratorsDto(); }
	| init_declarator_list { $$ = $1; }
	;


init_declarator_list:
	declarator { 
		auto vec = casem::DeclaratorsDto();
		vec.push_back($1);
		$$ = vec;
	}
	| init_declarator_list COMMA declarator {
		$1.push_back($3);
		$$ = $1;
	}
	;

type_specifier:
	VOID { $$ = ctx->get_void_type(); }
	| ETYPE { $$ = casem::parse_etype(ctx, $1); }
	| struct_or_union_specifier { $$ = ctx->get_void_type(); } // bonus
	| enum_specifier { $$ = ctx->get_void_type(); } // bonus
	| typedef_name { $$ = $1; }
	;

struct_or_union_specifier:
	STRUCT IDF LCUR member_declaration_list RCUR
	| STRUCT IDF
	;

member_declaration_list:
	member_declaration
	| member_declaration_list member_declaration
	;

member_declaration:
	specifier_qualifier_list member_declarator_list_opt SEMIC
	;

specifier_qualifier_list:
	type_specifier_qualifier { 
		auto vec = casem::DeclarationSpecifiersDto();
		vec.push_back($1);
		$$ = vec;
	}
	| type_specifier_qualifier specifier_qualifier_list {
		$2.push_back($1);
		$$ = $2;
	}
	;

type_specifier_qualifier:
	type_specifier { $$ = casem::DeclarationSpecifierDto{$1}; }
	| type_qualifier { $$ = $1; }
	;

member_declarator_list_opt:
	%empty
	| member_declarator_list
	;

member_declarator_list:
	declarator { 
		auto vec = casem::DeclaratorsDto();
		vec.push_back($1);
		$$ = vec;
	}
	| member_declarator_list COMMA declarator {
		$1.push_back($3);
		$$ = $1;
	}
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
	CONST { $$ = casem::DeclarationSpecifierDto{false, true}; }
	;

declarator: // type: casem::DeclaratorDto
	direct_declarator
	| pointer direct_declarator { $$ = $2; } // todo call constructor
	;

direct_declarator:
	IDF { $$ = casem::DeclaratorDto($1); }
	| LPAR declarator RPAR { $$ = casem::DeclaratorDto($2); }
	| array_declarator { $$ = casem::DeclaratorDto(); } // bonus
	| function_declarator { $$ = casem::DeclaratorDto(); } // bonus
	;

array_declarator:
	direct_declarator LBRA assignment_expression RBRA
	;

function_declarator:
	direct_declarator LPAR parameter_type_list RPAR
	;

pointer: // type: casem::PointersDto
	STAR type_qualifier_list_opt { $$ = $2; }
	| STAR type_qualifier_list_opt pointer { $$ = $2; }
	;

type_qualifier_list_opt:
	%empty
	| type_qualifier_list
	;

type_qualifier_list: //type: casem::PointersDto
	type_qualifier { 
		auto vec = casem::PointersDto();
		vec.push_back($1);
		$$ = vec;
	}
	| type_qualifier_list type_qualifier {
		$1.push_back($2);
		$$ = $1;
	}
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
	| declaration_specifiers abstract_declarator_opt
	;

type_name:
	specifier_qualifier_list abstract_declarator_opt
	;

abstract_declarator_opt:
	%empty
	| abstract_declarator
	;

abstract_declarator:
	pointer
	| direct_abstract_declarator
	| pointer direct_abstract_declarator
	;

direct_abstract_declarator_opt:
	direct_abstract_declarator
	// | %empty
	;

direct_abstract_declarator:
	LPAR abstract_declarator RPAR
	| array_abstract_declarator
	| function_abstract_declarator
	;

array_abstract_declarator:
	direct_abstract_declarator_opt LBRA assignment_expression RBRA
	;

function_abstract_declarator:
	direct_abstract_declarator_opt LPAR parameter_type_list RPAR
	;

typedef_name: 
	TYPEIDF { $$ = ctx->find_typedef($1)->get_type_pack().type; }
	;



/////////////// Statements

statement:
	statement_m
	| statement_u
	;

statement_m:
	expression_statement
	| compound_statement
	| selection_statement_m
	| iteration_statement_m
	| jump_statement
	;

statement_u:
	selection_statement_u
	| iteration_statement_u
	;

compound_statement:
	LCUR block_item_list_opt RCUR
	;

block_item_list_opt:
	%empty
	| block_item_list
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
	expression_opt SEMIC
	;

selection_statement_m:
	IF LPAR expression RPAR statement_m ELSE statement_m
	;
	
selection_statement_u:
	IF LPAR expression RPAR statement
	| IF LPAR expression RPAR statement_m ELSE statement_u
	;

iteration_statement_m:
	WHILE LPAR expression RPAR statement_m
	| DO statement_m WHILE LPAR expression RPAR SEMIC
	| FOR LPAR expression_opt SEMIC expression_opt SEMIC expression_opt RPAR statement_m
	;
	
iteration_statement_u:
	WHILE LPAR expression RPAR statement_u
	| DO statement_u WHILE LPAR expression RPAR SEMIC
	| FOR LPAR expression_opt SEMIC expression_opt SEMIC expression_opt RPAR statement_u
	;

jump_statement:
	RETURN expression_opt SEMIC
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