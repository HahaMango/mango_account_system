FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 6000
EXPOSE 6001

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MangoAccountSystem/MangoAccountSystem.csproj", "MangoAccountSystem/"]
RUN dotnet restore "MangoAccountSystem/MangoAccountSystem.csproj"
COPY . .
WORKDIR "/src/MangoAccountSystem"
RUN dotnet build "MangoAccountSystem.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MangoAccountSystem.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MangoAccountSystem.dll"]
