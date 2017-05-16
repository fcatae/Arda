FROM microsoft/aspnetcore-build:1.1.1
WORKDIR /app
ARG source=.
COPY $source .
RUN ["dotnet", "restore"]
EXPOSE 80
ENTRYPOINT ["dotnet", "run"]