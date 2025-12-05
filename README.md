# SchedulingSystem – Backend API

Backend for a basic **work scheduling system** built with **.NET** and **PostgreSQL**.  
The application is **headless** – it only exposes a REST API. Any frontend (Angular, React, Vue, etc.) can consume it.

---

## Overview

The system manages:

- **Job** – a type of work that can be scheduled.
- **User** – an employee using the system.
- **Role** – defines permissions (`Admin`, `Worker/User`).
- **Schedule** – when a user is assigned to an appointment.

### Roles & Permissions

- **Admin**
  - Review schedule requests.
  - Approve or reject schedule requests.
  - Manage schedules.
  - Submit schedule requests for specific jobs and time slots.

- **Worker / User**
  - Submit schedule requests for specific jobs and time slots.
  - View schedules and request statuses.

Authentication is done with **JWT Bearer tokens**.

---

## Tech Stack

- **.NET** (Web API)
- **Entity Framework Core** + **PostgreSQL**
- **CQRS + MediatR**
- **FluentValidation** for request validation
- **Serilog** for logging
- **JWT Bearer Authentication**

---

## Project Structure (High Level)

- `SchedulingSystem.Domain` – Entities (`Job`, `Schedule`, `User`, `Role`, etc.).
- `SchedulingSystem.Application` – CQRS commands/queries, interfaces, validators.
- `SchedulingSystem.Infrastructure` – EF Core DbContext, repositories, JWT generator, persistence.
- `SchedulingSystem.WebApi` – API controllers, middleware, DI configuration.

---

## Environment Configuration

The backend reads the PostgreSQL connection string from an environment variable:

```bash
POSTGRES_CONNECTION=Host=YOUR_HOST;Port=YOUR_PORT;Database=SchedulingSystemDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD
