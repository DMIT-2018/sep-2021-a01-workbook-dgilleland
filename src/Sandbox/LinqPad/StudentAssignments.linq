<Query Kind="Expression">
  <Connection>
    <ID>a9163a36-c1b1-4d5a-976d-774314a6d45d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Capstone</Database>
  </Connection>
</Query>

from person in Students
orderby person.LastName, person.FirstName, person.SchoolId
select new // StudentAssignment
{
    LastName = person.LastName,
    FirstName = person.FirstName,
    Assignment = (from team in person.TeamAssignments
                 select new // Team
                 {
                     Number = team.TeamNumber,
                     Client = team.Client.CompanyName,
                     IsConfirmed = team.Client.Confirmed
                 }).SingleOrDefault()
}