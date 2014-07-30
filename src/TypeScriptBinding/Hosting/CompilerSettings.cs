// 
// CompilerSettings.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2014 Matthew Ward
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class CompilerSettings
	{
		public CompilerSettings(ITypeScriptOptions options)
			: this()
		{
			removeComments = options.RemoveComments;
			mapSourceFiles = options.GenerateSourceMap;
			noImplicitAny = options.NoImplicitAny;
			codeGenTarget = options.GetLanguageVersion();
			moduleGenTarget = options.GetModuleTarget();
			gatherDiagnostics = true;
		}
		
		public CompilerSettings()
		{
			allowAutomaticSemicolonInsertion = true;
			outFileOption = "";
			outDirOption = "";
			mapRoot = "";
			sourceRoot = "";
			codeGenTarget = LanguageVersion.EcmaScript3;
			moduleGenTarget = ModuleGenTarget.Unspecified;
			noImplicitAny = true;
		}
		
		public bool propagateEnumConstants { get; set; }
		public bool removeComments { get; set; }
		public bool watch { get; set; }
		public bool noResolve { get; set; }
		public bool allowAutomaticSemicolonInsertion { get; set; }
		public bool noImplicitAny { get; set; }
		public bool noLib { get; set; }
		public LanguageVersion codeGenTarget { get; set; }
		public ModuleGenTarget moduleGenTarget { get; set; }
		public string outFileOption { get; set; }
		public string outDirOption { get; set; }
		public bool mapSourceFiles { get; set; }
		public string mapRoot { get; set; }
		public string sourceRoot { get; set; }
		public bool generateDeclarationFiles { get; set; }
		public bool useCaseSensitiveFileResolution { get; set; }
		public bool gatherDiagnostics { get; set; }
		public int codepage { get; set; }
		public bool createFileLog { get; set; }
	}
}
