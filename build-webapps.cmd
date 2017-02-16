set DOTNET_SKIP_FIRST_TIME_EXPERIENCE=false
set NUGET_PACKAGES=

dotnet restore -p .\src\Arda.Common\project.json

dotnet restore -p .\src\Arda.Main\project.json
dotnet build -p .\src\Arda.Main\project.json

dotnet restore

xcopy /e /y .\deploy\webapp\app_data\*.*  %WEBROOT_PATH%\app_data\

copy .\deploy\webapp\*.cmd  %WEBROOT_PATH%\
copy .\deploy\webapp\*.config  %WEBROOT_PATH%\

set NUGET_PACKAGES=D:\home\.nuget 
