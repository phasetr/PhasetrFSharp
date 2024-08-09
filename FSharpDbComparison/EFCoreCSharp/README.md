# README

`dotnet tool install`するとルートディレクトリの`.config/dotnet-tools.json`に追加される.

```shell
dotnet tool install --local dotnet-ef
```

```shell
dotnet ef migrations add InitialCreate
dotnet ef database update
```
