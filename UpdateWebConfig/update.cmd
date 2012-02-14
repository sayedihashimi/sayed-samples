set msdeploy="%ProgramFiles%\IIS\Microsoft Web Deploy V2\msdeploy.exe"
%msdeploy% -verb:sync -source:contentPath="%CD%\web.config" -dest:contentPath="Default Web Site/UpdateWebCfg/web.config"
pause
