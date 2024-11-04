CREATE DATABASE [SentimentAnalysis]
GO

USE [SentimentAnalysis]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 2/11/2024 12:34:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[text] [nvarchar](max) NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[userId] [int] NOT NULL,
	[is_depression] [bit] NULL,
	[post_date] [date] NOT NULL,
 CONSTRAINT [PK__Post__3213E83F29927342] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2/11/2024 12:34:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userName] [nvarchar](255) NULL,
	[email] [nvarchar](255) NOT NULL,
	[birthday] [date] NULL,
	[genre] [nvarchar](50) NULL,
 CONSTRAINT [PK__User__3213E83FFCC8376F] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_User_Post] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_User_Post]
GO
