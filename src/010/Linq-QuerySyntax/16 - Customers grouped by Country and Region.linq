<Query Kind="Expression">
  <Connection>
    <ID>9f795fec-6525-43c5-bbd0-2819df27768a</ID>
    <Server>.</Server>
    <Database>WestWind</Database>
  </Connection>
</Query>

// List all the customers grouped by country and region.
from row in Customers
// The following Key data type is an anonymous type with two properties: Country, Region
group row by new { row.Address.Country, row.Address.Region } into CustomerGroups
// The following Key data type is an Entity/Object
// group row by row.Address into CustomerGroups // This was too detailed
//   data type: \Address/ object
select new
{
   Key = CustomerGroups.Key,
   Country = CustomerGroups.Key.Country,
   Region = CustomerGroups.Key.Region,
   Customers = from data in CustomerGroups
               select new
               {
                   Company = data.CompanyName,
                   City = data.Address.City
               }
}