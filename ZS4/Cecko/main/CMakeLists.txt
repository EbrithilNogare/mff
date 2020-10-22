
MAKE_TARGET_DUMP("A1" "cecko1")
MAKE_TARGET("A2" "cecko2")
MAKE_TARGET("A3" "cecko3")
MAKE_TARGET("A4" "cecko4")
MAKE_TARGET("A5" "cecko5")

target_sources(${TARGET_A1} PUBLIC "cecko1.cpp")
target_sources(${TARGET_A2} PUBLIC "cecko2.cpp")
target_sources(${TARGET_A3} PUBLIC "cecko3.cpp")
target_sources(${TARGET_A4} PUBLIC "cecko4.cpp")
target_sources(${TARGET_A5} PUBLIC "cecko5.cpp")
