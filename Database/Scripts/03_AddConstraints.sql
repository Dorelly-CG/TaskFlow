
-- Usar la BD TaskFlowDB
USE TaskFlowDB;
GO


-- Crear los Default

-- -- Users
ALTER TABLE [TF].[Users]
ADD CONSTRAINT DF_Users_AddedAt
DEFAULT (SYSUTCDATETIME()) FOR AddedAt

ALTER TABLE [TF].[Users]
ADD CONSTRAINT DF_Users_IsActive
DEFAULT (1) FOR IsActive

-- -- TaskStatus
ALTER TABLE [TF].[TaskStatus]
ADD CONSTRAINT DF_TaskStatus_IsActive
DEFAULT (1) FOR IsActive

-- -- TaskPriority
ALTER TABLE [TF].[TaskPriority]
ADD CONSTRAINT DF_TaskPriority_IsActive
DEFAULT (1) FOR IsActive

-- -- Tasks
ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT DF_Tasks_AddedAt
DEFAULT (SYSUTCDATETIME()) FOR AddedAt

ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT DF_Tasks_IsDeleted
DEFAULT (0) FOR IsDeleted

-- -- TaskStatusAudit
ALTER TABLE [TF].[TaskStatusAudit]
ADD CONSTRAINT DF_TaskStatusAudit_ChangedAt
DEFAULT (SYSUTCDATETIME()) FOR ChangedAt

GO



-- Crear los Unique

-- -- Users
ALTER TABLE [TF].[Users]
ADD CONSTRAINT UQ_Users_Email
UNIQUE (Email)

-- -- TaskStatus
ALTER TABLE [TF].[TaskStatus]
ADD CONSTRAINT UQ_TaskStatus_Name
UNIQUE (Name)

-- -- TaskPriority
ALTER TABLE [TF].[TaskPriority]
ADD CONSTRAINT UQ_TaskPriority_Name
UNIQUE (Name)


GO



-- Crear las Foreign Keys

-- -- Tasks
ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT FK_Tasks_Users
FOREIGN KEY (ResponsibleUserID)
REFERENCES [TF].[Users] (UserID)

ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT FK_Tasks_TaskPriority
FOREIGN KEY (TaskPriorityID)
REFERENCES [TF].[TaskPriority] (TaskPriorityID)

ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT FK_Tasks_TaskStatus
FOREIGN KEY (TaskStatusID)
REFERENCES [TF].[TaskStatus] (TaskStatusID)

-- -- TaskStatusAudit
ALTER TABLE [TF].[TaskStatusAudit]
ADD CONSTRAINT FK_TaskStatusAudit_Tasks
FOREIGN KEY (TaskID)
REFERENCES [TF].[Tasks] (TaskID)

ALTER TABLE [TF].[TaskStatusAudit]
ADD CONSTRAINT FK_TaskStatusAudit_ChangedByUser
FOREIGN KEY (ChangedByUserID)
REFERENCES [TF].[Users] (UserID)

ALTER TABLE [TF].[TaskStatusAudit]
ADD CONSTRAINT FK_TaskStatusAudit_PreviousStatus
FOREIGN KEY (PreviousStatusID)
REFERENCES [TF].[TaskStatus] (TaskStatusID)

ALTER TABLE [TF].[TaskStatusAudit]
ADD CONSTRAINT FK_TaskStatusAudit_NewStatus
FOREIGN KEY (NewStatusID)
REFERENCES [TF].[TaskStatus] (TaskStatusID)


GO



-- Crear los Check

-- -- Users
ALTER TABLE [TF].[Users]
ADD CONSTRAINT CK_Users_ActiveState
CHECK
(
	(IsActive = 1 AND DeactivatedAt IS NULL)
	OR
	(IsActive = 0 AND DeactivatedAt IS NOT NULL)
)

-- -- Tasks
ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT CK_Tasks_StartDate
CHECK
(
	StartDate IS NULL
	OR StartDate >= AddedAt
)

ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT CK_Tasks_CompletionDate
CHECK
(
	CompletionDate IS NULL
	OR
	(StartDate IS NOT NULL AND CompletionDate >= StartDate)
)

--ALTER TABLE [TF].[Tasks]
--ADD CONSTRAINT CK_Tasks_DueDate
--CHECK
--(
--	DueDate >= AddedAt
--)

ALTER TABLE [TF].[Tasks]
ADD CONSTRAINT CK_Tasks_DeletedState
CHECK
(
	(IsDeleted = 0 AND DeletedAt IS NULL)
	OR
	(IsDeleted = 1 AND DeletedAt IS NOT NULL)
)


-- -- TaskStatusAudit
ALTER TABLE [TF].[TaskStatusAudit]
ADD CONSTRAINT CK_TaskStatusAudit_StatusChanged
CHECK
(
	PreviousStatusID <> NewStatusID
)


GO