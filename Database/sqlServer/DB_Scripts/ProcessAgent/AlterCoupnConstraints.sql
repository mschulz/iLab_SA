ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT [FK_Ticket_Coupon]

GO

ALTER TABLE [dbo].[Coupon] DROP CONSTRAINT [PK_Coupon]

GO

ALTER TABLE [dbo].[Coupon] ADD 
	CONSTRAINT [IX_Coupon] UNIQUE  NONCLUSTERED 
	(
		[Coupon_ID],
		[Issuer_GUID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Ticket] ADD 
	CONSTRAINT [FK_Ticket_Coupon] FOREIGN KEY 
	(
		[Coupon_ID], [Issuer_GUID]
	) REFERENCES [dbo].[Coupon] (
		[Coupon_ID] , [Issuer_GUID]
	) ON DELETE CASCADE  ON UPDATE CASCADE
go