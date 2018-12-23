CREATE TABLE [dbo].[Task](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[Taskname] [varchar](max) NULL,
	[AssignedtoID] [int] NULL,
	[CreatedByID] [int] NULL,
	[Startdate] [datetime] NULL,
	[Enddate] [datetime] NULL,
	[CompletedDate] [datetime] NULL,
	[EstimatedHours] [datetime] NULL,
	[Actualhours] [datetime] NULL,
	[Status] [int] NULL,
	[Percentage] [int] NULL,
	[ProjectID] [int] NULL,
	[Comments] [varchar](max) NULL,
	[TaskGroupID] [int] NULL,
	[Priority] [varchar](100) NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBY] [varchar](100) NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [df_Task_Status]  DEFAULT ((1)) FOR [Status]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [df_Task_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [df_Task_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [df_Task_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[Task]  ADD  CONSTRAINT [FK_Task_ProjectMaster] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[ProjectMaster] ([ProjectID])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_ProjectMaster]
GO