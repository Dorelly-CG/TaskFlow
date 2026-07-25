
-- Usar la BD TaskFlowDB
USE TaskFlowDB;
GO

-- Seed TaskStatus
INSERT INTO [TF].[TaskStatus] 
(TaskStatusID, Name, IsActive)
VALUES
(1, 'Pendiente', 1),
(2, 'En progreso', 1),
(3, 'Terminada', 1)

-- Seed TaskPriority
INSERT INTO [TF].[TaskPriority]
(TaskPriorityID, Name, IsActive)
VALUES
(1, 'Alta', 1),
(2, 'Media', 1),
(3, 'Baja', 1)

GO