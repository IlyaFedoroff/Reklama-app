## 1 клонировать репозиторий

```bash
git clone https://github.com/IlyaFedoroff/reklama-app.git
cd reklama-app
```

## 2 запустить Docker контейнер через docker-compose:

```bash
docker-compose up --build
```

## 3 для тестирования отправить запросы:

# Загрузка файла 

```http
http://localhost:8080/api/reklama/load
```

передавать путь как JSON-объект в body
в контейнере доступны два файла по пути app/Data для тестирования - reklamas.txt и large_reklamas.txt.
для запроса загрузки данных из файла:
стандартный путь пресета для загрузки файла:

```JSON
 {
  "FilePath": "/app/Data/reklamas.txt"
}
```

# Поиск по локации
```http
http://localhost:8080/api/reklama/search?location=/ru/msk/leninsk
```
где параметр запроса - location=/ru/msk/leninsk - локация для поиска рекламных площадок

Пример результата для файла reklamas.txt с локацией /ru/msk/leninsk:

```JSON
[
    "Газета уральских москвичей",
    "Яндекс.Директ",
    "Местная газета Ленинска"
]
```
