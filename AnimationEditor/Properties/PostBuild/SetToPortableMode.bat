@ECHO OFF
set Input=%1

echo {"PortableMode":true} > "%Input%\Settings\internal_switches.json"