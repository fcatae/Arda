#FROM microsoft/aspnetcore-build:1.1.1
#WORKDIR /app
#ARG source=.
#COPY $source .
#RUN ["dotnet", "restore"]
#EXPOSE 80
#ENTRYPOINT ["dotnet", "run"]

#Pointint to standard .net core image on docker hub
FROM microsoft/aspnetcore-build:1.1.1

#Defining default directory to ASP.NET Core app inside container
WORKDIR /app

#Copying the entire application files to /app directory
COPY ./bin/Debug/netcoreapp1.0/publish .

#Exposing the application to the internet through port 80
EXPOSE 80/tcp
 
#Building and running the dotnet application from within the container
ENTRYPOINT ["dotnet", "Arda.Kanban.dll"]