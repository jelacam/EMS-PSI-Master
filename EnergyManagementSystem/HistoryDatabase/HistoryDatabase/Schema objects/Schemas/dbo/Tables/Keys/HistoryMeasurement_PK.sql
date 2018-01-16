ALTER TABLE HistoryMeasurement ADD CONSTRAINT HistoryMeasurement_PK PRIMARY KEY CLUSTERED (Id)
WITH
  (
    ALLOW_PAGE_LOCKS = ON ,
    ALLOW_ROW_LOCKS  = ON
  )
  ON "default"
GO
