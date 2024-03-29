USE [FinancialSupportDB1]
GO
/****** Object:  Table [dbo].[Emprestimos]    Script Date: 08/12/2022 15:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Emprestimos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Valor] [decimal](10, 2) NOT NULL,
	[Data] [datetime] NOT NULL,
	[Ativo] [bit] NOT NULL,
	[NumeroParcelas] [int] NOT NULL,
 CONSTRAINT [PK_Emprestimos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Emprestimos]  WITH CHECK ADD  CONSTRAINT [FK_Emprestimos_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Emprestimos] CHECK CONSTRAINT [FK_Emprestimos_Usuarios]
GO
