CREATE TABLE [dbo].[Locations] (
    [LocationId]     INT            IDENTITY (1, 1) NOT NULL,
    [AddressLine1]   VARCHAR (1000) NULL,
    [AddressLine2]   VARCHAR (1000) NULL,
    [City]           VARCHAR (250)  NULL,
    [StateId]        INT            NULL,
    [CountryId]      INT            NULL,
    [ZipCode]        VARCHAR (12)   NULL,
    [RegistrationID] INT            NULL,
    [AddressType]    VARCHAR (50)   NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([LocationId] ASC)
);

