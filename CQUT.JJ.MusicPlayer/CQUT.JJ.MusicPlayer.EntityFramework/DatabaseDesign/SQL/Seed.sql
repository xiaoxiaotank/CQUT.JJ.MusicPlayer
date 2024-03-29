
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [UserName], [Password], [NickName], [IsAdmin], [LastModificationTime], [CreationTime], [IsDeleted], [DeletionTime]) VALUES (1, N'Admin', N'e10adc3949ba59abbe56e057f20f883e', N'贾建军', 1, NULL, CONVERT(varchar,GETDATE(),120), 0, NULL)

SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[Permission] ON 

INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (1, 1, NULL, N'Total', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (2, 1, NULL, N'Total.App.MenuManage.Update', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (3, 1, NULL, N'Total.App.MenuManage.Delete', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (4, 1, NULL, N'Total.App.MenuManage.Create', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (5, 1, NULL, N'Total.App.MenuManage', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (6, 1, NULL, N'Total.App.Role.Update', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (7, 1, NULL, N'Total.App.Role.Delete', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (8, 1, NULL, N'Total.App.Role.Create', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (9, 1, NULL, N'Total.App.Role', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (10, 1, NULL, N'Total.App.Member.Delete', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (11, 1, NULL, N'Total.App.Member', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (12, 1, NULL, N'Total.App.Employee.Update', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (13, 1, NULL, N'Total.App.Employee.Delete', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (14, 1, NULL, N'Total.App.Employee.Create',CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (15, 1, NULL, N'Total.App.Employee', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (16, 1, NULL, N'Total.App', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (17, 1, NULL, N'Total.App.MenuManage.Migrate', CONVERT(varchar,GETDATE(),120))
INSERT [dbo].[Permission] ([Id], [UserId], [RoleId], [Code], [CreationTime]) VALUES (18, 1, NULL, N'Total.App.MenuManage.Rename', CONVERT(varchar,GETDATE(),120))
SET IDENTITY_INSERT [dbo].[Permission] OFF
SET IDENTITY_INSERT [dbo].[Menu] ON 

INSERT [dbo].[Menu] ([Id], [Header], [TargetUrl], [ParentId], [RequiredAuthorizeCode], [Priority], [CreationTime], [LastModificationTime]) VALUES (1, N'用户管理', N'', 0, N'', 1, CONVERT(varchar,GETDATE(),120), NULL)
INSERT [dbo].[Menu] ([Id], [Header], [TargetUrl], [ParentId], [RequiredAuthorizeCode], [Priority], [CreationTime], [LastModificationTime]) VALUES (2, N'员工管理', N'/Admin/Employee/Index', 1, N'', 2, CONVERT(varchar,GETDATE(),120), NULL)
INSERT [dbo].[Menu] ([Id], [Header], [TargetUrl], [ParentId], [RequiredAuthorizeCode], [Priority], [CreationTime], [LastModificationTime]) VALUES (3, N'会员管理', N'/Admin/Member/Index', 1, N'', 1, CONVERT(varchar,GETDATE(),120), NULL)
SET IDENTITY_INSERT [dbo].[Menu] OFF
