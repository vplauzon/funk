FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Funk.Workbench/Funk.Workbench.csproj Funk.Workbench/
RUN dotnet restore Funk.Workbench/Funk.Workbench.csproj
COPY Funk.Workbench/ Funk.Workbench/
RUN dotnet publish Funk.Workbench/Funk.Workbench.csproj \
	--configuration Release \
	--no-restore \
	--output /app/publish \
	/p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "Funk.Workbench.dll"]
