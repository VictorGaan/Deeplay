# Deeplay
Для запуска нужен, SqlServer.
А также изменить стркоу подключения, которая лежит в файле DataContext.cs, по пути Deeplay.PersonnelRecords/Persistence/Contexts.
Тестовые данные лежат в миграции, а сама миграция выполняется в конструкторе App().
Так что для полноценного запуска, достаточно, изменить строку подключения на вашу.