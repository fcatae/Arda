dotnet restore

mkdir bin

cd src\Arda.Kanban
dotnet publish
xcopy /e /y .\bin\debug\netcoreapp1.0\publish ..\..\bin\Arda.Kanban
cd ..\..

cd src\Arda.Main
dotnet publish
xcopy /e /y .\bin\debug\netcoreapp1.0\publish ..\..\bin\Arda.Main
cd ..\..

cd src\Arda.Permissions
dotnet publish
xcopy /e /y .\bin\debug\netcoreapp1.0\publish ..\..\bin\Arda.Permissions
cd ..\..

cd src\Arda.Reports
dotnet publish
xcopy /e /y .\bin\debug\netcoreapp1.0\publish ..\..\bin\Arda.Reports
cd ..\..

