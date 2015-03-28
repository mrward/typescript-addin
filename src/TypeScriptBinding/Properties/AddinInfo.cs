
using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin ("TypeScript.Linux.32bit",
	Namespace = "MonoDevelop",
	Version = "0.5",
	Category = "Web Development")]

[assembly:AddinName ("TypeScript Linux 32 bit")]
[assembly:AddinDescription ("Adds TypeScript support for Linux 32 bit. Updated to use TypeScript 1.4")]

[assembly:AddinDependency ("Core", "5.0")]
[assembly:AddinDependency ("Ide", "5.0")]
[assembly:AddinDependency ("SourceEditor2", "5.0")]
[assembly:AddinDependency ("Refactoring", "5.0")]
