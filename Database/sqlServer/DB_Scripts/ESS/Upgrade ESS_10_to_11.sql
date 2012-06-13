-- Upgrade_ESS_10_to_11.sql

CREATE TABLE [DBO].[MIME_TYPE](
 	[Mime_ID] [int] IDENTITY (1, 1) NOT NULL ,
 	[Type] [varchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 	[Extension] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
 ) ON [PRIMARY]
 GO
 
 SET IDENTITY_INSERT Mime_Type ON
 	INSERT INTO Mime_Type(Mime_ID, Type) VALUES (0,'unknown');
 SET IDENTITY_INSERT Mime_Type OFF
 GO

ALTER TABLE [dbo].[Mime_Type] WITH NOCHECK ADD 
 	CONSTRAINT [PK_Mime_Type] PRIMARY KEY  CLUSTERED 
 	(
 		[Mime_ID]
 	)  ON [PRIMARY] 
 GO

alter table Experiment_BLOBs
add Blob_Status int DEFAULT 0 NOT NULL
GO

alter table Experiment_BLOBs
add Mime_ID int DEFAULT 0 NOT NULL
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PK_Record_Attributes]') and OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [dbo].[Record_Attributes] DROP CONSTRAINT PK_Record_Attributes
GO

ALTER TABLE Record_Attributes
ALTER COLUMN Attribute_ID bigint
GO

ALTER TABLE [dbo].[Record_Attributes] WITH NOCHECK ADD 
	CONSTRAINT [PK_Record_Attributes] PRIMARY KEY  CLUSTERED 
	(
		[Attribute_ID]
	)  ON [PRIMARY] 
GO


ALTER TABLE Record_Attributes
add EssExp_ID bigint null
go

UPDATE Record_Attributes set EssExp_ID = (select EssExp_Id from Experiment_records where Experiment_Records.Record_ID = Record_Attributes.Record_ID)
go

ALTER TABLE Record_Attributes
ALTER COLUMN EssExp_ID bigint NOT NULL
GO

ALTER TABLE [dbo].[Record_Attributes] ADD 
CONSTRAINT [FK_Record_Attributes_Experiment_Records] FOREIGN KEY 
	(
		[Record_ID]
	) REFERENCES [dbo].[Experiment_Records] (
		[Record_ID]
	) 
	



GO
ALTER TABLE [dbo].[Record_Attributes] ADD 

	CONSTRAINT [FK_Record_Attributes_Experiment_ID] FOREIGN KEY 
	(
		[EssExp_ID]
	) REFERENCES [dbo].[Experiments] (
		[EssExp_ID]
	) ON DELETE CASCADE 
GO

CREATE UNIQUE INDEX [Sequence_IDX] ON [dbo].[Experiment_Records] ([ESSExp_ID], [Sequence_No])
GO

ALTER TABLE [dbo].[Experiment_Blobs] ADD 
CONSTRAINT [FK_Experiment_Blobs_Mime_Type] FOREIGN KEY 
	(
		[Mime_ID]
	) REFERENCES [dbo].[Mime_Type] (
		[Mime_ID]
	)
GO