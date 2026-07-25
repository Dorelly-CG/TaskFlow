
-- Usar la BD TaskFlowDB
USE TaskFlowDB;
GO


-- Crear el StoredProcedure para obtener el usuario, el total pendientes y el total vencidas

-- -- sp_GetPendingTasks

CREATE OR ALTER PROCEDURE [TF].[sp_GetPendingTasks]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
	U.FullName AS Usuario,
	Count
	(
		CASE
			WHEN T.TaskStatusID <> 3
			THEN 1
		END
	)AS TotalPendientes,
	COUNT
	(
		CASE
			WHEN T.TaskStatusID <> 3
				AND T.DueDate < SYSUTCDATETIME()
			THEN 1
		END
	) AS TotalVencidas
	FROM 
		TF.Users AS U
	LEFT JOIN
		TF.Tasks AS T
			ON T.ResponsibleUserID = U.UserID
			AND T.IsDeleted = 0
	WHERE
		U.IsActive = 1
		GROUP BY
		U.UserID,
		U.FullName
	ORDER BY
		U.FullName

END
GO
