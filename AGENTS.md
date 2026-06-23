# SentenceStock Project Guide

## Purpose

This project is a portfolio-oriented learning project for catching up with ASP.NET-based web application development as a freelance engineer.

The goal is to build a small but complete example sentence stock application in about one day, while also gaining hands-on experience with CI/CD.

## Repository

Recommended repository name:

```text
aspnet-sentence-stock
```

Recommended GitHub description:

```text
ASP.NET Core Razor Pages app for saving, searching, and organizing example sentences with EF Core, SQLite, Docker, and GitHub Actions CI/CD.
```

## Application Scope

Build a simple example sentence management app.

Core features:

- Create example sentences
- List saved sentences
- Edit sentences
- Delete sentences
- Keyword search
- Tag search
- Favorite sentences
- Manage language, source, and notes

Out of scope for the initial version:

- CSV export
- User authentication
- Multi-user support
- Paid cloud infrastructure

Authentication can be listed as a future improvement using ASP.NET Core Identity.

## Technology Stack

Use a stack that can be completed for free:

- ASP.NET Core Razor Pages
- .NET 10 LTS
- Entity Framework Core
- SQLite
- xUnit
- Docker
- GitHub public repository
- GitHub Actions
- GitHub Container Registry

Razor Pages is preferred over MVC for this project because the app is a compact CRUD application and should remain easy to complete within the one-day scope.

## CI/CD Policy

Use GitHub Actions for both CI and delivery.

CI:

- Run on push and pull request
- Restore dependencies
- Build the solution
- Run tests

CD:

- Run on updates to the main branch
- Build a Docker image
- Publish the image to GitHub Container Registry

This keeps the project free while still demonstrating a practical CI/CD workflow. Always-on hosted deployment is not required for the first version.

## Suggested One-Day Plan

Morning:

- Initialize Git repository
- Create ASP.NET Core Razor Pages project
- Configure EF Core and SQLite
- Create the `Sentence` model
- Create initial migration
- Implement CRUD screens

Afternoon:

- Add keyword search, tag search, and favorite handling
- Polish the UI
- Add focused xUnit tests
- Add Dockerfile
- Add GitHub Actions workflow
- Write README with screenshots and CI/CD explanation

## Portfolio Notes

The README should clearly explain:

- This is an ASP.NET Core Razor Pages portfolio project
- EF Core migrations are used for database schema management
- SQLite is used for lightweight persistence
- GitHub Actions runs build and test automation
- Docker image publishing to GHCR demonstrates continuous delivery
- Future improvements include authentication, cloud deployment, full-text search, and tag normalization

Prioritize a small, complete, polished implementation over adding many features.
