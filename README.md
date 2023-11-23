# api

The API backend for 'Simplifying Small Business Payments'.  
Before starting the application:

1. Install Docker Desktop and ensure it is running.
2. Clone the repository

# Running Locally On Windows

1. Navigate to `C:\%USER_PROFILE%\AppData\Roaming\Microsoft\UserSecrets` and create a folder
   named `41019bab-28dd-41f1-a221-82ed1e906626`.
2. Create a file named secrets.json.
3. Add the following json to the file:

```
{
"DatabaseSettings": {
    "ConnectionString": "Server=kluster.database;Database=KlusterDB;User Id=SA;Password={PASSWORD};
            MultipleActiveResultSets=true;TrustServerCertificate=true;"
        },
    "JwtSettings": {
            "Audience": "{Audience}",
            "Issuer": "{Issuer}",
            "SecretKey": "{SecretKey}",
            "TokenLifetimeInHours": 1
        },
    "RabbitMqSettings": {
        "Host": "kluster.messaging",
        "Password": "{password}",
        "Username": "{password}"
    },
    "MailSettings": {
        "DisplayName": "{FirstName} {LastName}",
        "From": "{EmailAddresss}",
        "Host": "{host}",
        "Password": "{Password}",
        "Port": {Port},
        "UserName": "{UserName}",
        "UseSsl": false,
        "UseStartTls": true
    }
}
```

4. Navigate to the Kluster.Host directory in the project.
5. Run `docker compose up`.

# Running Locally On Mac

1. Navigate to `~/.microsoft/usersecrets` and create a folder named `41019bab-28dd-41f1-a221-82ed1e906626`.
2. Create a file named secrets.json.
3. Add the JSON above to the secrets.json file.
4. Navigate to the Kluster.Host directory in the project.
5. Run `docker compose up`.

# Note

- After the command is run, two containers will spin up. One is `kluster-api` and the other is `kluster-databse.` The
  names are self explanatory.
- You might need to restart the container named `kluster.api` as it requires the `kluster.database` container to be
  fully setup to work.
- Access the application's swagger documentation at [Swagger](http://localhost:5000/swagger/index.html). Requests can be
  sent to the api at `http://localhost:5000/api`.
- If the required folders do not exist, *create them*.

# Postman Documentation

You can access the postman collection here: [PostmanDoc](https://documenter.getpostman.com/view/22039666/2s9YeAAumQ).
