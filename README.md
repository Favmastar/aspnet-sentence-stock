# SentenceStock

SentenceStock is a compact portfolio project for saving, searching, and organizing example sentences.

The project intentionally keeps the application scope small while using this stack:

- TypeScript
- Python
- GraphQL
- C#
- AngularJS 1.x
- .NET Framework 4.8

## Architecture

```text
frontend/                         AngularJS 1.x + TypeScript client
src/SentenceStock.Api/            C# .NET Framework GraphQL HTTP API
src/SentenceStock.Domain/         Shared sentence model and search logic
tests/SentenceStock.Domain.Tests/ xUnit tests for domain behavior
tools/                            Python development utilities
```

The first version uses an in-memory repository so the GraphQL API and AngularJS client can be built quickly. Persistence can be added later without changing the user-facing scope.

AngularJS 1.x is used here as a legacy-framework learning constraint. It is no longer actively supported, so production use would require a supported Angular version or another maintained frontend framework.

## Requirements

- Windows
- .NET SDK 10
- .NET Framework 4.8 Developer Pack or targeting pack
- Node.js 22+
- Python 3.13+

## Build

Build the C# projects:

```powershell
dotnet restore SentenceStock.slnx
dotnet build SentenceStock.slnx
```

Run tests:

```powershell
dotnet test SentenceStock.slnx
```

Build the frontend:

```powershell
cd frontend
npm install
npm run build
```

## Run Locally

Start the GraphQL API:

```powershell
dotnet run --project src/SentenceStock.Api/SentenceStock.Api.csproj
```

In another terminal, start the frontend:

```powershell
cd frontend
npm run serve
```

Open the frontend at:

```text
http://127.0.0.1:5173
```

The API listens at:

```text
http://localhost:5000/graphql/
```

## Python Utility

Seed sample sentences through the GraphQL endpoint:

```powershell
python tools/seed_sentences.py
```

## Planned Features

- Create example sentences
- List saved sentences
- Delete sentences
- Keyword search
- Tag search
- Favorite sentences
- Manage language, source, and notes

## Future Improvements

- Durable persistence
- Authentication
- Full-text search
- Tag normalization
- CSV export
