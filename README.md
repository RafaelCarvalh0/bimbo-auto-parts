# 🔧 Bimbo Auto Parts

Aplicação web e-commerce para peças automotivas, construída com **ASP.NET Core (.NET 10)**, estruturada em múltiplas camadas e desenvolvida com princípios de código limpo, separação de responsabilidades e padrões de desenvolvimento profissional.

---

## 🏗️ Arquitetura da Solution

A solution é composta por **4 projetos**, cada um com responsabilidade bem definida:

```
LojaVirtual/          → Web Application MVC (Frontend + Camada Consumidora)
LojaVirtual.API/      → Web API RESTful (Backend)
LojaVirtual.Data/     → Camada de Acesso a Dados (Repositórios + EF Core)
LojaVirtual.Entities/ → Class Library Compartilhada (Domain Models + DTOs)
```

### Responsabilidades por Camada

| Projeto | Função |
|---|---|
| `LojaVirtual` | ASP.NET Core MVC — consome a Web API via HTTP, renderiza as views e gerencia a UX |
| `LojaVirtual.API` | ASP.NET Core Web API — expõe endpoints RESTful, aplica validações e orquestra o fluxo de dados |
| `LojaVirtual.Data` | Repository pattern — toda a lógica do EF Core, queries e persistência |
| `LojaVirtual.Entities` | Entidades compartilhadas — Domain models, DTOs (Request/Response) e contratos entre camadas |

---

## 🛠️ Stack Tecnológica

| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET | 10 | Runtime e framework |
| ASP.NET Core MVC | 10 | Camada web (frontend) |
| ASP.NET Core Web API | 10 | Camada de API (backend) |
| Entity Framework Core | 9.0 | ORM |
| Pomelo.EntityFrameworkCore.MySql | 9.0 | Provider MySQL para EF Core |
| MySQL | — | Banco de dados relacional (Docker) |
| FluentValidation | 12.1.1 | Validação de entrada |
| Bootstrap 5 | — | Framework de UI |
| DataTables | — | Grids interativos |
| SweetAlert2 | — | Diálogos de feedback ao usuário |
| jQuery | — | Comunicação AJAX |

---

## ✅ Funcionalidades Implementadas

### Categorias
- Listagem com DataTable carregado via AJAX
- Criação via modal (sem reload de página)
- Edição em página dedicada com formulário pré-preenchido
- Exclusão com diálogo de confirmação
- Validação de nome duplicado
- Proteção contra exclusão de categoria com produtos vinculados

### Produtos
- Listagem com nome da categoria, preço e descrição via AJAX
- Criação via modal com dropdown de categorias carregado dinamicamente
- Edição em página dedicada com seletor de categoria pré-selecionado
- Exclusão com diálogo de confirmação
- Validação de chave estrangeira contra categorias

---

## 🧱 Padrões e Boas Práticas

### Repository Pattern
Todas as operações de banco são abstraídas por interfaces (`ICategoryRepository`, `IProductRepository`), desacoplando os controllers do EF Core:

```csharp
public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task CreateCategoryAsync(CategoryCreateRequest request);
    Task PatchCategoryAsync(int id, CategoryUpdateRequest request);
    Task DeleteCategoryAsync(int id);
}
```

### Injeção de Dependência
Todos os serviços, repositórios e o contexto de banco são registrados via container nativo do .NET, promovendo baixo acoplamento e testabilidade.

### FluentValidation
Validação de entrada tratada de forma declarativa e separada dos controllers. Um Action Filter customizado `[Validate<T>]` intercepta as requisições e retorna 400 padronizado antes da execução do controller:

```csharp
public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
{
    public CategoryCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
```

### Implicit Operators
Conversão entre Domain models e DTOs via implicit operators, eliminando mapeamento manual e mantendo os controllers limpos:

```csharp
public static implicit operator ProductResponse(Product product)
    => new ProductResponse
    {
        Id          = product.Id,
        Name        = product.Name,
        Description = product.Description,
        Price       = product.Price,
        CategoryId  = product.CategoryId
    };
```

### EF Core Fluent API
Configurações de entidade definidas via Fluent API em classes dedicadas, mantendo os domain models livres de data annotations:

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);
    }
}
```

### PATCH ao invés de PUT
Endpoints de atualização usam semântica HTTP PATCH — apenas os campos enviados na requisição são atualizados, preservando os dados existentes para campos omitidos:

```csharp
if (!string.IsNullOrWhiteSpace(request.Name))
    existing.Name = request.Name;

if (request.Price > 0)
    existing.Price = request.Price.Value;
```

---

## 🖥️ Arquitetura Frontend

A camada MVC **não utiliza** Razor form submissions para operações de CRUD. Todas as interações com dados são realizadas via **AJAX**, proporcionando uma experiência fluída sem recarregamento de página:

- **DataTables** carrega os dados da grid assincronamente na inicialização da página
- Operações de **criação** abrem um modal e fazem POST via `$.ajax`
- Páginas de **edição** carregam dropdowns dinamicamente via AJAX
- Operações de **exclusão** exibem confirmação via SweetAlert2 antes de enviar DELETE
- Mensagens de erro da API são parseadas e exibidas inline nos campos do formulário ou via SweetAlert2

---

## 🐳 Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) e Docker Compose
- [just](https://github.com/casey/just) — task runner utilizado para gerenciar migrations

---

## 🚀 Rodando o Projeto

**1. Clone o repositório**
```bash
git clone https://github.com/RafaelCarvalh0/Bimbo-auto-parts.git
cd Bimbo-auto-parts
```

**2. Suba o MySQL via Docker**
```bash
docker-compose up -d
```

**3. Configure a connection string**

Em `LojaVirtual.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=loja_virtual;User=root;Password=yourpassword;"
  }
}
```

**4. Aplique as migrations**

Na raiz do projeto, com o `just` instalado:
```bash
just migrate
```

> Caso não tenha o `just` instalado, a alternativa é:
> ```bash
> cd LojaVirtual.Data
> dotnet ef database update --startup-project ../LojaVirtual.API
> ```

**5. Rode os projetos**

A forma recomendada é configurar **Multiple Startup Projects** no Visual Studio:
- Clique com o botão direito na Solution → *Set Startup Projects*
- Selecione **Multiple startup projects**
- Defina `LojaVirtual.API` e `LojaVirtual` como **Start**
- Pressione **F5**

Ou via terminal:
```bash
# Terminal 1 — API
cd LojaVirtual.API && dotnet run

# Terminal 2 — MVC
cd LojaVirtual && dotnet run
```

**6. Acesse a aplicação**

| Serviço | URL |
|---|---|
| Web Application | `https://localhost:7185` |
| Swagger UI | `https://localhost:PORT/swagger` |

---

## 🗂️ Comandos just disponíveis

Todos os comandos devem ser executados na **raiz do projeto**:

```bash
just migrate                        # Aplica as migrations pendentes
just create-migration NomeMigration # Cria uma nova migration
just delete-last-migration          # Remove a última migration
```

---

## 📁 Estrutura do Projeto

```
LojaVirtual.sln
│
├── LojaVirtual/                        # Web Application MVC
│   ├── Controllers/                    # Controllers MVC (consomem a API)
│   ├── Views/
│   │   ├── Categories/                 # Index, Edit, _New (partial)
│   │   └── Products/                   # Index, Edit, _New (partial)
│   └── wwwroot/css/mainpages.css       # Estilos compartilhados
│
├── LojaVirtual.API/                    # Web API RESTful
│   ├── Controllers/                    # API Controllers
│   └── Models/Validators/             # FluentValidation + Action Filter
│
├── LojaVirtual.Data/                   # Camada de Acesso a Dados
│   ├── DatabaseContext/                # AppDbContext + configurações Fluent API
│   └── Repositories/                  # ICategoryRepository, IProductRepository
│
└── LojaVirtual.Entities/              # Class Library Compartilhada
    └── Models/
        ├── Domain/                     # Category, Product
        └── Dto/
            ├── Request/                # CreateRequest, UpdateRequest
            └── Response/              # CategoryResponse, ProductResponse
```
