IF (
		SELECT COUNT(*)
		FROM dbo.DocumentType
		) < 1
BEGIN


INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Project trainings', CAST(0x0000A9BB01408BAE AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Risk Management', CAST(0x0000A9BB01408BB5 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Issues Log', CAST(0x0000A9BB01408BB5 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Project Budget', CAST(0x0000A9BB01408BB5 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Communication Plan', CAST(0x0000A9BB01408BB6 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Project Status Report', CAST(0x0000A9BB01408BB6 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Meeting Agenda/Minutes', CAST(0x0000A9BB01408BB6 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Quality Assurance (QA) Test Plan', CAST(0x0000A9BB01408BB6 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Project Management Plan', CAST(0x0000A9BB01408BB6 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Functional Specification Document', CAST(0x0000A9BB01408BB6 AS DateTime), 0)
INSERT [dbo].[DocumentType] ( [DocumentType], [CreatedOn], [IsDeleted]) VALUES ( N'Business Requirement Specification Document', CAST(0x0000A9BB01408BB6 AS DateTime), 0)

END