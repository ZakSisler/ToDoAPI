/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.5026)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ToDoItems]') AND type in (N'U'))
ALTER TABLE [dbo].[ToDoItems] DROP CONSTRAINT IF EXISTS [FK_ToDoItems_Categories]
GO
/****** Object:  Table [dbo].[ToDoItems]    Script Date: 4/8/2022 3:33:27 PM ******/
DROP TABLE IF EXISTS [dbo].[ToDoItems]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 4/8/2022 3:33:27 PM ******/
DROP TABLE IF EXISTS [dbo].[Categories]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 4/8/2022 3:33:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ToDoItems]    Script Date: 4/8/2022 3:33:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ToDoItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ToDoItems](
	[TodoId] [int] IDENTITY(1,1) NOT NULL,
	[Action] [nvarchar](max) NOT NULL,
	[Done] [bit] NOT NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_ToDoItems] PRIMARY KEY CLUSTERED 
(
	[TodoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (1, N'Chores', N'Chores to do')
INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (2, N'Work', N'Work to do')
INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (3, N'Study', N'Study to do')
INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (6, N'Boris', N'Horror Classics')
INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (7, N'test', N'tests to do')
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[ToDoItems] ON 

INSERT [dbo].[ToDoItems] ([TodoId], [Action], [Done], [CategoryId]) VALUES (1, N'study terms', 0, 3)
SET IDENTITY_INSERT [dbo].[ToDoItems] OFF
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ToDoItems_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ToDoItems]'))
ALTER TABLE [dbo].[ToDoItems]  WITH CHECK ADD  CONSTRAINT [FK_ToDoItems_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ToDoItems_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ToDoItems]'))
ALTER TABLE [dbo].[ToDoItems] CHECK CONSTRAINT [FK_ToDoItems_Categories]
GO
