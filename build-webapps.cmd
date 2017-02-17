dotnet restore

xcopy /e /y .\deploy\webapp\app_data\*.*  %WEBROOT_PATH%\app_data\

copy .\deploy\webapp\*.cmd  %WEBROOT_PATH%\
copy .\deploy\webapp\*.config  %WEBROOT_PATH%\
