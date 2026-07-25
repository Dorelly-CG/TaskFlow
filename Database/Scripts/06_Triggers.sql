
-- Usar la BD TaskFlowDB
USE TaskFlowDB;
GO

-- Trigger para insertar los cambios de estad de una Task en TaskStatusAudit

-- -- TaskStatusAudit
CREATE TRIGGER [TF].[TR_Tasks_TaskStatusAudit]
ON [TF].[Tasks]
AFTER UPDATE
AS
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO [TF].[TaskStatusAudit]
	(TaskID, PreviousStatusID, NewStatusID)

	SELECT 
	i.TaskID, d.TaskStatusID, i.TaskStatusID

	FROM 
		inserted AS i
	INNER JoIN 
		deleted AS d
	ON
		i.TaskID = d.TaskID
	WHERE
		i.TaskStatusID <> d.TaskStatusID

END
GO