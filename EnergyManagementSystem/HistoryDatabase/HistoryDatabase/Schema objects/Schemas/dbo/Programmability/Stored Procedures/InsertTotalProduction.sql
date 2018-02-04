CREATE PROCEDURE [dbo].[InsertTotalProduction](
	@totalProduction float,
	@timeOfCalculation datetime
	)
AS
begin
	insert into TotalProduction(TotalProduction, TimeOfCalculation)
	values (@totalProduction,@timeOfCalculation)
end
