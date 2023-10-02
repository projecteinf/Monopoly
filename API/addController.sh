# Exemple
# dotnet aspnet-codegenerator controller -name TeamController -async -api -m Team -dc DataContext -outDir Controllers

for FILE in Model/*.cs
do
    FILENAME=$(basename "$FILE")
    #echo "${FILENAME%.*}"
    dotnet aspnet-codegenerator controller -name "${FILENAME%.*}Controller" -async -api -m ${FILENAME%.*} -dc DataContext -outDir Controllers
done
#dotnet aspnet-codegenerator controller -name "$1Controller" -async -api -m $1 -dc DataContext -outDir Controllers
