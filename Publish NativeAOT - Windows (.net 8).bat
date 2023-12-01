
CD SimpleWebServer

:: Performing the Publish
SET CONFIG=Release
SET FRAMEWORK=net8.0
SET RUNTIME=win-x64

dotnet publish ^
  -c %CONFIG% ^
  -f %FRAMEWORK% ^
  -r %RUNTIME% ^
  -p:PublishAOT=true ^
  -p:InvariantGlobalization=true ^
  -p:IlcGenerateStackTraceData=false ^
  -p:IlcOptimizationPreference=Size ^
  -p:DebugType=none ^
  -p:IlcFoldIdenticalMethodBodies=true ^
  -p:IlcTrimMetadata=true  

PAUSE
