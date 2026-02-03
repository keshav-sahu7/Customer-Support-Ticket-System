-- Create Database
CREATE DATABASE IF NOT EXISTS csts_db;
USE csts_db;

-- Create Users Table
CREATE TABLE IF NOT EXISTS Users (
    Id CHAR(36) PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role ENUM('User', 'Admin') NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Create Tickets Table
CREATE TABLE IF NOT EXISTS Tickets (
    TicketId INT AUTO_INCREMENT PRIMARY KEY,
    Subject VARCHAR(255) NOT NULL,
    Description TEXT,
    Priority ENUM('Low', 'Medium', 'High') NOT NULL,
    Status ENUM('Open', 'In Progress', 'Closed') NOT NULL DEFAULT 'Open',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    CreatedById CHAR(36) NOT NULL,
    AssignedToId CHAR(36) NULL,
    FOREIGN KEY (CreatedById) REFERENCES Users(Id),
    FOREIGN KEY (AssignedToId) REFERENCES Users(Id)
);

-- Create TicketComments Table
CREATE TABLE IF NOT EXISTS TicketComments (
    CommentId INT AUTO_INCREMENT PRIMARY KEY,
    TicketId INT NOT NULL,
    UserId CHAR(36) NOT NULL, -- User who made the comment
    CommentText TEXT NOT NULL,
    IsInternal BOOLEAN DEFAULT FALSE, -- True for admin internal comments, False for user comments
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TicketId) REFERENCES Tickets(TicketId),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Create TicketStatusHistory Table
CREATE TABLE IF NOT EXISTS TicketStatusHistory (
    HistoryId INT AUTO_INCREMENT PRIMARY KEY,
    TicketId INT NOT NULL,
    ChangedByUserId CHAR(36) NOT NULL,
    OldStatus ENUM('Open', 'In Progress', 'Closed') NOT NULL,
    NewStatus ENUM('Open', 'In Progress', 'Closed') NOT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TicketId) REFERENCES Tickets(TicketId),
    FOREIGN KEY (ChangedByUserId) REFERENCES Users(Id)
);

-- Insert initial admin user (password 'root')
INSERT INTO Users (Id, Username, PasswordHash, Role) VALUES ('a1b2c3d4-e5f6-7890-1234-567890abcdef', 'admin', '$2a$11$w1R.y0H7R3Z9z2M0L5O7Z.u.X.s.M.w.k.F.g.T.l.i.j.p.o.', 'Admin');

-- Insert initial regular user (password 'user123')
INSERT INTO Users (Id, Username, PasswordHash, Role) VALUES ('f1e2d3c4-b5a6-0987-6543-210fedcba987', 'user', '$2a$11$e7K.0a.x.p.y.v.j.q.R.b.S.t.A.c.D.f.G.h.I.o.m.n.', 'User');
