/*
 VS [Menu] -> [Tool] -> [SQL Server] -> [New Query] ->
 [[Connect Server Dialog]] -> [Local] ->
 Server Name:   MSSQLLocalDB
 Database Name: WinFormGUI\WinFormSample\ReverseReference\RR10_EntityDataModel\EF_CodeFirstProduct\SubDbContextEntityProductRR.cs

 -> [OK] 
 
 => createted Table ProductModelRR.
      �� FQCN (= Full qualified class name) ���S�C���N���X��:
          WinFormGUI\WinFormSample\ReverseReference\RR10_EntityDataModel\
          EF_CodeFirstProduct\SubDbContextEntityProductRR.dbo.ProductModelRRs.cs

 => This file opened.
*/
/*
CREATE TABLE [dbo].[ProductModelRRs] (
 [ProductId] INT IDENTITY(1,1) NOT NULL,
 [Name] NVARCHAR(200) NOT NULL,
 [Price] INT ,
 PRIMARY KEY CLUSTERED ([ProductId] ASC)
 );
 */

-- SELECT * From ProductModelRRs;

/*
1	Pzk-III	300
2	Pzk-IV-H	800
3	Pzk-V Tiger	1700
4	Pzk-VI-A PanterA	1600
5	Pzk-III	300
6	Pzk-IV-H	800
7	Pzk-V Tiger	1700
8	Pzk-VI-A PanterA	1600
*/
 
 -- DROP TABLE [dbo].[ProductModelRRs];

 --

 /*
 ���f���ύX: int Stock�v���p�e�B��ǉ�
 PackageManagerConsole: Migration���s
   => �ڍׁkMigrationProductRR.txt�l
*/
