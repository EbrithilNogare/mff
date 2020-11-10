
add_library("framework")
COMMON_OPTIONS("framework")

add_library("framework_dump")
COMMON_OPTIONS("framework_dump")

target_include_directories("framework" PUBLIC ".")
target_include_directories("framework_dump" PUBLIC ".")

BISON_SOURCE("framework_dump" "ckdumper.y" "ckdumper.cpp" "ckdumper.hpp")

target_sources("framework" PRIVATE "ckir.cpp" "cktables.cpp" "ckcontext.cpp" "ckmain.cpp")
target_sources("framework" PUBLIC "ckbisonflex.hpp" "ckir.hpp" "cktables.hpp" "ckcontext.hpp" "ckgrptokens.hpp" "ckmain.hpp")

target_sources("framework_dump" PRIVATE "ckir.cpp" "cktables.cpp" "ckcontext.cpp" "ckmain.cpp")
target_sources("framework_dump" PUBLIC "ckbisonflex.hpp" "ckir.hpp" "cktables.hpp" "ckcontext.hpp" "ckgrptokens.hpp" "ckmain.hpp")

DOXYGEN("doxy" "ckbisonflex.hpp" "ckir.hpp" "cktables.hpp" "ckcontext.hpp" "ckgrptokens.hpp" "ckmain.hpp")

