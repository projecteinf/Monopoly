#!/bin/bash
DESCR=$1
if [ -z "$DESCR" ]; then
    DESCR="init"
fi
if [ ! -d "Migrations" ]; then
    mkdir Migrations
fi
# rm -r Migrations
dotnet ef migrations add "$DESCR" > Migrations/MigrationLog.txt
dotnet ef database update >> Migrations/MigrationLog.txt
cat Migrations/MigrationLog.txt
