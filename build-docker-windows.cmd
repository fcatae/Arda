copy run.cmd .\bin
copy Dockerfile.windows .\bin\Dockerfile

cd bin

docker build -t ardafull -f Dockerfile .