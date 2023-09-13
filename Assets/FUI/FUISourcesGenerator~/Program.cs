// See https://aka.ms/new-console-template for more information
using FUISourcesGenerator;

Console.WriteLine("Hello, World!");
const string output = "../output/test.dll";
var generator = new Generator();
generator.typeSyntaxModifiers.Add(new KeywordModifier());
generator.typeDefinationInjectors.Add(new PropertyChangedInjector());
generator.beforeCompilerSourcesGenerators.Add(new DataBindingContextGenerator());
generator.typeDefinationSourcesGenerators.Add(new PropertyDelegateGenerator());
await generator.LoadProject(@"D:\FUI\FUI.sln", "FUI.Test", output);