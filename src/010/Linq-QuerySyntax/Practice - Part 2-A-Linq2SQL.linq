<Query Kind="Expression">
  <Connection>
    <ID>aa51d8ae-a563-4514-a171-74a60525e3b2</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>WestWind</Database>
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
 // This solution uses LINQ to SQL
 from place in Regions
 select new
 {
 	Region = place.RegionDescription,
	Employees = (from area in place.Territories
				from assignment in area.EmployeeTerritories
				select assignment.Employee.FirstName + " " + assignment.Employee.LastName).Distinct()
 }