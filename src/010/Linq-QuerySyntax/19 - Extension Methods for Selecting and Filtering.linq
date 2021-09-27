<Query Kind="Program">
  <Connection>
    <ID>05a2444e-14ea-4451-ad3d-3398e9ff7898</ID>
    <Server>.</Server>
    <Database>WestWind</Database>
  </Connection>
</Query>

// Linq Extension Methods for Selections and Filtering
void Main()
{
    // .Select()
    var staff = Employees.Select(person => person.FirstName + " " + person.LastName);
    var alt = from person in Employees
              select person.FirstName + " " + person.LastName;
    staff.Dump("All staff");
    alt.Dump("All staff as query syntax");
    
    // .OrderBy()
    var alphabetical = staff.OrderBy(name => name);
    alphabetical.Dump("Staff by first name");
    
    // .Select() with an anonymous type
    var staff2 = Employees.Select(person => new { person.FirstName, person.LastName, person.BirthDate });
    staff2.Dump("Staff object result");
    staff2.OrderBy(name => name.LastName).Dump("Staff by last name");
    
    
    // .First() and .FirstOrDefault() - Expects a list of possibly many items, but only takes the first
    staff2.First().Dump("First person in the staff object list");
    staff2.OrderBy(person => person.BirthDate).First().Dump("The oldest staff member");
    staff2.First(person => person.FirstName.Contains("an")).Dump("First in list with 'an' in their name");
    staff2.OrderByDescending(person => person.LastName).First(person => person.FirstName.Contains("an")).Dump("First in Sorted");
    
    var me = Employees.FirstOrDefault(person => person.FirstName.StartsWith("Dan"));
    me.Dump("First person named Dan");
    
    // .Single() and .SingleOrDefault() - Expects only a single item matching the predicate
    staff.Single(person => person.StartsWith("R")).Dump("Staff member starting with 'R'");
    //staff.Single(person => person.StartsWith("M")).Dump("Staff member starting with 'M'");
    staff.SingleOrDefault(person => person.StartsWith("Z")).Dump("Staff member starting with 'Z'");
}

// Define other methods and classes here
