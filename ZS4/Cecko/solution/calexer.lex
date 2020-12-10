%{

// allow access to YY_DECL macro
#include "ckbisonflex.hpp"

#include INCLUDE_WRAP(BISON_HEADER)

	int comment_depth = 0;
	std::string string_buff;
	long char_buff = 0;

%}

/* NEVER SET %option outfile INTERNALLY - SHALL BE SET BY CMAKE */

%option noyywrap nounput noinput
%option batch never-interactive reentrant
%option nounistd 

/* AVOID backup perf-report - DO NOT CREATE UNMANAGEABLE BYPRODUCT FILES */

%x STRING
%x CeckoCHAR
%x COMMENT

%%

\n				ctx->incline();

<COMMENT><<EOF>>	{
		ctx->message(cecko::errors::EOFINCMT, ctx->line());
		return cecko::parser::make_EOF(ctx->line());
	}

\/\/.*$

\/\* {	
	comment_depth = 1;
	BEGIN(COMMENT);
}

<COMMENT>\/\*	{ comment_depth++; }

<COMMENT>\*\/ {	
	comment_depth--;
	if(comment_depth==0)
		BEGIN(INITIAL);
}

\*\/		ctx->message(cecko::errors::UNEXPENDCMT, ctx->line());

<COMMENT>\n		ctx->incline();
<COMMENT>.		

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


[a-zA-Z_][a-zA-Z0-9_]*	{
		if(ctx->is_typedef(yytext)){
			return cecko::parser::make_TYPEIDF(yytext,ctx->line());
		}else{
			return cecko::parser::make_IDF(yytext,ctx->line());
		}
	}

\"	{ //"{
	BEGIN(STRING);
	string_buff = "";
}

<STRING>\"	{ //"{
	BEGIN(INITIAL);
	return cecko::parser::make_STRLIT(string_buff,ctx->line());
}

<STRING><<EOF>>	{
		ctx->message(cecko::errors::EOFINSTRCHR, ctx->line());
		BEGIN(INITIAL);
		return cecko::parser::make_STRLIT(string_buff,ctx->line());
	}
<STRING>\n		{
		BEGIN(INITIAL);
		ctx->message(cecko::errors::EOLINSTRCHR, ctx->line());
		ctx->incline();
		return cecko::parser::make_STRLIT(string_buff,ctx->line()-1);
	}
<STRING>\\n		{ string_buff.append("\x0a"); }
<STRING>\\\"	{ string_buff.append("\x22"); /*"{*/ }
<STRING>(\'|\"|\?|\a|\b|\f|\r|\t|\v)		{ //'{
		string_buff.append(yytext);
	}
<STRING>\\x[^\"\n]{4,}	{ //"{
		std::string tmp = yytext; 
		ctx->message(cecko::errors::BADESCAPE, ctx->line(),tmp.substr(0,6));
		string_buff.append(tmp.substr(0,2));
		string_buff.append(tmp.substr(4));
	}
<STRING>\\[^\"\n]*	{ //"{
		std::string tmp = yytext; 
		ctx->message(cecko::errors::BADESCAPE, ctx->line(),yytext);
		string_buff.append(tmp.substr(1));
	}
<STRING>.	{
		string_buff.append(yytext);
	}

''		{
		ctx->message(cecko::errors::EMPTYCHAR, ctx->line());
		return cecko::parser::make_INTLIT(0,ctx->line());
	}
'	{ //'{
		BEGIN(CeckoCHAR);
		char_buff = 0;
	}
<CeckoCHAR>'	{ //'{
		BEGIN(INITIAL);
		if(char_buff >= 2147483648) // 2^32
			ctx->message(cecko::errors::MULTICHAR_LONG, ctx->line());
		return cecko::parser::make_INTLIT(char_buff,ctx->line());
	}
<CeckoCHAR><<EOF>>	{
		ctx->message(cecko::errors::EOFINSTRCHR, ctx->line());
		BEGIN(INITIAL);
		if(char_buff >= 2147483648) // 2^32
			ctx->message(cecko::errors::MULTICHAR_LONG, ctx->line());
		return cecko::parser::make_INTLIT(char_buff,ctx->line());
	}
<CeckoCHAR>\n		{
		BEGIN(INITIAL);
		if(char_buff >= 2147483648) // 2^32
			ctx->message(cecko::errors::MULTICHAR_LONG, ctx->line());
		ctx->message(cecko::errors::EOLINSTRCHR, ctx->line());
		ctx->incline();
		return cecko::parser::make_INTLIT(char_buff,ctx->line()-1);
	}
<CeckoCHAR>(\'|\"|\?|\a|\b|\f|\r|\t|\v)		{ //'{
		char_buff <<= 8;
		char_buff |= yytext[0];
	}
<CeckoCHAR>\\x[^'\n]*		{ //']{
		std::string tmp = yytext; 
		if(tmp.length()>5){
			ctx->message(cecko::errors::BADESCAPE, ctx->line(),tmp);
			char_buff |= tmp[tmp.length()-1];
		}
		else if(tmp.length()<3){
			ctx->message(cecko::errors::BADESCAPE, ctx->line(),tmp);
			char_buff <<= 8;
			char_buff += tmp.length() == 2 ? 'x' : 0;
		} else {
			char_buff <<= 8;
			char_buff |= std::stoi(tmp.substr(2),nullptr,16);
		}
	}
<CeckoCHAR>\\[^'\n]*	{ //'{
		std::string tmp = yytext;
		ctx->message(cecko::errors::BADESCAPE, ctx->line(),yytext);
		char_buff <<= 8;
		if(tmp.length()>1)
			char_buff |= tmp[tmp.length()-1];
	}
<CeckoCHAR>[^'\n]		{ //'{
		char_buff <<= 8;
		char_buff |= yytext[0];
	}

[0-9]{1,9}	{
		return cecko::parser::make_INTLIT(std::stod(yytext),ctx->line());
	}
0x[0-9a-zA-Z]+	{
		std::string tmp = yytext;
		unsigned long sum = 0;
		long long time = 1;
		const ulong INTMAX = 2147483648;
		bool happyending = true;
		tmp = tmp.substr(2);
		for(auto& tesla : tmp) {
			if((tesla <= '9' && tesla >= '0') || (tesla <= 'f' && tesla >= 'a') || (tesla <= 'F' && tesla >= 'A')) {
				sum*=16;
				if(sum >= INTMAX)
					happyending = false;
				sum%=INTMAX;
				sum+=std::stoi(std::string(1,tesla),0,16);
				if(sum >= INTMAX)
					happyending = false;
			} else{
				ctx->message(cecko::errors::BADINT, ctx->line(),yytext);
				break;
			}
			
		}
		if(!happyending)
			ctx->message(cecko::errors::INTOUTRANGE, ctx->line(),yytext);
		return cecko::parser::make_INTLIT(happyending?sum:INTMAX+sum,ctx->line());
	}
[0-9a-zA-Z]+	{
		std::string tmp = yytext;
		unsigned long sum = 0;
		long long time = 1;
		const ulong INTMAX = 2147483648;
		bool happyending = true;
		for(auto& tesla : tmp) {
			if(tesla > '9' || tesla < '0') {
				ctx->message(cecko::errors::BADINT, ctx->line(),yytext);
				break;
			} 
			sum*=10;
			if(sum >= INTMAX)
				happyending = false;
			sum%=INTMAX;
			sum+=(tesla - '0');
			if(sum >= INTMAX)
				happyending = false;
		}
		if(!happyending)
			ctx->message(cecko::errors::INTOUTRANGE, ctx->line(),yytext);
		return cecko::parser::make_INTLIT(happyending?sum:INTMAX+sum,ctx->line());
	}

\\x[0-9a-fA-F]*	{		
		return cecko::parser::make_IDF(std::basic_string(yytext),ctx->line());
	}


.				ctx->message(cecko::errors::UNCHAR, ctx->line(), yytext);


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
