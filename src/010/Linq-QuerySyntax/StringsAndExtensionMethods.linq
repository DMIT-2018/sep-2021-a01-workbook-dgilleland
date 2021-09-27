<Query Kind="Statements" />

string sentence = "To be or not to be? Whether it is nobler in the mind to accept the slings and arrows of whatever the rest of the quote says.";
var words = sentence.Replace("?", "").Replace(".", "").ToLower().Split(" ");
words.Dump();
words.Where(w => w.StartsWith("t")).Dump("Words starting with 't'");