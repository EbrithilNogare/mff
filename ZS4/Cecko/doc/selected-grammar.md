# Cecko grammar

## Lexical grammar

### Keyword

```
keyword: one of
	break
	const
	continue
	do
	else
	enum
	for
	goto
	if
	char
	int
	return
	sizeof
	struct
	typedef
	void
	while
	_Bool
```

### Identifier

```
identifier:
	identifier-nondigit
	identifier identifier-nondigit
	identifier digit

identifier-nondigit:
	nondigit

nondigit: one of
	_ a b c d e f g h i j k l m
	n o p q r s t u v w x y z
	A B C D E F G H I J K L M
	N O P Q R S T U V W X Y Z
```

### Constants

```
digit: one of
	0 1 2 3 4 5 6 7 8 9

constant:
	integer-constant
	enumeration-constant
	character-constant

integer-constant:
	decimal-constant
	hexadecimal-constant

decimal-constant:
	digit
	decimal-constant digit

hexadecimal-constant:
	hexadecimal-prefix hexadecimal-digit
	hexadecimal-constant hexadecimal-digit

hexadecimal-prefix: one of
	0x 0X

hexadecimal-digit: one of
	0 1 2 3 4 5 6 7 8 9
	a b c d e f
	A B C D E F


enumeration-constant:
	identifier
```

### Strings and characters

```
character-constant:
	' c-char-sequence '

c-char-sequence:
	c-char
	c-char-sequence c-char

c-char:
	any member of the source character set except
	the single-quote ', backslash \, or new-line character
	escape-sequence

escape-sequence:
	simple-escape-sequence
	hexadecimal-escape-sequence

simple-escape-sequence: one of
	\' \" \? \\
	\a \b \f \n \r \t \v

hexadecimal-escape-sequence:
	\x hexadecimal-digit
	hexadecimal-escape-sequence hexadecimal-digit

string-literal:
	" s-char-sequence_opt "

s-char-sequence:
	s-char
	s-char-sequence s-char

s-char:
	any member of the source character set except
	the double-quote ", backslash \, or new-line character
	escape-sequence

```

### Punctuators


```
punctuator: one of
	[ ] ( ) { } . ->
	++ -- & * + - !
	/ % < > <= >= == != && ||
	; : ,
	= *= /= %= += -=
```

## Phrase structure grammar

### Expressions

```
primary-expression:
	identifier
	constant
	string-literal
	( expression )

postfix-expression:
	primary-expression
	postfix-expression [ expression ]
	postfix-expression ( argument-expression-list_opt> )
	postfix-expression . identifier
	postfix-expression -> identifier
	postfix-expression ++
	postfix-expression --

argument-expression-list:
	assignment-expression
	argument-expression-list , assignment-expression

unary-expression:
	postfix-expression
	++ unary-expression
	-- unary-expression
	unary-operator cast-expression
	sizeof ( type-name )

unary-operator: one of
	& * + - !

cast-expression:
	unary-expression

multiplicative-expression:
	cast-expression
	multiplicative-expression * cast-expression
	multiplicative-expression / cast-expression
	multiplicative-expression % cast-expression

additive-expression:
	multiplicative-expression
	additive-expression + multiplicative-expression
	additive-expression - multiplicative-expression

relational-expression:
	additive-expression
	relational-expression < additive-expression
	relational-expression > additive-expression
	relational-expression <= additive-expression
	relational-expression >= additive-expression

equality-expression:
	relational-expression
	equality-expression == relational-expression
	equality-expression != relational-expression

logical-AND-expression:
	equality-expression
	logical-AND-expression && equality-expression

logical-OR-expression:
	logical-AND-expression
	logical-OR-expression || logical-AND-expression

assignment-expression:
	logical-OR-expression
	unary-expression assignment-operator assignment-expression

assignment-operator: one of
	= *= /= %= += -=

expression:
	assignment-expression

constant-expression:
	logical-OR-expression

```

### Declarations

```

declaration:
	no-leading-attribute-declaration

no-leading-attribute-declaration:
	declaration-specifiers init-declarator-list_opt ;

declaration-specifiers:
	declaration-specifier
	declaration-specifier declaration-specifiers

declaration-specifier:
	storage-class-specifier
	type-specifier-qualifier

init-declarator-list:
	init-declarator
	init-declarator-list , init-declarator

init-declarator:
	declarator

storage-class-specifier:
	typedef

type-specifier:
	void
	_Bool
	char
	int
	struct-or-union-specifier
	enum-specifier
	typedef-name

struct-or-union-specifier:
	struct-or-union identifier_opt { member-declaration-list }
	struct-or-union identifier

struct-or-union:
	struct

member-declaration-list:
	member-declaration
	member-declaration-list member-declaration

member-declaration:
	specifier-qualifier-list member-declarator-list_opt ;

specifier-qualifier-list:
	type-specifier-qualifier
	type-specifier-qualifier specifier-qualifier-list

type-specifier-qualifier:
	type-specifier
	type-qualifier

member-declarator-list:
	member-declarator
	member-declarator-list , member-declarator

member-declarator:
	declarator

enum-specifier:
	enum identifier_opt { enumerator-list }
	enum identifier_opt { enumerator-list , }
	enum identifier

enumerator-list:
	enumerator
	enumerator-list , enumerator

enumerator:
	enumeration-constant
	enumeration-constant = constant-expression

type-qualifier:
	const

declarator:
	one-pointer_opt direct-declarator

direct-declarator:
	identifier
	( declarator )
	array-declarator
	function-declarator

array-declarator:
	direct-declarator [assignment-expression ]

function-declarator:
	direct-declarator ( parameter-type-list )

one-pointer:
	* type-qualifier-list_opt

type-qualifier-list:
	type-qualifier
	type-qualifier-list type-qualifier

parameter-type-list:
	parameter-list

parameter-list:
	parameter-declaration
	parameter-list , parameter-declaration

parameter-declaration:
	declaration-specifiers declarator
	declaration-specifiers abstract-declarator_opt

identifier-list:
	identifier
	identifier-list , identifier

abstract-declarator:
	one-pointer
	one-pointer_opt direct-abstract-declarator

direct-abstract-declarator:
	( abstract-declarator )
	array-abstract-declarator
	function-abstract-declarator

array-abstract-declarator:
	direct-abstract-declarator_opt [ assignment-expression ]

function-abstract-declarator:
	direct-abstract-declarator_opt ( parameter-type-list_opt )

typedef-name:
	identifier
```

### Statements

```
statement:
	labeled-statement
	expression-statement
	compound-statement
	selection-statement
	iteration-statement
	jump-statement

labeled-statement:
	identifier : statement

compound-statement:
	{ block-item-list_opt }

block-item-list:
	block-item
	block-item-list block-item

block-item:
	declaration
	statement

expression-statement:
	expression_opt ;

selection-statement:
	if ( expression ) statement
	if ( expression ) statement else statement

iteration-statement:
	while ( expression ) statement
	do statement while ( expression ) ;
	for ( expression_opt ; expression_opt ; expression_opt ) statement

jump-statement:
	goto identifier ;
	continue ;
	break ;
	return expression_opt ;
```

### External definitions

```
translation-unit:
	external-declaration
	translation-unit external-declaration

external-declaration:
	function-definition
	declaration

function-definition:
	declaration-specifiers declarator compound-statement

```