CREATE TABLE [dbo].[Registration] (
	[RegistrationID] [int] IDENTITY(1, 1) NOT NULL
	,[FirstName] [varchar](100) NULL
	,[Mobileno] [varchar](20) NULL
	,[EmailID] [varchar](100) NULL
	,[Username] [varchar](20) NULL
	,[Password] [varchar](100) NULL
	,[ConfirmPassword] [varchar](100) NULL
	,[Gender] [varchar](10) NULL
	,[Birthdate] [datetime] NULL
	,[RoleID] [int] NULL
	,[CreatedOn] [datetime] NULL
	,[EmployeeID] [varchar](10) NULL
	,[DateofJoining] [date] NULL
	,[ForceChangePassword] [int] NULL
	,[DateofLeaving] [date] NULL
	,[IsActive] [bit] NULL
	,[UpdatedDate] [datetime] NULL
	,[LastName] [varchar](100) NULL
	,[MiddleName] [varchar](100) NULL
	,[WorkEmail] [varchar](100) NULL
	,[DeptId] [int] NULL
	,[EmergencyContact] [varchar](100) NULL
	,[EmergencyContactNumber] [varchar](15) NULL
	,[ManagerId] [int] NULL
	,[JobId] [int] NULL
	,CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED ([RegistrationID] ASC) WITH (
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		) ON [PRIMARY]
	) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Registration]
	 ADD CONSTRAINT [FK_Registration_Departments] FOREIGN KEY ([DeptId]) REFERENCES [dbo].[Department]([DeptId])
GO

ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Departments]
GO

ALTER TABLE [dbo].[Registration]
	 ADD CONSTRAINT [FK_Registration_Jobs] FOREIGN KEY ([JobId]) REFERENCES [dbo].[Jobs]([JobId])
GO

ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Jobs]
GO

ALTER TABLE [dbo].[Registration] 
 ADD CONSTRAINT [FK_Registration_Roles] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles]([RoleID])
GO

ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Roles]
GO


