# This file will be configured to contain variables for CPack. These variables
# should be set in the CMake list file of the project before CPack module is
# included. The list of available CPACK_xxx variables and their associated
# documentation may be obtained using
#  cpack --help-variable-list
#
# Some variables are common to all generators (e.g. CPACK_PACKAGE_NAME)
# and some are specific to a generator
# (e.g. CPACK_NSIS_EXTRA_INSTALL_COMMANDS). The generator specific variables
# usually begin with CPACK_<GENNAME>_xxxx.


set(CPACK_BUILD_SOURCE_DIRS "C:/Users/Nogare/Downloads/ncine/ncPong;C:/Users/Nogare/Downloads/ncine/ncPong-build")
set(CPACK_CMAKE_GENERATOR "Visual Studio 16 2019")
set(CPACK_COMPONENT_UNSPECIFIED_HIDDEN "TRUE")
set(CPACK_COMPONENT_UNSPECIFIED_REQUIRED "TRUE")
set(CPACK_DEFAULT_PACKAGE_DESCRIPTION_FILE "C:/Program Files/CMake/share/cmake-3.22/Templates/CPack.GenericDescription.txt")
set(CPACK_DEFAULT_PACKAGE_DESCRIPTION_SUMMARY "ncPong built using CMake")
set(CPACK_GENERATOR "7Z;ZIP")
set(CPACK_IGNORE_FILES "/CVS/;/\\.svn/;/\\.bzr/;/\\.hg/;/\\.git/;\\.swp\$;\\.#;/#")
set(CPACK_INSTALLED_DIRECTORIES "C:/Users/Nogare/Downloads/ncine/ncPong;/")
set(CPACK_INSTALL_CMAKE_PROJECTS "")
set(CPACK_INSTALL_PREFIX "C:/Program Files (x86)/nCine")
set(CPACK_MODULE_PATH "C:/Users/Nogare/Downloads/ncine/ncPong/cmake;C:/Program Files (x86)/nCine/project/cmake")
set(CPACK_NCPROJECT_NAME "ncPong")
set(CPACK_NSIS_COMPRESSOR "/SOLID lzma")
set(CPACK_NSIS_CREATE_ICONS_EXTRA "SetOutPath '$INSTDIR\\bin'
		CreateShortCut '$SMPROGRAMS\\$STARTMENU_FOLDER\\ncPong.lnk' '$INSTDIR\\bin\\ncpong.exe'
		CreateShortCut '$DESKTOP\\ncPong.lnk' '$INSTDIR\\bin\\ncpong.exe'
		SetOutPath '$INSTDIR'")
set(CPACK_NSIS_DELETE_ICONS_EXTRA "Delete '$SMPROGRAMS\\$MUI_TEMP\\ncPong.lnk'
		Delete '$DESKTOP\\ncPong.lnk'")
set(CPACK_NSIS_DISPLAY_NAME "ncPong")
set(CPACK_NSIS_INSTALLER_ICON_CODE "")
set(CPACK_NSIS_INSTALLER_MUI_ICON_CODE "")
set(CPACK_NSIS_INSTALL_ROOT "$PROGRAMFILES64")
set(CPACK_NSIS_MUI_ICON "C:/Users/Nogare/Downloads/ncine/ncPong-data/icons/icon.ico")
set(CPACK_NSIS_PACKAGE_NAME "ncPong")
set(CPACK_NSIS_UNINSTALL_NAME "Uninstall")
set(CPACK_OUTPUT_CONFIG_FILE "C:/Users/Nogare/Downloads/ncine/ncPong-build/CPackConfig.cmake")
set(CPACK_PACKAGE_CHECKSUM "MD5")
set(CPACK_PACKAGE_DEFAULT_LOCATION "/")
set(CPACK_PACKAGE_DESCRIPTION "An example game made with the nCine")
set(CPACK_PACKAGE_DESCRIPTION_FILE "C:/Program Files/CMake/share/cmake-3.22/Templates/CPack.GenericDescription.txt")
set(CPACK_PACKAGE_DESCRIPTION_SUMMARY "ncPong built using CMake")
set(CPACK_PACKAGE_FILE_NAME "ncPong-2021.10.18-Source")
set(CPACK_PACKAGE_HOMEPAGE_URL "https://ncine.github.io")
set(CPACK_PACKAGE_INSTALL_DIRECTORY "ncPong")
set(CPACK_PACKAGE_INSTALL_REGISTRY_KEY "ncPong")
set(CPACK_PACKAGE_NAME "ncPong")
set(CPACK_PACKAGE_RELOCATABLE "true")
set(CPACK_PACKAGE_VENDOR "Angelo Theodorou")
set(CPACK_PACKAGE_VERSION "2021.10.18")
set(CPACK_PACKAGE_VERSION_MAJOR "2021")
set(CPACK_PACKAGE_VERSION_MINOR "10")
set(CPACK_PACKAGE_VERSION_PATCH "18")
set(CPACK_RESOURCE_FILE_LICENSE "C:/Users/Nogare/Downloads/ncine/ncPong/LICENSE")
set(CPACK_RESOURCE_FILE_README "C:/Program Files/CMake/share/cmake-3.22/Templates/CPack.GenericDescription.txt")
set(CPACK_RESOURCE_FILE_WELCOME "C:/Program Files/CMake/share/cmake-3.22/Templates/CPack.GenericWelcome.txt")
set(CPACK_RPM_PACKAGE_SOURCES "ON")
set(CPACK_SET_DESTDIR "OFF")
set(CPACK_SOURCE_7Z "ON")
set(CPACK_SOURCE_GENERATOR "7Z;ZIP")
set(CPACK_SOURCE_IGNORE_FILES "/CVS/;/\\.svn/;/\\.bzr/;/\\.hg/;/\\.git/;\\.swp\$;\\.#;/#")
set(CPACK_SOURCE_INSTALLED_DIRECTORIES "C:/Users/Nogare/Downloads/ncine/ncPong;/")
set(CPACK_SOURCE_OUTPUT_CONFIG_FILE "C:/Users/Nogare/Downloads/ncine/ncPong-build/CPackSourceConfig.cmake")
set(CPACK_SOURCE_PACKAGE_FILE_NAME "ncPong-2021.10.18-Source")
set(CPACK_SOURCE_TOPLEVEL_TAG "win64-Source")
set(CPACK_SOURCE_ZIP "ON")
set(CPACK_STRIP_FILES "")
set(CPACK_SYSTEM_NAME "win64")
set(CPACK_THREADS "1")
set(CPACK_TOPLEVEL_TAG "win64-Source")
set(CPACK_WIX_SIZEOF_VOID_P "8")

if(NOT CPACK_PROPERTIES_FILE)
  set(CPACK_PROPERTIES_FILE "C:/Users/Nogare/Downloads/ncine/ncPong-build/CPackProperties.cmake")
endif()

if(EXISTS ${CPACK_PROPERTIES_FILE})
  include(${CPACK_PROPERTIES_FILE})
endif()
