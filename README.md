# Library System

A console-based library management system built in **C# (.NET 9)**, demonstrating object-oriented programming principles and persistent data storage with SQLite.

---

## Features

- **Three item types** — Books, Magazines, and Research Papers, all managed through a shared abstract class and factory pattern
- **Two membership tiers** — Standard and Premium, with different borrow limits, durations, and late fees
- **Admin panel** — protected by login credentials, supports adding, removing, searching, and viewing all items
- **Borrow & return system** — tracks borrow dates per item, calculates late fees automatically
- **SQLite persistence** — all items, members, and borrows are saved to a local database and survive program restarts
- **Keyword search** — fuzzy search across title, author, genre, type, and item number

---

## Tech Stack

| | |
|---|---|
| Language | C# |
| Framework | .NET 9 |
| Database | SQLite via `Microsoft.Data.Sqlite` |
| Architecture | OOP — abstract classes, interfaces, Factory pattern, generics |

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Run the project

```bash
git clone https://github.com/saltkh/Library-System.git
cd Library-System
dotnet run
```

The database file (`library.db`) is created automatically on first run. No setup needed.

---

## Default Admin Login

```
Username: admin
Password: admin1234
```

---

## Project Structure

```
├── Program.cs       # Entry point and main menu loop
├── Book.cs          # LibraryItem base class, Book / Magazine / ResearchPaper, Library, LibraryFactory
├── Member.cs        # Member base class, PremiumMember, StandardMember, Register
├── Borrow.cs        # Borrow and return logic, per-item date tracking, late fee calculation
└── Database.cs      # SQLite connection, all save / load / delete operations
```

---

## OOP Concepts Demonstrated

| Concept | Where |
|---|---|
| Abstract classes | `LibraryItem`, `Member` |
| Interfaces | `IDetails`, `ISearchable` |
| Factory pattern | `LibraryFactory.CreateLibraryItem()` |
| Generics | `BorrowedItems<T>`, `ReturnItem<T>` |
| Indexer | `borrowSystem[member]` returns borrowed items |
| Polymorphism | Premium vs Standard member behaviour |

---

## Membership Tiers

| | Standard | Premium |
|---|---|---|
| Monthly fee | £10 | £30 |
| Borrow limit | 10 items | 25 items |
| Borrow duration | 12 days | 30 days |
| Late fee | £1.00/day | £0.50/day |
| Access all items | ✗ | ✓ |

---

