# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY ./Reklama/*.csproj ./Reklama/
COPY ./Reklama.Tests/*.csproj ./Reklama.Tests/
RUN dotnet restore ./Reklama/Reklama.csproj

# Копируем все файлы проекта
COPY ./Reklama/ ./Reklama/
COPY ./Reklama.Tests/ ./Reklama.Tests/

# Собираем проект
RUN dotnet build ./Reklama/Reklama.csproj -c Release -o /app/build

# Публикуем приложение
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Reklama.dll"]
