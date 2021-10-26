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

// B) List all the Customers sorted by Company Name. Include the Customer's company name, contact name, and other contact information in the result.
Customers
	.Select(client => new
	{
		client.CompanyName,
		client.ContactName,
		client.ContactTitle,
		client.ContactEmail,
		client.Fax,
		client.Phone
	})
	.OrderBy(client => client.CompanyName)
