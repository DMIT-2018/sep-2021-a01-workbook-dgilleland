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

//D) List all the regions and the number of territories in each region
// Approach 1) Start with the Region entity
//from place in Regions
//select new
//{
//	Region = place.RegionDescription,
//	NumberOfTerritories = place.Territories.Count
//}

// Approach2) Start with the Territories entity
from place in Territories
group place by place.Region.RegionDescription into TerritoryGroups
select new
{
	Region = TerritoryGroups.Key,
	NumberOfTerritories = TerritoryGroups.Count()
}


