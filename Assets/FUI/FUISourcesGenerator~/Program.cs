// See https://aka.ms/new-console-template for more information
using FUISourcesGenerator;

Console.WriteLine("Hello, World!");
const string output = "../output/test.dll";
var generator = new Generator();
generator.typeSyntaxRootGenerators.Add(new DataBindingGenerator());
generator.typeDefinationInjectors.Add(new PropertyChangedInjector());
await generator.LoadProject(@"D:\FUI\FUI.sln", "FUI.Test", output);