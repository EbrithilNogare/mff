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
%type<cecko::CKTypeObs>
	type_specifier

%type<casem::CKDeclarationSpecifier>
	declaration_specifier
	type_specifier_qualifier

%type<casem::CKDeclarationSpecifierList>
	declaration_specifiers
	specifier_qualifier_list

%type<casem::CKDeclarator>
	declarator
	direct_declarator
	abstract_declarator
	direct_abstract_declarator
	array_declarator
	array_abstract_declarator
	function_declarator
	function_abstract_declarator

%type<casem::CKDeclaratorList>
	init_declarator_list
	init_declarator_list_opt
	member_declarator_list

%type<casem::CKPointerList>
	pointer

%type<casem::CKParameter> 
	parameter_declaration

%type<casem::CKParameterList>
	parameter_list


%type<casem::CKExpression>
	expression_opt
	expression






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
	| AMP cast_expression
	| STAR cast_expression
	| ADDOP cast_expression
	| EMPH cast_expression
	| SIZEOF LPAR specifier_qualifier_list  RPAR
	| SIZEOF LPAR specifier_qualifier_list abstract_declarator RPAR
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
	| unary_expression ASGN assignment_expression
	| unary_expression CASS assignment_expression
	;

expression_opt: // type: casem::CKExpression
	%empty { $$ = casem::CKExpression(); }
	| expression { $$ = $1; }
	;

expression: // type: casem::CKExpression
	assignment_expression { $$ = casem::CKExpression(); } // todo
	;

constant_expression:
	logical_OR_expression
	;




/////////////// Declarations

declaration:
	declaration_body SEMIC
	;

declaration_body:
	declaration_specifiers init_declarator_list_opt { casem::declare(ctx, $1, $2); }
	;

declaration_specifiers: // type: casem::CKDeclarationSpecifierList
	declaration_specifier { 
		auto vec = casem::CKDeclarationSpecifierList();
		vec.push_back($1);
		$$ = vec;
	}
	| declaration_specifiers declaration_specifier {
		$1.push_back($2);
		$$ = $1;
	}
	;

declaration_specifier: // type: casem::CKDeclarationSpecifier
	TYPEDEF { $$ = casem::CKDeclarationSpecifier{true, false}; }
	| type_specifier_qualifier { $$ = $1; }
	;

init_declarator_list_opt: // type: casem::CKDeclaratorList
	%empty { $$ = casem::CKDeclaratorList(); }
	| init_declarator_list { $$ = $1; }
	;


init_declarator_list: // type: casem::CKDeclaratorList
	declarator { 
		auto vec = casem::CKDeclaratorList();
		vec.push_back($1);
		$$ = vec;
	}
	| init_declarator_list COMMA declarator {
		$1.push_back($3);
		$$ = $1;
	}
	;

type_specifier: // type: cecko::CKTypeObs
	VOID { $$ = ctx->get_void_type(); }
	| ETYPE { $$ = casem::convert_etype(ctx, $1); }
	| struct_or_union_specifier { $$ = ctx->get_void_type(); } // bonus
	| enum_specifier { $$ = ctx->get_void_type(); } // bonus
	| TYPEIDF { $$ = ctx->find_typedef($1)->get_type_pack().type; }
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

specifier_qualifier_list: // type: casem::CKDeclarationSpecifierList
	type_specifier_qualifier { 
		auto vec = casem::CKDeclarationSpecifierList();
		vec.push_back($1);
		$$ = vec;
	}
	| type_specifier_qualifier specifier_qualifier_list {
		$2.push_back($1);
		$$ = $2;
	}
	;

type_specifier_qualifier: // type: casem::CKDeclarationSpecifier
	type_specifier { $$ = casem::CKDeclarationSpecifier{$1}; }
	| CONST { $$ = casem::CKDeclarationSpecifier{false, true}; }
	;

member_declarator_list_opt:
	%empty
	| member_declarator_list
	;

member_declarator_list: // type: casem::CKDeclaratorList
	declarator { 
		auto vec = casem::CKDeclaratorList();
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

declarator: // type: casem::CKDeclarator
	direct_declarator
	| pointer direct_declarator {
		$2.add_modifier($1);
		$$ = $2;
	}
	;

direct_declarator: // type: casem::CKDeclarator
	IDF { $$ = casem::CKDeclarator($1, @1); }
	| LPAR declarator RPAR { $$ = $2; }
	| array_declarator { $$ = $1; }
	| function_declarator { $$ = $1; }
	;

array_declarator: // type: casem::CKDeclarator
	// direct_declarator LBRA assignment_expression RBRA // hack
	direct_declarator LBRA INTLIT RBRA {
		$1.add_modifier(casem::CKArray(ctx->get_int32_constant($3)));
		$$ = $1;
	}
	;

function_declarator: // type: casem::CKDeclarator
	direct_declarator LPAR parameter_list RPAR {
		$1.add_modifier(casem::CKFunction($3));
		$$ = $1;
	}
	;

pointer: // type: casem::CKPointerList
	STAR {
		casem::CKPointer pointer(false);
		auto pointers = casem::CKPointerList();
		pointers.push_back(pointer);
		$$ = pointers;
	}
	| STAR type_qualifier_list {
		casem::CKPointer pointer(true);
		auto pointers = casem::CKPointerList();
		pointers.push_back(pointer);
		$$ = pointers;
	}
	| STAR pointer {
		casem::CKPointer pointer(false);
		$2.push_back(pointer);
		$$ = $2;
	}
	| STAR type_qualifier_list pointer {
		casem::CKPointer pointer(true);
		$3.push_back(pointer);
		$$ = $3;
	}
	;

type_qualifier_list:
	CONST
	| type_qualifier_list CONST
	;

parameter_list:  // type: casem::CKParameterList 
	parameter_declaration { 
		auto vec = casem::CKParameterList();
		vec.push_back($1);
		$$ = vec;
	}
	|  parameter_list COMMA parameter_declaration {
		$1.push_back($3);
		$$ = $1;
	}
	;

parameter_declaration: // type: casem::CKParameter
	declaration_specifiers {
		$$ = casem::CKParameter($1);
	}
	| declaration_specifiers declarator {
		auto declarators = casem::CKDeclaratorList();
		declarators.push_back($2);
		$$ = casem::CKParameter($1, declarators);
	}
	| declaration_specifiers abstract_declarator {
		auto declarators = casem::CKDeclaratorList();
		declarators.push_back($2);
		$$ = casem::CKParameter($1, declarators);
	}
	;

abstract_declarator: // type: casem::CKDeclarator
	pointer {
		auto declarator = casem::CKDeclarator();
		declarator.add_modifier($1);
		$$ = declarator;
	}
	| direct_abstract_declarator
	| pointer direct_abstract_declarator {
		$2.add_modifier($1);
		$$ = $2;
	}
	;

direct_abstract_declarator: // type casem::CKDeclarator
	LPAR abstract_declarator RPAR { $$ = casem::CKDeclarator($2); }
	| array_abstract_declarator { $$ = $1; }
	| function_abstract_declarator { $$ = $1; }
	;

array_abstract_declarator: // type casem::CKDeclarator
	// LBRA assignment_expression RBRA // hack
	//direct_abstract_declarator LBRA assignment_expression RBRA // hack
	LBRA INTLIT RBRA {
		auto declarator = casem::CKDeclarator();
		declarator.add_modifier(casem::CKDeclaratorModifier(casem::CKArray(ctx->get_int32_constant($2))));
		$$ = declarator;
	}
	| direct_abstract_declarator LBRA INTLIT RBRA {
		$1.add_modifier(casem::CKDeclaratorModifier(casem::CKArray(ctx->get_int32_constant($3))));
		$$ = $1;
	}
	;

function_abstract_declarator: // type casem::CKDeclarator
	LPAR parameter_list RPAR {
		auto declarator = casem::CKDeclarator();
		declarator.add_modifier(casem::CKFunction($2));
		$$ = declarator;
	}
	| direct_abstract_declarator LPAR parameter_list RPAR {
		$1.add_modifier(casem::CKFunction($3));
		$$ = $1;
	}
	;


/////////////// Statements

statement:
	statement_m
	| statement_u
	;

statement_m:
	expression_statement
	| enter_block block_item_list_opt exit_block
	| selection_statement_m
	| iteration_statement_m
	| jump_statement
	;

statement_u:
	selection_statement_u
	| iteration_statement_u
	;

enter_block:
	LCUR { ctx->enter_block(); } 
	;

exit_block:
	RCUR { ctx->exit_block(); }
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
	RETURN SEMIC {
		ctx->builder()->CreateRetVoid();
		ctx->builder()->ClearInsertionPoint();
	}
	| RETURN expression SEMIC {

		// get type of function

		// get type of expression_opt

		// if equal
			// return 

		// else
			// convert it

			
		ctx->builder()->ClearInsertionPoint();
	}
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
	// declaration_specifiers declarator LCUR block_item_list_opt RCUR
	function_definition_head function_definition_body RCUR
	;

function_definition_head:
	declaration_specifiers declarator { casem::declareFunctionDefinition(ctx, $1, $2); }
	;

function_definition_body:
	LCUR block_item_list_opt {
		if(ctx->builder()->GetInsertBlock() != NULL){
			// emit the implicit final return
			ctx->builder()->CreateRetVoid();
			ctx->builder()->ClearInsertionPoint();
		}

		ctx-> exit_function();
	}
	;









/////////////////////////////////

%%

namespace cecko {

	void parser::error(const location_type& l, const std::string& m)
	{
		ctx->message(cecko::errors::SYNTAX, l, m);
	}

}