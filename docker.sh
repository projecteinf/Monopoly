# Connexió amb Base de dades
# Instal·lació de Docker
su -
apt install docker docker-compose -y
# Instruccions: https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
# docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Passw0rd" -p 1433:1433 --name sql1 -h sql1 -d mcr.microsoft.com/mssql/server:2022-latest

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Passw0rd!" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest

# The server is ready for connections once the SQL Server error logs display the message: 
#   "SQL Server is now ready for client connections. This is an informational message; no user action is required."
# You can review the SQL Server error log inside the container using the command:

docker exec -t sql1 cat /var/opt/mssql/log/errorlog | grep connection

# Connect to SQL Server

docker exec -it sql1 "bash"
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Passw0rd!"
# Dins del docker de mssql
1> Create database TestDB;
2> Select Name from sys.databases;
3> go

# Instal·lar msqltools a la màquina host
wget https://packages.microsoft.com/keys/microsoft.asc | sudo tee /etc/apt/trusted.gpg.d/microsoft.asc
# curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list
apt update
apt-cache search mssql-tools
apt install mssql-tools


