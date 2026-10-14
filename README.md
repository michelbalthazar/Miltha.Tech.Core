# Miltha.Tech.Core

Modular .NET 10 foundation used to build Miltha.Tech applications. Distributed as small,
independent NuGet packages so each consumer references only what it needs.

- Target framework: `net10.0`
- License: MIT
- Versioning: SemVer (`0.1.0-preview.x` until the public API stabilizes)

## Packages

| Package | Purpose |
| --- | --- |
| `Miltha.Tech.Core` | Meta-package that references all modules below |
| `Miltha.Tech.Core.Result` | Result pattern primitives (`Result`, `Result<T>`, `Error`) |
| `Miltha.Tech.Core.Extensions` | Pure BCL helpers and cryptography (no web dependency) |
| `Miltha.Tech.Core.Http` | HTTP helpers and propagation built on ASP.NET Core |
| `Miltha.Tech.Core.Pagination` | Pagination models and helpers |
| `Miltha.Tech.Core.Validation` | FluentValidation integration helpers |
| `Miltha.Tech.Core.Security` | Security contracts and JWT bearer configuration |
| `Miltha.Tech.Core.MultiTenancy` | Tenant resolution abstractions |
| `Miltha.Tech.Core.Observability` | Serilog setup and injection |
| `Miltha.Tech.Core.Web` | ASP.NET Core middleware and base controllers |
| `Miltha.Tech.Core.WebApi` | Reusable Web API configuration (OpenAPI, CORS, health checks, versioning) |

## Installation

```bash
# everything (meta-package)
dotnet add package Miltha.Tech.Core

# or only what you need
dotnet add package Miltha.Tech.Core.Result
dotnet add package Miltha.Tech.Core.Pagination
dotnet add package Miltha.Tech.Core.Http
```

## Usage

### Result pattern

```csharp
public async Task<Result<User>> GetUserAsync(Guid id)
{
    var user = await _repository.GetUserAsync(id);

    return user is null
        ? Result<User>.Failure(Errors.NotFound("User.NotFound", "User was not found."))
        : Result<User>.Success(user);
}
```

### Web API

```csharp
public class Startup : StartupCore
{
    public Startup(IHostEnvironment environment) => BuildConfiguration<Startup>(environment);

    public override void ConfigureServices(IServiceCollection services) => ConfigureDefaultServices(services);

    public override void Configure(IApplicationBuilder app) => UseServices(app);
}
```

`ConfigureDefaultServices` wires JWT bearer authentication (`AddJwtConfiguration`), Serilog
(`AddObservability`) and the OpenAPI document (`AddOpenApiConfiguration`, exposed at
`/openapi/v1.json`), plus the base API configuration (CORS, health checks, versioning and
response compression).

## Development

```bash
# build
dotnet build Miltha.Tech.Core.sln

# tests
dotnet test Miltha.Tech.Core.sln

# pack
dotnet pack Miltha.Tech.Core.sln -c Release -o ./artifacts
```

## License

MIT - see [LICENSE](LICENSE).

## Versionamento e publicação (SemVer)

Os pacotes seguem **Semantic Versioning** (`MAJOR.MINOR.PATCH[-prerelease]`):

- **MAJOR**: mudança incompatível (quebra de contrato público).
- **MINOR**: novas funcionalidades retrocompatíveis.
- **PATCH**: correções retrocompatíveis.
- **Pré-release**: sufixos como `-preview.N` enquanto a superfície pública evolui.

Todos os módulos compartilham a **mesma versão** (versionamento uniforme). A publicação é feita por
**tag** `v*` (ex.: `v0.1.0-preview.7`) → workflow `core-publish.yml` empacota com a versão da tag e
publica no GitHub Packages. Cada pacote inclui **símbolos** (`.snupkg`), **documentação XML** e o
**README**; licença **MIT** (`PackageLicenseExpression`).

> Compatibilidade: módulos folha (`Result`, `Extensions`, `Observability`, `Pagination`, `Time`) são
> estáveis; alterações neles merecem atenção de SemVer por serem amplamente referenciados.