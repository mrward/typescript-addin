
using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin ("TypeScript",
	Namespace = "MonoDevelop",
	Version = "0.6",
	Category = "Web Development")]

[assembly:AddinName ("TypeScript")]
[assembly:AddinDescription ("Adds TypeScript support. Updated to use TypeScript 1.5.3")]

[assembly:AddinDependency ("Core", "5.0")]
[assembly:AddinDependency ("Ide", "5.0")]
[assembly:AddinDependency ("SourceEditor2", "5.0")]
[assembly:AddinDependency ("Refactoring", "5.0")]
