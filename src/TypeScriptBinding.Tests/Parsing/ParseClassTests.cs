
using System;
using ICSharpCode.SharpDevelop.Dom;
using NUnit.Framework;

namespace TypeScriptBinding.Tests.Parsing
{
	[TestFixture]
	public class ParseClassTests : ParseTests
	{
		[Test]
		public void Parse_EmptyStudentClass_OneClassFoundWithNameOfStudent()
		{
			string code =
				"class Student {\r\n" +
				"}\r\n";
			
			Parse(code);
			
			IClass c = GetFirstClass();
			Assert.AreEqual(1, CompilationUnit.Classes.Count);
			Assert.AreEqual("Student", c.Name);
		}
		
		[Test]
		public void Parse_EmptyStudentClass_ClassHasCorrectBodyRegion()
		{
			string code =
				"class Student {\r\n" +
				"}\r\n";
			var expectedBodyRegion = new DomRegion(
				beginLine: 1,
				beginColumn: 14,
				endLine: 2,
				endColumn: 2);
			
			Parse(code);
			
			IClass c = GetFirstClass();
			Assert.AreEqual(expectedBodyRegion, c.BodyRegion);
		}
	}
}