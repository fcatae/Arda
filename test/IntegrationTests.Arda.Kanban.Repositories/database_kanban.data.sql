delete metrics
delete fiscalyears
delete activities
delete technologies

INSERT [dbo].[FiscalYears] ([FiscalYearID], [FullNumericFiscalYear], [TextualFiscalYear]) VALUES (N'D8043C31-EBA9-41C8-81A1-EBC828D81DCB', 2016, N'FY16')
INSERT [dbo].[FiscalYears] ([FiscalYearID], [FullNumericFiscalYear], [TextualFiscalYear]) VALUES (N'D38759AB-E310-46F0-A6C3-B0594C2531AB', 2017, N'FY17')
INSERT [dbo].[FiscalYears] ([FiscalYearID], [FullNumericFiscalYear], [TextualFiscalYear]) VALUES (N'C6A45416-81A2-4034-ADAC-C7EAB5225ECE', 2018, N'FY18')

INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'20297652-9BE6-4560-AA36-586F99893FEF', N'Workshops and Events', N'D8043C31-EBA9-41C8-81A1-EBC828D81DCB', N'ISVs', N'Evangelism')
INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'6DA887CB-9EDD-42CB-87C9-83AC772D9B65', N'Customer satisfaction.', N'D38759AB-E310-46F0-A6C3-B0594C2531AB', N'Community', N'Customer SAT')
INSERT [dbo].[Metrics] ([MetricID], [Description], [FiscalYearID], [MetricCategory], [MetricName]) VALUES (N'45979112-AFF6-4BFA-878B-02BAA8FD1074', N'Azure training for students', N'D38759AB-E310-46F0-A6C3-B0594C2531AB', N'Education', N'Azure Training')

INSERT [dbo].[Activities] ([ActivityID], [ActivityName]) VALUES (N'06C15279-9AFB-4723-AF61-33EAF8974D47', N'Presentation')
INSERT [dbo].[Activities] ([ActivityID], [ActivityName]) VALUES (N'D23D46AC-9D8C-43F5-B673-8B169B04081F', N'Hackatons')
INSERT [dbo].[Activities] ([ActivityID], [ActivityName]) VALUES (N'1F265DF5-ADBE-4B7B-A05A-451AF058C482', N'POC')

INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'B2B1BCE9-DF05-4FBF-ABB7-89617D396B4C', N'Analytics')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'9C263D44-2C11-48CD-B876-5EBB540BBF51', N'Infra')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'CEC4B7BB-4F14-4118-AE31-29BC39AE93D8', N'IoT')
INSERT [dbo].[Technologies] ([TechnologyID], [TechnologyName]) VALUES (N'AF5D8796-0CA2-4D54-84F7-D3194F5F2426', N'Web & Mobile')

INSERT [Users](UniqueName, Name) VALUES ('user@ardademo.onmicrosoft.com', 'User1')
INSERT [Users](UniqueName, Name) VALUES ('admin@ardademo.onmicrosoft.com', 'Admin')
INSERT [Users](UniqueName, Name) VALUES ('guest@ardademo.onmicrosoft.com', 'Guest User')
