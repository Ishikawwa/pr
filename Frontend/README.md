# React frontend

React-интерфейс для API проекта. Ресурсы собраны в общие разделы: справочники, учебный процесс, успеваемость, лицензии и аттестации. Для них используется единая таблица с поиском и формами создания, редактирования и удаления; связанные сущности выбираются из справочников. Сборка Vite помещает статические файлы в `Presentation/WebApi/wwwroot`, откуда их раздаёт ASP.NET Web API.

## Разработка

В одном терминале запустите API по HTTP-профилю из корня репозитория:

```sh
dotnet run --project Presentation/WebApi --launch-profile http
```

В другом терминале:

```sh
cd Frontend
npm install
npm run dev
```

Откройте адрес Vite, показанный в терминале (обычно `http://localhost:5173`). Запросы `/api` проксируются на `http://localhost:5073`.

## Сборка для ASP.NET

```sh
cd Frontend
npm install
npm run build
cd ..
dotnet run --project Presentation/WebApi --launch-profile http
```

Также сборка React автоматически запускается перед сборкой WebApi, если в `Frontend` уже установлены npm-зависимости. Собранные файлы копируются в выходную и publish-папки ASP.NET. Чтобы пропустить шаг Vite, передайте MSBuild-свойство `SkipFrontendBuild=true`.
