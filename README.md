🔐 SafeVault – Secure Web Application

SafeVault is a secure ASP.NET Core MVC web application developed in three progressive security-focused activities.
The project demonstrates secure coding practices, authentication & authorization implementation, and vulnerability debugging to protect against common web attacks such as SQL Injection and Cross-Site Scripting (XSS).

📌 Project Overview

SafeVault manages sensitive user data (credentials, financial records, and notes) while enforcing strong security controls.

The project was developed in three stages:

Activity	Focus Area
Activity 1	Secure coding (Input validation, SQL injection prevention, XSS mitigation)
Activity 2	Authentication & Role-Based Authorization (RBAC)
Activity 3	Vulnerability debugging and security hardening
🛡️ Security Features Implemented
✅ Activity 1 – Secure Coding

Input validation using DataAnnotations

Custom validation attributes

Input sanitization

Parameterized SQL queries (SqlParameter)

Secure HTTP headers (CSP, X-Frame-Options, etc.)

NUnit tests for:

SQL injection attempts

XSS payload handling

🔑 Activity 2 – Authentication & Authorization
Authentication

Cookie-based authentication

Secure password hashing using BCrypt

Claims-based identity

Anti-forgery token protection

Secure cookie configuration (HttpOnly, Secure, SameSite)

Authorization (RBAC)

Role-based policies (AdminOnly)

Admin dashboard protected by [Authorize(Policy = "AdminOnly")]

Access denied handling

Tests

Unit tests for AuthService

Password hash verification tests

Unauthorized endpoint access tests

🔍 Activity 3 – Vulnerability Debugging & Fixes
Identified Vulnerabilities

SQL injection risk from unsafe query construction

Stored XSS risk from raw HTML rendering

Fixes Applied

Fully parameterized LIKE search queries

Removed all usage of Html.Raw(...)

Relied on Razor's automatic HTML encoding

Added strict input length validation

Security Tests Added

SQL injection simulation test (%' OR 1=1;--)

XSS prevention guard test

Static query validation test

🏗️ Architecture
SafeVault.sln
 ├── SafeVault.Web        (ASP.NET Core MVC)
 ├── SafeVault.Data       (Data access layer)
 └── SafeVault.Tests      (NUnit tests)

Technology Stack

.NET 8 / ASP.NET Core MVC

SQL Server 2022

Microsoft.Data.SqlClient

BCrypt.Net-Next

NUnit Testing Framework

🗄️ Database Setup
1️⃣ Create Database
CREATE DATABASE SafeVault;

2️⃣ Run Scripts

Execute in order:

database.sql (Activity 1)

database_v2_auth.sql (Activity 2)

database_v3_notes.sql (Activity 3)

⚙️ Configuration

Update connection string in:

SafeVault.Web/appsettings.json


Example:

"ConnectionStrings": {
  "SafeVaultDb": "Server=localhost;Database=SafeVault;Trusted_Connection=True;TrustServerCertificate=True;"
}

🚀 Running the Application

Open solution in Visual Studio 2022

Set SafeVault.Web as startup project

Press F5

Navigate to:

https://localhost:xxxx

🧪 Running Tests

Open Test Explorer

Click Run All

Tests verify:

Authentication logic

Role-based authorization

SQL injection prevention

XSS mitigation

🔒 Security Highlights

✔ Passwords are hashed using BCrypt (never stored in plaintext)
✔ SQL queries use parameterized statements only
✔ Razor encoding prevents stored XSS
✔ Anti-forgery tokens prevent CSRF
✔ Secure cookies configured
✔ Role-based access control implemented
✔ Defensive coding with layered validation

📚 Learning Outcomes

This project demonstrates:

Secure web application development

Defense-in-depth strategy

Authentication and authorization design

Vulnerability identification and mitigation

Security-focused testing methodology

👩‍💻 Author

Piyumi Darshika
MSc Student – Cardiff Metropolitan University
Full-Stack Software Engineer (.NET, SQL Server, Security Practices)
