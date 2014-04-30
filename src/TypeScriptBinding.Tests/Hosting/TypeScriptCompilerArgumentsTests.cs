
using System;
using System.Collections.Generic;
using System.Linq;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Internal.Templates;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.TypeScriptBinding;
using ICSharpCode.TypeScriptBinding.Hosting;
using NUnit.Framework;
using Rhino.Mocks;

namespace TypeScriptBinding.Tests.Hosting
{
	[TestFixture]
	public class TypeScriptCompilerArgumentsTests
	{
		TypeScriptCompilerArguments commandLine;
		TypeScriptProject project;
		MSBuildBasedProject msbuildProject;
		
		void CreateCommandLine()
		{
			var watcher = MockRepository.GenerateStub<IProjectChangeWatcher>();
			var solution = new Solution(watcher);
			var createInfo = new ProjectCreateInformation {
				Solution = solution,
				SolutionPath = @"d:\projects\MyProject\MyProject.sln",
				ProjectBasePath = @"d:\projects\MyProject",
				OutputProjectFileName = @"d:\projects\MyProject\MyProject.csproj"
			};
			
			msbuildProject = new MSBuildBasedProject(createInfo);
			
			project = new TypeScriptProject(msbuildProject);
			msbuildProject.SetProperty(TypeScriptProject.RemoveCommentsPropertyName, "False");
			msbuildProject.SetProperty(TypeScriptProject.GenerateSourceMapPropertyName, "False");
			msbuildProject.SetProperty(TypeScriptProject.TargetPropertyName, "");
			msbuildProject.SetProperty(TypeScriptProject.ModuleKindPropertyName, "");
			
			commandLine = new TypeScriptCompilerArguments(project);
		}
		
		void AssertArgsAreEqual(string[] actual, params string[] expected)
		{
			CollectionAssert.AreEqual(expected, actual);
		}
		
		void AddTypeScriptFiles(params string[] fileNames)
		{
			FileName[] convertedFileNames = fileNames
				.Select(f => new FileName(f))
				.ToArray();
			commandLine.AddTypeScriptFiles(convertedFileNames);
		}
		
		[Test]
		public void GetCommandLine_SingleFileWithSpaceInNameAndNoCompilerOptions_IncludesOnlyFileNameInQuotes()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\My Project\test.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --target ES5 \"d:\\projects\\My Project\\test.ts\"", args);
		}
		
		[Test]
		public void GetCommandLine_SingleFileWithNoSpacesInNameAndNoCompilerOptions_IncludesOnlyFileNameWithoutQuotes()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\MyProject\test.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --target ES5 d:\\projects\\MyProject\\test.ts", args);
		}
		
		[Test]
		public void GetArguments_SingleFileAndNoCompilerOptions_IncludesOnlyFileName()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\MyProject\test.ts");
			
			string[] arguments = commandLine.GetArguments();
			
			AssertArgsAreEqual(arguments, "--target", "ES5", @"d:\projects\MyProject\test.ts");
		}
		
		[Test]
		public void GetCommandLine_SingleFileAndNoCompilerOptions_IncludesOnlyFileNameInQuotes()
		{
			CreateCommandLine();
			AddTypeScriptFiles(@"d:\projects\MyProject\test.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --target ES5 d:\\projects\\MyProject\\test.ts", args);
		}
		
		[Test]
		public void GetCommandLine_TwoFilesAndNoCompilerOptions_IncludesOnlyFileNames()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts", "b.ts");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --target ES5 a.ts b.ts", args);
		}
		
		[Test]
		public void GetArguments_TwoFilesAndNoCompilerOptions_IncludesOnlyTwoFilesNames()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts", "b.ts");
			
			string[] arguments = commandLine.GetArguments();
			
			AssertArgsAreEqual(arguments, "--target", "ES5", "a.ts", "b.ts");
		}
		
		[Test]
		public void GetArguments_IncludeCommentsIsFalse_RemoteCommentsArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			msbuildProject.SetProperty(TypeScriptProject.RemoveCommentsPropertyName, "True");
			
			string[] arguments = commandLine.GetArguments();
			
			AssertArgsAreEqual(arguments, "--removeComments", "--target", "ES5", "a.ts");
		}
		
		[Test]
		public void GetCommandLine_GenerateSourceMap_SourceMapArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			msbuildProject.SetProperty(TypeScriptProject.GenerateSourceMapPropertyName, "True");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --sourcemap --target ES5 a.ts", args);
		}
		
		[Test]
		public void GetCommandLine_UseAmdModule_AmdModuleArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			msbuildProject.SetProperty(TypeScriptProject.ModuleKindPropertyName, "AMD");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --module AMD --target ES5 a.ts", args);
		}
		
		[Test]
		public void GetCommandLine_UseES3_TargetArgAdded()
		{
			CreateCommandLine();
			AddTypeScriptFiles("a.ts");
			msbuildProject.SetProperty(TypeScriptProject.TargetPropertyName, "ES3");
			
			string args = commandLine.GetCommandLine();
			
			Assert.AreEqual("tsc --target ES3 a.ts", args);
		}
	}
}
