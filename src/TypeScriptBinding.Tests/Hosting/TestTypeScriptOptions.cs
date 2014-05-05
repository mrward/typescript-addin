
using System;
using ICSharpCode.TypeScriptBinding;

namespace TypeScriptBinding.Tests.Hosting
{
	public class TestTypeScriptOptions : ITypeScriptOptions
	{
		public bool RemoveComments { get; set; }
		public bool GenerateSourceMap { get; set; }
		public string ModuleKind { get; set; }
		public string EcmaScriptVersion { get; set; }
	}
}
