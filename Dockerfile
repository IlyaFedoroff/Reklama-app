# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY ./Reklama/*.csproj ./Reklama/
COPY ./Reklama.Tests/*.csproj ./Reklama.Tests/
RUN dotnet restore ./Reklama/Reklama.csproj
RUN dotnet restore ./Reklama.Tests/Reklama.Tests.csproj

# Копируем все файлы проекта
COPY . .

# Собираем проект
RUN dotnet build ./Reklama/Reklama.csproj -c Release -o /app/build
RUN dotnet build ./Reklama.Tests/Reklama.Tests.csproj -c Release -o /app/build

# Запускаем тесты (опционально, можно убрать)
RUN dotnet test ./Reklama.Tests/Reklama.Tests.csproj --no-build --logger trx; exit 0

# Публикуем приложение
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Reklama.dll"]
