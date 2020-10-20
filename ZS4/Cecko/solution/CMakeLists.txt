# CMakeList.txt 
#

SOLUTION_LIBRARY("${SOL_PREFIX}solution" "framework")

SOLUTION_LIBRARY("${SOL_PREFIX}solution_dump" "framework_dump")

target_include_directories("${SOL_PREFIX}solution" PUBLIC ".")

target_include_directories("${SOL_PREFIX}solution_dump" PUBLIC ".")

FLEX_BISON_SOURCE("${SOL_PREFIX}solution" "calexer.lex" "caparser.y" "calexer.cpp" "caparser.cpp" "caparser.hpp")

FLEX_SOURCE("${SOL_PREFIX}solution_dump" "calexer.lex" "calexer_dump.cpp" "ckdumper.hpp")

target_sources("${SOL_PREFIX}solution" PUBLIC "casem.hpp")
target_sources("${SOL_PREFIX}solution" PRIVATE "casem.cpp")

target_sources("${SOL_PREFIX}solution_dump" PUBLIC "casem.hpp")
target_sources("${SOL_PREFIX}solution_dump" PRIVATE "casem.cpp")

