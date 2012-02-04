ALTER TABLE [dbo].[Contacts]
    ADD CONSTRAINT [Contact_Address] FOREIGN KEY ([Address_ID]) REFERENCES [dbo].[Addresses] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

