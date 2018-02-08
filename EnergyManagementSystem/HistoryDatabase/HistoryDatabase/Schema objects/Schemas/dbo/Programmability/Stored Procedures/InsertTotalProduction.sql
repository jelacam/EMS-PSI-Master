CREATE PROCEDURE [dbo].[InsertTotalProduction](
	@totalProduction float,
	@windProduction float,
	@windProductionPercent float,
	@totalCostWithoutRenewable float,
	@totalCostWithRenewable float,
	@profit float,
	@timeOfCalculation datetime
	)
AS
begin
	insert into TotalProduction(TotalProduction,WindProduction,WindProductionPercent,TotalCostWithoutRenewable,TotalCostWithRenewable,Profit,TimeOfCalculation)
	values (@totalProduction,@windProduction,@windProductionPercent,@totalCostWithoutRenewable,@totalCostWithRenewable,@profit,@timeOfCalculation)
end
