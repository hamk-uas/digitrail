/****** Object:  Table [dbo].[Difficulty]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Difficulty](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DifficultyTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DifficultyTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[name] [nvarchar](255) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
	[difficulty] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Difficul__3213E83E2E5163E4] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[text] [nvarchar](max) NOT NULL,
	[lat] [float] NULL,
	[lon] [float] NULL,
 CONSTRAINT [PK__Feedback__3213E83E26F047C4] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[code] [nvarchar](8) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__Language__3213E83ED2B9ABB0] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Location__3213E83E55B93987] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Marker]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marker](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[image] [nvarchar](max) NULL,
	[popupImage] [nvarchar](max) NULL,
	[lat] [float] NOT NULL,
	[lon] [float] NOT NULL,
	[impactrange] [int] NOT NULL,
	[markerType] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Marker__3213E83E77012C4B] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MarkerTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarkerTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[title] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NULL,
	[popupdescription] [nvarchar](max) NULL,
	[marker] [nvarchar](255) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
	[link] [nvarchar](max) NULL,
 CONSTRAINT [PK__MarkerTr__3213E83E212E1124] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MarkerType]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarkerType](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[showToUser] [bit] NOT NULL,
	[markerTypeIcon] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__MarkerTy__3213E83E7062556C] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MarkerTypeIcon]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarkerTypeIcon](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[filename] [nvarchar](255) NOT NULL,
	[url] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK__MarkerTy__3213E83E7B294CBB] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MarkerTypeTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarkerTypeTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[name] [nvarchar](max) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
	[markerType] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__MarkerTy__3213E83E7A3DFE48] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveThemeMarker]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveThemeMarker](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[marker] [nvarchar](255) NOT NULL,
	[themeObjective] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Objectiv__3213E83E1A3B04F6] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveThemeMarkerAnswer]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveThemeMarkerAnswer](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[correct] [bit] NOT NULL,
	[points] [tinyint] NOT NULL,
	[objectiveThemeMarker] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Objectiv__3213E83EEEECC138] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectiveThemeMarkerAnswerTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[text] [nvarchar](max) NOT NULL,
	[objectiveThemeMarkerAnswer] [nvarchar](255) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
 CONSTRAINT [PK__Objectiv__3213E83EE69D5F7B] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Theme]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Theme](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[ratingCount] [int] NULL,
	[ratingSum] [int] NULL,
 CONSTRAINT [PK__Theme__3213E83ECB8E3A6F] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThemeMarker]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThemeMarker](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[marker] [nvarchar](255) NOT NULL,
	[theme] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__ThemeMar__3213E83E3A25DA31] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThemeMarkerPage]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThemeMarkerPage](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[image] [nvarchar](max) NULL,
	[themeMarker] [nvarchar](255) NOT NULL,
	[orderNumber] [tinyint] NOT NULL,
 CONSTRAINT [PK__ThemeMar__3213E83E7CF63BE4] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThemeMarkerPageTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThemeMarkerPageTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[text] [nvarchar](max) NULL,
	[themeMarkerPage] [nvarchar](255) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
	[title] [nvarchar](max) NOT NULL,
	[link] [nvarchar](max) NULL,
 CONSTRAINT [PK__ThemeMar__3213E83E756DA47B] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThemeObjective]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThemeObjective](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[theme] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__ThemeObj__3213E83EC45BEDEB] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_ThemeObjective_Theme] UNIQUE NONCLUSTERED 
(
	[theme] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThemeObjectiveFeedback]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThemeObjectiveFeedback](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[image] [nvarchar](max) NULL,
	[scoreMin] [tinyint] NOT NULL,
	[scoreMax] [tinyint] NOT NULL,
	[themeObjective] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__ThemeObj__3213E83E7C9793BC] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThemeObjectiveFeedbackTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThemeObjectiveFeedbackTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[title] [nvarchar](max) NOT NULL,
	[text] [nvarchar](max) NULL,
	[themeObjectiveFeedback] [nvarchar](255) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
 CONSTRAINT [PK__ThemeObj__3213E83E7DB22996] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThemeTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThemeTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[name] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NULL,
	[theme] [nvarchar](255) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
 CONSTRAINT [PK__ThemeTra__3213E83E113DC210] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trail]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trail](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[image] [nvarchar](max) NULL,
	[file] [nvarchar](max) NOT NULL,
	[ratingCount] [int] NULL,
	[location] [nvarchar](255) NOT NULL,
	[difficulty] [nvarchar](255) NOT NULL,
	[ratingSum] [int] NULL,
 CONSTRAINT [PK__Trail__3213E83E57DDBE3B] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrailMarker]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrailMarker](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[marker] [nvarchar](255) NOT NULL,
	[trail] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__TrailMar__3213E83E3630EF18] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrailTheme]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrailTheme](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[default] [bit] NOT NULL,
	[theme] [nvarchar](255) NOT NULL,
	[trail] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__TrailThe__3213E83E69C15673] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrailTranslation]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrailTranslation](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[name] [nvarchar](max) NOT NULL,
	[description] [nvarchar](max) NULL,
	[trail] [nvarchar](255) NOT NULL,
	[language] [nvarchar](8) NOT NULL,
 CONSTRAINT [PK__TrailTra__3213E83EA90966CE] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Version]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Version](
	[id] [nvarchar](255) NOT NULL,
	[createdAt] [datetimeoffset](3) NOT NULL,
	[updatedAt] [datetimeoffset](3) NULL,
	[version] [timestamp] NOT NULL,
	[deleted] [bit] NULL,
	[versionCode] [int] NOT NULL,
	[versionName] [nvarchar](max) NULL,
 CONSTRAINT [PK__Version__3213E83EC9C8427D] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Difficulty] ADD  CONSTRAINT [DF_Difficulty_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Difficulty] ADD  CONSTRAINT [DF_Difficulty_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Difficulty] ADD  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[DifficultyTranslation] ADD  CONSTRAINT [DF_DifficultyTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[DifficultyTranslation] ADD  CONSTRAINT [DF_DifficultyTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[DifficultyTranslation] ADD  CONSTRAINT [DF__Difficult__delet__52593CB8]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Feedback] ADD  CONSTRAINT [DF_Feedback_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Feedback] ADD  CONSTRAINT [DF_Feedback_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Feedback] ADD  CONSTRAINT [DF__Feedback__delete__5812160E]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF__Language__delete__5DCAEF64]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Location] ADD  CONSTRAINT [DF_Location_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Location] ADD  CONSTRAINT [DF_Location_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Location] ADD  CONSTRAINT [DF__Location__delete__6383C8BA]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Marker] ADD  CONSTRAINT [DF_Marker_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Marker] ADD  CONSTRAINT [DF_Marker_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Marker] ADD  CONSTRAINT [DF__Marker__deleted__693CA210]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Marker] ADD  CONSTRAINT [DF_Marker_impactrange]  DEFAULT ((20)) FOR [impactrange]
GO
ALTER TABLE [dbo].[MarkerTranslation] ADD  CONSTRAINT [DF_MarkerTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[MarkerTranslation] ADD  CONSTRAINT [DF_MarkerTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[MarkerTranslation] ADD  CONSTRAINT [DF__MarkerTra__delet__6EF57B66]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[MarkerType] ADD  CONSTRAINT [DF_MarkerType_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[MarkerType] ADD  CONSTRAINT [DF_MarkerType_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[MarkerType] ADD  CONSTRAINT [DF__MarkerTyp__delet__74AE54BC]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[MarkerTypeIcon] ADD  CONSTRAINT [DF_MarkerTypeIcon_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[MarkerTypeIcon] ADD  CONSTRAINT [DF_MarkerTypeIcon_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[MarkerTypeIcon] ADD  CONSTRAINT [DF__MarkerTyp__delet__7A672E12]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[MarkerTypeTranslation] ADD  CONSTRAINT [DF_MarkerTypeTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[MarkerTypeTranslation] ADD  CONSTRAINT [DF_MarkerTypeTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[MarkerTypeTranslation] ADD  CONSTRAINT [DF__MarkerTyp__delet__00200768]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker] ADD  CONSTRAINT [DF_ObjectiveThemeMarker_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker] ADD  CONSTRAINT [DF_ObjectiveThemeMarker_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker] ADD  CONSTRAINT [DF__Objective__delet__65C116E7]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer] ADD  CONSTRAINT [DF_ObjectiveThemeMarkerAnswer_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer] ADD  CONSTRAINT [DF_ObjectiveThemeMarkerAnswer_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer] ADD  CONSTRAINT [DF__Objective__delet__75F77EB0]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer] ADD  CONSTRAINT [DF_ObjectiveThemeMarkerAnswer_points]  DEFAULT ((1)) FOR [points]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation] ADD  CONSTRAINT [DF_ObjectiveThemeMarkerAnswerTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation] ADD  CONSTRAINT [DF_ObjectiveThemeMarkerAnswerTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation] ADD  CONSTRAINT [DF__Objective__delet__04459E07]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Theme] ADD  CONSTRAINT [DF_Theme_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Theme] ADD  CONSTRAINT [DF_Theme_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Theme] ADD  CONSTRAINT [DF__Theme__deleted__05D8E0BE]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ThemeMarker] ADD  CONSTRAINT [DF_ThemeMarker_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ThemeMarker] ADD  CONSTRAINT [DF_ThemeMarker_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ThemeMarker] ADD  CONSTRAINT [DF__ThemeMark__delet__0B91BA14]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ThemeMarkerPage] ADD  CONSTRAINT [DF_ThemeMarkerPage_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ThemeMarkerPage] ADD  CONSTRAINT [DF_ThemeMarkerPage_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ThemeMarkerPage] ADD  CONSTRAINT [DF__ThemeMark__delet__114A936A]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation] ADD  CONSTRAINT [DF_ThemeMarkerPageTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation] ADD  CONSTRAINT [DF_ThemeMarkerPageTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation] ADD  CONSTRAINT [DF__ThemeMark__delet__17036CC0]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ThemeObjective] ADD  CONSTRAINT [DF_ThemeObjective_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ThemeObjective] ADD  CONSTRAINT [DF_ThemeObjective_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ThemeObjective] ADD  CONSTRAINT [DF__ThemeObje__delet__37FA4C37]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedback] ADD  CONSTRAINT [DF_ThemeObjectiveFeedback_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedback] ADD  CONSTRAINT [DF_ThemeObjectiveFeedback_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedback] ADD  CONSTRAINT [DF__ThemeObje__delet__46486B8E]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation] ADD  CONSTRAINT [DF_ThemeObjectiveFeedbackTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation] ADD  CONSTRAINT [DF_ThemeObjectiveFeedbackTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation] ADD  CONSTRAINT [DF__ThemeObje__delet__558AAF1E]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[ThemeTranslation] ADD  CONSTRAINT [DF_ThemeTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[ThemeTranslation] ADD  CONSTRAINT [DF_ThemeTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[ThemeTranslation] ADD  CONSTRAINT [DF__ThemeTran__delet__1CBC4616]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Trail] ADD  CONSTRAINT [DF_Trail_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Trail] ADD  CONSTRAINT [DF_Trail_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Trail] ADD  CONSTRAINT [DF__Trail__deleted__22751F6C]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[TrailMarker] ADD  CONSTRAINT [DF_TrailMarker_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[TrailMarker] ADD  CONSTRAINT [DF_TrailMarker_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[TrailMarker] ADD  CONSTRAINT [DF__TrailMark__delet__282DF8C2]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[TrailTheme] ADD  CONSTRAINT [DF_TrailTheme_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[TrailTheme] ADD  CONSTRAINT [DF_TrailTheme_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[TrailTheme] ADD  CONSTRAINT [DF__TrailThem__delet__23F3538A]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[TrailTranslation] ADD  CONSTRAINT [DF_TrailTranslation_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[TrailTranslation] ADD  CONSTRAINT [DF_TrailTranslation_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[TrailTranslation] ADD  CONSTRAINT [DF__TrailTran__delet__2DE6D218]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[Version] ADD  CONSTRAINT [DF_Version_id]  DEFAULT (CONVERT([nvarchar](255),newid(),(0))) FOR [id]
GO
ALTER TABLE [dbo].[Version] ADD  CONSTRAINT [DF_Version_createdAt]  DEFAULT (CONVERT([datetimeoffset](3),sysutcdatetime(),(0))) FOR [createdAt]
GO
ALTER TABLE [dbo].[Version] ADD  CONSTRAINT [DF__Version__deleted__13BCEBC1]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[DifficultyTranslation]  WITH CHECK ADD  CONSTRAINT [FK_DifficultyTranslation_Difficulty] FOREIGN KEY([difficulty])
REFERENCES [dbo].[Difficulty] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DifficultyTranslation] CHECK CONSTRAINT [FK_DifficultyTranslation_Difficulty]
GO
ALTER TABLE [dbo].[DifficultyTranslation]  WITH CHECK ADD  CONSTRAINT [FK_DifficultyTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DifficultyTranslation] CHECK CONSTRAINT [FK_DifficultyTranslation_Language]
GO
ALTER TABLE [dbo].[Marker]  WITH CHECK ADD  CONSTRAINT [FK_Marker_MarkerType] FOREIGN KEY([markerType])
REFERENCES [dbo].[MarkerType] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Marker] CHECK CONSTRAINT [FK_Marker_MarkerType]
GO
ALTER TABLE [dbo].[MarkerTranslation]  WITH CHECK ADD  CONSTRAINT [FK_MarkerTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MarkerTranslation] CHECK CONSTRAINT [FK_MarkerTranslation_Language]
GO
ALTER TABLE [dbo].[MarkerTranslation]  WITH CHECK ADD  CONSTRAINT [FK_MarkerTranslation_Marker] FOREIGN KEY([marker])
REFERENCES [dbo].[Marker] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MarkerTranslation] CHECK CONSTRAINT [FK_MarkerTranslation_Marker]
GO
ALTER TABLE [dbo].[MarkerType]  WITH CHECK ADD  CONSTRAINT [FK_MarkerType_MarkerTypeIcon] FOREIGN KEY([markerTypeIcon])
REFERENCES [dbo].[MarkerTypeIcon] ([filename])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[MarkerType] CHECK CONSTRAINT [FK_MarkerType_MarkerTypeIcon]
GO
ALTER TABLE [dbo].[MarkerTypeTranslation]  WITH CHECK ADD  CONSTRAINT [FK_MarkerTypeTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MarkerTypeTranslation] CHECK CONSTRAINT [FK_MarkerTypeTranslation_Language]
GO
ALTER TABLE [dbo].[MarkerTypeTranslation]  WITH CHECK ADD  CONSTRAINT [FK_MarkerTypeTranslation_MarkerType] FOREIGN KEY([markerType])
REFERENCES [dbo].[MarkerType] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MarkerTypeTranslation] CHECK CONSTRAINT [FK_MarkerTypeTranslation_MarkerType]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveThemeMarker_Marker] FOREIGN KEY([marker])
REFERENCES [dbo].[Marker] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker] CHECK CONSTRAINT [FK_ObjectiveThemeMarker_Marker]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveThemeMarker_ThemeObjective] FOREIGN KEY([themeObjective])
REFERENCES [dbo].[ThemeObjective] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker] CHECK CONSTRAINT [FK_ObjectiveThemeMarker_ThemeObjective]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveThemeMarkerAnswer_ObjectiveThemeMarker] FOREIGN KEY([objectiveThemeMarker])
REFERENCES [dbo].[ObjectiveThemeMarker] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer] CHECK CONSTRAINT [FK_ObjectiveThemeMarkerAnswer_ObjectiveThemeMarker]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveThemeMarkerAnswerTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation] CHECK CONSTRAINT [FK_ObjectiveThemeMarkerAnswerTranslation_Language]
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ObjectiveThemeMarkerAnswerTranslation_ObjectiveThemeMarkerAnswer] FOREIGN KEY([objectiveThemeMarkerAnswer])
REFERENCES [dbo].[ObjectiveThemeMarkerAnswer] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation] CHECK CONSTRAINT [FK_ObjectiveThemeMarkerAnswerTranslation_ObjectiveThemeMarkerAnswer]
GO
ALTER TABLE [dbo].[ThemeMarker]  WITH CHECK ADD  CONSTRAINT [FK_ThemeMarker_Marker] FOREIGN KEY([marker])
REFERENCES [dbo].[Marker] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeMarker] CHECK CONSTRAINT [FK_ThemeMarker_Marker]
GO
ALTER TABLE [dbo].[ThemeMarker]  WITH CHECK ADD  CONSTRAINT [FK_ThemeMarker_Theme] FOREIGN KEY([theme])
REFERENCES [dbo].[Theme] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeMarker] CHECK CONSTRAINT [FK_ThemeMarker_Theme]
GO
ALTER TABLE [dbo].[ThemeMarkerPage]  WITH CHECK ADD  CONSTRAINT [FK_ThemeMarkerPage_ThemeMarker] FOREIGN KEY([themeMarker])
REFERENCES [dbo].[ThemeMarker] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeMarkerPage] CHECK CONSTRAINT [FK_ThemeMarkerPage_ThemeMarker]
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ThemeMarkerPageTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation] CHECK CONSTRAINT [FK_ThemeMarkerPageTranslation_Language]
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ThemeMarkerPageTranslation_ThemeMarkerPage] FOREIGN KEY([themeMarkerPage])
REFERENCES [dbo].[ThemeMarkerPage] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation] CHECK CONSTRAINT [FK_ThemeMarkerPageTranslation_ThemeMarkerPage]
GO
ALTER TABLE [dbo].[ThemeObjective]  WITH CHECK ADD  CONSTRAINT [FK_ThemeObjective_Theme] FOREIGN KEY([theme])
REFERENCES [dbo].[Theme] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeObjective] CHECK CONSTRAINT [FK_ThemeObjective_Theme]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedback]  WITH CHECK ADD  CONSTRAINT [FK_ThemeObjectiveFeedback_ThemeObjective] FOREIGN KEY([themeObjective])
REFERENCES [dbo].[ThemeObjective] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedback] CHECK CONSTRAINT [FK_ThemeObjectiveFeedback_ThemeObjective]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ThemeObjectiveFeedbackTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation] CHECK CONSTRAINT [FK_ThemeObjectiveFeedbackTranslation_Language]
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ThemeObjectiveFeedbackTranslation_ThemeObjectiveFeedback] FOREIGN KEY([themeObjectiveFeedback])
REFERENCES [dbo].[ThemeObjectiveFeedback] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation] CHECK CONSTRAINT [FK_ThemeObjectiveFeedbackTranslation_ThemeObjectiveFeedback]
GO
ALTER TABLE [dbo].[ThemeTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ThemeTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeTranslation] CHECK CONSTRAINT [FK_ThemeTranslation_Language]
GO
ALTER TABLE [dbo].[ThemeTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ThemeTranslation_Theme] FOREIGN KEY([theme])
REFERENCES [dbo].[Theme] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeTranslation] CHECK CONSTRAINT [FK_ThemeTranslation_Theme]
GO
ALTER TABLE [dbo].[Trail]  WITH CHECK ADD  CONSTRAINT [FK_Trail_Difficulty] FOREIGN KEY([difficulty])
REFERENCES [dbo].[Difficulty] ([id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Trail] CHECK CONSTRAINT [FK_Trail_Difficulty]
GO
ALTER TABLE [dbo].[Trail]  WITH CHECK ADD  CONSTRAINT [FK_Trail_Location] FOREIGN KEY([location])
REFERENCES [dbo].[Location] ([name])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Trail] CHECK CONSTRAINT [FK_Trail_Location]
GO
ALTER TABLE [dbo].[TrailMarker]  WITH CHECK ADD  CONSTRAINT [FK_TrailMarker_Marker] FOREIGN KEY([marker])
REFERENCES [dbo].[Marker] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrailMarker] CHECK CONSTRAINT [FK_TrailMarker_Marker]
GO
ALTER TABLE [dbo].[TrailMarker]  WITH CHECK ADD  CONSTRAINT [FK_TrailMarker_Trail] FOREIGN KEY([trail])
REFERENCES [dbo].[Trail] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrailMarker] CHECK CONSTRAINT [FK_TrailMarker_Trail]
GO
ALTER TABLE [dbo].[TrailTheme]  WITH CHECK ADD  CONSTRAINT [FK_TrailTheme_Theme] FOREIGN KEY([theme])
REFERENCES [dbo].[Theme] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrailTheme] CHECK CONSTRAINT [FK_TrailTheme_Theme]
GO
ALTER TABLE [dbo].[TrailTheme]  WITH CHECK ADD  CONSTRAINT [FK_TrailTheme_Trail] FOREIGN KEY([trail])
REFERENCES [dbo].[Trail] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrailTheme] CHECK CONSTRAINT [FK_TrailTheme_Trail]
GO
ALTER TABLE [dbo].[TrailTranslation]  WITH CHECK ADD  CONSTRAINT [FK_TrailTranslation_Language] FOREIGN KEY([language])
REFERENCES [dbo].[Language] ([code])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrailTranslation] CHECK CONSTRAINT [FK_TrailTranslation_Language]
GO
ALTER TABLE [dbo].[TrailTranslation]  WITH CHECK ADD  CONSTRAINT [FK_TrailTranslation_Trail] FOREIGN KEY([trail])
REFERENCES [dbo].[Trail] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TrailTranslation] CHECK CONSTRAINT [FK_TrailTranslation_Trail]
GO
/****** Object:  Trigger [dbo].[TR_Difficulty_DifficultyTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_Difficulty_DifficultyTranslation_Soft_Delete]
   ON  [dbo].[Difficulty]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[DifficultyTranslation].[id]
		FROM [dbo].[DifficultyTranslation]
		JOIN [inserted] ON [dbo].[DifficultyTranslation].[difficulty] = [inserted].[id]
		WHERE [dbo].[DifficultyTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading DIFFICULTY column DELETE state to child DIFFICULTYTRANSLATION rows';
		UPDATE [dbo].[DifficultyTranslation]
		SET [deleted] = 1
		FROM [dbo].[DifficultyTranslation] join [inserted] ON [dbo].[DifficultyTranslation].[difficulty] = [inserted].[id]
		WHERE [dbo].[DifficultyTranslation].[difficulty] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Difficulty] ENABLE TRIGGER [TR_Difficulty_DifficultyTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Difficulty_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Difficulty_InsertUpdateDelete] ON [dbo].[Difficulty]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Difficulty] SET [dbo].[Difficulty].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Difficulty].[id]
		END
GO
ALTER TABLE [dbo].[Difficulty] ENABLE TRIGGER [TR_Difficulty_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Difficulty_Trail_Soft_Delete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_Difficulty_Trail_Soft_Delete]
   ON  [dbo].[Difficulty]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted) 
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[Trail].[id] 
		FROM [dbo].[Trail] JOIN [inserted] ON [dbo].[Trail].[Difficulty] = [inserted].[id] 
		WHERE [dbo].[Trail].[deleted] = 0
	)
	BEGIN
		UPDATE [dbo].[Difficulty]
		SET [deleted] = 0
		FROM [dbo].[Difficulty] JOIN [inserted] ON [dbo].[Difficulty].[id] = [inserted].[id]
		WHERE [dbo].[Difficulty].[id] = [inserted].[id];
		THROW 50000, N'Cannot delete parent DIFFICULTY with child TRAILS due to Foreign Key constraint NO ACTION, rolling back', 1;
	END
END
GO
ALTER TABLE [dbo].[Difficulty] ENABLE TRIGGER [TR_Difficulty_Trail_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_DifficultyTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_DifficultyTranslation_InsertUpdateDelete] ON [dbo].[DifficultyTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[DifficultyTranslation] SET [dbo].[DifficultyTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[DifficultyTranslation].[id]
		END
GO
ALTER TABLE [dbo].[DifficultyTranslation] ENABLE TRIGGER [TR_DifficultyTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Feedback_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Feedback_InsertUpdateDelete] ON [dbo].[Feedback]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Feedback] SET [dbo].[Feedback].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Feedback].[id]
		END
GO
ALTER TABLE [dbo].[Feedback] ENABLE TRIGGER [TR_Feedback_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Language_DifficultyTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Language_DifficultyTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[DifficultyTranslation].[id]
		FROM [dbo].[DifficultyTranslation]
		JOIN [inserted] ON [dbo].[DifficultyTranslation].[language] = [inserted].[code]
		WHERE [dbo].[DifficultyTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading LANGUAGE column DELETE state to child DIFFICULTYTRANSLATION rows';
		UPDATE [dbo].[DifficultyTranslation]
		SET [deleted] = 1
		FROM [dbo].[DifficultyTranslation] 
		JOIN [inserted] ON [dbo].[DifficultyTranslation].[language] = [inserted].[code]
		WHERE [dbo].[DifficultyTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_DifficultyTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Language_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Language_InsertUpdateDelete] ON [dbo].[Language]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Language] SET [dbo].[Language].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Language].[id]
		END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Language_MarkerTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Language_MarkerTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[MarkerTranslation].[id]
		FROM [dbo].[MarkerTranslation]
		JOIN [inserted] ON [dbo].[MarkerTranslation].[language] = [inserted].[code]
		WHERE [dbo].[MarkerTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading LANGUAGE column DELETE state to child MARKERTRANSLATION rows';
		UPDATE [dbo].[MarkerTranslation]
		SET [deleted] = 1
		FROM [dbo].[MarkerTranslation] 
		JOIN [inserted] ON [dbo].[MarkerTranslation].[language] = [inserted].[code]
		WHERE [dbo].[MarkerTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_MarkerTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Language_MarkerTypeTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Language_MarkerTypeTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[MarkerTypeTranslation].[id]
		FROM [dbo].[MarkerTypeTranslation]
		JOIN [inserted] ON [dbo].[MarkerTypeTranslation].[language] = [inserted].[code]
		WHERE [dbo].[MarkerTypeTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading LANGUAGE column DELETE state to child MARKERTYPETRANSLATION rows';
		UPDATE [dbo].[MarkerTypeTranslation]
		SET [deleted] = 1
		FROM [dbo].[MarkerTypeTranslation] 
		JOIN [inserted] ON [dbo].[MarkerTypeTranslation].[language] = [inserted].[code]
		WHERE [dbo].[MarkerTypeTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_MarkerTypeTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Language_ObjectiveThemeMarkerAnswerTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_Language_ObjectiveThemeMarkerAnswerTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ObjectiveThemeMarkerAnswerTranslation].[id]
		FROM [dbo].[ObjectiveThemeMarkerAnswerTranslation]
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarkerAnswerTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ObjectiveThemeMarkerAnswerTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading [Language] column DELETE state to child [ObjectiveThemeMarkerAnswerTranslation] rows';
		UPDATE [dbo].[ObjectiveThemeMarkerAnswerTranslation]
		SET [deleted] = 1
		FROM [dbo].[ObjectiveThemeMarkerAnswerTranslation] 
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarkerAnswerTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ObjectiveThemeMarkerAnswerTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_ObjectiveThemeMarkerAnswerTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Language_ThemeMarkerPageTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Language_ThemeMarkerPageTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeMarkerPageTranslation].[id]
		FROM [dbo].[ThemeMarkerPageTranslation]
		JOIN [inserted] ON [dbo].[ThemeMarkerPageTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ThemeMarkerPageTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading LANGUAGE column DELETE state to child THEMEMARKERPAGETRANSLATION rows';
		UPDATE [dbo].[ThemeMarkerPageTranslation]
		SET [deleted] = 1
		FROM [dbo].[ThemeMarkerPageTranslation] 
		JOIN [inserted] ON [dbo].[ThemeMarkerPageTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ThemeMarkerPageTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_ThemeMarkerPageTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Language_ThemeObjectiveFeedbackTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_Language_ThemeObjectiveFeedbackTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeObjectiveFeedbackTranslation].[id]
		FROM [dbo].[ThemeObjectiveFeedbackTranslation]
		JOIN [inserted] ON [dbo].[ThemeObjectiveFeedbackTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ThemeObjectiveFeedbackTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading Language column DELETE state to child ThemeObjectiveFeedbackTranslation rows';
		UPDATE [dbo].[ThemeObjectiveFeedbackTranslation]
		SET [deleted] = 1
		FROM [dbo].[ThemeObjectiveFeedbackTranslation] 
		JOIN [inserted] ON [dbo].[ThemeObjectiveFeedbackTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ThemeObjectiveFeedbackTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_ThemeObjectiveFeedbackTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Language_ThemeTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Language_ThemeTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeTranslation].[id]
		FROM [dbo].[ThemeTranslation]
		JOIN [inserted] ON [dbo].[ThemeTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ThemeTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading LANGUAGE column DELETE state to child THEMETRANSLATION rows';
		UPDATE [dbo].[ThemeTranslation]
		SET [deleted] = 1
		FROM [dbo].[ThemeTranslation] 
		JOIN [inserted] ON [dbo].[ThemeTranslation].[language] = [inserted].[code]
		WHERE [dbo].[ThemeTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_ThemeTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Language_TrailTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Language_TrailTranslation_Soft_Delete]
   ON  [dbo].[Language]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[TrailTranslation].[id]
		FROM [dbo].[TrailTranslation]
		JOIN [inserted] ON [dbo].[TrailTranslation].[language] = [inserted].[code]
		WHERE [dbo].[TrailTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading LANGUAGE column DELETE state to child TRAILTRANSLATION rows';
		UPDATE [dbo].[TrailTranslation]
		SET [deleted] = 1
		FROM [dbo].[TrailTranslation] 
		JOIN [inserted] ON [dbo].[TrailTranslation].[language] = [inserted].[code]
		WHERE [dbo].[TrailTranslation].[language] = [inserted].[code];
	END
END
GO
ALTER TABLE [dbo].[Language] ENABLE TRIGGER [TR_Language_TrailTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Location_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Location_InsertUpdateDelete] ON [dbo].[Location]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Location] SET [dbo].[Location].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Location].[id]
		END
GO
ALTER TABLE [dbo].[Location] ENABLE TRIGGER [TR_Location_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Location_Trail_Soft_Delete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_Location_Trail_Soft_Delete]
   ON  [dbo].[Location]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If column 'DELETED' was updated and the parent row had children
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	 AND EXISTS (
		 SELECT [dbo].[Trail].[id] 
		 FROM [dbo].[Trail] JOIN [inserted] ON [dbo].[Trail].[Location] = [inserted].[name] 
		 WHERE [dbo].[Trail].[deleted] = 0
	 )
	BEGIN
		UPDATE [dbo].[Location]
		SET [deleted] = 0
		FROM [dbo].[Location] JOIN [inserted] ON [dbo].[Location].[id] = [inserted].[id]
		WHERE [dbo].[Location].[id] = [inserted].[id];
		THROW 50000, N'Cannot delete parent LOCATION with child TRAILS due to Foreign Key constraint NO ACTION, rolling back', 1;
	END
END
GO
ALTER TABLE [dbo].[Location] ENABLE TRIGGER [TR_Location_Trail_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Marker_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Marker_InsertUpdateDelete] ON [dbo].[Marker]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Marker] SET [dbo].[Marker].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Marker].[id]
		END
GO
ALTER TABLE [dbo].[Marker] ENABLE TRIGGER [TR_Marker_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Marker_MarkerTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Marker_MarkerTranslation_Soft_Delete]
   ON  [dbo].[Marker]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[MarkerTranslation].[id]
		FROM [dbo].[MarkerTranslation]
		JOIN [inserted] ON [dbo].[MarkerTranslation].[Marker] = [inserted].[id]
		WHERE [dbo].[MarkerTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading TRAIL column DELETE state to child MarkerTranslation rows';
		UPDATE [dbo].[MarkerTranslation]
		SET [deleted] = 1
		FROM [dbo].[MarkerTranslation] 
		JOIN [inserted] ON [dbo].[MarkerTranslation].[Marker] = [inserted].[id]
		WHERE [dbo].[MarkerTranslation].[Marker] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Marker] ENABLE TRIGGER [TR_Marker_MarkerTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Marker_ObjectiveThemeMarker_Soft_Delete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_Marker_ObjectiveThemeMarker_Soft_Delete]
   ON  [dbo].[Marker]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ObjectiveThemeMarker].[id]
		FROM [dbo].[ObjectiveThemeMarker]
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarker].[Marker] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarker].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading Marker column DELETE state to child ObjectiveThemeMarker rows';
		UPDATE [dbo].[ObjectiveThemeMarker]
		SET [deleted] = 1
		FROM [dbo].[ObjectiveThemeMarker] 
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarker].[Marker] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarker].[Marker] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Marker] ENABLE TRIGGER [TR_Marker_ObjectiveThemeMarker_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Marker_ThemeMarker_Soft_Delete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Marker_ThemeMarker_Soft_Delete]
   ON  [dbo].[Marker]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeMarker].[id]
		FROM [dbo].[ThemeMarker]
		JOIN [inserted] ON [dbo].[ThemeMarker].[Marker] = [inserted].[id]
		WHERE [dbo].[ThemeMarker].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading TRAIL column DELETE state to child ThemeMarker rows';
		UPDATE [dbo].[ThemeMarker]
		SET [deleted] = 1
		FROM [dbo].[ThemeMarker] 
		JOIN [inserted] ON [dbo].[ThemeMarker].[Marker] = [inserted].[id]
		WHERE [dbo].[ThemeMarker].[Marker] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Marker] ENABLE TRIGGER [TR_Marker_ThemeMarker_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Marker_TrailMarker_Soft_Delete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Marker_TrailMarker_Soft_Delete]
   ON  [dbo].[Marker]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[TrailMarker].[id]
		FROM [dbo].[TrailMarker]
		JOIN [inserted] ON [dbo].[TrailMarker].[Marker] = [inserted].[id]
		WHERE [dbo].[TrailMarker].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading TRAIL column DELETE state to child TrailMarker rows';
		UPDATE [dbo].[TrailMarker]
		SET [deleted] = 1
		FROM [dbo].[TrailMarker] 
		JOIN [inserted] ON [dbo].[TrailMarker].[Marker] = [inserted].[id]
		WHERE [dbo].[TrailMarker].[Marker] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Marker] ENABLE TRIGGER [TR_Marker_TrailMarker_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_MarkerTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_MarkerTranslation_InsertUpdateDelete] ON [dbo].[MarkerTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[MarkerTranslation] SET [dbo].[MarkerTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[MarkerTranslation].[id]
		END
GO
ALTER TABLE [dbo].[MarkerTranslation] ENABLE TRIGGER [TR_MarkerTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_MarkerType_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_MarkerType_InsertUpdateDelete] ON [dbo].[MarkerType]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[MarkerType] SET [dbo].[MarkerType].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[MarkerType].[id]
		END
GO
ALTER TABLE [dbo].[MarkerType] ENABLE TRIGGER [TR_MarkerType_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_MarkerType_Marker_Soft_Delete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_MarkerType_Marker_Soft_Delete]
   ON  [dbo].[MarkerType]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[Marker].[id]
		FROM [dbo].[Marker]
		JOIN [inserted] ON [dbo].[Marker].[markerType] = [inserted].[id]
		WHERE [dbo].[Marker].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading MARKERTYPE column DELETE state to child MARKER rows';
		UPDATE [dbo].[Marker]
		SET [deleted] = 1
		FROM [dbo].[Marker] 
		JOIN [inserted] ON [dbo].[Marker].[markerType] = [inserted].[id]
		WHERE [dbo].[Marker].[markerType] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[MarkerType] ENABLE TRIGGER [TR_MarkerType_Marker_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_MarkerType_MarkerTypeTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_MarkerType_MarkerTypeTranslation_Soft_Delete]
   ON  [dbo].[MarkerType]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[MarkerTypeTranslation].[id]
		FROM [dbo].[MarkerTypeTranslation]
		JOIN [inserted] ON [dbo].[MarkerTypeTranslation].[markerType] = [inserted].[id]
		WHERE [dbo].[MarkerTypeTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading MARKERTYPE column DELETE state to child MARKERTYPETRANSLATION rows';
		UPDATE [dbo].[MarkerTypeTranslation]
		SET [deleted] = 1
		FROM [dbo].[MarkerTypeTranslation] 
		JOIN [inserted] ON [dbo].[MarkerTypeTranslation].[markerType] = [inserted].[id]
		WHERE [dbo].[MarkerTypeTranslation].[markerType] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[MarkerType] ENABLE TRIGGER [TR_MarkerType_MarkerTypeTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_MarkerTypeIcon_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_MarkerTypeIcon_InsertUpdateDelete] ON [dbo].[MarkerTypeIcon]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[MarkerTypeIcon] SET [dbo].[MarkerTypeIcon].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[MarkerTypeIcon].[id]
		END
GO
ALTER TABLE [dbo].[MarkerTypeIcon] ENABLE TRIGGER [TR_MarkerTypeIcon_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_MarkerTypeIcon_MarkerType_Soft_Delete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_MarkerTypeIcon_MarkerType_Soft_Delete]
   ON  [dbo].[MarkerTypeIcon]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If column 'DELETED' was updated and the parent row had children
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[MarkerType].[id] 
		FROM [dbo].[MarkerType] JOIN [inserted] ON [dbo].[MarkerType].[MarkerTypeIcon] = [inserted].[filename] 
		WHERE [dbo].[MarkerType].[deleted] = 0
	)
	BEGIN
		UPDATE [dbo].[MarkerTypeIcon]
		SET [deleted] = 0
		FROM [dbo].[MarkerTypeIcon] JOIN [inserted] ON [dbo].[MarkerTypeIcon].[id] = [inserted].[id]
		WHERE [dbo].[MarkerTypeIcon].[id] = [inserted].[id];
		THROW 50000, N'Cannot delete parent MarkerTypeIcon with child MARKERTYPES due to Foreign Key constraint NO ACTION, rolling back', 1;
	END
END
GO
ALTER TABLE [dbo].[MarkerTypeIcon] ENABLE TRIGGER [TR_MarkerTypeIcon_MarkerType_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_MarkerTypeTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_MarkerTypeTranslation_InsertUpdateDelete] ON [dbo].[MarkerTypeTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[MarkerTypeTranslation] SET [dbo].[MarkerTypeTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[MarkerTypeTranslation].[id]
		END
GO
ALTER TABLE [dbo].[MarkerTypeTranslation] ENABLE TRIGGER [TR_MarkerTypeTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ObjectiveThemeMarker_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ObjectiveThemeMarker_InsertUpdateDelete] ON [dbo].[ObjectiveThemeMarker]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ObjectiveThemeMarker] SET [dbo].[ObjectiveThemeMarker].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ObjectiveThemeMarker].[id]
		END
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker] ENABLE TRIGGER [TR_ObjectiveThemeMarker_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ObjectiveThemeMarker_ObjectiveThemeMarkerAnswer_Soft_Delete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_ObjectiveThemeMarker_ObjectiveThemeMarkerAnswer_Soft_Delete]
   ON  [dbo].[ObjectiveThemeMarker]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ObjectiveThemeMarkerAnswer].[id]
		FROM [dbo].[ObjectiveThemeMarkerAnswer]
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarkerAnswer].[ObjectiveThemeMarker] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarkerAnswer].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading [ObjectiveThemeMarker] column DELETE state to child [ObjectiveThemeMarkerAnswer] rows';
		UPDATE [dbo].[ObjectiveThemeMarkerAnswer]
		SET [deleted] = 1
		FROM [dbo].[ObjectiveThemeMarkerAnswer] 
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarkerAnswer].[ObjectiveThemeMarker] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarkerAnswer].[ObjectiveThemeMarker] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[ObjectiveThemeMarker] ENABLE TRIGGER [TR_ObjectiveThemeMarker_ObjectiveThemeMarkerAnswer_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ObjectiveThemeMarkerAnswer_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ObjectiveThemeMarkerAnswer_InsertUpdateDelete] ON [dbo].[ObjectiveThemeMarkerAnswer]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ObjectiveThemeMarkerAnswer] SET [dbo].[ObjectiveThemeMarkerAnswer].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ObjectiveThemeMarkerAnswer].[id]
		END
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer] ENABLE TRIGGER [TR_ObjectiveThemeMarkerAnswer_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ObjectiveThemeMarkerAnswer_ObjectiveThemeMarkerAnswerTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_ObjectiveThemeMarkerAnswer_ObjectiveThemeMarkerAnswerTranslation_Soft_Delete]
   ON  [dbo].[ObjectiveThemeMarkerAnswer]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ObjectiveThemeMarkerAnswerTranslation].[id]
		FROM [dbo].[ObjectiveThemeMarkerAnswerTranslation]
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarkerAnswerTranslation].[ObjectiveThemeMarkerAnswer] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarkerAnswerTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading [ObjectiveThemeMarkerAnswer] column DELETE state to child [ObjectiveThemeMarkerAnswerTranslation] rows';
		UPDATE [dbo].[ObjectiveThemeMarkerAnswerTranslation]
		SET [deleted] = 1
		FROM [dbo].[ObjectiveThemeMarkerAnswerTranslation] 
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarkerAnswerTranslation].[ObjectiveThemeMarkerAnswer] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarkerAnswerTranslation].[ObjectiveThemeMarkerAnswer] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswer] ENABLE TRIGGER [TR_ObjectiveThemeMarkerAnswer_ObjectiveThemeMarkerAnswerTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ObjectiveThemeMarkerAnswerTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ObjectiveThemeMarkerAnswerTranslation_InsertUpdateDelete] ON [dbo].[ObjectiveThemeMarkerAnswerTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ObjectiveThemeMarkerAnswerTranslation] SET [dbo].[ObjectiveThemeMarkerAnswerTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ObjectiveThemeMarkerAnswerTranslation].[id]
		END
GO
ALTER TABLE [dbo].[ObjectiveThemeMarkerAnswerTranslation] ENABLE TRIGGER [TR_ObjectiveThemeMarkerAnswerTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Theme_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Theme_InsertUpdateDelete] ON [dbo].[Theme]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Theme] SET [dbo].[Theme].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Theme].[id]
		END
GO
ALTER TABLE [dbo].[Theme] ENABLE TRIGGER [TR_Theme_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Theme_ThemeMarker_Soft_Delete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Theme_ThemeMarker_Soft_Delete]
   ON  [dbo].[Theme]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeMarker].[id]
		FROM [dbo].[ThemeMarker]
		JOIN [inserted] ON [dbo].[ThemeMarker].[Theme] = [inserted].[id]
		WHERE [dbo].[ThemeMarker].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading THEME column DELETE state to child THEMEMARKER rows';
		UPDATE [dbo].[ThemeMarker]
		SET [deleted] = 1
		FROM [dbo].[ThemeMarker] 
		JOIN [inserted] ON [dbo].[ThemeMarker].[Theme] = [inserted].[id]
		WHERE [dbo].[ThemeMarker].[Theme] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Theme] ENABLE TRIGGER [TR_Theme_ThemeMarker_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Theme_ThemeObjective_Soft_Delete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Theme_ThemeObjective_Soft_Delete]
   ON  [dbo].[Theme]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeObjective].[id]
		FROM [dbo].[ThemeObjective]
		JOIN [inserted] ON [dbo].[ThemeObjective].[Theme] = [inserted].[id]
		WHERE [dbo].[ThemeObjective].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading Theme column DELETE state to child ThemeObjective rows';
		UPDATE [dbo].[ThemeObjective]
		SET [deleted] = 1
		FROM [dbo].[ThemeObjective] 
		JOIN [inserted] ON [dbo].[ThemeObjective].[Theme] = [inserted].[id]
		WHERE [dbo].[ThemeObjective].[Theme] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Theme] ENABLE TRIGGER [TR_Theme_ThemeObjective_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Theme_ThemeTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Theme_ThemeTranslation_Soft_Delete]
   ON  [dbo].[Theme]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeTranslation].[id]
		FROM [dbo].[ThemeTranslation]
		JOIN [inserted] ON [dbo].[ThemeTranslation].[Theme] = [inserted].[id]
		WHERE [dbo].[ThemeTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading THEME column DELETE state to child THEMETRANSLATION rows';
		UPDATE [dbo].[ThemeTranslation]
		SET [deleted] = 1
		FROM [dbo].[ThemeTranslation] 
		JOIN [inserted] ON [dbo].[ThemeTranslation].[Theme] = [inserted].[id]
		WHERE [dbo].[ThemeTranslation].[Theme] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Theme] ENABLE TRIGGER [TR_Theme_ThemeTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Theme_TrailTheme_Soft_Delete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE TRIGGER [dbo].[TR_Theme_TrailTheme_Soft_Delete]
   ON  [dbo].[Theme]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[TrailTheme].[id]
		FROM [dbo].[TrailTheme]
		JOIN [inserted] ON [dbo].[TrailTheme].[Theme] = [inserted].[id]
		WHERE [dbo].[TrailTheme].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading Theme column DELETE state to child TrailTheme rows';
		UPDATE [dbo].[TrailTheme]
		SET [deleted] = 1
		FROM [dbo].[TrailTheme] 
		JOIN [inserted] ON [dbo].[TrailTheme].[Theme] = [inserted].[id]
		WHERE [dbo].[TrailTheme].[Theme] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Theme] ENABLE TRIGGER [TR_Theme_TrailTheme_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeMarker_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ThemeMarker_InsertUpdateDelete] ON [dbo].[ThemeMarker]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ThemeMarker] SET [dbo].[ThemeMarker].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ThemeMarker].[id]
		END
GO
ALTER TABLE [dbo].[ThemeMarker] ENABLE TRIGGER [TR_ThemeMarker_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeMarker_ThemeMarkerPage_Soft_Delete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_ThemeMarker_ThemeMarkerPage_Soft_Delete]
   ON  [dbo].[ThemeMarker]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeMarkerPage].[id]
		FROM [dbo].[ThemeMarkerPage]
		JOIN [inserted] ON [dbo].[ThemeMarkerPage].[ThemeMarker] = [inserted].[id]
		WHERE [dbo].[ThemeMarkerPage].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading THEMEMARKER column DELETE state to child THEMEMARKERPAGE rows';
		UPDATE [dbo].[ThemeMarkerPage]
		SET [deleted] = 1
		FROM [dbo].[ThemeMarkerPage] 
		JOIN [inserted] ON [dbo].[ThemeMarkerPage].[ThemeMarker] = [inserted].[id]
		WHERE [dbo].[ThemeMarkerPage].[ThemeMarker] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[ThemeMarker] ENABLE TRIGGER [TR_ThemeMarker_ThemeMarkerPage_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeMarkerPage_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ThemeMarkerPage_InsertUpdateDelete] ON [dbo].[ThemeMarkerPage]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ThemeMarkerPage] SET [dbo].[ThemeMarkerPage].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ThemeMarkerPage].[id]
		END
GO
ALTER TABLE [dbo].[ThemeMarkerPage] ENABLE TRIGGER [TR_ThemeMarkerPage_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeMarkerPage_ThemeMarkerPageTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_ThemeMarkerPage_ThemeMarkerPageTranslation_Soft_Delete]
   ON  [dbo].[ThemeMarkerPage]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeMarkerPageTranslation].[id]
		FROM [dbo].[ThemeMarkerPageTranslation]
		JOIN [inserted] ON [dbo].[ThemeMarkerPageTranslation].[ThemeMarkerPage] = [inserted].[id]
		WHERE [dbo].[ThemeMarkerPageTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading THEMEMARKERPAGE column DELETE state to child THEMEMARKERPAGETRANSLATION rows';
		UPDATE [dbo].[ThemeMarkerPageTranslation]
		SET [deleted] = 1
		FROM [dbo].[ThemeMarkerPageTranslation] 
		JOIN [inserted] ON [dbo].[ThemeMarkerPageTranslation].[ThemeMarkerPage] = [inserted].[id]
		WHERE [dbo].[ThemeMarkerPageTranslation].[ThemeMarkerPage] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[ThemeMarkerPage] ENABLE TRIGGER [TR_ThemeMarkerPage_ThemeMarkerPageTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeMarkerPageTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ThemeMarkerPageTranslation_InsertUpdateDelete] ON [dbo].[ThemeMarkerPageTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ThemeMarkerPageTranslation] SET [dbo].[ThemeMarkerPageTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ThemeMarkerPageTranslation].[id]
		END
GO
ALTER TABLE [dbo].[ThemeMarkerPageTranslation] ENABLE TRIGGER [TR_ThemeMarkerPageTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeObjective_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ThemeObjective_InsertUpdateDelete] ON [dbo].[ThemeObjective]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ThemeObjective] SET [dbo].[ThemeObjective].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ThemeObjective].[id]
		END
GO
ALTER TABLE [dbo].[ThemeObjective] ENABLE TRIGGER [TR_ThemeObjective_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeObjective_ObjectiveThemeMarker_Soft_Delete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_ThemeObjective_ObjectiveThemeMarker_Soft_Delete]
   ON  [dbo].[ThemeObjective]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ObjectiveThemeMarker].[id]
		FROM [dbo].[ObjectiveThemeMarker]
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarker].[ThemeObjective] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarker].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading [ThemeObjective] column DELETE state to child [ObjectiveThemeMarker] rows';
		UPDATE [dbo].[ObjectiveThemeMarker]
		SET [deleted] = 1
		FROM [dbo].[ObjectiveThemeMarker] 
		JOIN [inserted] ON [dbo].[ObjectiveThemeMarker].[ThemeObjective] = [inserted].[id]
		WHERE [dbo].[ObjectiveThemeMarker].[ThemeObjective] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[ThemeObjective] ENABLE TRIGGER [TR_ThemeObjective_ObjectiveThemeMarker_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeObjective_ThemeObjectiveFeedback_Soft_Delete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_ThemeObjective_ThemeObjectiveFeedback_Soft_Delete]
   ON  [dbo].[ThemeObjective]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeObjectiveFeedback].[id]
		FROM [dbo].[ThemeObjectiveFeedback]
		JOIN [inserted] ON [dbo].[ThemeObjectiveFeedback].[ThemeObjective] = [inserted].[id]
		WHERE [dbo].[ThemeObjectiveFeedback].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading ThemeObjective column DELETE state to child ThemeObjectiveFeedback rows';
		UPDATE [dbo].[ThemeObjectiveFeedback]
		SET [deleted] = 1
		FROM [dbo].[ThemeObjectiveFeedback] 
		JOIN [inserted] ON [dbo].[ThemeObjectiveFeedback].[ThemeObjective] = [inserted].[id]
		WHERE [dbo].[ThemeObjectiveFeedback].[ThemeObjective] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[ThemeObjective] ENABLE TRIGGER [TR_ThemeObjective_ThemeObjectiveFeedback_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeObjectiveFeedback_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ThemeObjectiveFeedback_InsertUpdateDelete] ON [dbo].[ThemeObjectiveFeedback]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ThemeObjectiveFeedback] SET [dbo].[ThemeObjectiveFeedback].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ThemeObjectiveFeedback].[id]
		END
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedback] ENABLE TRIGGER [TR_ThemeObjectiveFeedback_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeObjectiveFeedback_ThemeObjectiveFeedbackTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TR_ThemeObjectiveFeedback_ThemeObjectiveFeedbackTranslation_Soft_Delete]
   ON  [dbo].[ThemeObjectiveFeedback]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[ThemeObjectiveFeedbackTranslation].[id]
		FROM [dbo].[ThemeObjectiveFeedbackTranslation]
		JOIN [inserted] ON [dbo].[ThemeObjectiveFeedbackTranslation].[ThemeObjectiveFeedback] = [inserted].[id]
		WHERE [dbo].[ThemeObjectiveFeedbackTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading ThemeObjectiveFeedback column DELETE state to child ThemeObjectiveFeedbackTranslation rows';
		UPDATE [dbo].[ThemeObjectiveFeedbackTranslation]
		SET [deleted] = 1
		FROM [dbo].[ThemeObjectiveFeedbackTranslation] 
		JOIN [inserted] ON [dbo].[ThemeObjectiveFeedbackTranslation].[ThemeObjectiveFeedback] = [inserted].[id]
		WHERE [dbo].[ThemeObjectiveFeedbackTranslation].[ThemeObjectiveFeedback] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedback] ENABLE TRIGGER [TR_ThemeObjectiveFeedback_ThemeObjectiveFeedbackTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeObjectiveFeedbackTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ThemeObjectiveFeedbackTranslation_InsertUpdateDelete] ON [dbo].[ThemeObjectiveFeedbackTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ThemeObjectiveFeedbackTranslation] SET [dbo].[ThemeObjectiveFeedbackTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ThemeObjectiveFeedbackTranslation].[id]
		END
GO
ALTER TABLE [dbo].[ThemeObjectiveFeedbackTranslation] ENABLE TRIGGER [TR_ThemeObjectiveFeedbackTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_ThemeTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_ThemeTranslation_InsertUpdateDelete] ON [dbo].[ThemeTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[ThemeTranslation] SET [dbo].[ThemeTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[ThemeTranslation].[id]
		END
GO
ALTER TABLE [dbo].[ThemeTranslation] ENABLE TRIGGER [TR_ThemeTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Trail_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Trail_InsertUpdateDelete] ON [dbo].[Trail]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Trail] SET [dbo].[Trail].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Trail].[id]
		END
GO
ALTER TABLE [dbo].[Trail] ENABLE TRIGGER [TR_Trail_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Trail_TrailMarker_Soft_Delete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Trail_TrailMarker_Soft_Delete]
   ON  [dbo].[Trail]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[TrailMarker].[id]
		FROM [dbo].[TrailMarker]
		JOIN [inserted] ON [dbo].[TrailMarker].[Trail] = [inserted].[id]
		WHERE [dbo].[TrailMarker].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading TRAIL column DELETE state to child TrailMarker rows';
		UPDATE [dbo].[TrailMarker]
		SET [deleted] = 1
		FROM [dbo].[TrailMarker] 
		JOIN [inserted] ON [dbo].[TrailMarker].[Trail] = [inserted].[id]
		WHERE [dbo].[TrailMarker].[Trail] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Trail] ENABLE TRIGGER [TR_Trail_TrailMarker_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_Trail_TrailTranslation_Soft_Delete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_Trail_TrailTranslation_Soft_Delete]
   ON  [dbo].[Trail]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- If the updated column was DELETED
	IF UPDATE(deleted)
	-- And the inserted value was TRUE/1
	AND EXISTS (
		SELECT [inserted].[deleted]
		FROM [inserted]
		WHERE [inserted].[deleted] = 1
	)
	-- And the parent row had children that don't have their DELETED set to TRUE/1
	AND EXISTS (
		SELECT [dbo].[TrailTranslation].[id]
		FROM [dbo].[TrailTranslation]
		JOIN [inserted] ON [dbo].[TrailTranslation].[Trail] = [inserted].[id]
		WHERE [dbo].[TrailTranslation].[deleted] = 0
	)
	BEGIN
		PRINT N'Cascading TRAIL column DELETE state to child TRAILTRANSLATION rows';
		UPDATE [dbo].[TrailTranslation]
		SET [deleted] = 1
		FROM [dbo].[TrailTranslation] 
		JOIN [inserted] ON [dbo].[TrailTranslation].[Trail] = [inserted].[id]
		WHERE [dbo].[TrailTranslation].[Trail] = [inserted].[id];
	END
END
GO
ALTER TABLE [dbo].[Trail] ENABLE TRIGGER [TR_Trail_TrailTranslation_Soft_Delete]
GO
/****** Object:  Trigger [dbo].[TR_TrailMarker_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_TrailMarker_InsertUpdateDelete] ON [dbo].[TrailMarker]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[TrailMarker] SET [dbo].[TrailMarker].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[TrailMarker].[id]
		END
GO
ALTER TABLE [dbo].[TrailMarker] ENABLE TRIGGER [TR_TrailMarker_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_TrailTheme_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_TrailTheme_InsertUpdateDelete] ON [dbo].[TrailTheme]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[TrailTheme] SET [dbo].[TrailTheme].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[TrailTheme].[id]
		END
GO
ALTER TABLE [dbo].[TrailTheme] ENABLE TRIGGER [TR_TrailTheme_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_TrailTranslation_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_TrailTranslation_InsertUpdateDelete] ON [dbo].[TrailTranslation]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[TrailTranslation] SET [dbo].[TrailTranslation].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[TrailTranslation].[id]
		END
GO
ALTER TABLE [dbo].[TrailTranslation] ENABLE TRIGGER [TR_TrailTranslation_InsertUpdateDelete]
GO
/****** Object:  Trigger [dbo].[TR_Version_InsertUpdateDelete]    Script Date: 28.6.2019 16.55.52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[TR_Version_InsertUpdateDelete] ON [dbo].[Version]
		   AFTER INSERT, UPDATE, DELETE
		AS
		BEGIN
			SET NOCOUNT ON;
			IF TRIGGER_NESTLEVEL() > 3 RETURN;

			UPDATE [dbo].[Version] SET [dbo].[Version].[updatedAt] = CONVERT (DATETIMEOFFSET(3), SYSUTCDATETIME())
			FROM INSERTED
			WHERE INSERTED.id = [dbo].[Version].[id]
		END
GO
ALTER TABLE [dbo].[Version] ENABLE TRIGGER [TR_Version_InsertUpdateDelete]
GO
