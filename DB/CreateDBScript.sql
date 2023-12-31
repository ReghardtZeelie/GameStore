USE [master]
GO
/****** Object:  Database [GameStoreDB]    Script Date: 10/12/2023 13:22:50 ******/
CREATE DATABASE [GameStoreDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GameStoreDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\GameStoreDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GameStoreDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\GameStoreDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [GameStoreDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GameStoreDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GameStoreDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GameStoreDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GameStoreDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GameStoreDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GameStoreDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [GameStoreDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GameStoreDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GameStoreDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GameStoreDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GameStoreDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GameStoreDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GameStoreDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GameStoreDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GameStoreDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GameStoreDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GameStoreDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GameStoreDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GameStoreDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GameStoreDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GameStoreDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GameStoreDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GameStoreDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GameStoreDB] SET RECOVERY FULL 
GO
ALTER DATABASE [GameStoreDB] SET  MULTI_USER 
GO
ALTER DATABASE [GameStoreDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GameStoreDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GameStoreDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GameStoreDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GameStoreDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'GameStoreDB', N'ON'
GO
ALTER DATABASE [GameStoreDB] SET QUERY_STORE = OFF
GO
USE [GameStoreDB]
GO
/****** Object:  User [Admin]    Script Date: 10/12/2023 13:22:50 ******/
CREATE USER [Admin] FOR LOGIN [Admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Admin]
GO
/****** Object:  Table [dbo].[tCart]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCart](
	[cID] [int] IDENTITY(1,1) NOT NULL,
	[cUserID] [int] NOT NULL,
	[cCartTotal] [decimal](18, 2) NULL,
	[cNumberofItems] [int] NULL,
 CONSTRAINT [PK_tCart] PRIMARY KEY CLUSTERED 
(
	[cID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tCartItems]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCartItems](
	[ciID] [int] IDENTITY(1,1) NOT NULL,
	[ciCartID] [int] NULL,
	[ciItemCode] [int] NULL,
	[ciQty] [int] NULL,
 CONSTRAINT [PK_tCartItems] PRIMARY KEY CLUSTERED 
(
	[ciID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tItems]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tItems](
	[IID] [int] IDENTITY(1,1) NOT NULL,
	[IName] [varchar](30) NULL,
	[ICost] [decimal](18, 2) NULL,
	[Iwholesale] [decimal](18, 2) NULL,
	[IRetail] [decimal](18, 2) NULL,
	[IMakeID] [int] NULL,
	[IModelID] [int] NULL,
	[Iimage] [image] NULL,
	[iFileName] [varchar](500) NULL,
	[iFileType] [varchar](5) NULL,
	[iDescription] [varchar](500) NULL,
 CONSTRAINT [PK_tItems] PRIMARY KEY CLUSTERED 
(
	[IID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tMake]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tMake](
	[mID] [int] IDENTITY(1,1) NOT NULL,
	[mDesc] [varchar](30) NULL,
 CONSTRAINT [PK_tMake] PRIMARY KEY CLUSTERED 
(
	[mID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tModel]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tModel](
	[TID] [int] IDENTITY(1,1) NOT NULL,
	[tDesc] [varchar](30) NULL,
 CONSTRAINT [PK_tType] PRIMARY KEY CLUSTERED 
(
	[TID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tUsers]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tUsers](
	[uID] [int] IDENTITY(1,1) NOT NULL,
	[UName] [varchar](30) NULL,
	[UPassword] [varchar](100) NULL,
	[UAge] [date] NULL,
	[Ulogin] [bit] NULL,
 CONSTRAINT [PK_tUsers] PRIMARY KEY CLUSTERED 
(
	[uID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tCart]  WITH CHECK ADD  CONSTRAINT [FK_tCart_tCart] FOREIGN KEY([cID])
REFERENCES [dbo].[tCart] ([cID])
GO
ALTER TABLE [dbo].[tCart] CHECK CONSTRAINT [FK_tCart_tCart]
GO
ALTER TABLE [dbo].[tCartItems]  WITH CHECK ADD  CONSTRAINT [FK_tCartItems_tItems] FOREIGN KEY([ciItemCode])
REFERENCES [dbo].[tItems] ([IID])
GO
ALTER TABLE [dbo].[tCartItems] CHECK CONSTRAINT [FK_tCartItems_tItems]
GO
ALTER TABLE [dbo].[tItems]  WITH CHECK ADD  CONSTRAINT [FK_tItems_tMake] FOREIGN KEY([IMakeID])
REFERENCES [dbo].[tMake] ([mID])
GO
ALTER TABLE [dbo].[tItems] CHECK CONSTRAINT [FK_tItems_tMake]
GO
ALTER TABLE [dbo].[tItems]  WITH CHECK ADD  CONSTRAINT [FK_tItems_tModel] FOREIGN KEY([IModelID])
REFERENCES [dbo].[tModel] ([TID])
GO
ALTER TABLE [dbo].[tItems] CHECK CONSTRAINT [FK_tItems_tModel]
GO
/****** Object:  StoredProcedure [dbo].[DCartItems]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DCartItems] --6 ,4,1

@CartID as INT,
@ItemCode as INT,
@Qty as INT

AS


if(select count(iid) FROM tItems where IID = @ItemCode) > 0
BEGIN


	if(select count(ciid) FROM tCartItems where ciCartID = @CartID and ciItemCode = @ItemCode) > 0
	BEGIN
	UPDATE tCartItems SET ciQty = (ciQty - @Qty) where ciCartID = @CartID and ciItemCode = @ItemCode

	if(select ciQty FROM tCartItems where ciCartID = @CartID and ciItemCode = @ItemCode) <= 0
		BEGIN
		DELETE tCartItems where ciCartID = @CartID and ciItemCode = @ItemCode
		END
		select ISNULL(cCartTotal,cast(0.00 as decimal(18,2))) as 'Total' from tCart with(nolock) where cID = @CartID
	END
	



END
GO
/****** Object:  StoredProcedure [dbo].[DItem]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DItem]

@itemCode as int
	
AS

if(select count(iid) FROM tItems where IID = @itemCode) > 0
BEGIN
delete tItems where IID = @itemCode
select cast(1 as bit) as 'status'
END
else
BEGIN
select cast(0 as bit) as 'status'
END
GO
/****** Object:  StoredProcedure [dbo].[ICart]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ICart]

@UserID as int

	
AS

if(select count(*) FROM tcart where cUserID = @UserID) = 0
BEGIN

insert INTO tcart 
(
cUserID
)
values
(
@UserID
)
END

select(select cid FROM tcart where cUserID = @UserID ) as 'CartID'



GO
/****** Object:  StoredProcedure [dbo].[ICartItem]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ICartItem]--3,6,2

@CartID as INT,
@ItemCode as INT,
@Qty as INT

AS


if(select count(iid) FROM tItems where IID = @ItemCode) > 0
BEGIN
if(select count(ciid) FROM tCartItems where ciCartID = @CartID and ciItemCode = @ItemCode) > 0
BEGIN
UPDATE tCartItems SET ciQty = (ciQty + @Qty) where ciCartID = @CartID and ciItemCode = @ItemCode
END

ELSE
BEGIN

INSERT INTO tCartItems
(
ciCartID,
ciItemCode,
ciQty
)
Values
(
@CartID,
@ItemCode,
@Qty
)
END

select ISNULL(cCartTotal,cast(0.00 as decimal(18,2))) as 'Total' from tCart with(nolock) where cID = @CartID
END
GO
/****** Object:  StoredProcedure [dbo].[IItem]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[IItem]

@Name As varchar(30),
@Description  As varchar(500),
@CostPrice as Decimal(18,2),
@Wholesale as Decimal(18,2),
@Retail as Decimal(18,2),
@Filetype As Varchar(5),
@Filename as varchar(256),
@ImageFile As image,
@Make As varchar(30),
@Model As varchar(30)

AS

Declare @MakeID AS int = 0
Declare @ModelID AS int = 0
--------------------------------------------------------------------------------------------
SET @MakeID = ISNULL((select mid FROM tMake (ROWLOCK) where UPPER(mDesc) = UPPER(@Make)),0)

if(@MakeID = 0)
BEGIN

INSERT INTO tMake
select @Make

SET @MakeID = (select mid FROM tMake (ROWLOCK) where UPPER(mDesc) = UPPER(@Make))
END

----------------------------------------------------------------------------------------------

SET @ModelID = ISNULL((select tid FROM tModel (ROWLOCK) where UPPER(tDesc) = UPPER(@Model)),0)

if(@ModelID = 0)
BEGIN

INSERT INTO tModel
select @Model

SET @ModelID = (select tid FROM tModel (ROWLOCK) where UPPER(tDesc) = UPPER(@Model))
END
-----------------------------------------------------------------------------------------------


if ISNULL((select  COUNt(iid) FROM tItems(rowlock)  where UPPER(IName) = UPPER(@Name)),0) = 0
BEGIN

insert INTO tItems
(
Iname,
iDescription,
ICost,
Iwholesale,
IRetail,
iFileType,
iFileName,
Iimage,
IMakeID,
IModelID
)
values
(
@Name,
@Description,
@CostPrice,
@Wholesale,
@Retail,
@Filetype,
@Filename,
@ImageFile,
@MakeID,
@ModelID
)

select (select cast(iid as varchar)  FROM tItems(rowlock)  where UPPER(IName) = UPPER(@Name)) as 'NewItemCode'

END
ELSE
BEGIN

select 'ExistingItem' as NewItemCode

END


GO
/****** Object:  StoredProcedure [dbo].[IUser]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[IUser] --'Reghardt',132,'05/12/2023 06:23:10'

@Name As varchar(30),
@Password As varchar(100),
@Age as Date
	
AS
Declare @UserID as INT
Declare @CartID AS INT 

if(select COUNT(uName) FROM  tusers(rowlock) where UPPER(uname) = UPPER(@Name)) = 0
BEGIN
Insert INTO tusers
(
uName,
UPassword,
uAge,
ULogin
)
values
(
@Name,
@Password,
@Age,
0
)
select (SELECT uName FROM tUsers WHERE UName = @Name) as UName --SCOPE_IDENTITY()
END
ELSE
BEGIN
select 'User name already exist' as uName
END



GO
/****** Object:  StoredProcedure [dbo].[LogoutUser]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[LogoutUser] 

@UserID as int

AS

if( select uLogin FROM tUSers where UID = @UserID)  = 1
BEGIN
Update tUSers SET uLogin = 0  where UID = @UserID
select 'Loggedout' as 'status'
END
else
BEGIN
select 'NotLoggedin' as 'status'
END

	

GO
/****** Object:  StoredProcedure [dbo].[QAllItems]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[QAllItems] --'dirt'

@Itemname as varchar(30)
	
AS

	
select 
i.IID as 'ItemCode',
i.IName as 'ItemName',
i.iDescription as 'Description',
m.mDesc as 'Make',
mo.tDesc as 'Model',
i.ICost as 'Cost',
i.Iwholesale as 'wholesale',
i.IRetail as 'Retail',
i.iFileName as 'ImageFileName',
i.iFileType as 'Extention',
 'Please use the view Image end point.' 'Image'

FROM 
tItems as i
INNER join tmake as m on m.mID = i.IMakeID
INNER join tModel as mo on mo.TID = i.IModelID
where i.IName LIKE '%' + @Itemname + '%'

GO
/****** Object:  StoredProcedure [dbo].[QCart]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[QCart] --6
@UserID as int
AS

select 

i.IName as 'ItemName',
i.iDescription as 'Description',
m.mDesc as 'Make',
mo.tDesc as 'Model',
i.IRetail as 'Retail',
tcartItems.ciQty as 'Qty'

FROM 
tcartItems inner join tCart on tCart.cID = tcartItems.ciCartID  and tCart.cUserID = @UserID
inner join tItems as i on tcartItems.ciItemCode = i.IID
INNER join tmake as m on m.mID = i.IMakeID
INNER join tModel as mo on mo.TID = i.IModelID
GO
/****** Object:  StoredProcedure [dbo].[QImageItem]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[QImageItem]
@ItemCode as int
AS

select 
Iimage as 'Image',
iFileName as 'FileName',
iFileType as 'extention' 

FROM tItems where IID = @ItemCode

GO
/****** Object:  StoredProcedure [dbo].[QUser]    Script Date: 10/12/2023 13:22:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[QUser] --'Reghardt','123'
	
@UserName As varchar(30),
@UserPassword As Varchar(100)

AS

select 
uId,
Uname,
isNULL(cID,0) as cartID,
ISNULL(ULogin,0) as ULogin
INTO #A
FRom 
tUsers as u
left join tCart as c on c.cUserID = u.uID
where  
UName = @UserName 
And uPassword = @UserPassword 


if(select COUNT(*) FROM #A) > 0
BEGIN
Update tUsers set Ulogin = 1 where uID = (select uid FROM #A)
END


select * FROM #A

drop table #A
GO
USE [master]
GO
ALTER DATABASE [GameStoreDB] SET  READ_WRITE 
GO
