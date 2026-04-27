# Agile Project Manager

A console-based Agile/Scrum project management tool built with **C# .NET** and **MySQL (XAMPP)**, following a clean three-tier MVC architecture.

---

## Requirements

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or later)
- [XAMPP](https://www.apachefriends.org/) (for MySQL)
- .NET 6.0 or later
- NuGet Package: `MySql.Data`

---

## Setup Guide

### Step 1 — Start XAMPP

1. Open **XAMPP Control Panel**
2. Start **Apache** and **MySQL**
3. Note the MySQL port (default is `3306`)

---

### Step 2 — Create the Database

1. Open your browser and go to `http://localhost/phpmyadmin`
2. Click **New** in the left sidebar
3. Name the database: `agile_project_manager`
4. Click **Create**

---

### Step 3 — Run the SQL Schema

1. Select the `agile_project_manager` database in phpMyAdmin
2. Click the **SQL** tab
3. Paste the entire script below and click **Go**

```sql
-- ============================================================
-- Agile Project Manager — Full Database Schema
-- ============================================================

CREATE TABLE IF NOT EXISTS project (
    projectID   VARCHAR(50)  NOT NULL,
    name        VARCHAR(100) NOT NULL,
    description TEXT,
    PRIMARY KEY (projectID)
);

CREATE TABLE IF NOT EXISTS userstory (
    storyID       VARCHAR(50) NOT NULL,
    projectID     VARCHAR(50) NOT NULL,
    content       TEXT        NOT NULL,
    priorityLevel VARCHAR(20) DEFAULT NULL,
    state         VARCHAR(50) NOT NULL DEFAULT 'project backlog',
    PRIMARY KEY (storyID),
    FOREIGN KEY (projectID) REFERENCES project(projectID),
    CHECK (state IN ('project backlog', 'in sprint', 'done'))
);

CREATE TABLE IF NOT EXISTS taskitem (
    taskID           VARCHAR(50)  NOT NULL,
    storyID          VARCHAR(50)  NOT NULL,
    state            VARCHAR(50)  NOT NULL DEFAULT 'to be done',
    priority         VARCHAR(50)  DEFAULT NULL,
    plannedTime      DECIMAL(6,2) DEFAULT 0,
    actualTime       DECIMAL(6,2) DEFAULT 0,
    plannedStartDate DATE         DEFAULT NULL,
    actualEndDate    DATE         DEFAULT NULL,
    difficultyLevel  VARCHAR(50)  DEFAULT NULL,
    categoryLabels   VARCHAR(200) DEFAULT NULL,
    PRIMARY KEY (taskID),
    FOREIGN KEY (storyID) REFERENCES userstory(storyID),
    CHECK (state IN ('to be done', 'in process', 'done')),
    CHECK (plannedTime >= 0),
    CHECK (actualTime  >= 0)
);

CREATE TABLE IF NOT EXISTS person (
    personID VARCHAR(50)  NOT NULL,
    name     VARCHAR(100) NOT NULL,
    role     VARCHAR(100) NOT NULL,
    PRIMARY KEY (personID)
);

-- Junction table: which persons are linked to which projects
CREATE TABLE IF NOT EXISTS projectperson (
    projectID VARCHAR(50) NOT NULL,
    personID  VARCHAR(50) NOT NULL,
    PRIMARY KEY (projectID, personID),
    FOREIGN KEY (projectID) REFERENCES project(projectID),
    FOREIGN KEY (personID)  REFERENCES person(personID)
);

-- Junction table: which persons are assigned to which tasks
CREATE TABLE IF NOT EXISTS taskperson (
    taskID   VARCHAR(50) NOT NULL,
    personID VARCHAR(50) NOT NULL,
    PRIMARY KEY (taskID, personID),
    FOREIGN KEY (taskID)   REFERENCES taskitem(taskID),
    FOREIGN KEY (personID) REFERENCES person(personID)
);

-- Junction table: user story dependencies
CREATE TABLE IF NOT EXISTS storydependency (
    storyID          VARCHAR(50) NOT NULL,
    dependsOnStoryID VARCHAR(50) NOT NULL,
    PRIMARY KEY (storyID, dependsOnStoryID),
    FOREIGN KEY (storyID)          REFERENCES userstory(storyID),
    FOREIGN KEY (dependsOnStoryID) REFERENCES userstory(storyID)
);
```

---

### Step 4 — Configure the Connection String

Open `DbConnectionFactory.cs` and update this line to match your setup:

```csharp
_connectionString = "server=localhost;port=3306;database=agile_project_manager;uid=root;pwd=;";
```

| Field      | Default   | Change if...                          |
|------------|-----------|---------------------------------------|
| `server`   | localhost | Running on a remote machine           |
| `port`     | 3306      | Your XAMPP uses a different port      |
| `database` | agile_project_manager | You named the DB differently |
| `uid`      | root      | You use a different MySQL user        |
| `pwd`      | *(empty)* | You set a MySQL root password         |

---

### Step 5 — Install NuGet Package

In Visual Studio:

1. Go to **Tools → NuGet Package Manager → Package Manager Console**
2. Run:

```
Install-Package MySql.Data
```

---

### Step 6 — Build and Run

1. Open the solution in Visual Studio
2. Set `AgileProjectManager.ConsoleUI` as the **Startup Project**
3. Press **F5** or click **Run**

---

## Project Structure

```
AgileProjectManager/
├── ConsoleUI/          → View layer (Program.cs — all console menus)
├── Controller/         → Service layer (business logic)
│   ├── ProjectService.cs
│   ├── UserStoryService.cs
│   ├── TaskService.cs
│   └── PersonService.cs
└── Model/
    ├── Entities/       → Data classes (Project, UserStory, TaskItem, Person)
    └── DataAccess/     → Repository layer (direct DB queries)
        ├── DbConnectionFactory.cs
        ├── ProjectRepository.cs
        ├── UserStoryRepository.cs
        ├── TaskRepository.cs
        └── PersonRepository.cs
```

---

## Features

| # | Feature |
|---|---------|
| 1 | Add / Edit / Delete Projects |
| 2 | Add / Edit / Remove User Stories |
| 3 | Add / Edit / Delete Tasks |
| 4 | Add / Edit / Delete Persons |
| 5 | Link persons to projects |
| 6 | Assign / remove persons from tasks |
| 7 | Change user story state (with dependency validation) |
| 8 | Change task state (with sprint validation) |
| 9 | Task report |
| 10 | Sprint feasibility check |

---

## Notes

- Default MySQL root password in XAMPP is **empty** — leave `pwd=` blank unless you set one
- The app uses **string-based IDs** — enter your own IDs when prompted (e.g. `P1`, `US1`, `T1`)
- All business rules (state transitions, dependency checks) are enforced in the Controller layer