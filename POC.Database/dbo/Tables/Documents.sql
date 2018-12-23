CREATE TABLE [dbo].[Documents](
	[DocumentId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentBytes] [varbinary](max) NULL,
	[AssignedToId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ProjectId] [int] NULL,
	[DocumentTypeId] [int] NULL,
	[DocumentTitle] [varchar](1000) NULL,
	[IsDeleted] [bit] NULL,
	[FileNameUrl] [varchar](1000) NULL,
	[UploadedBy] [int] NULL,
	[DocumentDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [df_Documents_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO


