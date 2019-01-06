FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 54309
EXPOSE 44336

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Auto_Parts_2019/Auto_Parts_2019.csproj Auto_Parts_2019/
RUN dotnet restore Auto_Parts_2019/Auto_Parts_2019.csproj
COPY . .
WORKDIR /src/Auto_Parts_2019
RUN dotnet build Auto_Parts_2019.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Auto_Parts_2019.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Auto_Parts_2019.dll"]
