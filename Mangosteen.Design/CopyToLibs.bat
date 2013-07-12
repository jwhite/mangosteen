@echo ON
:%1 = ProjectDir
:%2 = Outdir
echo ProjectDir = %1
echo OutDir = %2

set EXTENSIONDIR=%~1libs\Windows\v8.0\ExtensionSDKs\

echo ExtensionDir = %EXTENSIONDIR%
set CONTROLSDIR=%EXTENSIONDIR%MangosteenSDK\1.0\

echo ControlsDir = %CONTROLSDIR%

copy "%~1SDKManifest.xml" "%CONTROLSDIR%"
copy "%~2Mangosteen.pri" "%CONTROLSDIR%\Redist\CommonConfiguration\neutral\"

copy "%~2themes\generic.xaml" "%CONTROLSDIR%\Redist\CommonConfiguration\neutral\themes\"
copy "%~2Mangosteen.dll" "%CONTROLSDIR%\References\CommonConfiguration\neutral\"
copy "%~2Mangosteen.Design.dll" "%CONTROLSDIR%\DesignTime\CommonConfiguration\neutral\"