<Query Kind="Program" />

void Main()
{
	// C# Statements are like writing the code that would appear inside a method.
	Dictionary<string, Person> people = new(); // In C# 9, you can instantiate in a shorter way
	Person someone;
	someone = Generate("Dan", "Gilleland", new DateTime(1979, 4, 2));
	people.Add(someone.FullName, someone);

	someone = Generate("Robbin", "Law", new DateTime(1970, 7, 22));
	people.Add(someone.FullName, someone);

	someone = Generate("Guido", "Drozdowski", new DateTime(1985, 10, 1));
	people.Add(someone.FullName, someone);

	people.Dump("My list of people");
}

public Person Generate(string first, string last, DateTime dob)
{
	return new Person(first, last, dob);
}

// You can define other methods, fields, classes and namespaces here
public record Person(string FirstName, string LastName, DateTime DateOfBirth)
{
	public string FullName => $"{FirstName} {LastName}";
}
