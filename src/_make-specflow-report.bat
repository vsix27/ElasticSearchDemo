
set proj=Kafka.Features\Kafka.features.csproj
set projDll=Kafka.Features\bin\Debug\Kafka.Features.dll

@echo  see nunit-console.exe
set Path=%path%;C:\Program Files (x86)\NUnit 2.6.4\bin\;

@echo copy specflow config for latest framework usage
if not exist "packages\SpecFlow.1.9.0\tools\specflow.exe.config" (
	copy specflow.exe.config packages\SpecFlow.1.9.0\tools\
)

@echo    make html specflow report:
@echo 1. run nunit for xml output from dll with tests
if not exist "%projDll%" ( nunit-console.exe /labels /out=TestResult.txt /xml=TestResult.xml "%projDll%" )

@echo 2. run html specflow.exe to make report
packages\SpecFlow.1.9.0\tools\specflow.exe nunitexecutionreport %proj% /out:_specReport.html


pause