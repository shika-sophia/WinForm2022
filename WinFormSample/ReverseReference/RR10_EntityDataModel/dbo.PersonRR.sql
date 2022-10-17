/*
CREATE TABLE [dbo].[PersonRR] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50)  NOT NULL,
    [Address]    NVARCHAR (MAX) NULL,
    [Tel]        VARCHAR (50)   NULL,
    [Email]      NVARCHAR (MAX) NULL,
    [CreateDate] DATE DEFAULT GETDATE()
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
*/
/*
SET IDENTITY_INSERT [dbo].[PersonRR] ON
INSERT INTO [dbo].[PersonRR] ([Id], [Name], [Address], [Tel], [Email], [CreateDate]) VALUES (1, N'Sophia', N'Osaka', NULL, NULL, N'2022-10-17')
INSERT INTO [dbo].[PersonRR] ([Id], [Name], [Address], [Tel], [Email], [CreateDate]) VALUES (2, N'Shika', N'Yokohama', NULL, NULL, N'2022-10-17')
INSERT INTO [dbo].[PersonRR] ([Id], [Name], [Address], [Tel], [Email], [CreateDate]) VALUES (3, N'Lily', N'New York', NULL, NULL, N'2022-10-17')
INSERT INTO [dbo].[PersonRR] ([Id], [Name], [Address], [Tel], [Email], [CreateDate]) VALUES (4, N'Yuri', N'Berlin', NULL, NULL, N'2022-10-17')
INSERT INTO [dbo].[PersonRR] ([Id], [Name], [Address], [Tel], [Email], [CreateDate]) VALUES (5, N'TeriChan', N'Georgia Tbilisi', NULL, NULL, N'2022-10-17')
SET IDENTITY_INSERT [dbo].[PersonRR] OFF
*/

-- VS [サーバーエクスプローラ] -> [テーブル] テーブル名を右クリック ->
-- [新しいクエリ] で SQL文を実行

-- SELECT * FROM PersonRR;
/*
1	Sophia	Osaka	NULL	NULL	2022-10-17
2	Shika	Yokohama	NULL	NULL	2022-10-17
3	Lily	New York	NULL	NULL	2022-10-17
4	Yuri	Berlin	NULL	NULL	2022-10-17
5	TeriChan	Georgia Tbilisi	NULL	NULL	2022-10-17
*/