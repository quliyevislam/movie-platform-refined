# 🎬 MoviePlatform

A REST API for browsing, reviewing, and managing movies — built as a hands-on exploration of **Clean Architecture**, **CQRS**, and **Domain-Driven Design** in .NET 10.

---

## 🏗️ Architecture

The solution follows **Clean Architecture**, with dependencies flowing strictly inward:

```
MoviePlatform.Api ────────────┐
                              ├──▶ MoviePlatform.Application ──▶ MoviePlatform.Domain
MoviePlatform.Infrastructure ─┘
```

| Layer | Responsibility |
|---|---|
| **Domain** | Entities, aggregate roots, value objects, domain events, and repository interfaces. Zero external dependencies. |
| **Application** | CQRS command/query handlers (via MediatR), DTOs, read-model repository interfaces, and orchestration logic. |
| **Infrastructure** | EF Core write persistence, Dapper read persistence, JWT authentication, and other external concerns. |
| **Api** | ASP.NET Core controllers, request models, Swagger/JWT wiring, and composition root. |

### Key design decisions

- **CQRS read/write split** — Writes go through `IMovieWriteRepository` / `IUserWriteRepository` in the Domain layer using rich value objects and EF Core. Reads go through `IMovieReadRepository` in the Application layer, returning flat DTOs via **Dapper** for performance — no domain reconstruction needed for queries.
- **Value objects everywhere** — `Title`, `Genre`, `ReleaseDate`, `Score`, `Email`, `PasswordHash`, etc. are `readonly record struct`s with private constructors and `Result<T>` factory methods, enforcing invariants at creation time. A `FromPersistence` factory bypasses validation when rehydrating from the database.
- **Result/Error pattern** — No exceptions for expected failures. Handlers return `Result` / `Result<T>`, and a shared `ApiController.HandleFailure` maps domain `ErrorType`s (`Validation`, `NotFound`, `Conflict`, `Unauthorized`, `Forbidden`, `Failure`) to the correct HTTP status codes via RFC 7807 `ProblemDetails`.
- **Validate-first handlers** — Command handlers validate all inputs up front using `Result.Combine`, short-circuiting before touching persistence.
- **Domain events without an Application dependency** — Domain raises `IDomainEvent`s; a bridging `IDomainEventNotification : IDomainEvent, INotification` interface lives in the Application layer so MediatR isn't referenced from Domain.
- **Ownership-scoped routes** — Public browsing lives under `/api/movies`; authenticated, ownership-scoped CRUD lives under `/api/me/movies`, where the user ID is read from the JWT rather than the URL.

---

## 🧰 Tech Stack

- **.NET 10** / **C# 14**, ASP.NET Core Web API
- **MediatR** — CQRS command/query dispatch
- **Entity Framework Core** + **Npgsql** — write-side persistence & migrations
- **Dapper** — read-side persistence (paged, projection-friendly queries)
- **PostgreSQL 17**
- **JWT Bearer Authentication**
- **BCrypt.Net** — password hashing
- **Swashbuckle (Swagger/OpenAPI)** — API docs with JWT bearer auth support
- **Docker Compose** — containerized `db`, `db-migration`, and `api` services

---

## 📚 API Overview

| Method | Route | Auth | Description |
|---|---|---|---|
| `POST` | `/api/auth/register` | – | Register a new user |
| `POST` | `/api/auth/login` | – | Authenticate and receive a JWT |
| `GET` | `/api/movies` | – | Browse all movies (paged) |
| `GET` | `/api/movies/{movieId}` | – | Get a movie by ID |
| `GET` | `/api/me/movies` | ✅ | List the current user's movies (paged) |
| `GET` | `/api/me/movies/{movieId}` | ✅ | Get one of the current user's movies |
| `POST` | `/api/me/movies` | ✅ | Create a movie |
| `PUT` | `/api/me/movies/{movieId}` | ✅ | Update a movie you own |
| `DELETE` | `/api/me/movies/{movieId}` | ✅ | Delete a movie you own |
| `GET` | `/health` | – | Health check |

Full interactive documentation is available via **Swagger UI** once the API is running (see below).

### 📝 Comments & Reviews

| Method | Route | Auth | Description |
|---|---|---|---|
| `GET` | `/api/movies/{movieId}/comments` | – | List comments for a movie |
| `POST` | `/api/movies/{movieId}/comments` | ✅ | Add a comment to a movie |
| `PUT` | `/api/movies/{movieId}/comments/{commentId}` | ✅ | Update a comment you own |
| `DELETE` | `/api/movies/{movieId}/comments/{commentId}` | ✅ | Delete a comment you own |
| `GET` | `/api/movies/{movieId}/reviews` | – | List reviews for a movie |
| `POST` | `/api/movies/{movieId}/reviews` | ✅ | Submit a review (score) for a movie — creates it if none exists yet, otherwise updates your existing review |

> As with movies, ownership will be enforced via `MovieErrors.Forbidden`, using the `UserId` extracted from the JWT (`GetUserId()`) rather than trusting a value from the request body or route.

---

## 🚀 Getting Started

### Prerequisites

- [Docker](https://www.docker.com/) & Docker Compose
- (For local, non-Docker development) [.NET 10 SDK](https://dotnet.microsoft.com/) and a running PostgreSQL instance

### Run with Docker Compose

1. Create a `.env` file in the repo root:

   ```env
   POSTGRES_USER=postgres
   POSTGRES_PASSWORD=your_password
   POSTGRES_DB=movie_platform_db

   JWT_SECRET=your_super_secret_key_min_32_chars
   JWT_ISSUER=MoviePlatform
   JWT_AUDIENCE=MoviePlatform
   JWT_EXPIRY_MINUTE=60
   ```

2. Start everything:

   ```bash
   docker compose up --build
   ```

   This spins up three services:
   - `db` — PostgreSQL 17 with a health check
   - `db-migration` — applies EF Core migrations via a bundled `efbundle`, then exits
   - `api` — the ASP.NET Core API, starting only after migrations succeed

3. The API is available at `http://localhost:8080`, with Swagger UI at `http://localhost:8080/swagger`.

### Running migrations manually (local dev)

```bash
dotnet ef migrations add <MigrationName> \
  --project src/MoviePlatform.Infrastructure \
  --startup-project src/MoviePlatform.Api
```

---

## 📁 Project Structure

```
MoviePlatform/
├── src/
│   ├── MoviePlatform.Domain/
│   │   ├── Movies/
│   │   └── Users/
│   ├── MoviePlatform.Application/
│   │   ├── Movies/
│   │   └── Users/
│   ├── MoviePlatform.Infrastructure/
│   │   ├── Authentication/
│   │   └── Persistence/
│   └── MoviePlatform.Api/
│       ├── Controllers/
│       └── Requests/
├── compose.yaml
└── MoviePlatform.slnx
```
