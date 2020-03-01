USE [master]
GO
/****** Object:  Database [LinijskiMenadzer]    Script Date: 26.02.2020 10:54:18 ******/
CREATE DATABASE [LinijskiMenadzer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LinijskiMenadzer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\LinijskiMenadzer.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LinijskiMenadzer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\LinijskiMenadzer_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [LinijskiMenadzer] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LinijskiMenadzer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LinijskiMenadzer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET ARITHABORT OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LinijskiMenadzer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LinijskiMenadzer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LinijskiMenadzer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LinijskiMenadzer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET RECOVERY FULL 
GO
ALTER DATABASE [LinijskiMenadzer] SET  MULTI_USER 
GO
ALTER DATABASE [LinijskiMenadzer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LinijskiMenadzer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LinijskiMenadzer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LinijskiMenadzer] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LinijskiMenadzer] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LinijskiMenadzer] SET QUERY_STORE = OFF
GO
USE [LinijskiMenadzer]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [LinijskiMenadzer]
GO
/****** Object:  Table [dbo].[DolazakOdlazak]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DolazakOdlazak](
	[DolazakOdlazakID] [smallint] IDENTITY(1,1) NOT NULL,
	[GornjaGranicaDolaska] [time](7) NULL,
	[DonjaGranicaDolaska] [time](7) NULL,
	[GornjaGranicaOdlaska] [time](7) NULL,
	[DonjaGranicaOdlaska] [time](7) NULL,
 CONSTRAINT [PK_DolazakOdlazak] PRIMARY KEY CLUSTERED 
(
	[DolazakOdlazakID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EvidencijaRada]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EvidencijaRada](
	[EvidencijaRadaID] [smallint] IDENTITY(1,1) NOT NULL,
	[TipID] [tinyint] NOT NULL,
	[RadnikID] [smallint] NOT NULL,
	[Pocetak] [datetime] NOT NULL,
	[Kraj] [datetime] NOT NULL,
 CONSTRAINT [PK_EvidencijaRada] PRIMARY KEY CLUSTERED 
(
	[EvidencijaRadaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GodisnjiOdmor]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GodisnjiOdmor](
	[GodisnjiOdmorID] [smallint] IDENTITY(1,1) NOT NULL,
	[RadnikID] [smallint] NOT NULL,
	[BrojDana] [tinyint] NOT NULL,
 CONSTRAINT [PK_GodisnjiOdmor] PRIMARY KEY CLUSTERED 
(
	[GodisnjiOdmorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GrupaRadnik]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrupaRadnik](
	[GrupaRadnikID] [smallint] IDENTITY(1,1) NOT NULL,
	[GrupaID] [smallint] NOT NULL,
	[RadnikID] [smallint] NOT NULL,
	[Vođa] [bit] NULL,
	[DatumOd] [datetime] NULL,
	[DatumDo] [datetime] NULL,
 CONSTRAINT [PK_GrupaRadnik] PRIMARY KEY CLUSTERED 
(
	[GrupaRadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GrupaZaDorucak]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrupaZaDorucak](
	[GrupaZaDorucakID] [smallint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_GrupaZaDorucak] PRIMARY KEY CLUSTERED 
(
	[GrupaZaDorucakID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Grupe]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grupe](
	[GrupaID] [smallint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](70) NOT NULL,
	[TipGrupeID] [smallint] NULL,
	[DatumOd] [datetime] NULL,
	[DatumDo] [datetime] NULL,
 CONSTRAINT [PK_Grupe] PRIMARY KEY CLUSTERED 
(
	[GrupaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Izlaz]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Izlaz](
	[IzlazID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Izlaz] PRIMARY KEY CLUSTERED 
(
	[IzlazID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NadredjeniRadnik]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NadredjeniRadnik](
	[ID] [smallint] IDENTITY(1,1) NOT NULL,
	[RadnikID] [smallint] NOT NULL,
	[NadredjeniRadnikID] [smallint] NOT NULL,
 CONSTRAINT [PK_NadredjeniRadnik] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NeradniDani]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NeradniDani](
	[NeradniDaniID] [smallint] IDENTITY(1,1) NOT NULL,
	[NeradniDan] [datetime] NOT NULL,
 CONSTRAINT [PK_NeradniDani] PRIMARY KEY CLUSTERED 
(
	[NeradniDaniID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Obavjestenje]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Obavjestenje](
	[ObavjestenjeID] [smallint] IDENTITY(1,1) NOT NULL,
	[PosiljalacID] [smallint] NOT NULL,
	[PrimalacID] [smallint] NOT NULL,
	[SadrzajID] [smallint] NOT NULL,
	[Odobreno] [bit] NULL,
	[Pregledano] [bit] NULL,
	[TipObavjestenjaID] [smallint] NOT NULL,
	[DatumObavjestenja] [datetime] NULL,
	[Odgovor] [nvarchar](max) NULL,
	[GrupaID] [smallint] NULL,
 CONSTRAINT [PK_Obavjestenje] PRIMARY KEY CLUSTERED 
(
	[ObavjestenjeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Radnik]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Radnik](
	[RadnikID] [smallint] IDENTITY(1,1) NOT NULL,
	[Ime] [nvarchar](100) NOT NULL,
	[Prezime] [nvarchar](100) NOT NULL,
	[SektorID] [tinyint] NULL,
	[DomenskoIme] [nvarchar](100) NULL,
	[EmailAdresa] [nvarchar](50) NOT NULL,
	[Lozinka] [nvarchar](50) NOT NULL,
	[SlikaID] [smallint] NULL,
	[BrojTelefona] [varchar](15) NULL,
	[Pol] [nvarchar](1) NULL,
 CONSTRAINT [PK_Radnik] PRIMARY KEY CLUSTERED 
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RadnikGrupaZaDorucak]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RadnikGrupaZaDorucak](
	[RadnikGrupaZaDorucakID] [smallint] IDENTITY(1,1) NOT NULL,
	[RadnikID] [smallint] NOT NULL,
	[GrupaZaDorucakID] [smallint] NOT NULL,
 CONSTRAINT [PK_RadnikGrupaZaDorucak] PRIMARY KEY CLUSTERED 
(
	[RadnikGrupaZaDorucakID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RadnikRola]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RadnikRola](
	[RadnikRolaID] [smallint] IDENTITY(1,1) NOT NULL,
	[RolaID] [tinyint] NOT NULL,
	[RadnikID] [smallint] NOT NULL,
 CONSTRAINT [PK_RadnikRola] PRIMARY KEY CLUSTERED 
(
	[RadnikRolaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RadnikUser]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RadnikUser](
	[RadnikUserID] [smallint] IDENTITY(1,1) NOT NULL,
	[RadnikID] [smallint] NULL,
	[UserID] [smallint] NULL,
 CONSTRAINT [PK_RadnikUser] PRIMARY KEY CLUSTERED 
(
	[RadnikUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RadnoVrijeme]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RadnoVrijeme](
	[RadnoVrijemeID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Mjesec] [tinyint] NOT NULL,
	[RadnihSatiUMjesecu] [tinyint] NOT NULL,
 CONSTRAINT [PK_RadnoVrijeme] PRIMARY KEY CLUSTERED 
(
	[RadnoVrijemeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rola]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rola](
	[RolaID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Rola] PRIMARY KEY CLUSTERED 
(
	[RolaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SadrzajObavjestenja]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SadrzajObavjestenja](
	[SadrzajObavjestenjaID] [smallint] IDENTITY(1,1) NOT NULL,
	[TextObavjestenja] [nvarchar](max) NULL,
	[RadnikID] [smallint] NULL,
	[DatumOd] [datetime] NULL,
	[DatumDo] [datetime] NULL,
 CONSTRAINT [PK_SadrzajObavjestenja] PRIMARY KEY CLUSTERED 
(
	[SadrzajObavjestenjaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sektor]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sektor](
	[SektorID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](70) NOT NULL,
 CONSTRAINT [PK_Sektor] PRIMARY KEY CLUSTERED 
(
	[SektorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Slika]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slika](
	[SlikaID] [smallint] IDENTITY(1,1) NOT NULL,
	[Slika1] [varbinary](max) NULL,
 CONSTRAINT [PK_Slika] PRIMARY KEY CLUSTERED 
(
	[SlikaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SluzbeniPut]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SluzbeniPut](
	[SluzbeniPutID] [smallint] IDENTITY(1,1) NOT NULL,
	[RadnikID] [smallint] NOT NULL,
	[DatumPolaska] [datetime] NOT NULL,
	[DatumDolaska] [datetime] NOT NULL,
 CONSTRAINT [PK_SluzbeniPut] PRIMARY KEY CLUSTERED 
(
	[SluzbeniPutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TerminiZaDorucak]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerminiZaDorucak](
	[TerminiZaDorucakID] [smallint] IDENTITY(1,1) NOT NULL,
	[GrupaZaDorucakID] [smallint] NOT NULL,
	[DatumOd] [datetime] NOT NULL,
	[DatumDo] [datetime] NOT NULL,
	[VrijemPocetka] [time](7) NOT NULL,
 CONSTRAINT [PK_TerminiZaDorucak] PRIMARY KEY CLUSTERED 
(
	[TerminiZaDorucakID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tip]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tip](
	[TipID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tip] PRIMARY KEY CLUSTERED 
(
	[TipID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TipGrupe]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipGrupe](
	[TipGrupeID] [smallint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TipGrupe] PRIMARY KEY CLUSTERED 
(
	[TipGrupeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TipObavjestenja]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipObavjestenja](
	[TipObavjestenjaID] [smallint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](50) NOT NULL,
	[OpisObavjestenja] [nvarchar](150) NULL,
	[Odgovor] [bit] NULL,
 CONSTRAINT [PK_TipObavjestenja] PRIMARY KEY CLUSTERED 
(
	[TipObavjestenjaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TipPauze]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipPauze](
	[TipPauzeID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Naziv] [nvarchar](1) NULL,
 CONSTRAINT [PK_TipPauze] PRIMARY KEY CLUSTERED 
(
	[TipPauzeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [smallint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VrijemePauze]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VrijemePauze](
	[VrijemePauzeID] [tinyint] IDENTITY(1,1) NOT NULL,
	[TipPauzeID] [tinyint] NOT NULL,
	[PauzaMinute] [tinyint] NOT NULL,
	[VrijemePauzeOd] [time](7) NULL,
	[VrijemePauzeDo] [time](7) NULL,
 CONSTRAINT [PK_VrijemePauze] PRIMARY KEY CLUSTERED 
(
	[VrijemePauzeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VrijemeZaposlenja]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VrijemeZaposlenja](
	[VrijemeZaposlenjaID] [smallint] IDENTITY(1,1) NOT NULL,
	[RadnikID] [smallint] NOT NULL,
	[DatumZaposlenja] [datetime] NOT NULL,
	[DatumPrestankaRada] [datetime] NULL,
 CONSTRAINT [PK_VrijemeZaposlenja] PRIMARY KEY CLUSTERED 
(
	[VrijemeZaposlenjaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WorkTime]    Script Date: 26.02.2020 10:54:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkTime](
	[WorkTimeID] [smallint] IDENTITY(1,1) NOT NULL,
	[UserID] [smallint] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[InTimeText] [nvarchar](100) NOT NULL,
	[OutTimeText] [nvarchar](100) NOT NULL,
	[InTime] [time](7) NULL,
	[OutTime] [time](7) NULL,
	[IzlazID] [tinyint] NOT NULL,
 CONSTRAINT [PK_WorkTime] PRIMARY KEY CLUSTERED 
(
	[WorkTimeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_FK__Evidencij__TipID__74AE54BC]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK__Evidencij__TipID__74AE54BC] ON [dbo].[EvidencijaRada]
(
	[TipID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_EvidencijaRada_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_EvidencijaRada_Radnik] ON [dbo].[EvidencijaRada]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_GodisnjiOdmor_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_GodisnjiOdmor_Radnik] ON [dbo].[GodisnjiOdmor]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK__GrupaRadn__Grupa__6C190EBB]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK__GrupaRadn__Grupa__6C190EBB] ON [dbo].[GrupaRadnik]
(
	[GrupaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_GrupaRadnik_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_GrupaRadnik_Radnik] ON [dbo].[GrupaRadnik]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Grupe_TipGrupe]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Grupe_TipGrupe] ON [dbo].[Grupe]
(
	[TipGrupeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_NadrdjeniRadnik_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_NadrdjeniRadnik_Radnik] ON [dbo].[NadredjeniRadnik]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_NadrdjeniRadnik_Radnik1]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_NadrdjeniRadnik_Radnik1] ON [dbo].[NadredjeniRadnik]
(
	[NadredjeniRadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Obavjestenje_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Obavjestenje_Radnik] ON [dbo].[Obavjestenje]
(
	[PosiljalacID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Obavjestenje_Radnik1]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Obavjestenje_Radnik1] ON [dbo].[Obavjestenje]
(
	[PrimalacID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Obavjestenje_SadrajObavjestenja]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Obavjestenje_SadrajObavjestenja] ON [dbo].[Obavjestenje]
(
	[SadrzajID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Obavjestenje_TipObavjestenja]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Obavjestenje_TipObavjestenja] ON [dbo].[Obavjestenje]
(
	[TipObavjestenjaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK__Radnik__SlikaID__02084FDA]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK__Radnik__SlikaID__02084FDA] ON [dbo].[Radnik]
(
	[SlikaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_RadnikGrupaZaDorucak_GrupaZaDorucak]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_RadnikGrupaZaDorucak_GrupaZaDorucak] ON [dbo].[RadnikGrupaZaDorucak]
(
	[GrupaZaDorucakID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_RadnikGrupaZaDorucak_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_RadnikGrupaZaDorucak_Radnik] ON [dbo].[RadnikGrupaZaDorucak]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK__RadnikRol__RolaI__6B24EA82]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK__RadnikRol__RolaI__6B24EA82] ON [dbo].[RadnikRola]
(
	[RolaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_RadnikRola_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_RadnikRola_Radnik] ON [dbo].[RadnikRola]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_RadnikUser_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_RadnikUser_Radnik] ON [dbo].[RadnikUser]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_RadnikUser_User]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_RadnikUser_User] ON [dbo].[RadnikUser]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_SadrajObavjestenja_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_SadrajObavjestenja_Radnik] ON [dbo].[SadrzajObavjestenja]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_SluzbeniPut_Radnik]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_SluzbeniPut_Radnik] ON [dbo].[SluzbeniPut]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_TerminiZaDorucak_GrupaZaDorucak]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_TerminiZaDorucak_GrupaZaDorucak] ON [dbo].[TerminiZaDorucak]
(
	[GrupaZaDorucakID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_VrijemePauze_TipPauze]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_VrijemePauze_TipPauze] ON [dbo].[VrijemePauze]
(
	[TipPauzeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_VrijemeZaposlenja_RadnikView]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_VrijemeZaposlenja_RadnikView] ON [dbo].[VrijemeZaposlenja]
(
	[RadnikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK__WorkTime__UserID__797309D9]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK__WorkTime__UserID__797309D9] ON [dbo].[WorkTime]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_WorkTime_Izlaz]    Script Date: 26.02.2020 10:54:18 ******/
CREATE NONCLUSTERED INDEX [IX_FK_WorkTime_Izlaz] ON [dbo].[WorkTime]
(
	[IzlazID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EvidencijaRada]  WITH CHECK ADD  CONSTRAINT [FK__Evidencij__TipID__74AE54BC] FOREIGN KEY([TipID])
REFERENCES [dbo].[Tip] ([TipID])
GO
ALTER TABLE [dbo].[EvidencijaRada] CHECK CONSTRAINT [FK__Evidencij__TipID__74AE54BC]
GO
ALTER TABLE [dbo].[EvidencijaRada]  WITH CHECK ADD  CONSTRAINT [FK_EvidencijaRada_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[EvidencijaRada] CHECK CONSTRAINT [FK_EvidencijaRada_Radnik]
GO
ALTER TABLE [dbo].[GodisnjiOdmor]  WITH CHECK ADD  CONSTRAINT [FK_GodisnjiOdmor_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[GodisnjiOdmor] CHECK CONSTRAINT [FK_GodisnjiOdmor_Radnik]
GO
ALTER TABLE [dbo].[GrupaRadnik]  WITH CHECK ADD  CONSTRAINT [FK__GrupaRadn__Grupa__6C190EBB] FOREIGN KEY([GrupaID])
REFERENCES [dbo].[Grupe] ([GrupaID])
GO
ALTER TABLE [dbo].[GrupaRadnik] CHECK CONSTRAINT [FK__GrupaRadn__Grupa__6C190EBB]
GO
ALTER TABLE [dbo].[GrupaRadnik]  WITH CHECK ADD  CONSTRAINT [FK_GrupaRadnik_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[GrupaRadnik] CHECK CONSTRAINT [FK_GrupaRadnik_Radnik]
GO
ALTER TABLE [dbo].[Grupe]  WITH CHECK ADD  CONSTRAINT [FK_Grupe_TipGrupe] FOREIGN KEY([TipGrupeID])
REFERENCES [dbo].[TipGrupe] ([TipGrupeID])
GO
ALTER TABLE [dbo].[Grupe] CHECK CONSTRAINT [FK_Grupe_TipGrupe]
GO
ALTER TABLE [dbo].[NadredjeniRadnik]  WITH CHECK ADD  CONSTRAINT [FK_NadrdjeniRadnik_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[NadredjeniRadnik] CHECK CONSTRAINT [FK_NadrdjeniRadnik_Radnik]
GO
ALTER TABLE [dbo].[NadredjeniRadnik]  WITH CHECK ADD  CONSTRAINT [FK_NadrdjeniRadnik_Radnik1] FOREIGN KEY([NadredjeniRadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[NadredjeniRadnik] CHECK CONSTRAINT [FK_NadrdjeniRadnik_Radnik1]
GO
ALTER TABLE [dbo].[Obavjestenje]  WITH CHECK ADD  CONSTRAINT [FK_Obavjestenje_Radnik] FOREIGN KEY([PosiljalacID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[Obavjestenje] CHECK CONSTRAINT [FK_Obavjestenje_Radnik]
GO
ALTER TABLE [dbo].[Obavjestenje]  WITH CHECK ADD  CONSTRAINT [FK_Obavjestenje_Radnik1] FOREIGN KEY([PrimalacID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[Obavjestenje] CHECK CONSTRAINT [FK_Obavjestenje_Radnik1]
GO
ALTER TABLE [dbo].[Obavjestenje]  WITH CHECK ADD  CONSTRAINT [FK_Obavjestenje_SadrajObavjestenja] FOREIGN KEY([SadrzajID])
REFERENCES [dbo].[SadrzajObavjestenja] ([SadrzajObavjestenjaID])
GO
ALTER TABLE [dbo].[Obavjestenje] CHECK CONSTRAINT [FK_Obavjestenje_SadrajObavjestenja]
GO
ALTER TABLE [dbo].[Obavjestenje]  WITH CHECK ADD  CONSTRAINT [FK_Obavjestenje_TipObavjestenja] FOREIGN KEY([TipObavjestenjaID])
REFERENCES [dbo].[TipObavjestenja] ([TipObavjestenjaID])
GO
ALTER TABLE [dbo].[Obavjestenje] CHECK CONSTRAINT [FK_Obavjestenje_TipObavjestenja]
GO
ALTER TABLE [dbo].[Radnik]  WITH CHECK ADD  CONSTRAINT [FK__Radnik__SlikaID__02084FDA] FOREIGN KEY([SlikaID])
REFERENCES [dbo].[Slika] ([SlikaID])
GO
ALTER TABLE [dbo].[Radnik] CHECK CONSTRAINT [FK__Radnik__SlikaID__02084FDA]
GO
ALTER TABLE [dbo].[RadnikGrupaZaDorucak]  WITH CHECK ADD  CONSTRAINT [FK_RadnikGrupaZaDorucak_GrupaZaDorucak] FOREIGN KEY([GrupaZaDorucakID])
REFERENCES [dbo].[GrupaZaDorucak] ([GrupaZaDorucakID])
GO
ALTER TABLE [dbo].[RadnikGrupaZaDorucak] CHECK CONSTRAINT [FK_RadnikGrupaZaDorucak_GrupaZaDorucak]
GO
ALTER TABLE [dbo].[RadnikGrupaZaDorucak]  WITH CHECK ADD  CONSTRAINT [FK_RadnikGrupaZaDorucak_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[RadnikGrupaZaDorucak] CHECK CONSTRAINT [FK_RadnikGrupaZaDorucak_Radnik]
GO
ALTER TABLE [dbo].[RadnikRola]  WITH CHECK ADD  CONSTRAINT [FK__RadnikRol__RolaI__6B24EA82] FOREIGN KEY([RolaID])
REFERENCES [dbo].[Rola] ([RolaID])
GO
ALTER TABLE [dbo].[RadnikRola] CHECK CONSTRAINT [FK__RadnikRol__RolaI__6B24EA82]
GO
ALTER TABLE [dbo].[RadnikRola]  WITH CHECK ADD  CONSTRAINT [FK_RadnikRola_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[RadnikRola] CHECK CONSTRAINT [FK_RadnikRola_Radnik]
GO
ALTER TABLE [dbo].[RadnikUser]  WITH CHECK ADD  CONSTRAINT [FK_RadnikUser_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[RadnikUser] CHECK CONSTRAINT [FK_RadnikUser_Radnik]
GO
ALTER TABLE [dbo].[RadnikUser]  WITH CHECK ADD  CONSTRAINT [FK_RadnikUser_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[RadnikUser] CHECK CONSTRAINT [FK_RadnikUser_User]
GO
ALTER TABLE [dbo].[SadrzajObavjestenja]  WITH CHECK ADD  CONSTRAINT [FK_SadrajObavjestenja_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[SadrzajObavjestenja] CHECK CONSTRAINT [FK_SadrajObavjestenja_Radnik]
GO
ALTER TABLE [dbo].[SluzbeniPut]  WITH CHECK ADD  CONSTRAINT [FK_SluzbeniPut_Radnik] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[SluzbeniPut] CHECK CONSTRAINT [FK_SluzbeniPut_Radnik]
GO
ALTER TABLE [dbo].[TerminiZaDorucak]  WITH CHECK ADD  CONSTRAINT [FK_TerminiZaDorucak_GrupaZaDorucak] FOREIGN KEY([GrupaZaDorucakID])
REFERENCES [dbo].[GrupaZaDorucak] ([GrupaZaDorucakID])
GO
ALTER TABLE [dbo].[TerminiZaDorucak] CHECK CONSTRAINT [FK_TerminiZaDorucak_GrupaZaDorucak]
GO
ALTER TABLE [dbo].[VrijemePauze]  WITH CHECK ADD  CONSTRAINT [FK_VrijemePauze_TipPauze] FOREIGN KEY([TipPauzeID])
REFERENCES [dbo].[TipPauze] ([TipPauzeID])
GO
ALTER TABLE [dbo].[VrijemePauze] CHECK CONSTRAINT [FK_VrijemePauze_TipPauze]
GO
ALTER TABLE [dbo].[VrijemeZaposlenja]  WITH CHECK ADD  CONSTRAINT [FK_VrijemeZaposlenja_RadnikView] FOREIGN KEY([RadnikID])
REFERENCES [dbo].[Radnik] ([RadnikID])
GO
ALTER TABLE [dbo].[VrijemeZaposlenja] CHECK CONSTRAINT [FK_VrijemeZaposlenja_RadnikView]
GO
ALTER TABLE [dbo].[WorkTime]  WITH CHECK ADD  CONSTRAINT [FK__WorkTime__UserID__797309D9] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[WorkTime] CHECK CONSTRAINT [FK__WorkTime__UserID__797309D9]
GO
ALTER TABLE [dbo].[WorkTime]  WITH CHECK ADD  CONSTRAINT [FK_WorkTime_Izlaz] FOREIGN KEY([IzlazID])
REFERENCES [dbo].[Izlaz] ([IzlazID])
GO
ALTER TABLE [dbo].[WorkTime] CHECK CONSTRAINT [FK_WorkTime_Izlaz]
GO
USE [master]
GO
ALTER DATABASE [LinijskiMenadzer] SET  READ_WRITE 
GO
