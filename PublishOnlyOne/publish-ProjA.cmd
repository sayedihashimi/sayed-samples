
echo This script assumes that the Web Deploy password is set at %password%
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe PublishOnlyOne.sln /p:Configuration=Release /p:VisualStudioVersion=11.0 /p:DeployOnBuild=true /p:PublishProfile="siteone - Web Deploy" /p:Password=%password%

