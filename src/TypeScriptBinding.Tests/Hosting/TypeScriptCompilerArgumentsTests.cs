
using System;
using System.Collections.Generic;
using System.Linq;

using ICSharpCode.TypeScriptBinding;
using ICSharpCode.TypeScriptBinding.Hosting;
using MonoDevelop.Core;
using NUnit.Framework;

namespace TypeScriptBinding.Tests.Hosting
{
	[TestFixture]
	public class TypeScriptCompilerArgumentsTests
	{
		TypeScriptCompilerArguments commandLine;
		Properties properties;
		TypeScriptOptions options;
		
		void CreateCommandLine()
		{
			properties = new Properties();
			options = new TypeScriptOptions(properties);
			options.IncludeComments = true;
			options.GenerateSourceMap = false;
			options.EcmaScriptTargetVersion = "";
			options.ModuleKind = "";
			
			commandLine = new TypeScriptCompilerArguments(options);
		}
		
		void AssertArgsAreEqual(string[] actual, params string[] expected)
		{
			CollectionAssert.AreEqual(expected, actual);
		}
		
		void AddTypeScriptFiles(params string[] fileNames)
		{
			FilePath[] convertedFileNames = fileNames
				.Select(f => new FilePath(f))
				.ToArray();
			commandLine.AddTypeScriptFiles(convertedFileNames);
		}
		
		[Test]
		public void GetCommandLine_SingleFileWithSpaceInNameAndNoCompilerOptions_IncludesOnlyFileNameInQuotes()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\My Project\test.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual(args, "tsc \"d:\\projects\\My Project\\test.ts\"");
		}
		
		[Test]
		public void GetCommandLine_SingleFileWithNoSpacesInNameAndNoCompilerOptions_IncludesOnlyFileNameWithoutQuotes()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\MyProject\test.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc d:\\projects\\MyProject\\test.ts", args);
		}
		
		[Test]
		public void GetArguments_SingleFileAndNoCompilerOptions_IncludesOnlyFileName()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\MyProject\test.ts");
			
			string[] arguments = commandLine.GetArguments();
			
			AssertArgsAreEqual(arguments, @"d:\projects\MyProject\test.ts");
		}
		
		[Test]
		public void GetCommandLine_SingleFileAndNoCompilerOptions_IncludesOnlyFileNameInQuotes()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\MyProject\test.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc d:\\projects\\MyProject\\test.ts", args);
		}
		
		[Test]
		public void GetCommandLine_TwoFilesAndNoCompilerOptions_IncludesOnlyFileNames()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts", "b.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc a.ts b.ts", args);
		}
		
		[Test]
		public void GetArguments_TwoFilesAndNoCompilerOptions_IncludesOnlyTwoFilesNames()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts", "b.ts");
			
			string[] arguments = commandLine.GetArguments();
			
			AssertArgsAreEqual(arguments, "a.ts", "b.ts");
		}
		
		[Test]
		public void GetArguments_IncludeCommentsIsFalse_RemoteCommentsArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			options.IncludeComments = false;
			
			string[] arguments = commandLine.GetArguments();
			
			AssertArgsAreEqual(arguments, "--removeComments", "a.ts");
		}
		
		[Test]
		public void GetCommandLine_GenerateSourceMap_SourceMapArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			options.GenerateSourceMap = true;
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --sourcemap a.ts", args);
		}
		
		[Test]
		public void GetCommandLine_UseAmdModule_AmdModuleArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			options.ModuleKind = "AMD";
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --module AMD a.ts", args);
		}
		
		[Test]
		public void GetCommandLine_UseES3_TargetArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			options.EcmaScriptTargetVersion = "ES3";
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --target ES3 a.ts", args);
		}
	}
}
