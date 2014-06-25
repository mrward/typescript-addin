
using System;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace TypeScriptBinding.Tests.Parsing
{
	public class FakeTypeScriptContextFactory : ITypeScriptContextFactory
	{
		public TypeScriptContext CreateContext()
		{
			var scriptLoader = new ParseTestScriptLoader();
			var logger = new LanguageServiceLogger();
			return new TypeScriptContext(scriptLoader, logger);
		}
	}
}
