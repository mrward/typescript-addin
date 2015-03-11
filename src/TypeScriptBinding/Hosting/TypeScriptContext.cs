// 
// TypeScriptContext.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013 Matthew Ward
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
using System.IO;
using System.Linq;

using MonoDevelop.Core;


using TypeScriptLanguageService;
using TypeScriptHosting;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TypeScriptContext : IDisposable
	{
		IScriptLoader scriptLoader;
        LanguageServiceHost host = null; 
		
		public TypeScriptContext(IScriptLoader scriptLoader, ILogger logger)
		{
			this.scriptLoader = scriptLoader;

            host = (LanguageServiceHost) V8TypescriptProvider.TService().Host;

            host.AddDefaultLibScript(new FilePath(scriptLoader.LibScriptFileName), scriptLoader.GetLibScript());
		}
		
        #region TypeScriptLanguageService calls
		
		public CompletionInfo GetCompletionItems(FilePath fileName, int offset, string text, bool memberCompletion)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			host.isMemberCompletion = memberCompletion;
			
            return V8TypescriptProvider.TService().GetCompletionsAtPosition(fileName,offset);
		}
		
		public CompletionEntryDetails GetCompletionEntryDetails(FilePath fileName, int offset, string entryName)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			host.completionEntry = entryName;
			
            return V8TypescriptProvider.TService().GetCompletionEntryDetails(fileName, offset, entryName);
		}
		
		public SignatureHelpItems GetSignature(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
            return V8TypescriptProvider.TService().GetSignatureHelpItems(fileName, offset);
		}
		
		public ReferenceEntry[] FindReferences(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
            return V8TypescriptProvider.TService().GetReferencesAtPosition(fileName, offset);
		}
		
		public DefinitionInfo[] GetDefinition(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
            return V8TypescriptProvider.TService().GetDefinitionAtPosition(fileName, offset);
		}
		
		public NavigationBarItem[] GetNavigationInfo(FilePath fileName)
		{
			host.UpdateFileName(fileName);
            return V8TypescriptProvider.TService().GetNavigationBarItems(fileName);
		}
				
		public EmitOutput Compile(FilePath fileName, ICompilerOptions options)
		{
			host.UpdateCompilerSettings(options);
			host.UpdateFileName(fileName);

            return V8TypescriptProvider.TService().GetEmitOutput(fileName);
		}
		
		public Diagnostic[] GetDiagnostics(FilePath fileName, ICompilerOptions options)
		{
			host.UpdateCompilerSettings(options);
			host.UpdateFileName(fileName);

            var syntactic = V8TypescriptProvider.TService().GetSyntacticDiagnostics(fileName);
            var semantic = V8TypescriptProvider.TService().GetSemanticDiagnostics(fileName);
            var result = semantic.Concat(syntactic).ToArray();
            return result;
		}

        #endregion


        #region LanguageServiceHostEnvironment calls
        public void Dispose()
        {
            //          context.Dispose();
        }

        public void AddFile(FilePath fileName, string text)
        {
            host.AddFile(fileName, text);
        }


        public void UpdateFile(FilePath fileName, string text)
        {
            host.UpdateFile(fileName, text);
        }

        public void RemoveFile(FilePath fileName)
        {
            host.RemoveFile(fileName);
        }
		
		public void AddFiles(IEnumerable<TypeScriptFile> files)
		{
			foreach (TypeScriptFile file in files) {
				AddFile(file.FileName, file.Text);
			}
		}
        #endregion

	}
}
