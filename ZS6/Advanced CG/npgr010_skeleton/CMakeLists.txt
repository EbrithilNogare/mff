cmake_minimum_required(VERSION 3.10)

project(NPGR010)

add_executable(NPGR010 src/pg3render.cpp)

target_compile_features(NPGR010 PRIVATE cxx_std_17)

find_package(OpenMP)
if(OpenMP_CXX_FOUND)
    target_link_libraries(NPGR010 PUBLIC OpenMP::OpenMP_CXX)
endif()