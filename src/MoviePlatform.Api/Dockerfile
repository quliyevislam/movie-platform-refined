FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH=amd64
WORKDIR /source/
COPY --link ./MoviePlatform.slnx ./
COPY --link ./Directory.Packages.props ./
COPY --link ./Directory.Build.props ./
COPY --link ./src/MoviePlatform.Api/MoviePlatform.Api.csproj ./src/MoviePlatform.Api/
COPY --link ./src/MoviePlatform.Application/MoviePlatform.Application.csproj ./src/MoviePlatform.Application/
COPY --link ./src/MoviePlatform.Domain/MoviePlatform.Domain.csproj ./src/MoviePlatform.Domain/
COPY --link ./src/MoviePlatform.Infrastructure/MoviePlatform.Infrastructure.csproj ./src/MoviePlatform.Infrastructure/
RUN	--mount=type=cache,target=/root/.nuget/packages/ \
	dotnet restore ./MoviePlatform.slnx  -a $TARGETARCH --locked-mode

COPY --link ./ ./

RUN --mount=type=cache,target=/root/.dotnet/tools/ \
	dotnet tool install --global dotnet-ef --version 10.0.*
ENV PATH="$PATH:/root/.dotnet/tools/"
RUN	--mount=type=cache,target=/root/.nuget/packages/ \
	dotnet restore ./MoviePlatform.slnx -a $TARGETARCH --locked-mode
RUN --mount=type=cache,target=/root/.nuget/packages/ \
	--mount=type=cache,target=/root/.dotnet/tools/ \
	dotnet ef migrations bundle --project ./src/MoviePlatform.Infrastructure/ \
	--startup-project ./src/MoviePlatform.Api/ -r linux-$TARGETARCH -o /bundle/efbundle

WORKDIR ./src/MoviePlatform.Api/
RUN --mount=type=cache,target=/root/.nuget/packages/ \
	dotnet publish ./MoviePlatform.Api.csproj -a $TARGETARCH --no-restore -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
EXPOSE 8080
WORKDIR /application/
COPY --link --from=build /bundle/efbundle .
COPY --link --from=build /publish/ .
HEALTHCHECK --interval=30s --timeout=5s --retries=3 CMD curl -f http://localhost:8080/health || exit 1
USER $APP_UID
ENTRYPOINT ["dotnet", "./MoviePlatform.Api.dll"]
