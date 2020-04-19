;--------------------------------
; Headers

!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "FileFunc.nsh"

;--------------------------------
; Defintions

; Images
!define MUI_WELCOMEFINISHPAGE_BITMAP 	"InstallerBanner.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP 	"InstallerBanner.bmp"

; Metadata (Edit these Freely)
!define APP_NAME 				"Maniac Editor - Generations Edition"
!define COMP_NAME 				"CarJem Generations"
!define WEB_SITE 				"https://twitter.com/carter5467_99"
!define COPYRIGHT 				"CarJem Generations © 2020"
!define DESCRIPTION 			"Application"
!define VERSION 				"1.00.00.00"

; File Paths/Names (Edit these Freely)
!define INSTALLER_DEST_FILE 	"Setup.exe"
!define INSTALLER_DEST_DIR 		"."
!define INSTALLER_DEST 			"${INSTALLER_DEST_DIR}\${INSTALLER_DEST_FILE}"
!define DEFAULT_NORMAL_DESTINATON "$PROGRAMFILES\${INSTALL_DIR_NAME}"
!define DEFAULT_PORTABLE_DESTINATON "$EXEPATH\${INSTALL_DIR_NAME}"
!define INSTALL_DIR_NAME 		"ManiacEditor"
!define MAIN_APP_EXE 			"ManiacEditor.exe"
!define MAKE_DIRECTORY			"D:\Users\CarJem\source\rsdk_repos\ManiacEditor-GenerationsEdition\ManiacEditor\bin\Release\*.*"
!define STARTMENU_DIR_NAME		"ManiacEditor"
!define APPDATA_DIRECTORY		"ManiacEditor"

Var NormalDestDir
Var PortableDestDir
Var PortableMode

; Section Descriptors

!define DESC_Section1 "Maniac Editor's Core and Essential Files"
!define DESC_Section2 "Install Maniac Editor with Shortcuts and an Uninstaller Linked to the System"
!define DESC_Section3 "Extract Maniac Editor to a Folder for More Lightweight use"
!define DESC_Section4 "Clear Maniac Editor's App Data Folder and All of it's Data"
!define DESC_Section5 "Shortcuts Originally Created on Install"


; Misc Installer Paths/Defintions
!define REG_APP_PATH 			"Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
!define UNINSTALL_PATH 			"Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"
!define INSTALL_TYPE 			"SetShellVarContext current"
!define REG_ROOT 				"HKCU"

; Version Keys
VIProductVersion 				"${VERSION}"
VIAddVersionKey 				"ProductName"  		"${APP_NAME}"
VIAddVersionKey 				"CompanyName"  		"${COMP_NAME}"
VIAddVersionKey 				"LegalCopyright"  	"${COPYRIGHT}"
VIAddVersionKey 				"FileDescription"  	"${DESCRIPTION}"
VIAddVersionKey 				"FileVersion"  		"${VERSION}"

; Interface Settings
!define MUI_ABORTWARNING

; Installer Declarations
SetCompressor 		ZLIB
Name 				"${APP_NAME}"
Caption 			"${APP_NAME}"
OutFile 			"${INSTALLER_DEST}"
BrandingText 		"${APP_NAME}"
XPStyle 			on
InstallDirRegKey 	"${REG_ROOT}" "${REG_APP_PATH}" ""
InstallDir 			"$PROGRAMFILES\${INSTALL_DIR_NAME}"

;--------------------------------
; Pages

; Installer Pages ;---------

!insertmacro MUI_PAGE_WELCOME
Page Custom PortableModePageCreate PortableModePageLeave
!insertmacro MUI_PAGE_DIRECTORY

!ifdef REG_START_MENU
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "${STARTMENU_DIR_NAME}"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY 	"${UNINSTALL_PATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
!insertmacro MUI_PAGE_STARTMENU Application $SM_Folder
!endif
!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_RUN "$INSTDIR\${MAIN_APP_EXE}"
!insertmacro MUI_PAGE_FINISH

;---------


; Uninstaller Pages ;---------

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_COMPONENTS
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;---------

;--------------------------------
; Languages

!insertmacro MUI_LANGUAGE "English"

;--------------------------------
; Install Section

Section "Essential Files" ESSENTIAL_FILES
SectionIn RO
SetOverwrite ifnewer
SetOutPath "$INSTDIR"
File /r "${MAKE_DIRECTORY}"


${If} $PortableMode = 0
    WriteUninstaller "$INSTDIR\uninstall.exe"
    CreateDirectory "$SMPROGRAMS\${STARTMENU_DIR_NAME}"
    CreateShortCut "$SMPROGRAMS\${STARTMENU_DIR_NAME}\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    CreateShortCut "$SMPROGRAMS\${STARTMENU_DIR_NAME}\Uninstall ${APP_NAME}.lnk" "$INSTDIR\uninstall.exe"
${Else}
    CreateDirectory "$INSTDIR\Settings"
    FileOpen $0 "$INSTDIR\Settings\internal_switches.json" w
    FileWrite $0 "{$\"PortableMode$\":true}"
    FileClose $0
${EndIf}

SectionEnd


;--------------------------------
; Uninstall Section

Section "Un.Essential Files" UN_ESSENTIAL_FILES
SectionIn RO
Delete "$SMPROGRAMS\${STARTMENU_DIR_NAME}\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\${STARTMENU_DIR_NAME}\Uninstall ${APP_NAME}.lnk"
Delete "$DESKTOP\${APP_NAME}.lnk"
RmDir "$SMPROGRAMS\${STARTMENU_DIR_NAME}"
DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"
RmDir /r "$INSTDIR"
SectionEnd

Section /o "Un.Application Data" UN_APP_DATA
RmDir /r "$APPDATA\${APPDATA_DIRECTORY}"
SectionEnd

;--------------------------------
; Descriptions

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${ESSENTIAL_FILES} "${DESC_Section1}"
!insertmacro MUI_DESCRIPTION_TEXT ${STANDARD_INSTALL} "${DESC_Section2}"
!insertmacro MUI_DESCRIPTION_TEXT ${PORTABLE_INSTALL} "${DESC_Section3}"
!insertmacro MUI_FUNCTION_DESCRIPTION_END

!insertmacro MUI_UNFUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${UN_ESSENTIAL_FILES} "${DESC_Section1}"
!insertmacro MUI_DESCRIPTION_TEXT ${UN_APP_DATA} "${DESC_Section4}"
!insertmacro MUI_DESCRIPTION_TEXT ${UN_SHORTCUTS} "${DESC_Section5}"
!insertmacro MUI_UNFUNCTION_DESCRIPTION_END

;--------------------------------
; Portable Mode Selection Page

Function SetModeDestinationFromInstdir
${If} $PortableMode = 0
    StrCpy $NormalDestDir $InstDir
${Else}
    StrCpy $PortableDestDir $InstDir
${EndIf}
FunctionEnd

Function PortableModePageCreate
Call SetModeDestinationFromInstdir ; If the user clicks BACK on the directory page we will remember their mode specific directory
!insertmacro MUI_HEADER_TEXT "Install Mode" "Choose how you want to install ${APP_NAME}."
nsDialogs::Create 1018
Pop $0
${NSD_CreateLabel} 0 10u 100% 24u "Select install mode:"
Pop $0
${NSD_CreateRadioButton} 30u 50u -30u 8u "Normal install"
Pop $1
${NSD_CreateRadioButton} 30u 70u -30u 8u "Portable"
Pop $2
${If} $PortableMode = 0
    SendMessage $1 ${BM_SETCHECK} ${BST_CHECKED} 0
${Else}
    SendMessage $2 ${BM_SETCHECK} ${BST_CHECKED} 0
${EndIf}
nsDialogs::Show
FunctionEnd

Function PortableModePageLeave
${NSD_GetState} $1 $0
${If} $0 <> ${BST_UNCHECKED}
    StrCpy $PortableMode 0
    StrCpy $InstDir $NormalDestDir
${Else}
    StrCpy $PortableMode 1
    StrCpy $InstDir $PortableDestDir
${EndIf}
FunctionEnd

;--------------------------------