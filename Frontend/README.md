# React frontend

React-интерфейс для существующих API студентов и организаций. Сборка Vite помещает статические файлы в `Presentation/WebApi/wwwroot`, откуда их раздаёт ASP.NET Web API.

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
