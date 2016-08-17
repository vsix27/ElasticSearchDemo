@echo off

set name=ElasticTest.Features
set proj=..\..\%name%.csproj
::set proj=..\..\Fuse.Features.csproj

set projDll=%name%.dll

@echo having valid app.dev.config file OR valid bin\Debug\%name%.dll.config
@echo and compiled bin\Debug\%name%.dll - runs tests creates specflow html report and opens it in default browser
@echo.......................................................................
@echo.
@echo running tests for %name%
@echo.
@echo.......................................................................

:: add specflow.exe.config config file for higher framework version
set spconfig=..\packages\SpecFlow.1.9.0\tools\specflow.exe.config
if not exist %spconfig% ( (
  @echo ^<?xml version="1.0" encoding="utf-8" ?^> 
  @echo ^<configuration^>
  @echo   ^<startup^> 
  @echo     ^<supportedRuntime version="v4.0.30319" /^> 
  @echo   ^</startup^>
  @echo ^</configuration^> 
) >> %spconfig% )


if exist app.dev.config copy app.dev.config bin\Debug\%name%.dll.config

set Path=%path%;C:\Program Files (x86)\NUnit 2.6.4\bin\;
cd bin\Debug\
nunit-console.exe /labels /out=TestResult.txt /xml=TestResult.xml %name%.dll
..\..\..\packages\SpecFlow.1.9.0\tools\specflow.exe nunitexecutionreport "%proj%" /out:_Report.html

:: report will be created in bin\Debug\ folder

start _Report.html