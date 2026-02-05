
-- Create User Table
CREATE TABLE `User` (
    `Id` CHAR(36) NOT NULL,
    `Username` VARCHAR(50) NOT NULL,
    `PasswordHash` TEXT NOT NULL,
    `Role` INT NOT NULL,
    PRIMARY KEY (`Id`)
);

-- Create Ticket Table
CREATE TABLE `Ticket` (
    `Id` CHAR(36) NOT NULL,
    `TicketNumber` VARCHAR(6) NOT NULL UNIQUE,
    `Subject` VARCHAR(100) NOT NULL,
    `Description` TEXT NOT NULL,
    `Priority` INT NOT NULL,
    `Status` INT NOT NULL,
    `CreatedAt` DATETIME NOT NULL,
    `CreatedById` CHAR(36) NOT NULL,
    `AssignedToId` CHAR(36),
    PRIMARY KEY (`Id`),
    FOREIGN KEY (`CreatedById`) REFERENCES `User`(`Id`),
    FOREIGN KEY (`AssignedToId`) REFERENCES `User`(`Id`)
);

-- Create TicketComment Table
CREATE TABLE `TicketComment` (
    `Id` CHAR(36) NOT NULL,
    `Content` TEXT NOT NULL,
    `CreatedAt` DATETIME NOT NULL,
    `TicketId` CHAR(36) NOT NULL,
    `CreatedById` CHAR(36) NOT NULL,
    PRIMARY KEY (`Id`),
    FOREIGN KEY (`TicketId`) REFERENCES `Ticket`(`Id`),
    FOREIGN KEY (`CreatedById`) REFERENCES `User`(`Id`)
);

-- Create TicketStatusHistory Table
CREATE TABLE `TicketStatusHistory` (
    `Id` CHAR(36) NOT NULL,
    `TicketId` CHAR(36) NOT NULL,
    `OldStatus` INT NOT NULL,
    `NewStatus` INT NOT NULL,
    `ChangedAt` DATETIME NOT NULL,
    `ChangedById` CHAR(36) NOT NULL,
    PRIMARY KEY (`Id`),
    FOREIGN KEY (`TicketId`) REFERENCES `Ticket`(`Id`),
    FOREIGN KEY (`ChangedById`) REFERENCES `User`(`Id`)
);

-- Create TicketAssignmentHistory Table
CREATE TABLE `TicketAssignmentHistory` (
    `Id` CHAR(36) NOT NULL,
    `TicketId` CHAR(36) NOT NULL,
    `OldAssignedToId` CHAR(36),
    `NewAssignedToId` CHAR(36),
    `ChangedAt` DATETIME NOT NULL,
    `ChangedById` CHAR(36) NOT NULL,
    PRIMARY KEY (`Id`),
    FOREIGN KEY (`TicketId`) REFERENCES `Ticket`(`Id`),
    FOREIGN KEY (`OldAssignedToId`) REFERENCES `User`(`Id`),
    FOREIGN KEY (`NewAssignedToId`) REFERENCES `User`(`Id`),
    FOREIGN KEY (`ChangedById`) REFERENCES `User`(`Id`)
);



-- Insert initial admin user (password 'root')
INSERT INTO Users (Id, Username, PasswordHash, Role) VALUES (UUID(), 'admin', '$2a$11$tEOVZdU5mjU/kIhBlOMiKOBly2hVPdzPOnwxncdgZ2p59rUsfQSfa', 'admin');

-- Insert initial regular user (password 'user123')
INSERT INTO Users (Id, Username, PasswordHash, Role) VALUES (UUID(), 'John', '$2a$11$s.ej8.WhAvtlbjtQi3YLxOnGz4iz/k4/lUrdWZsM014d0UDfdoPre', 'user');

