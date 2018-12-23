CREATE TABLE [dbo].[AssignedProjects](
	[AssignedProjectID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[ManagerId] [int] NULL,
	[Status] [varchar](1) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_AssignedProject] PRIMARY KEY CLUSTERED 
(
	[AssignedProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[AssignedProjects]  WITH CHECK ADD  CONSTRAINT [FK_AssignedProjects_ProjectMaster] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[ProjectMaster] ([ProjectID])
GO

ALTER TABLE [dbo].[AssignedProjects] CHECK CONSTRAINT [FK_AssignedProjects_ProjectMaster]
GO
