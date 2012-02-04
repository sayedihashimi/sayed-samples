@echo off

set MsdeployExe="C:\Program Files\IIS\Microsoft Web Deploy V2\msdeploy.exe"
set SqcmdExe="C:\Program Files\Microsoft SQL Server\100\Tools\Binn\sqlcmd.exe"

echo                                                                             .
echo -----------------------------------------------------------------------------
echo      Deleting ContactManager site
echo -----------------------------------------------------------------------------

echo %MsdeployExe% -verb:delete -dest:appHostConfig="DemoSite/ContactManager"
%MsdeployExe% -verb:delete -dest:appHostConfig="DemoSite/ContactManager"


echo                                                                             .
echo -----------------------------------------------------------------------------
echo      Deleting ContactManagerSvervice site
echo -----------------------------------------------------------------------------

echo %MsdeployExe% -verb:delete -dest:appHostConfig="DemoSite/ContactManagerService"
%MsdeployExe% -verb:delete -dest:appHostConfig="DemoSite/ContactManagerService"

echo                                                                             .
echo -----------------------------------------------------------------------------
echo      Resetting ContactManager database
echo -----------------------------------------------------------------------------
echo %SqcmdExe% -S sayedha-hp2 -U sa -P p@ssw0rd -i Reset-Database.sql
%SqcmdExe% -S sayedha-hp2 -U sa -P p@ssw0rd -i Reset-Database.sql