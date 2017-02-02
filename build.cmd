dotnet restore

mkdir bin

cd src\Arda.Kanban
dotnet publish -o .\bin\publish
xcopy /e /y .\bin\publish ..\..\bin\Arda.Kanban
cd ..\..

cd src\Arda.Main
dotnet publish -o .\bin\publish
xcopy /e /y .\bin\publish ..\..\bin\Arda.Main
cd ..\..

cd src\Arda.Permissions
dotnet publish -o .\bin\publish
xcopy /e /y .\bin\publish ..\..\bin\Arda.Permissions
cd ..\..

cd src\Arda.Reports
dotnet publish -o .\bin\publish
xcopy /e /y .\bin\publish ..\..\bin\Arda.Reports
cd ..\..

