CREATE TABLE [dbo].[Alarms]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[GID] bigint not null,
	[AlarmValue] float not null,
	[MinValue] float not null,
	[MaxValue] float not null,
	[AlarmTimeStamp] datetime not null,
	[Severity] int null,
	[InitiatingValue] float null,
	[LastChange] datetime null,
	[CurrentState] VARCHAR(50) NOT null,
	[AckState] int NOT null,
	[PubStatus] int null,
	[AlarmType] int null,
	[Persistent] int null,
	[Inhibit] int null,
	[AlarmMessage] NTEXT null
)
