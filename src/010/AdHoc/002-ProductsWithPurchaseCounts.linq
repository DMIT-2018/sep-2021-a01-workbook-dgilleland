<Query Kind="Expression">
  <Connection>
    <ID>a1bb44b6-ce71-4e35-b4b6-f01744d2abfc</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>.</Server>
    <Database>WestWind</Database>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

from item in Products
select new
{
	Name = item.ProductName,
	ID = item.ProductID,
	Supplier = item.Supplier.CompanyName,
	Category = item.Category.CategoryName,
	NumberOfPurchases = item.OrderDetails.Count
}
