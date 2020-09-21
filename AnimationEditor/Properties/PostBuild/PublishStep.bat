@ECHO OFF
SET ConfigurationName=%1
SET SolutionDir=%~2
SET TargetDir=%~3
SET ProjectDir=%~4

SET CurrentDir=%~dp0
SET AssistantPath="D:\Users\CarJem\source\personal_repos\GenerationsLib\GenerationsLib.UpdateAssistant\bin\x86\Release\GenerationsLib.UpdateAssistant.exe"

:: Generate Installer and Prepare to Publish
if %ConfigurationName% == "Publish" (
	call "%CurrentDir%\MakeInstaller.bat" %ProjectDir%
	call "%CurrentDir%\MakeZIP.bat" "%TargetDir%" "%CurrentDir%Build.zip"
	call "%AssistantPath%" "ManiacEditor"
)

:: Generate a ZIP for a Experimental Testing Build
if %ConfigurationName% == "Experiment" (
	mkdir %SettingsPath%
	call "%CurrentDir%\MakeZIP.bat" "%TargetDir%" "%CurrentDir%Build.zip
	call "%CurrentDir%"
)