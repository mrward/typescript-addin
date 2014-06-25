
using System;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Ide.TypeSystem;
using NUnit.Framework;

namespace TypeScriptBinding.Tests.Parsing
{
	[TestFixture]
	public class ParseClassTests : ParseTests
	{
//		[Test]
//		public void Parse_EmptyStudentClass_OneClassFoundWithNameOfStudent()
//		{
//			string code =
//				"class Student {\r\n" +
//				"}\r\n";
//			
//			Parse(code);
//			
//			IClass c = GetFirstClass();
//			Assert.AreEqual(1, CompilationUnit.Classes.Count);
//			Assert.AreEqual("Student", c.Name);
//		}
		
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
			
			FoldingRegion folding = ParsedDocument.Foldings.FirstOrDefault();
			Assert.AreEqual(expectedBodyRegion, folding.Region);
			Assert.AreEqual(FoldType.Type, folding.Type);
		}
		
		[Test]
		public void Parse_EmptyStudentClass_ClassHasBeginLineAndColumnSetForRegion()
		{
			string code =
				"class Student {\r\n" +
				"}\r\n";
			var expectedRegion = new DomRegion(
				beginLine: 1,
				beginColumn: 14,
				endLine: 2,
				endColumn: 2);
			
			Parse(code);
			
			FoldingRegion folding = ParsedDocument.Foldings.FirstOrDefault();
			Assert.AreEqual(expectedRegion, folding.Region);
			Assert.AreEqual(FoldType.Type, folding.Type);
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
			
			FoldingRegion folding = ParsedDocument.Foldings.LastOrDefault();
			Assert.AreEqual(2, folding.Region.BeginLine);
			Assert.AreEqual(15, folding.Region.BeginColumn);
			Assert.AreEqual(4, folding.Region.EndLine);
			Assert.AreEqual(6, folding.Region.EndColumn);
			Assert.AreEqual(FoldType.Member, folding.Type);
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
			
			FoldingRegion folding = ParsedDocument.Foldings.LastOrDefault();
			Assert.AreEqual(5, folding.Region.BeginLine);
			Assert.AreEqual(15, folding.Region.BeginColumn);
			Assert.AreEqual(7, folding.Region.EndLine);
			Assert.AreEqual(6, folding.Region.EndColumn);
			Assert.AreEqual(FoldType.Member, folding.Type);
		}
		
		[Test]
		public void Parse_ClassWithConstructor_ClassHasOneConstructorMethodWithCorrectBodyRegionAndName()
		{
			string code =
				"class Student {\r\n" +
				"    constructor() {\r\n" +
				"    \r\n" +
				"    }\r\n" +
				"}\r\n";
			
			Parse(code);
			
			FoldingRegion folding = ParsedDocument.Foldings.LastOrDefault();
			Assert.AreEqual(2, folding.Region.BeginLine);
			Assert.AreEqual(18, folding.Region.BeginColumn);
			Assert.AreEqual(4, folding.Region.EndLine);
			Assert.AreEqual(6, folding.Region.EndColumn);
			Assert.AreEqual(FoldType.Member, folding.Type);
		}
		
//		[Test]
//		public void Parse_EmptyInterface_InterfaceAddedToCompilationUnit()
//		{
//			string code =
//				"interface Student {\r\n" +
//				"}\r\n";
//			
//			Parse(code);
//			
//			IClass c = GetFirstClass();
//			Assert.AreEqual("Student", c.Name);
//			Assert.AreEqual(ClassType.Interface, c.ClassType);
//		}
		
//		[Test]
//		public void Parse_ModuleWithOneClass_ModuleClassHasOneNestedClass()
//		{
//			string code =
//				"module MyModule {\r\n" +
//				"    class Student {\r\n" +
//				"    }\r\n" +
//				"}\r\n";
//			
//			Parse(code);
//			
//			IClass module = GetFirstClass();
//			Assert.AreEqual("MyModule", module.Name);
//			Assert.AreEqual(ClassType.Module, module.ClassType);
//			IClass c = module.InnerClasses.FirstOrDefault();
//			Assert.AreEqual("MyModule.Student", c.FullyQualifiedName);
//			Assert.AreEqual(1, CompilationUnit.Classes.Count);
//		}
		
//		[Test]
//		public void Parse_ModuleWithOneClassThatHasOneMethod_NestedClassHasOneMethod()
//		{
//			string code =
//				"module MyModule {\r\n" +
//				"    class Student {\r\n" +
//				"         speak() {\r\n" +
//				"         }\r\n" +
//				"    }\r\n" +
//				"}\r\n";
//			
//			Parse(code);
//			
//			IClass module = GetFirstClass();
//			IClass c = module.InnerClasses.FirstOrDefault();
//			IMethod method = c.Methods[0];
//			Assert.AreEqual("speak", method.Name);
//		}
	}
}