CREATE TABLE [dbo].[TotalProduction]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [TotalProduction] FLOAT NOT NULL,	
	[TotalCost] FLOAT NOT NULL,
	[TotalCostWithoutWindAndSolar] FLOAT NOT NULL,
	[Profit] FLOAT NOT NULL,
	[TimeOfCalculation] DATETIME NOT NULL, 
    [WindProduction] FLOAT NULL,
	[WindProductionPercent] FLOAT NULL,
    [SolarProduction] FLOAT NULL,
	[SolarProductionPercent] FLOAT NULL,
    [HydroProduction] FLOAT NULL,
	[HydroProductionPercent] FLOAT NULL,
    [CoalProduction] FLOAT NULL,
	[CoalProductionPercent] FLOAT NULL,
    [OilProduction] FLOAT NULL,
	[OilProductionPercent] FLOAT NULL,
)
