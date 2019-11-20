CREATE PROCEDURE [dbo].[usp_CustomerSelect]
@CustomerID int
AS
BEGIN
    select * from Customers
	where CustomerID = @CustomerID;
END

CREATE PROCEDURE [dbo].[usp_CustomerSelectAll] 
AS
BEGIN
	SELECT * from Customers order by CustomerID;
END

CREATE PROCEDURE [dbo].[usp_CustomerCreate] 
	-- Add the parameters for the stored procedure here
	@CustomerID int output,
	@Name varchar(100), 
	@Address varchar(50), 
	@City varchar(20),
	@State char(2),
	@ZipCode char(15)
AS
BEGIN
    -- Insert statements for procedure here
	Insert into Customers
	(Name, Address, City, State, Zipcode, ConcurrencyID)
	values (@Name, @Address, @City, @State, @ZipCode, 1);
	set @CustomerID = @@IDENTITY;
END

CREATE PROCEDURE [dbo].[usp_CustomerDelete] 
	-- Add the parameters for the stored procedure here
	@CustomerID int, @ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Delete from Customers where CustomerID = @CustomerID and ConcurrencyID = @ConcurrencyID;
END

CREATE PROCEDURE [dbo].[usp_CustomerUpdate] 
	-- Add the parameters for the stored procedure here
	@CustomerID int,
	@Name varchar(100), 
	@Address varchar(50), 
	@City varchar(20),
	@State char(2),
	@ZipCode char(15),
	@ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Update Customers set
		Name = @Name,
		Address = @Address, 
		City = @City,
		State = @State,
		Zipcode = @ZipCode,
		ConcurrencyID = (@ConcurrencyID + 1)
	Where CustomerID = @CustomerID and ConcurrencyID = @ConcurrencyID;

END