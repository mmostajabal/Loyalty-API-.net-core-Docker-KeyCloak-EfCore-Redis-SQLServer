# Loyalty API

This project is a simple loyalty system API developed using .NET Core. It allows users to earn points. The project includes several functionalities such as logging with Serilog, database access with Entity Framework Core (EFCore), request validation with FluentValidation, authentication with OAuth2, caching with Redis, unit testing with XUnit and Moq, and containerization with Docker.

## Features

1. **API Development**: Use .Net Core to develop the API with endpoints for earning points.
2. **Logging**: Integrate Serilog for logging.
3. **Database**: Use EFCore for database access with migrations.
4. **Validation**: Use FluentValidation for validating incoming requests.
5. **Unit Testing**: Write basic unit tests using XUnit and Moq.
6. **Authentication**: Implement OAuth2 authentication, using KeyCloak or another provider.
7. **Caching**: Integrate Redis for caching user points.
8. **Containerization**: Containerize the application using Docker.

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download)
- [Docker](https://www.docker.com/get-started)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/mmostajabal/Loyalty-API-.net-core-Docker-KeyCloak-EfCore-Redis-SQLServer.git
   cd Loyalty

### DataBase Connection

**appsettings.json :**
Conection String to Connect the database

"ConnectionStrings": {
    "DefaultConnection": "Server=host.docker.internal;Database=LoyaltyDb;User ID=sa;Password=XXXX;TrustServerCertificate=True;",
}

Redis setting :
"ConnectionStrings": {
     "Redis": "host.docker.internal:6379"
}

**database migrations:**

1. Add-Migration InitialCreate

2. Update-Database

**Run docker**
please move to directory LoyaltyAPI
run

1. **build** : docker-compose  up  -- build
2. **up the service** : docker-compose  up
3. **stop** : docker-compose  down
4. **start services** : docker-compose up -d
5. **Build the Docker image**: docker build -t Loyalty
6. **Run the Docker container**: docker run -p 8080:80 Loyalty

### Get Token

**Postman**
(http://localhost:8080/realms/master/protocol/openid-connect/token)
in post man In body section mark x-www-form-urlencoded then add these keys :
1. key : grant_type  value :client_credentials
2. key : client_id  value: client idin keycloack(exapmle loyaltyapi)
3. key : client_secret value: you can find in keycloack then client then credential tab

**Command Prompt**
you can get token in command prompt by using this command

curl -X POST http://localhost:8080/realms/master/protocol/openid-connect/token -H "Content-Type: application/x-www-form-urlencoded" -d "client_id=LoyaltyAPI_Net" -d "client_secret=XXXXX" -d "grant_type=client_credentials"

see all the usefull links for keyCloack

-[usefull keycloack](http://localhost:8080/realms/master/.well-known/openid-configuration)

## End Point

-[Earn](https://localhost:32768/api/Users/1/earn)
-[Get Points](https://localhost:32768/api/Users/GetPoints?id=1)
