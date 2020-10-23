
MAKE_TARGET_DUMP("A1" "${SOL_PREFIX}cecko1")
MAKE_TARGET("A2" "${SOL_PREFIX}cecko2")
MAKE_TARGET("A3" "${SOL_PREFIX}cecko3")
MAKE_TARGET("A4" "${SOL_PREFIX}cecko4")
MAKE_TARGET("A5" "${SOL_PREFIX}cecko5")

target_sources(${TARGET_A1} PUBLIC "cecko1.cpp")
target_sources(${TARGET_A2} PUBLIC "cecko2.cpp")
target_sources(${TARGET_A3} PUBLIC "cecko3.cpp")
target_sources(${TARGET_A4} PUBLIC "cecko4.cpp")
target_sources(${TARGET_A5} PUBLIC "cecko5.cpp")
