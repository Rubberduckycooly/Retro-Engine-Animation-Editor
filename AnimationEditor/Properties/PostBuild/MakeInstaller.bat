@ECHO OFF
SET ProjectDir=%~1

SET ScriptLocation="%ProjectDir%\Properties\PostBuild\Installer\Script.nsi"
SET NISIWLocation="C:\Program Files (x86)\NSIS\makensisw.exe"

%NISIWLocation% /V1 %ScriptLocation%