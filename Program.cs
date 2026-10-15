Console.Write("Please enter your name: ");
string name = Console.ReadLine()!.Trim();
if (string.IsNullOrWhiteSpace(name)) name = "user";
Console.WriteLine($"Welcome, {name}!");