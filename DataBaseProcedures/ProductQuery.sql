CREATE PROCEDURE [dbo].[usp_ProductSelect]
@ProductID int
AS
BEGIN
    select * from Products
	where ProductID = @ProductID;
END

CREATE PROCEDURE [dbo].[usp_ProductSelectAll] 
AS
BEGIN
	SELECT * from Products order by ProductID;
END

CREATE PROCEDURE [dbo].[usp_ProductCreate] 
	-- Add the parameters for the stored procedure here
	@ProductID int output,
	@ProductCode char(10), 
	@Description varchar(50), 
	@UnitPrice money,
	@OnHandQuantity int
AS
BEGIN
    -- Insert statements for procedure here
	Insert into Products
	(ProductCode, Description, UnitPrice, OnHandQuantity, ConcurrencyID)
	values (@ProductCode, @Description, @UnitPrice, @OnHandQuantity, 1);
	set @ProductID = @@IDENTITY;
END

CREATE PROCEDURE [dbo].[usp_ProductDelete] 
	-- Add the parameters for the stored procedure here
	@ProductID int, @ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Delete from Products where ProductID = @ProductID and ConcurrencyID = @ConcurrencyID;
END

CREATE PROCEDURE [dbo].[usp_ProductUpdate] 
	-- Add the parameters for the stored procedure here
	@ProductID int output,
	@ProductCode char(10), 
	@Description varchar(50), 
	@UnitPrice money,
	@OnHandQuantity int,
	@ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Update Products set
		ProductCode = @ProductCode,
		Description = @Description, 
		Unitprice = @UnitPrice,
		OnHandQuantity = @OnHandQuantity,
		ConcurrencyID = (@ConcurrencyID + 1)
	Where ProductID = @ProductID and ConcurrencyID = @ConcurrencyID;

END