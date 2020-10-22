# CMakeList.txt 
#

SOLUTION_LIBRARY("solution" "framework")

SOLUTION_LIBRARY("solution_dump" "framework_dump")

target_include_directories("solution" PUBLIC ".")

target_include_directories("solution_dump" PUBLIC ".")

FLEX_BISON_SOURCE("solution" "calexer.lex" "caparser.y" "calexer.cpp" "caparser.cpp" "caparser.hpp")

FLEX_SOURCE("solution_dump" "calexer.lex" "calexer_dump.cpp" "ckdumper.hpp")

target_sources("solution" PUBLIC "casem.hpp" "casem.cpp")

target_sources("solution_dump" PUBLIC "casem.hpp" "casem.cpp")

