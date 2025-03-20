1 клонировать репозиторий
git clone https://github.com/IlyaFedoroff/reklama-app.git
cd reklama-app
2 запустить Docker контейнер через docker-compose:
docker-compose up --build
3 для тестирования отправить запросы:

1 http://localhost:8080/api/reklama/load - для загрузки файла.
передавать путь как JSON-объект в body
в контейнере доступны два файла по пути app/Data для тестирования - reklamas.txt и large_reklamas.txt.
для запроса загрузки данных из файла:
стандартный путь пресета для загрузки файла:
```
 {
  "FilePath": "/app/Data/reklamas.txt"
}
```
2 http://localhost:8080/api/reklama/search?location=/ru/msk/leninsk
где параметр запроса - location=/ru/msk/leninsk - локация для поиска рекламных площадок

пример результата для файла reklamas.txt с локацией /ru/msk/leninsk:
[
    "Газета уральских москвичей",
    "Яндекс.Директ",
    "Местная газета Ленинска"
]
