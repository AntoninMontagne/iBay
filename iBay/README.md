Auteurs : Lou-Anne Gautherie et Antonin Montagne
# Prérequis:

Avoir une base de données SqlExpress

Changer les informations de connection de votre base de données dans dans le fichier iBay\exec\WebAPI\appsettings.json et dans WebAPI\appsettings.json:
- Remplacez la valeur de "DefaultConnection" par la chaine de connection à votre base de donnée.

# Lancement:

Se placer dans iBay\WebAPI\bin\Release\net8.0\
Lancer WebAPI.exe

L'API est disponbile à l'adresse https://localhost:7129

Pour tester l'API, il faut utiliser soit postman soit le client API en executant l'exécutable qui se trouve dans iBay\exec\ClientAPI

# Documentations:

La documentation est disponible en lançant directement le projet WebAPI depuis le code source.
Le projet redirige vers une page Swagger.

