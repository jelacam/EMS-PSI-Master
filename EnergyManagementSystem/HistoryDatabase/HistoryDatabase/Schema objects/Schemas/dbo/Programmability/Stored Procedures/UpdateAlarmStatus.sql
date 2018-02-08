CREATE PROCEDURE [dbo].UpdateAlarmStatus(
	@gid bigint,
	@currentState varchar(50),
	@pubStatus int
	)
AS
begin
	update Alarms
	set CurrentState = @currentState,
		PubStatus = @pubStatus
	where GID=@gid;
end
