CREATE PROCEDURE [dbo].[InsertMeasurement](
	@gidMeasurement bigint,
	@timeMeasurement datetime,
	@valueMeasurement float
	)
AS
begin
	insert into HistoryMeasurement(GID, MEASUREMENTTIME, MEASUREMENTVALUE)
	values (@gidMeasurement,@timeMeasurement,@valueMeasurement)
end
