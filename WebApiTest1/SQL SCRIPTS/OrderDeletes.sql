USE [ECommerce]
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderDetail') DROP TABLE [OrderDetail];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Product') DROP TABLE [Product];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Price') DROP TABLE [Price];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PriceType') DROP TABLE [PriceType];
IF EXISTS(select * from sys.tables where name = 'Order') DROP TABLE [Order];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentMethod') DROP TABLE [PaymentMethod];
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Payment') DROP TABLE [Payment];