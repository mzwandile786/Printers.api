🖨 Printers API
📌 Overview

Printers API is a RESTful Web API built using ASP.NET Core that manages printers, users, and designations within an organization.
The API provides endpoints for performing CRUD operations and acts as the backend service for the Company Printers Management System.

This project follows a layered architecture using:

Controllers

Business Logic Layer (BLL)

Data Access Layer (DAL)

Models

This structure helps keep the code clean, scalable, and maintainable.

🚀 Technologies Used

ASP.NET Core Web API

C#

SQL Server

REST API Architecture

ADO.NET

Git & GitHub

🏗 Project Architecture

The project follows a multi-layer architecture:

Controllers
   │
   ▼
Business Logic Layer (BLL)
   │
   ▼
Data Access Layer (DAL)
   │
   ▼
Database (SQL Server)
Layers Explanation
Controllers

Handle HTTP requests and responses.
Example:
PrintersController
UsersController
DesignationController

BLL (Business Logic Layer)
Contains the business rules and application logic.
Example:
Processing printer operations
Managing user data
Handling designations

DAL (Data Access Layer)
Responsible for communication with the database.

Functions include:
Executing SQL queries
Retrieving data
Saving or updating records

Models
Models represent the data structure used by the application.
Printer
User
Designation

⚙ API Features

✔ Manage printers
✔ Manage users
✔ Manage designations
✔ RESTful API endpoints
✔ Layered architecture
✔ Scalable backend service

📡 Example API Endpoints
Get All Printers
GET /api/printers
Get Printer by ID
GET /api/printers/{id}

Add Printer
POST /api/printers
Update Printer
PUT /api/printers/{id}
Delete Printer
DELETE /api/printers/{id}

The API can be tested using:
Swagger UI
Browser (for GET requests)

⚙ Configuration
The database connection string can be configured in:
appsettings.json


👨‍💻 Author
Mzwandile Byron Mngadi
🎓 Advanced Diploma in ICT
💻 Software Developer

📧 mzwamngadi786@gmail.com
