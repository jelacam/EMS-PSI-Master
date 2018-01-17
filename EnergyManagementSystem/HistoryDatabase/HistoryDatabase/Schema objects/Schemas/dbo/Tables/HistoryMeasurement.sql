CREATE TABLE [dbo].[HistoryMeasurement]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [GID] BIGINT NOT NULL, 
    [MeasurementTime] DATETIME NOT NULL, 
    [MeasurementValue] FLOAT NOT NULL
)
