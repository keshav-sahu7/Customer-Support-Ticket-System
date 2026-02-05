# Customer Support Ticket System

## Project Overview
This project implements a Customer Support Ticket System consisting of a Windows Desktop Application (WPF) as the frontend and an ASP.NET Web API as the backend, utilizing MySQL as the database. The desktop application communicates exclusively with the Web API, with no direct database access.

The system evaluates understanding of C# desktop application development, REST API usage, database design and data handling, and business logic implementation.

## Technology Stack
*   **Frontend:** C# Desktop Application (WPF)
*   **Backend:** ASP.NET Web API / Minimal API
*   **Database:** MySQL
*   **Data Communication:** JSON over HTTP

## User Roles and Functionality
The system supports two primary user roles:

### User
*   Create support tickets.
*   View tickets they have created.
*   Add comments to their own tickets.

### Admin
*   View all tickets.
*   Assign tickets to other users.
*   Update ticket status.
*   Add internal comments.



## Database Design
The database design includes the following suggested tables, with primary keys, foreign keys, and proper relationships expected:
*   `Users`
*   `Tickets`
*   `TicketStatusHistory`
*   `TicketComments`
*   `TicketAssignmentHistory`

## Steps to Run the Project Locally

### 1. Database Setup
1.  Ensure you have a MySQL server running (e.g., via XAMPP, Docker, or a standalone installation).
2.  Create a new database for the application (e.g., `csts_db`).
3.  Execute the `generated_schema.sql` file in the project root to set up the necessary tables and relationships.
    ```bash
    mysql -u username -p database_name < generated_schema.sql
    ```
    (Replace `username` and `database_name` with your MySQL credentials and database name.)

### 2. Backend (ASP.NET Web API) Setup
1.  Navigate to the `CSTS Server` directory:
    ```bash
    cd "CSTS Server"
    ```
2.  Update the connection string in `appsettings.json` or `appsettings.Development.json` to point to your MySQL database.
3.  Build the project:
    ```bash
    dotnet build
    ```
4.  Run the API:
    ```bash
    dotnet run
    ```
    The API should start and listen on the configured port (e.g. `http://localhost:5171`).

### 3. Frontend (WPF Desktop Application) Setup
1.  Navigate to the `CSTS Client` directory:
    ```bash
    cd "CSTS Client"
    ```
2.  Open the project in Visual Studio.
3.  Ensure the `appsettings.json` points to the correct API endpoint (e.g., `http://localhost:5171`).
4.  Build and run the application from Visual Studio.

## Assumptions or Design Decisions
*   Initial user roles (User, Admin) are handled based on the `UserRole` enum.
*   GUIDs are used for primary keys in all entities.


## Queries

See [Sql Queries](queries.sql)





