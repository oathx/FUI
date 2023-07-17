// See https://aka.ms/new-console-template for more information
using FUISourcesGenerator;

Console.WriteLine("Hello, World!");
var generator = new Generator();
generator.generators.Add(new DataBindingGenerator());
await generator.LoadProject(@"D:\FUI\FUI.sln", "FUI.Test");