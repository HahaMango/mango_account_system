FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 6000
EXPOSE 6001

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet build -c Release

FROM build AS publish
WORKDIR /src
RUN dotnet publish "MangoAccountSystem.csproj" -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY ./key.json /app
COPY ./localhost.pfx /app
COPY ./mangoidentity.pfx /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MangoAccountSystem.dll"]
