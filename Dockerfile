FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["accounting-for-dentists.csproj", "."]
COPY . .
RUN dotnet restore "accounting-for-dentists.csproj"

FROM build AS publish
RUN dotnet publish "accounting-for-dentists.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "accounting-for-dentists.dll"]