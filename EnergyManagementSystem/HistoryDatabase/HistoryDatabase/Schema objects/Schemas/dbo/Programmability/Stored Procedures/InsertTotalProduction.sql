CREATE PROCEDURE [dbo].[InsertTotalProduction]
(
    @totalProduction float,
    @totalCost float,
    @totalCostWithoutWindAndSolar float,
    @profit float,
    @timeOfCalculation datetime,
    @windProduction float,
    @windProductionPercent float,
    @solarProduction float,
    @solarProductionPercent float,
    @hydroProduction float,
    @hydroProductionPercent float,
    @coalProduction float,
    @coalProductionPercent float,
    @oilProduction float,
    @oilProductionPercent float
)
AS
begin
    insert into TotalProduction (
    TotalProduction,
    TotalCost,
    TotalCostWithoutWindAndSolar,
    Profit,
    TimeOfCalculation,
    WindProduction,
    WindProductionPercent,
    SolarProduction,
    SolarProductionPercent,
    HydroProduction,
    HydroProductionPercent,
    CoalProduction,
    CoalProductionPercent,
    OilProduction,
    OilProductionPercent)
    values (
    @totalProduction,
    @totalCost,
    @totalCostWithoutWindAndSolar,
    @profit,
    @timeOfCalculation,
    @windProduction,
    @windProductionPercent,
    @solarProduction,
    @solarProductionPercent,
    @hydroProduction,
    @hydroProductionPercent,
    @coalProduction,
    @coalProductionPercent,
    @oilProduction,
    @oilProductionPercent)
end