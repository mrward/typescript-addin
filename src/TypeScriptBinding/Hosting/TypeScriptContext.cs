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
		TypeScriptLanguageServices context;
		IScriptLoader scriptLoader;
        LanguageServiceHost host;
		bool runInitialization = true;
		
		public TypeScriptContext(IScriptLoader scriptLoader, ILogger logger)
		{
			this.scriptLoader = scriptLoader;
			this.host = new LanguageServiceHost(logger);
			this.context = new TypeScriptLanguageServices(host,scriptLoader.GetTypeScriptServicesScript());

            host.AddDefaultLibScript(new FilePath(scriptLoader.LibScriptFileName), scriptLoader.GetLibScript());
		}
		
        #region TypeScriptLanguageService calls
		
		public CompletionInfo GetCompletionItems(FilePath fileName, int offset, string text, bool memberCompletion)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			host.isMemberCompletion = memberCompletion;
			
            return context.GetCompletionsAtPosition(fileName,offset);
		}
		
		public CompletionEntryDetails GetCompletionEntryDetails(FilePath fileName, int offset, string entryName)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			host.completionEntry = entryName;
			
            return context.GetCompletionEntryDetails(fileName, offset, entryName);
		}
		
		public SignatureHelpItems GetSignature(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
            return context.GetSignatureHelpItems(fileName, offset);
		}
		
		public ReferenceEntry[] FindReferences(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
            return context.GetReferencesAtPosition(fileName, offset);
		}
		
		public DefinitionInfo[] GetDefinition(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
            return context.GetDefinitionAtPosition(fileName, offset);
		}
		
		public NavigationBarItem[] GetNavigationInfo(FilePath fileName)
		{
			host.UpdateFileName(fileName);
            return context.GetNavigationBarItems(fileName);
		}
				
		public EmitOutput Compile(FilePath fileName, ICompilerOptions options)
		{
			host.UpdateCompilerSettings(options);
			host.UpdateFileName(fileName);

            return context.GetEmitOutput(fileName);
		}
		
		public Diagnostic[] GetDiagnostics(FilePath fileName, ICompilerOptions options)
		{
			host.UpdateCompilerSettings(options);
			host.UpdateFileName(fileName);

            var syntactic = context.GetSyntacticDiagnostics(fileName);
            var semantic = context.GetSemanticDiagnostics(fileName);
            var result = semantic.Concat(syntactic).ToArray();
            return result;
		}

		public void Dispose()
		{
			//context._dispose();
		}

        #endregion


        #region LanguageServiceHostEnvironment calls

		public void GetCompletionItemsForTheFirstTime()
		{
			// HACK - run completion on first file so the user does not have to wait about 
			// 1-2 seconds for the completion list to appear the first time it is triggered.
			string fileName = host.GetFileNames().FirstOrDefault();
			if (fileName != null) {
				GetCompletionItems(fileName, 1, String.Empty, false);
			}
		}

		public void RunInitialisationScript()
		{
			if (runInitialization) {
				runInitialization = false;
				context.CleanupSemanticCache();
			}
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
