USE [Desafio]
GO

SET IDENTITY_INSERT [BOTICARIO].[StatusCompra] ON 
INSERT [BOTICARIO].[StatusCompra] ([Id], [Descricao], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (1, N'Em validação', 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [BOTICARIO].[StatusCompra] ([Id], [Descricao], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (2, N'Aprovado', 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [BOTICARIO].[StatusCompra] OFF

SET IDENTITY_INSERT [BOTICARIO].[TipoHistorico] ON 
INSERT [BOTICARIO].[TipoHistorico] ([Id], [Descricao], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (1, N'Criação', 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [BOTICARIO].[TipoHistorico] ([Id], [Descricao], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (2, N'Alteração', 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [BOTICARIO].[TipoHistorico] ([Id], [Descricao], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (3, N'Exclusão', 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [BOTICARIO].[TipoHistorico] ([Id], [Descricao], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (4, N'Cadastro', 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [BOTICARIO].[TipoHistorico] OFF

SET IDENTITY_INSERT [BOTICARIO].[ParametroSistema] ON 
INSERT [BOTICARIO].[ParametroSistema] ([Id], [NomeParametro], [Valor], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (1, N'CpfCoringa', N'15350946056', 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [BOTICARIO].[ParametroSistema] OFF

SET IDENTITY_INSERT [BOTICARIO].[RegraCashback] ON 
INSERT [BOTICARIO].[RegraCashback] ([Id], [Inicio], [Fim], [Percentual], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (1, 0, 1000, 10, 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [BOTICARIO].[RegraCashback] ([Id], [Inicio], [Fim], [Percentual], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (2, 1000, 1500, 15, 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
INSERT [BOTICARIO].[RegraCashback] ([Id], [Inicio], [Fim], [Percentual], [Ativo], [DataCriacao], [DataAlteracao]) VALUES (3, 1500, 0, 20, 1, CAST(GETDATE() AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [BOTICARIO].[RegraCashback] OFF
