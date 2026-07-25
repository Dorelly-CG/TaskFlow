

-- Crear DB para las entidades
USE master
Go
IF DB_ID(N'TaskFlowDB') IS NULL
BEGIN
	CREATE DATABASE TaskFlowDB;
END
Go


-- Comprobar que se creó y usarla
USE TaskFlowDB
GO


-- Crear schema para las entidades
CREATE SCHEMA TF;
GO


-- Comprobar que se creó
SELECT
    schema_id,
    name
FROM sys.schemas
WHERE name like '%TFDB%';
