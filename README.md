# HomeLocalStorage

A self-hosted web application for storing and accessing your files from any device via a browser — a lightweight personal cloud storage within your own network, deployable to any server.

## Features

- 📁 Upload, download, copy, rename and delete files and directories
- 🔐 JWT-based authentication with secure password hashing
- 🌐 Access from any device via browser — no client software needed
- 🐳 Dockerized backend for easy deployment

## Tech Stack

**Backend**
- .NET 10, ASP.NET Core
- Entity Framework Core (Code First, Migrations)
- PostgreSQL
- JWT Bearer authentication, `PasswordHasher<>`

**Frontend**
- React + Vite
- Ant Design

**Infrastructure**
- Docker (backend)
- Deployed: backend on [Render](https://render.com), frontend on [Vercel](https://vercel.com)

## Architecture

The backend follows a layered architecture:

```
HomeLocalStorage/          # ASP.NET Core Web API
HomeLocalStorage.Core/     # Domain models, interfaces
HomeLocalStorage.DataAccess/  # EF Core, repositories, migrations
HomeLocalStorage.Services/ # Business logic
frontend/homecloud/        # React + Vite frontend
```

## License

MIT
