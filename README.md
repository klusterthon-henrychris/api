# api
The API backend for 'Simplifying Small Business Payments'.
Before starting the application, install Docker Desktop and ensure it is running.
# Running Locally On Windows
1. Navigate to `C:\%USER_PROFILE%\AppData\Roaming\Microsoft\UserSecrets` and create a folder named `41019bab-28dd-41f1-a221-82ed1e906626`.
2. Create a file named secrets.json.
3. Add the following json to the file:
 ```
   {

    "DatabaseSettings": 
    {"ConnectionString": "Server=sql_server2022;Database=KlusterDB;UserId=SA;Password{PASSWORD};
        MultipleActiveResultSets=true;TrustServerCertificate=true;"

    },

    "JwtSettings": {
        "Audience": "{Audience}",
        "Issuer": "{Issuer}",
        "SecretKey": "{SecretKey}",
        "TokenLifetimeInHours": 1
    }
   }
```

5. Navigate to the Kluster.Host directory in the project.
6. Run `docker compose up`.
# Running Locally On Mac

1. Navigate to `~/.microsoft/usersecrets` and create a folder named `41019bab-28dd-41f1-a221-82ed1e906626`.
2. Create a file named secrets.json.
3. Add the following json to the file:
```
{
    "DatabaseSettings": {
        "ConnectionString": "Server=sql_server2022;Database=KlusterDB;User Id=SA;Password={PASSWORD};
           MultipleActiveResultSets=true;TrustServerCertificate=true;"
    },
    "JwtSettings": {
        "Audience": "{Audience}",
        "Issuer": "{Issuer}",
        "SecretKey": "{SecretKey}",
        "TokenLifetimeInHours": 1
    }
}
```

5. Navigate to the Kluster.Host directory in the project.
6. Run `docker compose up`.
# Note
- You might need to restart the container named `kluster.api` as it requires the `kluster.database` container to be fully setup.
- Access the application's swagger documentation at [Swagger](http://localhost:5000/swagger/index.html). Requests can be sent to the api at `http://localhost:5000/api`.
# Postman Documentation
You can access the postman collection here: [PostmanDoc](https://documenter.getpostman.com/view/22039666/2s9YeAAumQ).