# 📝 Task Manager API

A simple RESTful API for managing personal tasks. Users can register, log in, and manage their own tasks with statuses, priorities, and due dates. Ideal for practicing core REST API concepts.

---

## 📌 Project Goals

This project is designed to help you recap and reinforce your knowledge of REST APIs, authentication, CRUD operations, filtering, pagination, and more.

---

## 📦 Core Features

- ✅ User Registration and Login (JWT Authentication)
- ✅ Create, Read, Update, Delete (CRUD) for tasks
- ✅ Task filtering by status, priority, and due date
- ✅ Pagination and sorting for task lists
- ✅ Task archiving (soft delete)
- ✅ Secure routes using role-based access (optional)

---

## 🧱 Entity Models

### User
| Field         | Type    | Description                      |
|---------------|---------|----------------------------------|
| Id            | Guid    | Unique identifier                |
| Username      | string  | Unique username                  |
| Email         | string  | Unique email                     |
| PasswordHash  | string  | Hashed password                  |
| Role          | string  | `User` or `Admin` (optional)     |

### Task
| Field         | Type      | Description                     |
|---------------|-----------|---------------------------------|
| Id            | Guid      | Unique task ID                  |
| Title         | string    | Task title                      |
| Description   | string    | Optional detailed text          |
| Status        | string    | `ToDo`, `InProgress`, `Done`    |
| Priority      | string    | `Low`, `Medium`, `High`         |
| DueDate       | DateTime  | Task deadline                   |
| IsArchived    | bool      | Marks task as archived          |
| CreatedAt     | DateTime  | Creation timestamp              |
| UpdatedAt     | DateTime  | Last update timestamp           |
| UserId        | Guid      | Owner of the task               |

---

## 🔐 Authentication

- JWT-based authentication
- Endpoints protected by default (except login/register)
- Optional: Admin role with special access

---

## 🔧 Endpoints

### 🧑‍💻 Auth
| Method | Endpoint         | Description         |
|--------|------------------|---------------------|
| POST   | `/auth/register` | Register a new user |
| POST   | `/auth/login`    | Login and get token |

### ✅ Tasks
| Method | Endpoint                     | Description                            |
|--------|------------------------------|----------------------------------------|
| GET    | `/tasks`                     | List tasks (with filters & pagination) |
| GET    | `/tasks/{id}`                | Get task by ID                         |
| POST   | `/tasks`                     | Create new task                        |
| PUT    | `/tasks/{id}`                | Update existing task                   |
| PATCH  | `/tasks/{id}/status`         | Update only task status                |
| DELETE | `/tasks/{id}`                | Delete task                            |
| PATCH  | `/tasks/{id}/archive`        | Archive/unarchive task                 |

---

## 🔍 Filtering & Pagination

**Available query parameters for `GET /tasks`:**

- `status=ToDo`
- `priority=High`
- `dueBefore=2025-07-01`
- `search=meeting`
- `page=1`
- `size=10`
- `sortBy=dueDate`
- `sortOrder=asc` or `desc`

---

## ⚙️ Technologies

- ASP.NET Core Web API
- Entity Framework Core (or Dapper)
- SQL Server / PostgreSQL
- JWT Authentication
- Swagger (OpenAPI)

---

## 🎯 Stretch Goals (Optional)

- `/tasks/summary`: Task stats grouped by status
- Reminders for tasks due soon (simulate background job)
- File uploads on tasks (e.g., notes or PDFs)
- Track time spent on tasks
- Refresh tokens for session management

---

## 🧪 Testing Ideas

- Unit test services and logic
- Integration test key endpoints
- Test for unauthorized access and invalid input

---

## 🗂 Suggested Folder Structure

