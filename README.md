# yap.

A simple community forum built with **ASP.NET Core Web API** and **Blazor** as part of the DNP1 course at VIA University College.

Users can create communities, share posts, participate in discussions, and vote on content.

## Tech Stack

- **Backend:** C# / ASP.NET Core Web API (.NET 10)
- **Frontend:** Blazor Interactive Server
- **Persistence:** JSON file repositories
- **Authentication:** JWT and ASP.NET Core cookie authentication

## Project Structure

```text
├── Client/          # Blazor frontend
├── Server/          # Web API, services and repositories
├── Shared/          # DTOs and mappers
└── Documentation/   # Assignment documentation
```

## Getting Started

Requires the **.NET 10 SDK**.

1. Configure `Jwt:Key` using .NET user secrets or environment variables. The key must be a Base64-encoded value containing at least 32 bytes.
2. Start `Server/WebAPI`.
3. Start `Client/WebApp`.

By default:

- **Web API:** `http://localhost:5092`
- **Blazor client:** `http://localhost:5230`

## Assignments

The project is developed incrementally. Each completed assignment is preserved using Git tags.

- [Assignment 4](https://github.com/emilTejnRasmussen/TS-DNP1-A26-Eksamens-Projekt/releases/tag/assignment-4)
- [Assignment 5](https://github.com/emilTejnRasmussen/TS-DNP1-A26-Eksamens-Projekt/releases/tag/assignment-5)
- [Assignment 6](https://github.com/emilTejnRasmussen/TS-DNP1-A26-Eksamens-Projekt/releases/tag/assignment-6)

---

*DNP1 · Software Engineering · VIA University College*
