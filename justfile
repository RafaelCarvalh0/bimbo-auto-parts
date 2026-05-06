set shell := ["cmd", "/c"]

migrate:
    dotnet ef database update --project LojaVirtual.Data --startup-project LojaVirtual.API

create-migration name:
    dotnet ef migrations add {{name}} --project LojaVirtual.Data --startup-project LojaVirtual.API

delete-last-migration:
    dotnet ef migrations remove --project LojaVirtual.Data --startup-project LojaVirtual.API

run:
    dotnet run --project LojaVirtual.API

build:
    dotnet build

watch:
    dotnet watch run --project LojaVirtual.API