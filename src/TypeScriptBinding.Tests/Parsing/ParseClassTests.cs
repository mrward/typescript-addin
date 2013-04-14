
using System;
using System.Linq;
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
		
		[Test]
		public void Parse_ClassWithOneMethod_ClassHasOneMethodWithCorrectBodyRegionAndName()
		{
			string code =
				"class Student {\r\n" +
				"    sayHello() {\r\n" +
				"        return \"Hello\";\r\n" +
				"    }\r\n" +
				"}\r\n";
			
			Parse(code);
			
			IClass c = GetFirstClass();
			IMethod method = c.Methods.First();
			Assert.AreEqual("sayHello", method.Name);
			Assert.AreEqual(2, method.Region.EndLine);
			Assert.AreEqual(15, method.Region.EndColumn);
			Assert.AreEqual(4, method.BodyRegion.EndLine);
			Assert.AreEqual(6, method.BodyRegion.EndColumn);
		}
		
		[Test]
		public void Parse_TwoClassesAndSecondOneHasOneMethod_SecondClassHasOneMethodWithCorrectBodyRegionAndName()
		{
			string code =
				"class Class1 {\r\n" +
				"}\r\n" +
				"\r\n"+ 
				"class Class2 {\r\n" +
				"    sayHello() {\r\n" +
				"        return \"Hello\";\r\n" +
				"    }\r\n" +
				"}\r\n";
			
			Parse(code);
			
			IClass c = GetSecondClass();
			IMethod method = c.Methods.FirstOrDefault();
			Assert.AreEqual(2, CompilationUnit.Classes.Count);
			Assert.AreEqual("sayHello", method.Name);
			Assert.AreEqual(5, method.Region.EndLine);
			Assert.AreEqual(15, method.Region.EndColumn);
			Assert.AreEqual(7, method.BodyRegion.EndLine);
			Assert.AreEqual(6, method.BodyRegion.EndColumn);
		}
	}
}