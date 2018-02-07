CREATE PROCEDURE [dbo].InsertAlarm(
	@gid bigint,
	@alarmValue float,
	@minValue float,
	@maxValue float,
	@timeStamp datetime,
	@severity int,
	@initiatingValue float,
	@lastChange datetime,
	@currentState varchar(50),
	@ackState int,
	@pubStatus int,
	@alarmType int,
	@persistent int,
	@inhibit int,
	@message ntext
	)
AS
begin
	insert into Alarms(GID,AlarmValue,MinValue,MaxValue,AlarmTimeStamp,Severity,InitiatingValue,LastChange,CurrentState,AckState,PubStatus,AlarmType,Persistent,Inhibit,AlarmMessage)
	values (@gid,@alarmValue,@minValue,@maxValue,@timeStamp,@severity,@initiatingValue,@lastChange,@currentState,@ackState,@pubStatus,@alarmType,@persistent,@inhibit,@message)
end
