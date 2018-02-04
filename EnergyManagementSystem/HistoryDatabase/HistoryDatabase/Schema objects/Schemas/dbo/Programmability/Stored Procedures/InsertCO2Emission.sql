CREATE PROCEDURE [dbo].[InsertCO2Emission](
	@nonRenewable float,
	@renewable float,
	@timeOfMeasurement datetime
	)
AS
begin
	insert into CO2Emission(NonRenewable, WithRenewable,MeasurementTime)
	values (@nonRenewable,@renewable,@timeOfMeasurement)
end
