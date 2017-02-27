/****** Object:  Table [dbo].[Appointments]    Script Date: 2/6/2017 11:33:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentID] [uniqueidentifier] NOT NULL,
	[AppointmentComment] [nvarchar](max) NULL,
	[AppointmentDate] [datetime2](7) NOT NULL,
	[AppointmentHoursDispensed] [int] NOT NULL,
	[AppointmentTE] [decimal](18, 2) NOT NULL,
	[AppointmentUserUniqueName] [nvarchar](450) NOT NULL,
	[AppointmentWorkloadWBID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[AppointmentID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  View [dbo].[AllocationTE]    Script Date: 2/6/2017 11:33:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [dbo].[AllocationTE] AS
SELECT AppointmentUserUniqueName AS 'User', SUM(AppointmentHoursDispensed) AS 'Hours_Last_7_Days', CAST(SUM(AppointmentHoursDispensed) AS Float)/(0.4) AS 'Perc_Allocation'
FROM Appointments
WHERE AppointmentDate BETWEEN (Getdate() - 6) AND Getdate() 
GROUP BY AppointmentUserUniqueName
GO
/****** Object:  Table [dbo].[Activities]    Script Date: 2/6/2017 11:33:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activities](
	[ActivityID] [uniqueidentifier] NOT NULL,
	[ActivityName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Technologies]    Script Date: 2/6/2017 11:33:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Technologies](
	[TechnologyID] [uniqueidentifier] NOT NULL,
	[TechnologyName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Technology] PRIMARY KEY CLUSTERED 
(
	[TechnologyID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[WorkloadBacklogs]    Script Date: 2/6/2017 11:33:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkloadBacklogs](
	[WBID] [uniqueidentifier] NOT NULL,
	[WBActivityActivityID] [uniqueidentifier] NULL,
	[WBComplexity] [int] NOT NULL,
	[WBCreatedBy] [nvarchar](max) NOT NULL,
	[WBCreatedDate] [datetime2](7) NOT NULL,
	[WBDescription] [nvarchar](max) NULL,
	[WBEndDate] [datetime2](7) NOT NULL,
	[WBExpertise] [int] NOT NULL,
	[WBIsWorkload] [bit] NOT NULL,
	[WBStartDate] [datetime2](7) NOT NULL,
	[WBStatus] [int] NOT NULL,
	[WBTitle] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_WorkloadBacklog] PRIMARY KEY CLUSTERED 
(
	[WBID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[WorkloadBacklogTechnologies]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkloadBacklogTechnologies](
	[WBUTechnologyID] [uniqueidentifier] NOT NULL,
	[TechnologyTechnologyID] [uniqueidentifier] NULL,
	[WorkloadBacklogWBID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_WorkloadBacklogTechnology] PRIMARY KEY CLUSTERED 
(
	[WBUTechnologyID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  View [dbo].[IPR]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from Activities

--select * from Metrics

--select * from Technologies

--select * from Appointments

--select Activities.ActivityName as activity, Technologies.TechnologyName as technology, Appointments.AppointmentUserUniqueName as uniquename
--from Appointments 
--inner join WorkloadBacklogs on Appointments.AppointmentWorkloadWBID = WorkloadBacklogs.WBID
--inner join Activities on WorkloadBacklogs.WBActivityActivityID = Activities.ActivityID
--inner join WorkloadBacklogTechnologies on WorkloadBacklogs.WBID = WorkloadBacklogTechnologies.WorkloadBacklogWBID
--inner join Technologies on WorkloadBacklogTechnologies.TechnologyTechnologyID = Technologies.TechnologyID

create view [dbo].[IPR] as 
select Activities.ActivityName as activity, Technologies.TechnologyName as technology, Appointments.AppointmentUserUniqueName as uniquename
from Appointments 
inner join WorkloadBacklogs on Appointments.AppointmentWorkloadWBID = WorkloadBacklogs.WBID
inner join Activities on WorkloadBacklogs.WBActivityActivityID = Activities.ActivityID
inner join WorkloadBacklogTechnologies on WorkloadBacklogs.WBID = WorkloadBacklogTechnologies.WorkloadBacklogWBID
inner join Technologies on WorkloadBacklogTechnologies.TechnologyTechnologyID = Technologies.TechnologyID
GO
/****** Object:  Table [dbo].[Files]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[FileID] [uniqueidentifier] NOT NULL,
	[FileDescription] [nvarchar](max) NULL,
	[FileLink] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[WorkloadBacklogWBID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[FiscalYears]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FiscalYears](
	[FiscalYearID] [uniqueidentifier] NOT NULL,
	[FullNumericFiscalYear] [int] NOT NULL,
	[TextualFiscalYear] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_FiscalYear] PRIMARY KEY CLUSTERED 
(
	[FiscalYearID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Metrics]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Metrics](
	[MetricID] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[FiscalYearID] [uniqueidentifier] NULL,
	[MetricCategory] [nvarchar](max) NOT NULL,
	[MetricName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Metric] PRIMARY KEY CLUSTERED 
(
	[MetricID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UniqueName] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UniqueName] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
INSERT Users VALUES ('user@ardademo.onmicrosoft.com', 'User 1')

GO
/****** Object:  Table [dbo].[WorkloadBacklogMetrics]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkloadBacklogMetrics](
	[WBMetricID] [uniqueidentifier] NOT NULL,
	[MetricMetricID] [uniqueidentifier] NULL,
	[WorkloadBacklogWBID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_WorkloadBacklogMetric] PRIMARY KEY CLUSTERED 
(
	[WBMetricID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[WorkloadBacklogUsers]    Script Date: 2/6/2017 11:33:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkloadBacklogUsers](
	[WBUserID] [uniqueidentifier] NOT NULL,
	[UserUniqueName] [nvarchar](450) NULL,
	[WorkloadBacklogWBID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_WorkloadBacklogUser] PRIMARY KEY CLUSTERED 
(
	[WBUserID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
INSERT [dbo].[Activities] ([ActivityID], [ActivityName]) VALUES (N'cd8dc5ec-e200-4290-9faa-1127b65c51a3', N'Presentation')
INSERT [dbo].[Activities] ([ActivityID], [ActivityName]) VALUES (N'd33c434e-a007-4eec-b0b6-16c505b721ff', N'Hackatons')
INSERT [dbo].[Activities] ([ActivityID], [ActivityName]) VALUES (N'e23d098e-013b-4afe-a966-1f515c9a1a12', N'POC')
INSERT [dbo].[FiscalYears] ([FiscalYearID], [FullNumericFiscalYear], [TextualFiscalYear]) VALUES (N'e0fd2d01-020e-475f-9c28-a90f5d857877', 2017, N'FY17')
INSERT [dbo].[FiscalYears] ([FiscalYearID], [FullNumericFiscalYear], [TextualFiscalYear]) VALUES (N'C6A45416-81A2-4034-ADAC-C7EAB5225ECE', 2018, N'FY18')
INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'0ac53748-932f-4479-8f25-4b8154d7099a', N'Customer stories with success', N'e0fd2d01-020e-475f-9c28-a90f5d857877', N'ISVs', N'Win Cases')
INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'821acb1a-2761-48a6-9e18-509c67a06c80', N'Workshops and Events', N'e0fd2d01-020e-475f-9c28-a90f5d857877', N'ISVs', N'Evangelism')
INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'2025272f-0bed-48a6-80e5-b2c01f198000', N'Windows market for PC', N'e0fd2d01-020e-475f-9c28-a90f5d857877', N'PC', N'Windows market')
INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'7479b502-74b9-4673-9e2f-d35f7fd19edd', N'Customer satisfaction.', N'e0fd2d01-020e-475f-9c28-a90f5d857877', N'Community', N'Customer SAT')
INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'819193e6-ea01-4c4e-a948-fc44453b2604', N'Azure training for students', N'e0fd2d01-020e-475f-9c28-a90f5d857877', N'Education', N'Azure Training')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'f938b4ba-bdf3-4d07-b5ca-8389e7256460', N'Microsoft Azure: Analytics')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'f28c67ff-5250-45b5-a750-9ba5ec580752', N'Microsoft Azure: Infra')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'230fa523-917a-4076-b58d-c4022f187bb7', N'Microsoft Azure: IoT')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'2edba5ef-721f-4d9f-a1a5-010beafb2812', N'Microsoft Azure: Web & Mobile')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'68abee73-bae1-4853-aa8e-147092bc3745', N'Microsoft Azure: Machine Learning')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'7e6b619a-59f0-45fb-bbe7-34cc8bf43e53', N'Microsoft Azure: Bot Framework')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'68f21275-213d-4fa8-bdcb-64561a5e31d7', N'Enterprise Mobility Suite (EMS)')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'a9c7182f-aa1a-4878-99c7-723027c66e8b', N'Operations Management Suite (OMS)')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'a43254d1-d99c-43bb-a39f-7c84ae100f8f', N'Visual Studio Team Services (VSTS)')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'a43253f1-d99c-43bb-a39f-7c84ae1e0f8d', N'Office 365')
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_User_AppointmentUserUniqueName] FOREIGN KEY([AppointmentUserUniqueName])
REFERENCES [dbo].[Users] ([UniqueName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointment_User_AppointmentUserUniqueName]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_WorkloadBacklog_AppointmentWorkloadWBID] FOREIGN KEY([AppointmentWorkloadWBID])
REFERENCES [dbo].[WorkloadBacklogs] ([WBID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK_Appointment_WorkloadBacklog_AppointmentWorkloadWBID]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_File_WorkloadBacklog_WorkloadBacklogWBID] FOREIGN KEY([WorkloadBacklogWBID])
REFERENCES [dbo].[WorkloadBacklogs] ([WBID])
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_File_WorkloadBacklog_WorkloadBacklogWBID]
GO
ALTER TABLE [dbo].[Metrics]  WITH CHECK ADD  CONSTRAINT [FK_Metrics_FiscalYears_FiscalYearID] FOREIGN KEY([FiscalYearID])
REFERENCES [dbo].[FiscalYears] ([FiscalYearID])
GO
ALTER TABLE [dbo].[Metrics] CHECK CONSTRAINT [FK_Metrics_FiscalYears_FiscalYearID]
GO
ALTER TABLE [dbo].[WorkloadBacklogMetrics]  WITH CHECK ADD  CONSTRAINT [FK_WorkloadBacklogMetric_Metric_MetricMetricID] FOREIGN KEY([MetricMetricID])
REFERENCES [dbo].[Metrics] ([MetricID])
GO
ALTER TABLE [dbo].[WorkloadBacklogMetrics] CHECK CONSTRAINT [FK_WorkloadBacklogMetric_Metric_MetricMetricID]
GO
ALTER TABLE [dbo].[WorkloadBacklogMetrics]  WITH CHECK ADD  CONSTRAINT [FK_WorkloadBacklogMetric_WorkloadBacklog_WorkloadBacklogWBID] FOREIGN KEY([WorkloadBacklogWBID])
REFERENCES [dbo].[WorkloadBacklogs] ([WBID])
GO
ALTER TABLE [dbo].[WorkloadBacklogMetrics] CHECK CONSTRAINT [FK_WorkloadBacklogMetric_WorkloadBacklog_WorkloadBacklogWBID]
GO
ALTER TABLE [dbo].[WorkloadBacklogs]  WITH CHECK ADD  CONSTRAINT [FK_WorkloadBacklog_Activity_WBActivityActivityID] FOREIGN KEY([WBActivityActivityID])
REFERENCES [dbo].[Activities] ([ActivityID])
GO
ALTER TABLE [dbo].[WorkloadBacklogs] CHECK CONSTRAINT [FK_WorkloadBacklog_Activity_WBActivityActivityID]
GO
ALTER TABLE [dbo].[WorkloadBacklogTechnologies]  WITH CHECK ADD  CONSTRAINT [FK_WorkloadBacklogTechnology_Technology_TechnologyTechnologyID] FOREIGN KEY([TechnologyTechnologyID])
REFERENCES [dbo].[Technologies] ([TechnologyID])
GO
ALTER TABLE [dbo].[WorkloadBacklogTechnologies] CHECK CONSTRAINT [FK_WorkloadBacklogTechnology_Technology_TechnologyTechnologyID]
GO
ALTER TABLE [dbo].[WorkloadBacklogTechnologies]  WITH CHECK ADD  CONSTRAINT [FK_WorkloadBacklogTechnology_WorkloadBacklog_WorkloadBacklogWBID] FOREIGN KEY([WorkloadBacklogWBID])
REFERENCES [dbo].[WorkloadBacklogs] ([WBID])
GO
ALTER TABLE [dbo].[WorkloadBacklogTechnologies] CHECK CONSTRAINT [FK_WorkloadBacklogTechnology_WorkloadBacklog_WorkloadBacklogWBID]
GO
ALTER TABLE [dbo].[WorkloadBacklogUsers]  WITH CHECK ADD  CONSTRAINT [FK_WorkloadBacklogUser_User_UserUniqueName] FOREIGN KEY([UserUniqueName])
REFERENCES [dbo].[Users] ([UniqueName])
GO
ALTER TABLE [dbo].[WorkloadBacklogUsers] CHECK CONSTRAINT [FK_WorkloadBacklogUser_User_UserUniqueName]
GO
ALTER TABLE [dbo].[WorkloadBacklogUsers]  WITH CHECK ADD  CONSTRAINT [FK_WorkloadBacklogUser_WorkloadBacklog_WorkloadBacklogWBID] FOREIGN KEY([WorkloadBacklogWBID])
REFERENCES [dbo].[WorkloadBacklogs] ([WBID])
GO
ALTER TABLE [dbo].[WorkloadBacklogUsers] CHECK CONSTRAINT [FK_WorkloadBacklogUser_WorkloadBacklog_WorkloadBacklogWBID]
GO