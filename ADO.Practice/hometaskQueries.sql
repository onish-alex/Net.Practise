--1
SELECT EmployeeID, FirstName, LastName FROM Northwind.dbo.Employees WHERE City = 'London' 

--2
SELECT TOP(1) Count(CustomerId) as Customers_Count  
FROM Northwind.dbo.Orders ords
GROUP BY EmployeeID
ORDER BY Count(OrderID) desc

--3
SELECT ords.ShipCity, ords.ShipCountry, Count(Distinct *) FROM Northwind.dbo.Orders ords
GROUP BY ords.ShipCity, ords.ShipCountry
HAVING Count(*) > 2

--4
SELECT TOP(1) ProductName 
FROM Northwind.dbo.Products pr JOIN Northwind.dbo.Categories cts ON pr.CategoryID = cts.CategoryID
WHERE cts.CategoryName = 'Seafood'
ORDER BY UnitPrice desc 

--5
SELECT DISTINCT csts.CustomerID, csts.ContactName 
FROM Northwind.dbo.Customers csts JOIN Northwind.dbo.Orders ords ON csts.CustomerID = ords.CustomerID
WHERE csts.City != ords.ShipCity

--6
CREATE FUNCTION getIndexedCustomersOrdersJoin()
RETURNS TABLE
AS
	RETURN SELECT ROW_NUMBER() over(PARTITION BY csts.CustomerID ORDER BY ords.OrderDate) as ix, 
	   csts.CustomerID, 
	   csts.ContactName, 
	   ords.OrderID, 
	   ords.OrderDate
	FROM Northwind.dbo.Orders ords 
	JOIN Northwind.dbo.Customers csts ON ords.CustomerID = csts.CustomerID

SELECT DISTINCT t1.CustomerID, t1.ContactName 
FROM
	getIndexedCustomersOrdersJoin() t1 JOIN
	getIndexedCustomersOrdersJoin() t2
ON 	t1.CustomerID = t2.CustomerID and t1.ix = t2.ix + 1
WHERE DATEDIFF(d, t2.OrderDate, t1.OrderDate) >= 183





