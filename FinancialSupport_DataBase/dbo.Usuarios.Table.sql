USE [FinancialSupportDB1]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 08/12/2022 15:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nchar](100) NOT NULL,
	[Foto] [nchar](100) NULL,
	[Limite] [decimal](10, 2) NULL,
	[DDDTelefone1] [nchar](2) NULL,
	[Telefone1] [nchar](9) NULL,
	[DDDTelefone2] [nchar](2) NULL,
	[Telefone2] [nchar](9) NULL,
	[Endereco] [nchar](100) NULL,
	[Empresa] [nchar](100) NULL,
	[LimiteDisponivel] [decimal](10, 2) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
