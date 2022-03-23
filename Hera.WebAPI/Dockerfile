#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Hera.WebAPI/Hera.WebAPI.csproj", "Hera.WebAPI/"]
COPY ["Hera.Common/Hera.Common.csproj", "Hera.Common/"]
COPY ["Hera.Services/Hera.Services.csproj", "Hera.Services/"]
COPY ["Hera.Data/Hera.Data.csproj", "Hera.Data/"]
COPY ["Hera.CryptoService/Hera.CryptoService.csproj", "Hera.CryptoService/"]
RUN dotnet restore "Hera.WebAPI/Hera.WebAPI.csproj"
COPY . .
WORKDIR "/src/Hera.WebAPI"
RUN dotnet build "Hera.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hera.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hera.WebAPI.dll"]