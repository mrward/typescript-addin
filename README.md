# TypeScript Addin for SharpDevelop 4.3

This is a simple proof of concept that adds very basic [TypeScript](http://www.typescriptlang.org/) support to [SharpDevelop](http://www.icsharpcode.net/OpenSource/SD/).

## Features

1. Syntax highlighting for TypeScript files (.ts)
2. TypeScript file template
3. Code completion - using the TypeScript language services.
4. Code folding.
5. Quick Class Browser support.
6. Rename refactoring.
7. Generate JavaScript file when saving TypeScript file.

## Dependencies

1. [TypeScript](http://typescript.codeplex.com/) - The TypeScript language services are used to provide code completion.
2. [Javascript.NET](http://javascriptdotnet.codeplex.com/) - A library that hosts Google's V8 JavaScript engine and allows .NET objects to be used directly from JavaScript. Currently only works on Windows.
3. [Json.NET](http://json.codeplex.com/) - Json library for .NET created by [James Newton-King](http://james.newtonking.com/).

## How it works

The addin  glues together the TypeScript language services and SharpDevelop using [Javascript.NET](http://javascriptdotnet.codeplex.com/) as the bridge between them. JavaScript code is executed by V8 and uses the TypeScript language services to get information about the TypeScript files in the project. This information is delivered to the C# host class which interacts with SharpDevelop.
