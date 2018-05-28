CREATE TABLE `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `Users` (
    `UserId` char(36) NOT NULL,
    `AccountStatus` int NOT NULL,
    `AccountType` int NOT NULL,
    `Created` datetime(6) NOT NULL,
    `CreatedBy` longtext NOT NULL,
    `DateOfBirth` datetime(6),
    `EmailAddress` longtext NOT NULL,
    `FirstName` longtext NOT NULL,
    `Gender` tinyint unsigned NOT NULL,
    `LastName` longtext NOT NULL,
    `LastUpdate` datetime(6) NOT NULL,
    `PhoneNumber` longtext NOT NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`UserId`)
);

CREATE TABLE `Doctors` (
    `DoctorId` int NOT NULL AUTO_INCREMENT,
    `ProfileSummary` longtext,
    `UserAccountUserId` char(36),
    `UserId` longtext,
    CONSTRAINT `PK_Doctors` PRIMARY KEY (`DoctorId`),
    CONSTRAINT `FK_Doctors_Users_UserAccountUserId` FOREIGN KEY (`UserAccountUserId`) REFERENCES `Users` (`UserId`) ON DELETE NO ACTION
);

CREATE TABLE `Patients` (
    `PatientId` int NOT NULL AUTO_INCREMENT,
    `BloodGroup` longtext,
    `Occupation` longtext,
    `PatientType` int NOT NULL,
    `UserAccountUserId` char(36),
    `UserId` int NOT NULL,
    CONSTRAINT `PK_Patients` PRIMARY KEY (`PatientId`),
    CONSTRAINT `FK_Patients_Users_UserAccountUserId` FOREIGN KEY (`UserAccountUserId`) REFERENCES `Users` (`UserId`) ON DELETE NO ACTION
);

CREATE TABLE `Appointments` (
    `PatientId` int NOT NULL,
    `DoctorId` int NOT NULL,
    `AilmentDescription` longtext,
    `Created` datetime(6) NOT NULL,
    `ScheduledDate` datetime(6) NOT NULL,
    `Status` int NOT NULL,
    `Symptoms` longtext,
    CONSTRAINT `PK_Appointments` PRIMARY KEY (`PatientId`, `DoctorId`),
    CONSTRAINT `FK_Appointments_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`DoctorId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Appointments_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`PatientId`) ON DELETE CASCADE
);

CREATE INDEX `IX_Appointments_DoctorId` ON `Appointments` (`DoctorId`);

CREATE INDEX `IX_Doctors_UserAccountUserId` ON `Doctors` (`UserAccountUserId`);

CREATE INDEX `IX_Patients_UserAccountUserId` ON `Patients` (`UserAccountUserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20180528023359_migv1-DbCreate', '2.0.1-rtm-125');

