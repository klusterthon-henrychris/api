# API

Welcome to the backend API for 'Simplifying Small Business Payments.'

## Before You Begin

To set up and run the application locally, follow these steps:

### Windows Setup

1. **Create User Secrets**
    - Navigate to `C:\%USERPROFILE%\AppData\Roaming\Microsoft\UserSecrets` and create a folder
      named `41019bab-28dd-41f1-a221-82ed1e906626`.
    - Create a file named `secrets.json`.
    - Add the following JSON to the file:

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

2. **Run the Application**
    - Navigate to the `Kluster.Host/` directory in the project.
    - Execute `docker compose up`.

### macOS Setup
1. **Create User Secrets**
   - Navigate to `~/.microsoft/usersecrets` and create a folder named `41019bab-28dd-41f1-a221-82ed1e906626`.
   - Create a file named `secrets.json`.
   - Add the same JSON content provided earlier.

2. **Run the Application**  
   - Navigate to the Kluster.Host directory in the project.
   - Run docker compose up.

## Important Notes
- After running the command, three containers will start: `kluster-api`, `kluster-database` and `kluster.messaging`.
- Restart the `kluster.api` container if needed, ensuring proper setup with the other containers.
- Access the Swagger documentation at [Swagger](http://localhost:5000/swagger/index.html).
- Send requests to the API at http://localhost:5000/api.
- If required folders are missing, ensure to create them.

You can access the postman collection here, with examples: [PostmanDoc](https://documenter.getpostman.com/view/22039666/2s9YeAAumQ).
