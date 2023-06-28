// See https://aka.ms/new-console-template for more information
using FUISourcesGenerator;

Console.WriteLine("Hello, World!");
var generator = new Generator();
generator.Generate(new string[] {generator.TestString}, null, string.Empty);