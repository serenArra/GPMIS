#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/MFAE.Jobs.Web.Public/MFAE.Jobs.Web.Public.csproj", "src/MFAE.Jobs.Web.Public/"]
COPY ["src/MFAE.Jobs.Web.Core/MFAE.Jobs.Web.Core.csproj", "src/MFAE.Jobs.Web.Core/"]
COPY ["src/MFAE.Jobs.Application/MFAE.Jobs.Application.csproj", "src/MFAE.Jobs.Application/"]
COPY ["src/MFAE.Jobs.Application.Shared/MFAE.Jobs.Application.Shared.csproj", "src/MFAE.Jobs.Application.Shared/"]
COPY ["src/MFAE.Jobs.Core.Shared/MFAE.Jobs.Core.Shared.csproj", "src/MFAE.Jobs.Core.Shared/"]
COPY ["src/MFAE.Jobs.Core/MFAE.Jobs.Core.csproj", "src/MFAE.Jobs.Core/"]
COPY ["src/MFAE.Jobs.EntityFrameworkCore/MFAE.Jobs.EntityFrameworkCore.csproj", "src/MFAE.Jobs.EntityFrameworkCore/"]
COPY ["src/MFAE.Jobs.GraphQL/MFAE.Jobs.GraphQL.csproj", "src/MFAE.Jobs.GraphQL/"]
RUN dotnet restore "src/MFAE.Jobs.Web.Public/MFAE.Jobs.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/MFAE.Jobs.Web.Public"
RUN dotnet build "MFAE.Jobs.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MFAE.Jobs.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MFAE.Jobs.Web.Public.dll"]
