USE [FinancialSupportDB1]
GO
/****** Object:  Table [dbo].[Parcelas]    Script Date: 08/12/2022 15:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parcelas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdEmprestimo] [int] NOT NULL,
	[DataParcela] [datetime] NULL,
	[ValorParcela] [decimal](10, 2) NULL,
	[DataPagamento] [datetime] NULL,
	[ValorPagamento] [decimal](10, 2) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Parcelas]  WITH CHECK ADD  CONSTRAINT [FK_Parcelas_Emprestimos] FOREIGN KEY([IdEmprestimo])
REFERENCES [dbo].[Emprestimos] ([Id])
GO
ALTER TABLE [dbo].[Parcelas] CHECK CONSTRAINT [FK_Parcelas_Emprestimos]
GO
