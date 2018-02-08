CREATE PROCEDURE [dbo].UpdateAlarm(
	@gid bigint,
	@alarmValue float,
	@lastChange datetime,
	@currentState varchar(50)
	)
AS
begin
	update Alarms
	set AlarmValue = @alarmValue,
		LastChange = @lastChange,
		CurrentState = @currentState
	where GID=@gid;
end
