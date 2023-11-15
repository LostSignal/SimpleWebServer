
@ECHO OFF

CD SimpleWebServer

:: Performing the Publish
SET CONFIG=Release
SET RUNTIME=win-x64

dotnet publish ^
  -r %RUNTIME% ^
  -c %CONFIG% ^
  -p:PublishAOT=true ^
  -p:InvariantGlobalization=true ^
  -p:IlcGenerateStackTraceData=false ^
  -p:IlcOptimizationPreference=Size ^
  -p:DebugType=none ^
  -p:IlcFoldIdenticalMethodBodies=true ^
  -p:IlcTrimMetadata=true  

:: Removing unecessary files and copying the output to the root folder
DEL  .\bin\%CONFIG%\net8.0\%RUNTIME%\publish\*.dll
MOVE .\bin\%CONFIG%\net8.0\%RUNTIME%\publish\SimpleWebServer.exe ..\SWS.exe

PAUSE
