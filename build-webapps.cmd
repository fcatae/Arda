REM set NUGET_PACKAGES=D:\home\site\.nuget

set DOTNET_SKIP_FIRST_TIME_EXPERIENCE=false

dotnet restore --packages d:\home\.nuget -v Debug

dotnet build .\src\Arda.Main\project.json

copy .\deploy\webapp\*.cmd  %WEBROOT_PATH%\
copy .\deploy\webapp\*.config  %WEBROOT_PATH%\
copy .\deploy\webapp\appsettings.json  %WEBROOT_PATH%\
copy .\deploy\webapp\arda.pfx  %WEBROOT_PATH%\

xcopy /e /y .\deploy\webapp\app_data\*.*  %WEBROOT_PATH%\app_data\
