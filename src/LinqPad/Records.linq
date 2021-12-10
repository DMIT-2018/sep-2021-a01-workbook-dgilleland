<Query Kind="Program" />

using static System.Console;
void Main()
{
	/*
	Records are Classes but with "value semantics". In other words, records are a special kind of class that provides built-in and automatic equality comparisons and immutibility.
	*/
	// Classes and comparisons - Equality in classes is, by default, a Reference Equality
	Person a = new Person{Name = "Anna"};
	Person b = new Person{Name = "Anna"};
	WriteLine($"a == b -> {a == b}");
	// Records and comparisons - Equality in Records is, by default, member based
	Human first = new Human("Anna");
	Human second = new Human("Anna");
	WriteLine($"first == second -> {first == second}");
	WriteLine($"first same-as second -> {object.ReferenceEquals(first, second)}");

	// Classes and immutability - Classes are Mutable by default
	Hugh = new Person { Name = "Hugh Mann"};
	WriteLine(Hugh);
	Rename(Hugh, "Huwey Mann");
	WriteLine(Hugh);
	
	// Records are immutable. You cannot change the property values
	Sam = new Human("Sam");
	var copy = Sam;
	WriteLine($"copy and Sam are the same: {IsSame(copy, Sam)}");
	// Effectively "clones" the original object with whatever changes I add.
	copy = Sam with { Name = "Bob" };
	WriteLine(copy);
	WriteLine($"copy and Sam are the same: {IsSame(copy, Sam)}");
	copy = Sam with {Name = "Sam"};
	WriteLine($"Are Same: {IsSame(copy, Sam)}");
	WriteLine($"Are Equal: {copy == Sam}");
	
}

private Person Hugh;
private Human Sam;

public void Rename(Person person, string newName)
{
	person.Name = newName;
}

public bool IsSame(Human first, Human second)
{
	return object.ReferenceEquals(first, second);
}

// You can define other methods, fields, classes and namespaces here
public class Person
{
	public string Name {get;set;}
}

public record Human(string Name);
