CREATE TABLE [dbo].[TotalProduction]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [TotalProduction] FLOAT NOT NULL,
	[WindProduction] FLOAT NOT NULL,
	[WindProductionPercent] FLOAT NOT NULL,
	[TotalCostWithoutRenewable] FLOAT NOT NULL,
	[TotalCostWithRenewable] FLOAT NOT NULL,
	[Profit] FLOAT NOT NULL,
	[TimeOfCalculation] DATETIME NOT NULL, 
)
