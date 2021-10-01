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

//G) List all the product names that contain the word "chef" in the name.
Products                                              // is a DbSet<Product>
	.Where(item => item.ProductName.Contains("chef")) // Returns an IQueryable<Product>
	.Select(item => item.ProductName)                 // Returns an IQueryable<string>