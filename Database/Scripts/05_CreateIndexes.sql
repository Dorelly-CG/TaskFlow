
-- Usar la BD TaskFlowDB
USE TaskFlowDB;
GO

-- Indexes

-- -- Tasks
CREATE NONCLUSTERED INDEX IX_Tasks_ResponsibleUserID
ON [TF].[Tasks] (ResponsibleUserID)

CREATE NONCLUSTERED INDEX IX_Tasks_TaskStatusID
ON [TF].[Tasks] (TaskStatusID)

CREATE NONCLUSTERED INDEX IX_Tasks_TaskPriorityID
ON [TF].[Tasks] (TaskPriorityID)

GO

-- -- TaskStatusAudit
CREATE NONCLUSTERED INDEX IX_TaskStatusAudit_TaskID
ON [TF].[TaskStatusAudit] (TaskID)

GO