# Exemple
# dotnet aspnet-codegenerator controller -name TeamController -async -api -m Team -dc DataContext -outDir Controllers
if [ $# -ne 1 ]
  then
    echo "Usage: ./addController.sh <ModelName>"
    exit 1
fi

dotnet aspnet-codegenerator controller -name "$1Controller" -async -api -m $1 -dc DataContext -outDir Controllers
