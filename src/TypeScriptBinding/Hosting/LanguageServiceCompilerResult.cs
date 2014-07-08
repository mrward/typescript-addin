// 
// LanguageServiceCompilerResult.cs
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
using System.Collections.Generic;
using System.Linq;

using ICSharpCode.Core;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class LanguageServiceCompilerResult
	{
		EmitOutput emitOutput;
		FileName inputFileName;
		string errorMessage;
		
		public LanguageServiceCompilerResult(Exception ex)
		{
			HasErrors = true;
			errorMessage = ex.Message;
		}
		
		public LanguageServiceCompilerResult(EmitOutput emitOutput, FileName inputFileName)
		{
			this.emitOutput = emitOutput;
			this.inputFileName = inputFileName;
			
			HasErrors = !(emitOutput.emitOutputResult == EmitOutputResult.Succeeded);
			errorMessage = GetErrorMessage(emitOutput.emitOutputResult);
		}
		
		public bool HasErrors { get; set; }
		
		public string GetError()
		{
			return errorMessage;
		}
		
		string GetErrorMessage(EmitOutputResult result)
		{
			switch (result) {
				case EmitOutputResult.FailedBecauseOfCompilerOptionsErrors:
					return "Compiler Options errors";
				case EmitOutputResult.FailedBecauseOfSyntaxErrors:
					return "Syntax errors";
				case EmitOutputResult.FailedToGenerateDeclarationsBecauseOfSemanticErrors:
					return "Semantic errors causing declaration generation to fail.";
				case EmitOutputResult.Succeeded:
					return String.Empty;
				default:
					return result.ToString();
			}
		}
		
		public IEnumerable<GeneratedTypeScriptFile> GetGeneratedFiles()
		{
			if (!HasOutputFiles()) {
				return Enumerable.Empty<GeneratedTypeScriptFile>();
			}
			
			return emitOutput
				.outputFiles
				.ToList()
				.Select(outputFile => new GeneratedTypeScriptFile(inputFileName, new FileName(outputFile.name)));
		}
		
		public bool HasOutputFiles()
		{
			return (emitOutput != null) &&
				(emitOutput.outputFiles != null);
		}
	}
}
