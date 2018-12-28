/*Orders*/

USE [ECommerce]

IF EXISTS(select * from sys.tables where name = 'Inventory') DROP TABLE [Inventory];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StatusType') DROP TABLE [StatusType];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Payment') DROP TABLE [Payment];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentMethod') DROP TABLE [PaymentMethod];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderDetail') DROP TABLE [OrderDetail];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Product') DROP TABLE [Product];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ProductFeature') DROP TABLE [ProductFeature];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'FeatureType') DROP TABLE [FeatureType];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Price') DROP TABLE [Price];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PriceType') DROP TABLE [PriceType];
IF EXISTS(select * from sys.tables where name = 'Order') DROP TABLE [Order];
-------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Promotion') DROP TABLE [Promotion];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PromotionType') DROP TABLE [PromotionType];
IF EXISTS (SELECT * FROM sys.tables where name = 'UserRole') DROP TABLE [UserRole];
IF EXISTS (SELECT * FROM sys.tables where name = 'UserClaim') DROP TABLE [UserClaim];
IF EXISTS (SELECT * FROM sys.tables where name = 'RoleRight') DROP TABLE [RoleRight];
IF EXISTS (SELECT * FROM sys.tables where name = 'Resource') DROP TABLE [Resource];
IF EXISTS (SELECT * FROM sys.tables where name = 'ResourceType') DROP TABLE [ResourceType];
IF EXISTS (SELECT * FROM sys.tables where name = 'RefreshToken') DROP TABLE [RefreshToken];
IF EXISTS (SELECT * FROM sys.tables where name = 'Module') DROP TABLE [Module];
IF EXISTS (SELECT * FROM sys.tables where name = 'Audience') DROP TABLE [Audience];
IF EXISTS(select * from sys.tables where name = 'User') DROP TABLE [User];
IF EXISTS(select * from sys.tables where name = 'UserType') DROP TABLE [UserType];
IF EXISTS(select * from sys.tables where name = 'Role') DROP TABLE [Role];
--------------------------------------------------------------------------------------------------------------------

GO

USE [ECommerce]

--------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL ROWGUIDCOL CONSTRAINT [DF_Role_Id]  DEFAULT (newsequentialid()),
	[Name] [varchar](100) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Role_Active]  DEFAULT (1),
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[UserType](
	[Id] [uniqueidentifier] NOT NULL ROWGUIDCOL CONSTRAINT [DF_UserType]  DEFAULT (newsequentialid()) ,
	[Name] [varchar](100) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_UserType_Active]  DEFAULT (1),
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO



CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL ROWGUIDCOL CONSTRAINT [DF_User]  DEFAULT (newsequentialid()),
	[UserTypeId] [UNIQUEIDENTIFIER] NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](64) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_User_Active]  DEFAULT (1),
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),
	CONSTRAINT [UN_User_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
	,CONSTRAINT [FK_User_UserType] FOREIGN KEY ([UserTypeId]) REFERENCES [dbo].[UserType] ([Id])

)
GO


CREATE TABLE [dbo].[Audience] (
    [Id]                   UNIQUEIDENTIFIER CONSTRAINT [DF_Audience] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [Name]                 VARCHAR (100)    NOT NULL,
    [Secret]               VARCHAR (100)    NOT NULL,
    [ApplicationType]      VARCHAR (100)    NOT NULL,
    [RefreshTokenLifeTime] INT              NOT NULL,
    [AllowedOrigin]        VARCHAR (100)    NOT NULL,
    [Active]               BIT              CONSTRAINT [DF_Audience_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion]           ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Audience] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO


CREATE TABLE [dbo].[Module] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_Module_Id] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [Name]        VARCHAR (100)    NOT NULL,
    [Description] VARCHAR (250)    NOT NULL,
    [Active]      BIT              CONSTRAINT [DF_Module_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion]  ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[RefreshToken] (
    [Id]              UNIQUEIDENTIFIER CONSTRAINT [DF_RefreshToken_Id] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [AudienceId]      UNIQUEIDENTIFIER NOT NULL,
    [IssuedUtc]       DATETIME         NOT NULL,
    [ExpiresUtc]      DATETIME         NOT NULL,
    [ProtectedTicket] VARCHAR (MAX)    NOT NULL,
    [Active]          BIT              CONSTRAINT [DF_RefreshToken_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion]      ROWVERSION       NOT NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RefreshToken_Audience] FOREIGN KEY ([AudienceId]) REFERENCES [dbo].[Audience] ([Id]),
    CONSTRAINT [FK_RefreshToken_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



CREATE TABLE [dbo].[ResourceType] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_ResourceType_Id] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [Name]        VARCHAR (100)    NOT NULL,
    [Description] VARCHAR (250)    NOT NULL,
    [Active]          BIT              CONSTRAINT [DF_ResourceType_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion]  ROWVERSION       NOT NULL,
    CONSTRAINT [PK_ResourceType] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO



CREATE TABLE [dbo].[Resource] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_Resource_Id] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [ParentId]       UNIQUEIDENTIFIER NULL,
    [ModuleId]       UNIQUEIDENTIFIER NOT NULL,
    [ResourceTypeId] UNIQUEIDENTIFIER NOT NULL,
    [Name]           VARCHAR (100)    NOT NULL,
    [Description]    VARCHAR (250)    NOT NULL,
    [Active]         BIT              CONSTRAINT [DF_Resource_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion]     ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Resource_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([Id]),
    CONSTRAINT [FK_Resource_Resource] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Resource] ([Id]),
    CONSTRAINT [FK_Resource_ResourceType] FOREIGN KEY ([ResourceTypeId]) REFERENCES [dbo].[ResourceType] ([Id])
);
GO


CREATE TABLE [dbo].[RoleRight] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF_RoleRight_Id] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [RoleId]     UNIQUEIDENTIFIER NOT NULL,
    [ResourceId] UNIQUEIDENTIFIER NOT NULL,
    [Active]     BIT              CONSTRAINT [DF_RoleRight_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion] ROWVERSION       NOT NULL,
    CONSTRAINT [PK_RoleRight] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RoleRight_Resource] FOREIGN KEY ([ResourceId]) REFERENCES [dbo].[Resource] ([Id]),
    CONSTRAINT [FK_RoleRight_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);
GO

CREATE TABLE [dbo].[UserClaim] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF_UserClaim_Id] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [Type]       VARCHAR (MAX)    NOT NULL,
    [Value]      VARCHAR (MAX)    NOT NULL,
    [Active]     BIT              CONSTRAINT [DF_UserClaim_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion] ROWVERSION       NOT NULL,
    CONSTRAINT [PK_UserClaim] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserClaim_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);
GO

CREATE TABLE [dbo].[UserRole] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF_UserRole_Id] DEFAULT (newsequentialid()) ROWGUIDCOL NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [RoleId]     UNIQUEIDENTIFIER NOT NULL,
    [Active]     BIT              CONSTRAINT [DF_UserRole_Active] DEFAULT ((1)) NOT NULL,
    [RowVersion] ROWVERSION       NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);
GO




CREATE TABLE [dbo].[PromotionType](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_PromotionType]  DEFAULT (newsequentialid()),
	[Value] [VARCHAR](255) NOT NULL,
	[Active][BIT] CONSTRAINT [DF_PromotionType_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_PromotionType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO





CREATE TABLE [dbo].[Promotion](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_Promotion]  DEFAULT (newsequentialid()),
	[PromotionTypeId] [UNIQUEIDENTIFIER] NOT NULL,
	[ResourceTypeId] [UNIQUEIDENTIFIER] NOT NULL,
	[StartDate] [DATETIME] NOT NULL,
	[EndDate] [DATETIME] NOT NULL,
	[Value] [VARCHAR] (255) NOT NULL,
	[Active][BIT] CONSTRAINT [DF_Promotion_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_Promotion] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),
	CONSTRAINT [FK_Promotion_PromotionType] FOREIGN KEY ([PromotionTypeId]) REFERENCES [dbo].[PromotionType] ([Id]),
	CONSTRAINT [FK_Promotion_ResourceType] FOREIGN KEY ([ResourceTypeId]) REFERENCES [dbo].[ResourceType] ([Id])
)
GO

---------------------------------------------------------------------------------
CREATE TABLE [dbo].[Order](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_Order]  DEFAULT (newsequentialid()),
	[CustomerId] [UNIQUEIDENTIFIER] NOT NULL,
	[Date] [DATETIME] NOT NULL,
	[Status] [VARCHAR](50) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_Order_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO



CREATE TABLE [dbo].[PriceType](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_PriceType]  DEFAULT (newsequentialid()),
	[Value] [VARCHAR](255) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_PriceType_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_PriceType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO


CREATE TABLE [dbo].[Price](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_Price]  DEFAULT (newsequentialid()),
	[MSRP] [varchar](255) NULL,
	[Discount] [varchar](255) NULL,
	[Taxes] [varchar](255) NULL,
	[PriceTypeId] [UNIQUEIDENTIFIER] NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_Price_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_Price] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),CONSTRAINT [FK_Price_PriceType] FOREIGN KEY ([PriceTypeId]) REFERENCES [dbo].[PriceType] ([Id])
)
GO

CREATE TABLE [dbo].[FeatureType](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_FeatureType]  DEFAULT (newsequentialid()),
	[Value] [VARCHAR](255) NOT NULL,
	[Key] [VARCHAR](255) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_FeatureType_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_FeatureType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE TABLE [dbo].[ProductFeature](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [ProductDF_Feature]  DEFAULT (newsequentialid()),
	[FeatureTypeId] [UNIQUEIDENTIFIER] NOT NULL,
	[Reference] [VARCHAR](255) NOT NULL,
	[Title] [VARCHAR](255) NOT NULL,
	[Description] [VARCHAR](255) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_ProductFeature_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [ProductPK_Feature] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),
	CONSTRAINT [FK_ProductFeature_FeatureType] FOREIGN KEY ([FeatureTypeId]) REFERENCES [dbo].[FeatureType] ([Id])
)
GO

CREATE TABLE [dbo].[Product](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_Product]  DEFAULT (newsequentialid()),
 	[PriceId] [UNIQUEIDENTIFIER] NOT NULL,
	[ProductFeatureId] [UNIQUEIDENTIFIER] NULL,
	[ModuleId] [UNIQUEIDENTIFIER] NOT NULL,
	[ModelNumber] [VARCHAR](20) NOT NULL,
 	[Variant] [VARCHAR](100) NOT NULL,
 	[Description] [varchar](255) NOT NULL,
 	[Active] [BIT] CONSTRAINT [DF_Product_Active] DEFAULT ((1)) NOT NULL,
 	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),
	CONSTRAINT [FK_Product_Price] FOREIGN KEY ([PriceId]) REFERENCES [dbo].[Price] ([Id]),
	CONSTRAINT [FK_Product_ProductFeature] FOREIGN KEY ([ProductFeatureId]) REFERENCES [dbo].[ProductFeature] ([Id]),
	CONSTRAINT [FK_Product_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([Id])
)
GO


CREATE TABLE [dbo].[OrderDetail](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_OrderDetail]  DEFAULT (newsequentialid()),
	[OrderId] [UNIQUEIDENTIFIER] NOT NULL,
	[ProductId] [UNIQUEIDENTIFIER] NOT NULL,
	[QuantityRequested] [INT] NOT NULL,
	[Price] [VARCHAR](20) NOT NULL,
	[Total] [VARCHAR](20) NOT NULL,
	[ShipmentStatus] [VARCHAR](20) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_OrderDetail_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id])
	 ,CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])

)
GO





CREATE TABLE [dbo].[PaymentMethod](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_PaymentMethod]  DEFAULT (newsequentialid()),
	[Description] [VARCHAR](100) NOT NULL,
	[Extra] [VARCHAR](100) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_PaymentMethod_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO


CREATE TABLE [dbo].[Payment](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_Payment]  DEFAULT (newsequentialid()),
	[OrderId] [UNIQUEIDENTIFIER] NOT NULL,
	[PaymentMethodId]	[UNIQUEIDENTIFIER]  NOT NULL,
	[PaymentIndicator]	[VARCHAR](255) NOT NULL,
	[Amount]	[VARCHAR](255) NOT NULL,
	[Status]	[VARCHAR](255) NOT NULL,
	[Installments]	[VARCHAR](255) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_Payment_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),CONSTRAINT [FK_Payment_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id])
	,CONSTRAINT [FK_Payment_PaymentMethod] FOREIGN KEY ([PaymentMethodId]) REFERENCES [dbo].[PaymentMethod] ([Id])
)
GO


CREATE TABLE [dbo].[StatusType](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_StatusType]  DEFAULT (newsequentialid()),
	[Value] [VARCHAR](255) NOT NULL,
	[Active] [BIT] CONSTRAINT [DF_StatusType_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_StatusType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO


CREATE TABLE [dbo].[Inventory](
	[Id] [UNIQUEIDENTIFIER] NOT NULL ROWGUIDCOL CONSTRAINT [DF_Inventory]  DEFAULT (newsequentialid()),
	[ProductId] [UNIQUEIDENTIFIER] NOT NULL,
	[StatusTypeId] [UNIQUEIDENTIFIER] NOT NULL,
	[QuantityOnReserve] [INT] NULL,
	[QuantityAvailable] [INT] NULL,
	[MinimumQuantityAvailable] [INT] NULL,
	[Active] [BIT] CONSTRAINT [DF_Inventory_Active] DEFAULT ((1)) NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
	CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),CONSTRAINT [FK_Inventory_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
	,CONSTRAINT [FK_Inventory_StatusType] FOREIGN KEY ([StatusTypeId]) REFERENCES [dbo].[StatusType] ([Id])
)
GO


