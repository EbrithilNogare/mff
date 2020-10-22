%{

// allow access to YY_DECL macro
#include "ckbisonflex.hpp"

#include INCLUDE_WRAP(BISON_HEADER)

	int comment_depth = 0;
	std::string string_buff;

%}

/* NEVER SET %option outfile INTERNALLY - SHALL BE SET BY CMAKE */

%option noyywrap nounput noinput
%option batch never-interactive reentrant
%option nounistd 

/* AVOID backup perf-report - DO NOT CREATE UNMANAGEABLE BYPRODUCT FILES */

%x STRING
%x ML_COMMENT

%%

\n				ctx->incline();

<ML_COMMENT><<EOF>>	{
		ctx->message(cecko::errors::EOFINCMT, ctx->line());
		return cecko::parser::make_EOF(ctx->line());
	}

\/\/.*$

\/\* {	
	comment_depth = 1;
	BEGIN(ML_COMMENT);
}

<ML_COMMENT>\/\*	{ comment_depth++; }

<ML_COMMENT>\*\/ {	
	comment_depth--;
	if(comment_depth==0)
		BEGIN(INITIAL);
}

\*\/		ctx->message(cecko::errors::UNEXPENDCMT, ctx->line());

<ML_COMMENT>\n		ctx->incline();
<ML_COMMENT>.		

[\n\r\t\f ]	

"typedef"	return cecko::parser::make_TYPEDEF(ctx->line());		
"void"		return cecko::parser::make_VOID(ctx->line());	
"_Bool"		return cecko::parser::make_ETYPE(cecko::gt_etype::BOOL,ctx->line());	
"char"		return cecko::parser::make_ETYPE(cecko::gt_etype::CHAR,ctx->line());	
"int"		return cecko::parser::make_ETYPE(cecko::gt_etype::INT,ctx->line());	
"struct"	return cecko::parser::make_STRUCT(ctx->line());		
"enum"		return cecko::parser::make_ENUM(ctx->line());	
"const"		return cecko::parser::make_CONST(ctx->line());	
"if"		return cecko::parser::make_IF(ctx->line());	
"else"		return cecko::parser::make_ELSE(ctx->line());	
"do"		return cecko::parser::make_DO(ctx->line());	
"while"		return cecko::parser::make_WHILE(ctx->line());	
"for"		return cecko::parser::make_FOR(ctx->line());	
"goto"		return cecko::parser::make_GOTO(ctx->line());	
"continue"	return cecko::parser::make_CONTINUE(ctx->line());		
"break"		return cecko::parser::make_BREAK(ctx->line());	
"return"	return cecko::parser::make_RETURN(ctx->line());		
"sizeof"	return cecko::parser::make_SIZEOF(ctx->line());		


"["			return cecko::parser::make_LBRA(ctx->line());
"]"			return cecko::parser::make_RBRA(ctx->line());
"("			return cecko::parser::make_LPAR(ctx->line());
")"			return cecko::parser::make_RPAR(ctx->line());
"."			return cecko::parser::make_DOT(ctx->line());
"->"		return cecko::parser::make_ARROW(ctx->line());
"++"		return cecko::parser::make_INCDEC(cecko::gt_incdec::INC,ctx->line());
"--"		return cecko::parser::make_INCDEC(cecko::gt_incdec::DEC,ctx->line());
","			return cecko::parser::make_COMMA(ctx->line());
"&"			return cecko::parser::make_AMP(ctx->line());
"*"			return cecko::parser::make_STAR(ctx->line());
"+"			return cecko::parser::make_ADDOP(cecko::gt_addop::ADD,ctx->line());
"-"			return cecko::parser::make_ADDOP(cecko::gt_addop::SUB,ctx->line());
"!"			return cecko::parser::make_EMPH(ctx->line());
"/"			return cecko::parser::make_DIVOP(cecko::gt_divop::DIV,ctx->line());
"%"			return cecko::parser::make_DIVOP(cecko::gt_divop::MOD,ctx->line());
"<"			return cecko::parser::make_CMPO(cecko::gt_cmpo::LT,ctx->line());
">"			return cecko::parser::make_CMPO(cecko::gt_cmpo::GT,ctx->line());
"<="		return cecko::parser::make_CMPO(cecko::gt_cmpo::LE,ctx->line());
">="		return cecko::parser::make_CMPO(cecko::gt_cmpo::GE,ctx->line());
"=="		return cecko::parser::make_CMPE(cecko::gt_cmpe::EQ,ctx->line());
"!="		return cecko::parser::make_CMPE(cecko::gt_cmpe::NE,ctx->line());
"&&"		return cecko::parser::make_DAMP(ctx->line());
"||"		return cecko::parser::make_DVERT(ctx->line());
"="			return cecko::parser::make_ASGN(ctx->line());
"*="		return cecko::parser::make_CASS(cecko::gt_cass::MULA,ctx->line());
"/="		return cecko::parser::make_CASS(cecko::gt_cass::DIVA,ctx->line());
"%="		return cecko::parser::make_CASS(cecko::gt_cass::MODA,ctx->line());
"+="		return cecko::parser::make_CASS(cecko::gt_cass::ADDA,ctx->line());
"-="		return cecko::parser::make_CASS(cecko::gt_cass::SUBA,ctx->line());
";"			return cecko::parser::make_SEMIC(ctx->line());
"{"			return cecko::parser::make_LCUR(ctx->line());
"}"			return cecko::parser::make_RCUR(ctx->line());
":"			return cecko::parser::make_COLON(ctx->line());


[a-zA-Z_][a-zA-Z0-9_]*	{
		return cecko::parser::make_IDF(std::basic_string(yytext),ctx->line());
	}

\"	{ //"{
	BEGIN(STRING);
	string_buff = "";
}

<STRING>\"	{ //"{
	BEGIN(INITIAL);
	return cecko::parser::make_STRLIT(string_buff,ctx->line());
}

<STRING>\\n		{ string_buff.append("\x0a"); }
<STRING>\\\"	{ string_buff.append("\x22"); /*"{*/ }


<STRING>.	{
		string_buff.append(yytext);
	}

'.+'	{
		std::string tmp = std::basic_string(yytext);
		return cecko::parser::make_INTLIT(tmp[1],ctx->line());
	}


[0-9]+	{
		return cecko::parser::make_INTLIT(std::stod(yytext),ctx->line());
	}

[\\][x][0-9]{1,3}	{
		return cecko::parser::make_TYPEIDF(std::basic_string(yytext),ctx->line());
	}



.		ctx->message(cecko::errors::UNCHAR, ctx->line(), yytext);


<<EOF>>			return cecko::parser::make_EOF(ctx->line());

%%

namespace cecko {

	yyscan_t lexer_init(FILE * iff)
	{
		yyscan_t scanner;
		yylex_init(&scanner);
		yyset_in(iff, scanner);
		return scanner;
	}

	void lexer_shutdown(yyscan_t scanner)
	{
		yyset_in(nullptr, scanner);
		yylex_destroy(scanner);
	}

}
