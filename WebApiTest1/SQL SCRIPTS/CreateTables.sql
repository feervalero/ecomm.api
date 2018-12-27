
DROP TABLE [MXBrand].[dbo].[Products];

CREATE TABLE [MXBrand].[dbo].[Products](
	[ID] uniqueidentifier primary key,
	[SKU] varchar(50),
	[Description] nvarchar(100),
	[Price] nvarchar(50),
	[BrandCode] nvarchar(3),
	[Segmento] nvarchar(100),
	[Category] nvarchar(100)
)