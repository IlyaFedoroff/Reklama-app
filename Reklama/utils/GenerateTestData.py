# Генерация большого тестового файла с данными
locations_data = [
    ("Яндекс.Директ", "/ru"),
    ("Ревдинский рабочий", "/ru/svrd/revda,/ru/svrd/pervik"),
    ("Газета уральских москвичей", "/ru/msk,/ru/permobl,/ru/chelobl"),
    ("Крутая реклама", "/ru/svrd"),
    ("Местная газета", "/ru/svrd/chelny"),
    ("Уральская пресса", "/ru/ural"),
    ("Московский взгляд", "/ru/msk"),
    ("Новости региона", "/ru/permobl"),
    ("Телеканал 24", "/ru/svrd/leninsk"),
    ("Пресса севера", "/ru/svrd/arkhangelsk"),
    ("Тема дня", "/ru/svrd/samara"),
    ("Городские новости", "/ru/msk"),
    ("Журнал России", "/ru/chelobl"),
    ("Вестник Европы", "/ru/svrd/perm"),
    ("Далеко от города", "/ru/msk/moscow"),
    ("Киноновости", "/ru/svrd/chelyabinsk"),
    ("Газета города", "/ru/ural/sverdlovsk"),
    ("Телевидение", "/ru/svrd/udmurtia")
]

# Создание большого списка данных с множеством строк
large_test_data = []
for i in range(1000):  # Генерируем 1000 строк
    for name, locations in locations_data:
        large_test_data.append(f"{name}: {locations}")

# Сохранение данных в файл
file_path = "E:/Study/learning/Reklama/Reklama/Data/large_reklamas.txt"
with open(file_path, "w", encoding="utf-8") as file:
    file.write("\n".join(large_test_data))

file_path
