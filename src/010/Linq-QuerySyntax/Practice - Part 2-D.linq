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

// D) Group all customers by city. Output the city name, along with the company name, contact name and title, and the phone number.
from client in Customers.AsEnumerable() // .AsEnumerable() will cause the data to come into RAM
group client by client.Address.City into cityClients
select new
{
	City = cityClients.Key,
	Customer = from company in cityClients
			   select new
			   {
			   		company.CompanyName,
					company.ContactName,
					company.ContactTitle,
					company.Phone
			   }
}