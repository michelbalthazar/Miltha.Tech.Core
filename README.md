<!-- BADGES -->

<p align="center">
  <img src="https://img.shields.io/github/actions/workflow/status/seu-repo/publish-public.yml?label=build&style=for-the-badge" />
  <img src="https://img.shields.io/nuget/v/Miltha.Tech.Core?style=for-the-badge" />
  <img src="https://img.shields.io/github/license/seu-repo?style=for-the-badge" />
  <img src="https://img.shields.io/github/stars/seu-repo?style=for-the-badge" />
</p>

<h1 align="center">🚀 Miltha.Tech.Core</h1>

<p align="center">
  A modular, production-ready foundation for building scalable .NET applications.
</p>

---

## ✨ Why this project exists

Most .NET projects suffer from:

* ❌ Fat "Core" libraries
* ❌ Tight coupling between layers
* ❌ Repeated infrastructure code
* ❌ Hard-to-maintain solutions

**Miltha.Tech.Core solves this** by providing a **modular, composable architecture** designed for real-world systems.

---

## 🧠 Key Concepts

* 🧩 **Modular architecture** — each concern is isolated
* 📦 **NuGet-first design** — everything is reusable
* 🔄 **Composable modules** — plug only what you need
* 🧼 **Clean separation** — no leaking infrastructure

---

## 📦 Installation

```bash
dotnet add package Miltha.Tech.Core
```

---

## 🚀 Quick Start

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore();

var app = builder.Build();

app.MapControllers();

app.Run();
```

---

## 🧩 Modules Overview

| Module          | Responsibility               |
| --------------- | ---------------------------- |
| `Core`          | Facade / entry point         |
| `Result`        | Functional result pattern    |
| `Web`           | Middleware + base controller |
| `WebApi`        | API configuration            |
| `Security`      | JWT authentication           |
| `Observability` | Logging & tracing            |
| `MultiTenancy`  | Tenant resolution            |
| `Pagination`    | Paging utilities             |
| `Validation`    | FluentValidation extensions  |
| `Extensions`    | Helpers & utilities          |

---

## 🏗️ Architecture

```text
           ┌──────────────────────┐
           │     Applications     │
           └─────────┬────────────┘
                     │
             ┌───────▼────────┐
             │     Core       │  (Facade)
             └───────┬────────┘
                     │
 ┌────────────────────────────────────────────┐
 │ Modules                                   │
 │                                            │
 │ Abstractions  Result  Extensions            │
 │        │         │         │                │
 │        └────┬────┴────┬────┘                │
 │             │         │                     │
 │           Http     Validation               │
 │             │                               │
 │         Web / Security / Observability      │
 │             │                               │
 │           WebApi                            │
 └────────────────────────────────────────────┘
```

---

## 💡 Example: Result Pattern

```csharp
public TryException<UserDto> GetUser(Guid id)
{
    if (id == Guid.Empty)
        return ResultWithFailure.Factory(HttpStatusCode.BadRequest, "Invalid ID");

    return new UserDto { Id = id };
}
```

---

## 🌐 Example: HTTP Client

```csharp
var result = await _httpClientInternal.ExecuteGetRequestAsync<MyResponse>("api/users");

if (result.IsFailure)
    return BadRequest(result.Failure);

return Ok(result.Success);
```

---

## 🧱 Design Principles

* ✔️ Single Responsibility per module
* ✔️ No circular dependencies
* ✔️ Infrastructure isolation
* ✔️ Explicit dependencies

---

## 📦 NuGet Strategy

| Package         | Type       |
| --------------- | ---------- |
| Core.Result     | 🌍 Public  |
| Core.Extensions | 🌍 Public  |
| Core.Http       | 🔒 Private |
| Core.Web        | 🔒 Private |
| Core            | 🔒 Private |

---

## 🔄 Versioning

This project follows **Semantic Versioning**:

```bash
v1.0.0 → Initial release
v1.1.0 → New features
v2.0.0 → Breaking changes
```

---

## ⚙️ CI/CD

Automated pipelines:

* ✔️ Build & test
* ✔️ Package generation
* ✔️ Publish to NuGet
* ✔️ Version via tags

---

## 🛠️ Development

### Build

```bash
dotnet build
```

### Pack

```bash
dotnet pack -c Release
```

---

## 📚 Best Practices

* Keep modules independent
* Avoid adding heavy dependencies
* Prefer composition over inheritance
* Version modules individually

---

## ⚠️ Anti-Patterns

* ❌ Monolithic "Core" packages
* ❌ Mixing domain + infra
* ❌ Hidden dependencies
* ❌ Over-engineering

---

## 🧪 Testing (coming soon)

* Core.Testing module
* Integration helpers
* Mocking utilities

---

## 🗺️ Roadmap

* [ ] Automatic versioning (Conventional Commits)
* [ ] Changelog generation
* [ ] Source Generators
* [ ] OpenTelemetry support
* [ ] Distributed tracing

---

## 🤝 Contributing

```bash
feat: add new feature
fix: bug fix
refactor: code improvement
```

1. Fork the repository
2. Create a branch
3. Submit a PR

---

## 📄 License

MIT

---

## 👨‍💻 Maintainers

Miltha Tech

---

## ⭐ Support

If this project helps you, consider giving it a star ⭐

---

<p align="center">
  Built with using .NET
</p>
