CREATE TABLE [dbo].[CO2Emission]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[NonRenewable] float NOT NULL,
	[WithRenewable] float NOT NULL,
	[MeasurementTime] datetime NOT NULL,
)
