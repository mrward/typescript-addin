# TypeScript Addin for MonoDevelop and Xamarin Studio 5

This addin provides [TypeScript](http://www.typescriptlang.org/) support in MonoDevelop and Xamarin Studio 5.

MonoDevelop and Xamarin Studio 6 are not supported.

## Features

1. Syntax highlighting for TypeScript files (.ts)
2. TypeScript file template.
3. Code completion - using the TypeScript language services.
4. Code folding.
5. Rename refactoring.
6. Generate JavaScript file when saving TypeScript file or building the project.
7. Configurable TypeScript compiler options in Tools Options dialog.
8. Find References.
9. Go to Definition.

For more detailed look at the features please read the [TypeScript Support in Xamarin Studio blog post](http://lastexitcode.com/blog/2015/04/01/TypeScriptSupportInXamarinStudio/)

## Requirements

 * MonoDevelop 5.0 or Xamarin Studio 5.0
 
## Installation

The addin is available from the [MonoDevelop addin repository](http://addins.monodevelop.com/). To install the addin:

 * Open the **Add-in Manager** dialog.
 * Select the **Gallery** tab.
 * Select **Xamarin Studio Add-in Repository (Alpha channel)** from  the drop down list.
 * Expand **Web Development**.
 * Select **TypeScript**.
 * Click the **Refresh** button if the addin is not visible.
 * Click **Install...** to install the addin.

## Dependencies

1. [TypeScript](https://github.com/microsoft/typescript) - The TypeScript language services are used to provide code completion.
2. [V8.NET](http://v8dotnet.codeplex.com/) - A library that hosts Google's V8 JavaScript engine and allows .NET objects to be used directly from JavaScript.
3. [Json.NET](http://json.codeplex.com/) - Json library for .NET created by [James Newton-King](http://james.newtonking.com/).

## How it works

The addin glues together the TypeScript language services and MonoDevelop using [V8.NET](http://v8dotnet.codeplex.com/) as the bridge between them. JavaScript code is executed by V8 and uses the TypeScript language services to get information about the TypeScript files in the project. This information is delivered to the C# host class which interacts with MonoDevelop.
