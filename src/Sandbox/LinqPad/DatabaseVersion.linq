<Query Kind="Statements">
  <Connection>
    <ID>d8466b08-f0c8-4437-8055-09808ed91f96</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>.</Server>
    <Database>Capstone</Database>
    <IsProduction>true</IsProduction>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

//DbVersions.Single()
var rows =  from aRow in DbVersions.ToList()
			select new // <-- This is where we would introduce our own concrete data type (class)
			{
				Version = new Version(aRow.Major, aRow.Minor, aRow.Build),
				ReleaseDate = aRow.ReleaseDate
			};
rows.Dump("All the rows");
var result = rows.Single(); // grab the only row that "should" exist
result.Dump("This is the version that matters");