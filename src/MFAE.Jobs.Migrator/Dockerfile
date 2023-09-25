#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MFAE.Jobs.Migrator/MFAE.Jobs.Migrator.csproj", "src/MFAE.Jobs.Migrator/"]
COPY ["src/MFAE.Jobs.EntityFrameworkCore/MFAE.Jobs.EntityFrameworkCore.csproj", "src/MFAE.Jobs.EntityFrameworkCore/"]
COPY ["src/MFAE.Jobs.Core/MFAE.Jobs.Core.csproj", "src/MFAE.Jobs.Core/"]
COPY ["src/MFAE.Jobs.Core.Shared/MFAE.Jobs.Core.Shared.csproj", "src/MFAE.Jobs.Core.Shared/"]
RUN dotnet restore "src/MFAE.Jobs.Migrator/MFAE.Jobs.Migrator.csproj"
COPY . .
WORKDIR "/src/src/MFAE.Jobs.Migrator"
RUN dotnet build "MFAE.Jobs.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MFAE.Jobs.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MFAE.Jobs.Migrator.dll", "-s"]
