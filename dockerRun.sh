#!/bin/bash
idContainer=$(docker container ls -a | cut -d " " -f1 | tail -n1)
echo "Container" $idContainer

if [ ! -z "$idContainer" ]
    then

        docker rm container $idContainer
fi

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Passw0rd!" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest

docker ps -a