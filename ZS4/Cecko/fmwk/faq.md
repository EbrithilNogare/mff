@page faq FAQ

### I assume at this stage we are constructing a syntax tree? At least part of it.

No. There is no need to build a syntax tree. Bison will perform a bottom-up post-order traversal through a virtual tree and it will execute a fragment of your C++ code when exiting every subtree.

Since your editing environment does not recognize `.y` files as C++ sources, it is a good idea to keep the fragments minimal, like 
\code{.cpp}
{ $$ = rule_something( ctx, $1, $3, @2); }
\endcode
and implement the `rule_something` functions in `casem.*`.

So the first part is that you need to understand how the `$$`, `$i` and `@i` work, including setting their types. A short description is in the slides, details are in the bison manual.

### I assume that @n will return loc_t?

Yes, `@i` is of type `loc_t`. `loc_t` is just an `int`. It is defined in the framework and told bison via some `%%define` declaration in `.y`

### But if we don't build the tree itself, then are we also supposed to later type check and translate in these fragments? I assumed we are recording this tree and then in a separate run we are type checking it and then in a separate run we are translating it.

No. There is no separate run. Directly when the source is parsed, bison calls your C++ fragments and you call framework functions which create some internal representation of the types and variables in the ctx. There will be no physical tree in our code. We are building a single-pass compiler (which generates the LLVM IR).

With some dirty tricks, you can implement the compiler inside these C++ fragments. For Assignment 3, the only really dirty tricks are associated with the IDF/TYPEIDF distinction in the lexer which you already did in Asgn 2, and the related warnings (in the slides) on the effect of look-ahead in bison.

### Is it better to start writing from the top symbol, for example from declaration?

Yes, that might be a good approach. It is a good idea to rearrange the rules just under `declaration` so that you have a nice place where one `init-declarator` meets with `declaration-specifiers`. Then, you will write a C++ fragment into this place to generate the variable/function/typedef.

Of course, your code will need some inputs from the underlying syntactic parts - this will tell you what data you need from the parts, i.e. the types for `%type` (you will need to define some of them in `casem.hpp`, the framework types are not sufficient).

To start with Assignment 3, you will probably need to create your own test input, starting from the simplest things like 
\code{.cpp}
int x;
\endcode
For this case, your job is essentially to call these framework functions:
\code{.cpp}
loc_t loc = /* actual line number where the token "x" is located */;
CKTypeSafeObs tp = ctx->get_int_type();
ctx->define_var("x", CKTypeRefPack(tp, false), loc);
\endcode

Of course, you can't do it in a single code fragment - you have to split it into several pieces, capable of handling much more complex cases.

The `ctx->get_int_type()` call will neatly fit into the rule `type-specifier: int` (it will however be slightly different in your grammar due to grouped tokens).

The `ctx->define_var()` call must be located at a place where both the `declaration-specifiers` (which tell you the base type) and `init-declarator` (which contains the variable name and additional type constructions) are available. For this, it is a good idea to regroup the grammar slightly (as shown in the slides) to let the two pieces of information meet already when one `init-declarator` is encountered (i.e. not above the `init-declarator-list_opt`).

The most complex part of Assignment 3 is dealing with the declarators. They are structured in exactly the opposite way than you need. So, for this part, you will need a temporary structure which will somehow hold the declarators (i.e. pointer/array/function constructs) together with the identifier placed inside. Above the declarators (when you finally meet the information on the base type from the `declaration-specifiers`, you will have to execute the correct framework functions (in the correct order) to create the pointer/array/function types from the base type.

Example:
`const int * p[10]` is syntactically grouped as `const int (* (p[10]))`. The meaning is "p is an array of pointers to const int". Therefore, you have to call this:
\code{.cpp}
CKTypeSafeObs tp1 = ctx->get_int_type();
bool is_const1 = true;
CKTypeSafeObs tp2 = ctx->get_pointer_type(CKTypeRefPack(tp1, is_const1));
bool is_const2 = false; // the pointer is not const (but points to const)
CKTypeSafeObs tp3 = ctx->get_array_type(tp2,ctx->get_int32_constant(10));
bool is_const3 = is_const2; // const element implies const array
ctx->define_var("x", CKTypeRefPack(tp3, is_const3), loc);
\endcode
Your job is to arrange a temporary structure which will allow you to execute the sequence of `get_pointer_type` and `get_array_type` in the right moment (when you have the `tp1` at hand).
In addition, if the last type (`tp3`) were a function, you have to call `declare_function` instead of `define_var`. And if there were a `typedef` keyword, you call `define_typedef` instead.

### What if declaration-specifiers contain conflicting or duplicate specifiers or qualifiers? E.g. const const int i; bool int i;.

It is your responsibility to report `errors::INVALID_SPECIFIERS` in cases like these: 
\code{.cpp}
FILE int v1;
char int v2;
const v3;
typedef T4;
typedef int f5() { /.../ }
int f6(typedef int x);
\endcode

On the other hand, double `const` like in 
\code{.cpp}
const const int v7;
typedef const int T8; 
const T8 v9;
\endcode
is not considered an error in Cecko (although `v7` would be an error in C) - it is silently collapsed into one `const` flag.

### Is a declaration like int func(bool b)[]; correct? It is allowed by the grammar but it results in returning an array by value.

It is not correct, a function must not return an array by value (it is one of the reasons for the existence of `std::array` in C++). It is however impossible to elliminate this case syntactically because it could sneak in via a TYPEIDF.

In our framework, the functions `get_array_type` and  `get_function_type` return `nullptr` when the requested type would be invalid. It is your responsibility to test the return value and report `errors::INVALID_ARRAY_TYPE` or `errors::INVALID_FUNCTION_TYPE` when `nullptr`. 

On the other hand, the function `declare_function` tests its `CKTypeObs` argument for correctness - if it is not a function type (like in `int f {/.../}`), `declare_function` itself will report an error (`INVALID_FUNCTION_TYPE`).

### Is it possible that the return type of a function is declared const? If so, there is no place to store the flag in the framework.

The `const` modifier at function return is allowed in both syntax and semantics, it is however ignored (because a function call in C is never an L-value and thus could not be modified anyhow). Therefore, our framework does not store the flag.

### Is it possible to define a struct, enum or typedef inside function arguments?

The definition of `struct`/`enum` may be placed within any declaration (in C, it is prohibited only inside cast and `sizeof` expressions). 

On the other hand, `typedef` must not appear in a formal argument (although allowed by the grammar od C due to being in the same syntactic category as the archaic `register` keyword). You may solve it either by removing the `typedef` from the grammar rules for the arguments (i.e. producing a syntax error in the malformed case) or by semantic check leading to `errors::INVALID_SPECIFIERS`.

### What does "CKTypeRefPack" represent?

Example:

\code{.cpp}
typedef int T1;
typedef const int T2;
typedef const T1 T3[10];
typedef const T2 T4[10];
typedef T2 T5[10];
\endcode
In this case, the three typedefs T3, T4, T5 are by definition identical, i.e. it does not matter where exactly the const was added (and the double const in T4 is silently merged).
A related case is in the definition of a function type:
\code{.cpp}
typedef int TF1(int, int);
typedef int TF2(const int, const int);
\endcode
These two typedefs are by definition identical (the const flag on a parameter is simply ignored because it does not affect the caller).
Therefore, it is easier to handle the type descriptors independently of the const flag. The only case where the const flag is a part of a type descriptor is a pointer, i.e. `const X*` is different from `X*`.
A typedef stores a pair of a type descriptor and a const flag, which is exactly the contents of the structure `CKTypeRefPack`. And you may also use the structure in your code, for instance when representing a type specifier list like `T2` or `const int`.

### Is "IRBuilderBase::getCurrentFunctionReturnType" the correct way to retrieve the return type of the actual function?

No, it only returns the LLVM IR type descriptor which is insufficient to correctly check the type rules of Cecko.
`CKContext::current_function_return_type` returns the correct type descriptor.

### What is the underlying type for enums in Cecko?

`int`. The behavior of Cecko is slightly different from the definition of (modern) C. It is wired into the framework because `get_ir` on an enum descriptor returns the LLVM IR descriptor for `i32`, simply because LLVM IR does not recognize enums. Furthermore, `get_type` on enum constants returns the descriptor for `int`, i.e. not their respective enum descriptor. Since `int` and enums are implicitly convertible, there is no consequence of this behavior. (On the other hand, the pointer to `int` is different from the pointer to an enum; therefore, variables must correctly keep their enum type because pointers may point at them.)

### How can I call "enter_block()" at the beginning of the compound_statment? 

You have to rewrite the grammar slightly so that you can call enter_block before or immediately after LCUR.

### What is the best way to handle the recursive structure of direct_declarator? Is it better to transform the grammar somehow or to create a temporary data structure representing the actual declarator?

Rewriting the grammar does not help. Logically, you need a list of objects of three different types (representing the array/pointer/function declarators); you will later have to traverse the list in the opposite order with respect to the order of construction. It may be represented by a linked list or a container; in any case, you need to solve the variability of the types. There are three options in C++: Inheritance (the cleanest but the most work-intensive and also the slowest), `std::variant` or just a struct containing all the required data elements (always leaving some of them unused). In addition, functional-programming gurus may use `std::function` and lambdas (creating, in fact, a linked list of functor objects).

Furtheremore, there is (optionally) an identifier buried inside the declarator. It may be represented as a fourth type of objects in the list (providing an ellegant terminator if implemented by a linked list). However, passing the identifier (and its line number) besides the list is probably simpler. In any case, the `%type` for a declarator will be the most complex temporary data structure needed in the semantic analysis of Cecko.

### How to retrieve the enum/struct descriptor for a previously declared enum/struct? 

Identifiers occuring after the keyword `struct` or `enum` belong to a special category of identifiers, called _Tags_. The functions `find` and `find_typedef` work with other categories and, thus, they are not usable to find the meaning of a Tag.
Furthermore, every presence of `struct` or `enum` may be a declaration of the Tag. Therefore, the functions `declare_struct_type` and `declare_enum_type` are the only way how to retrieve a struct/enum descriptor - internally, the functions will decide whether to return a previously created descriptor or a new one.

### How to retrieve the "CKFunctionObs" needed for "enter_function()" when the function was already declared? 

The correct way is to call `declare_function` (again) - it will not report any error for duplicite declaration if the type is identical to the previous declaration. 

### How to represent declarations like "int f(void)"?

The `void` is only a syntactic matter (due to the distant history of C), no parameter representation shall be created for the `void`. The case of a single `void` argument must be specifically detected (in the semantics; handling by grammar adjustment would be too difficult) and `get_function_type` must be called with an empty list of arguments in this case.

### "declare_struct_type" returns the same descriptor pointer for both the global and the local "struct" in this example:
\code{.cpp}
struct mighty_str;
int main(int argc, char** argv)
{
	struct mighty_str;
	return 0;
}
\endcode

`struct X;` shall create a new struct descriptor only if no previous declaration is in sight. Because of this, you see the same pointer.
On the other hand, `struct X {};` must always create a new descriptor - therefore, you have to call `define_struct_type_open` instead of `declare_struct_type` in this case.

Note that, for simplicity, Cecko rules are slightly different from C: In C, `struct X a;` behaves differently from `struct X;` - the former enforces a new declaration if none is visible while the latter is a declaration if none is in the same scope. In Cecko, the latter case has no special handling.


### What is the required contents of "CKFunctionFormalPackArray" in the call to "enter_function()"? Is it possible to retrieve it from "CKFunctionSafeObs"?

`CKFunctionFormalPackArray` represent exactly the information contained in the function header which is not (by definition) the part if the function type and, therefore, it is not stored in `CKFunctionSafeObs`. You have to collect this information on your own, i.e. to split the information in a parameter list into two groups - parameter types and the rest (const flags and identifiers, if any). The rest goes into the `CKFunctionFormalPackArray`. If the function declarator is not a part of function definition, the rest is silently ignored.

### How to represent the "char" in "int(*pfix)(char)"?

Notice that this a pointer to a function that returns `int` and accepts `char` as an argument. So what you need to do:
1. process function parameters (in this case it is `char`) to list of arguments (`CKTypeObsArray`)
2. call `get_function_type` with return type as `int`, arguments being the list you got from 1, and `variadic` being false.
3. wrap the return value from 2 into `CKTypeRefPack`.
4. pass the pack from 3 to `get_pointer_type`.
5. Wrap what you got from 4 into a `CKTypeRefPack`.
6. Then you can `define_var` with the name `pfix` and the pack that you got from 5.

### How to access struct members when we only have its value (R-value) and not its address (L-value)?

Call `CreateExtractValue` instead of `CreateStructGEP`. The former converts a value to a value, the latter a pointer to a pointer.
Note that the former is needed only in cases like `f().m`.

### What is the result type of (==, !=, <, <=, >, >=, &&, ||, !) - "int" or "_Bool"?

With respect to the language behavior, the question has no real meaning because the results are R-values and there are consistent implicit conversions between `int` and `_Bool` in both directions. In other words, you cannot determine which type the compiler has selected from the behavior of the resulting program.

The pragmatic choice is `_Bool` because the `ICmp` instructions return `i1` which is the LLVM IR representation of Cecko `_Bool`. Choosing `int` would require additional (forth and back) conversion instructions in cases like `_Bool x; x = a < b;`.

### What is the correct sequence of builder calls to create a string literal representation and to cast it to a pointer to char? CreateConstInBoundsGEP2_32 keeps asserting for every type I supplied.

`CreateGlobalString` returns the IR representation of a pointer to a string literal; however, its IR type is `char (*)[N+1]`. The semantics of `CreateConstInBoundsGEP2_32` (with two zeros) is `&p[0][0]`; therefore, it converts the IR type `char (*)[n]` to `char *`. For unknown reasons, the first argument to it shall be the IR type descriptor for `char[n]`, i.e. the type in between the two `[0]` operators.

The two builder calls will be separated in the compiler: The `STRLIT` will produce an L-value expression of the (Cecko) type `const char [N+1]` whose address is the result of `CreateGlobalString`, having the IR type `char (*)[N+1]`. The second step is a more general implicit conversion of the L-value of type `T[n]` to an R-value of type `T*`, which is done by the `CreateConstInBoundsGEP2_32` call. The conversion is a part of the next operation above the `STRLIT` (except of the case of &"ABC", that produces an R-value of (Cecko) type `const char (*)[N+1]`, i.e. the `&` operator does nothing in the IR).

### Is it true that an array can be assigned only an array of the same type and size, and only when passing function arguments?

No, assignment of arrays (when not packed into a struct) the C does not support anywhere. `get_function_type` will refuse to create a function with an argument of an array type (there is a small difference between Cecko and C - in C, the compiler automatically converts all array-typed formal arguments into pointers, e.g. `char *argv[]` is equivalent to `char **argv`).
If the actual argument is an array, it will always trigger the implicit conversion to a pointer.

### Is it possible to assign structs directly or is is necessary to iterate through every field? 

The IR instructions `Load`, `Store`, `Ret` as well as the arguments of `Call` can copy any IR type including structures (they can even copy arrays, only the rules of C prohibit that). No iteration through fields in your part of compiler is required (and there is no support for it in the framework).

