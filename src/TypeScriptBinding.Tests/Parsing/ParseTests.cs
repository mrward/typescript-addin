
using System;
using System.Linq;
using ICSharpCode.TypeScriptBinding;

namespace TypeScriptBinding.Tests.Parsing
{
	public abstract class ParseTests
	{
		public TypeScriptParsedDocument ParsedDocument { get; set; }
		
		public void Parse(string text, string fileName = @"d:\projects\MyProject\test.ts")
		{
			var contextFactory = new FakeTypeScriptContextFactory();
			var parser = new TypeScriptParser(contextFactory);
			ParsedDocument = parser.Parse(fileName, text);
		}
	}
}
