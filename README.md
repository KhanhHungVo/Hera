# Hera

## Technologies
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)

## Getting Started

* Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
* Install postgre (https://www.postgresql.org/download/)
* dotnet restore
* dotnet build
* dotnet run --project hera.webapi
* Open swagger by access link http://localhost:5000/swagger/index.html


## Docker configuration

* Build docker `docker build -f "Hera.WebApi\Dockerfile" -r heraapi .`
* Run docker ` docker  run -p 8080:80 -d heraapi` 