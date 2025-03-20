## 1. Клонировать репозиторий
```bash
git clone https://github.com/IlyaFedoroff/reklama-app.git
cd reklama-app
```
## 2. Запустить Docker контейнер через docker-compose:
```bash
docker-compose up --build
```
## 3 Тестирование
# Загрузка файла 
Для загрузки файла используйте следующий запрос:
**URL:**
```
http://localhost:8080/api/reklama/load
```
**Метод:**
```
POST
```
**BODY:**
```JSON
 {
  "FilePath": "/app/Data/reklamas.txt"
}
```
В контейнере доступны два файла по пути app/Data для тестирования: reklamas.txt и large_reklamas.txt.


# Поиск рекламных площадок
Для поиска рекламных площадок используйте следующий запрос:
**URL:**
```
http://localhost:8080/api/reklama/search?location=/ru/msk/leninsk
```
**Метод:**
```
GET
```
**Параметр запроса:**
* location: Локация для поиска рекламных площадок (например, /ru/msk/leninsk).

**Пример результата для файла** reklamas.txt **с локацией** /ru/msk/leninsk:

```JSON
[
    "Газета уральских москвичей",
    "Яндекс.Директ",
    "Местная газета Ленинска"
]
```
