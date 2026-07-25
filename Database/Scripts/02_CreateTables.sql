
-- Usar la BD TaskFlowDB
USE TaskFlowDB;
GO

--Crear las tablas

-- Tabla Users
CREATE TABLE [TF].[Users](

	[UserID] INT IDENTITY(1,1) NOT NULL,
	[FullName] NVARCHAR(150) NOT NULL,
	[Email] VARCHAR(250) NOT NULL,
	[AddedAt] DATETIME2(0) NOT NULL,
	[IsActive] BIT NOT NULL,
	[DeactivatedAt] DATETIME2(0) NULL,
	
        CONSTRAINT PK_Users
        PRIMARY KEY CLUSTERED (UserID)

);

-- Tabla TaskStatus
CREATE TABLE [TF].[TaskStatus](

	[TaskStatusID] TINYINT NOT NULL,
	[Name] VARCHAR(30) NOT NULL,
	[IsActive] BIT NOT NULL,
	
        CONSTRAINT PK_TaskStatus
        PRIMARY KEY CLUSTERED (TaskStatusID)

);

-- Tabla TaskPriority
CREATE TABLE [TF].[TaskPriority](

	[TaskPriorityID] TINYINT NOT NULL,
	[Name] VARCHAR(30) NOT NULL,
	[IsActive] BIT NOT NULL,
	
        CONSTRAINT PK_TaskPriority
        PRIMARY KEY CLUSTERED (TaskPriorityID)

);

-- Tabla Tasks
CREATE TABLE [TF].[Tasks](

	[TaskID] INT IDENTITY(1,1) NOT NULL,
	[ResponsibleUserID] INT NOT NULL,
	[Title] NVARCHAR(200) NOT NULL,
	[Description] NVARCHAR(500) NULL,
	[TaskPriorityID] TINYINT NOT NULL,
	[TaskStatusID] TINYINT NOT NULL,
	[StartDate] DATETIME2(0) NULL,
	[CompletionDate] DATETIME2(0) NULL,
	[DueDate] DATETIME2(0) NOT NULL,
	[AddedAt] DATETIME2(0) NOT NULL,
	[IsDeleted] BIT NOT NULL,
	[DeletedAt] DATETIME2(0) NULL,
	
        CONSTRAINT PK_Tasks
        PRIMARY KEY CLUSTERED (TaskID)

);

-- Tabla TaskStatusAudit
CREATE TABLE [TF].[TaskStatusAudit](

	[TaskStatusAuditID] BIGINT IDENTITY(1,1) NOT NULL,
	[TaskID] INT NOT NULL,
	[ChangedByUserID] INT NULL,
	[PreviousStatusID] TINYINT NOT NULL,
	[NewStatusID] TINYINT NOT NULL,
	[ChangedAt] DATETIME2(0) NOT NULL,
	
        CONSTRAINT PK_TaskStatusAudit
        PRIMARY KEY CLUSTERED (TaskStatusAuditID)

);

GO