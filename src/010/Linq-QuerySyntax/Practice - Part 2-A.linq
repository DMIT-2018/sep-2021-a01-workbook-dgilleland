<Query Kind="Statements">
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

// A) Group employees by region and show the results in this format:
/* ----------------------------------------------
 * | REGION        | EMPLOYEES                  |
 * ----------------------------------------------
 * | Eastern       | ------------------------   |
 * |               | | Nancy Devalio        |   |
 * |               | | Fred Flintstone      |   |
 * |               | | Bill Murray          |   |
 * |               | ------------------------   |
 * |---------------|----------------------------|
 * | ...           |                            |
 * 
 */
var rawListing = // Gives me duplicate employee names
	from place in Regions
	select new
	{
		Region = place.RegionDescription,
		Employees = from area in place.Territories
					from assignment in area.EmployeeTerritories
					select new
					{
						Name = $"{assignment.Employee.FirstName} {assignment.Employee.LastName}"
					}
	};
var regionReps = // Get rid of duplicates with the .Distinct() method
	rawListing
        // .ToList() will force the rawListing query to execute and generate the SQL results
		.ToList() // and bring the data in memory (RAM)
		.Select(data => new 
		{
			data.Region,
			Employees = data.Employees.Distinct()
		});
regionReps.Dump();
//EmployeeTerritories
//	.GroupBy(area => area.Territory.Region.RegionDescription)
//	.Select(area => new
//	{
//		Region = area.Key,
//		Employees = area.Select(place => place.Employee.FirstName + " " +place.Employee.LastName)
//	})