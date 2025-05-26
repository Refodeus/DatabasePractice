FROM mcr.microsoft.com/dotnet/sdk:8.0 AS testrunner

WORKDIR /src

COPY UnitTestingOfDatabaseProject/UnitTestingOfDatabaseProject.csproj UnitTestingOfDatabaseProject/
COPY DatabasePractice/DatabasePractice.csproj DatabasePractice/
RUN dotnet restore UnitTestingOfDatabaseProject/UnitTestingOfDatabaseProject.csproj

COPY . .

WORKDIR /src/UnitTestingOfDatabaseProject

RUN dotnet test --logger:trx