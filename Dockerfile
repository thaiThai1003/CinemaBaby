FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY CinemaBaby/*.csproj ./CinemaBaby/
RUN dotnet restore ./CinemaBaby/CinemaBaby.csproj

COPY . .
WORKDIR /app/CinemaBaby
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/CinemaBaby/out .

ENTRYPOINT ["dotnet", "CinemaBaby.dll"]
