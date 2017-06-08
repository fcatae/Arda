FROM microsoft/aspnetcore-build:1.1.1
WORKDIR /app
ARG source=.
COPY $source .
RUN ["dotnet", "restore"]

# I'm using Environment Variable here to simplify the .NET Core sample.
# ENV certPassword Pa$$w0rd
 
# # Use opnssl to generate a self signed certificate arda.pfx with password $env:certPassword
# RUN openssl genrsa -des3 -passout pass:${certPassword} -out server.key 2048
# RUN openssl rsa -passin pass:${certPassword} -in server.key -out server.key
# RUN openssl req -sha256 -new -key server.key -out server.csr -subj '/CN=localhost'
# RUN openssl x509 -req -sha256 -days 365 -in server.csr -signkey server.key -out server.crt
# RUN openssl pkcs12 -export -out arda.pfx -inkey server.key -in server.crt -certfile server.crt -passout pass:${certPassword}


EXPOSE 80 443
ENTRYPOINT ["dotnet", "run"]