FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
ARG source
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT QA
ENV ASPNETCORE_URLS http://+:83
EXPOSE 83
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "CoreAPI.dll"]