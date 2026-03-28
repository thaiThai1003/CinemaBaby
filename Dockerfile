FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# copy đúng thư mục chứa project
COPY CinemaBaby/*.csproj ./CinemaBaby/
RUN dotnet restore ./CinemaBaby/CinemaBaby.csproj

# copy toàn bộ code
COPY . .
WORKDIR /app/CinemaBaby
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/CinemaBaby/out .

ENTRYPOINT ["dotnet", "CinemaBaby.dll"]
