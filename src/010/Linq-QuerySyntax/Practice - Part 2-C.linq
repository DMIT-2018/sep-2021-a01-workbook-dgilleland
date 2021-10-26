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

// C) List all the employees and sort the result in ascending order by last name, then first name. Show the employee's first and last name separately, along with the number of customer orders they have worked on.
Employees
	.Select(person => new 
	{
		person.FirstName,
		person.LastName,
		ProcessedCustomerOrders = person.SalesRepOrders.Count
	})
	.OrderBy(person => person.LastName)
	.ThenBy(person => person.FirstName)
	// Query Syntax:
	// orderby person.LastName, person.FirstName

