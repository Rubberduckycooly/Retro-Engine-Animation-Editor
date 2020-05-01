@echo off
set ConfigurationName=%1
set SolutionDir=%~2
set TargetDir=%~3
set ProjectDir=%~4

if %ConfigurationName% == "Publish" (
	call "%SolutionDir%Properties\Versioning\GenerationsLib.Versioning.exe" "%SolutionDir%Properties\AssemblyInfo.cs" "RSDK Animation Editor" true true
)