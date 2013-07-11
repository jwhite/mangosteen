:%1 = ProjectDir
:%2 = Outdir
echo ProjectDir = %1
echo OutDir = %2

copy "%~1SDKManifest.xml" "%~1libs\MangosteenSDK\1.0\"
copy "%~2Mangosteen.pri" "%~1libs\MangosteenSDK\1.0\Redist\CommonConfiguration\neutral\"

copy "%~2themes\generic.xaml" "%~1libs\MangosteenSDK\1.0\Redist\CommonConfiguration\neutral\themes\"
copy "%~2Mangosteen.dll" "%~1libs\MangosteenSDK\1.0\References\CommonConfiguration\neutral\"
copy "%~2Mangosteen.Design.dll" "%~1libs\MangosteenSDK\1.0\DesignTime\CommonConfiguration\neutral\"