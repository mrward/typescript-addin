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
using Noesis.Javascript;
using TypeScriptHosting;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TypeScriptContext : IDisposable
	{
		JavascriptContext context = new JavascriptContext();
		LanguageServiceShimHost host;
		IScriptLoader scriptLoader;
		bool runInitialization = true;
		
		public TypeScriptContext(IScriptLoader scriptLoader, ILogger logger)
		{
			this.scriptLoader = scriptLoader;
			host = new LanguageServiceShimHost(logger);
			host.AddDefaultLibScript(new FilePath(scriptLoader.LibScriptFileName), scriptLoader.GetLibScript());
			context.SetParameter("host", host);
			context.Run(scriptLoader.GetTypeScriptServicesScript());
		}
		
		public void Dispose()
		{
			context.Dispose();
		}
		
		public void AddFile(FilePath fileName, string text)
		{
			host.AddFile(fileName, text);
		}
		
		public void RunInitialisationScript()
		{
			if (runInitialization) {
				runInitialization = false;
				context.Run(scriptLoader.GetMainScript());
			}
		}
		
		public void GetCompletionItemsForTheFirstTime()
		{
			// HACK - run completion on first file so the user does not have to wait about 
			// 1-2 seconds for the completion list to appear the first time it is triggered.
			string fileName = host.GetFileNames().FirstOrDefault();
			if (fileName != null) {
				GetCompletionItems(fileName, 1, null, false);
			}
		}
		
		public void UpdateFile(FilePath fileName, string text)
		{
			host.UpdateFile(fileName, text);
		}
		
		public CompletionInfo GetCompletionItems(FilePath fileName, int offset, string text, bool memberCompletion)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			host.isMemberCompletion = memberCompletion;
			
			context.Run(scriptLoader.GetMemberCompletionScript());
			
			return host.CompletionResult.result;
		}
		
		public CompletionEntryDetails GetCompletionEntryDetails(FilePath fileName, int offset, string entryName)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			host.completionEntry = entryName;
			
			context.Run(scriptLoader.GetCompletionDetailsScript());
			
			return host.CompletionEntryDetailsResult.result;
		}
		
		public SignatureInfo GetSignature(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
			context.Run(scriptLoader.GetFunctionSignatureScript());
			
			return host.SignatureResult.result;
		}
		
		public ReferenceEntry[] FindReferences(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
			context.Run(scriptLoader.GetFindReferencesScript());
			
			return host.ReferencesResult.result;
		}
		
		public DefinitionInfo[] GetDefinition(FilePath fileName, int offset)
		{
			host.position = offset;
			host.UpdateFileName(fileName);
			
			context.Run(scriptLoader.GetDefinitionScript());
			
			return host.DefinitionResult.result;
		}
		
		public NavigateToItem[] GetLexicalStructure(FilePath fileName)
		{
			host.UpdateFileName(fileName);
			context.Run(scriptLoader.GetNavigationScript());
			
			return host.LexicalStructure.result;
		}
		
		public void RemoveFile(FilePath fileName)
		{
			host.RemoveFile(fileName);
		}
		
		public EmitOutput Compile(FilePath fileName, ITypeScriptOptions options)
		{
			host.UpdateCompilerSettings(options);
			host.UpdateFileName(fileName);
			context.Run(scriptLoader.GetLanguageServicesCompileScript());
			
			return host.CompilerResult.result;
		}
		
		public Diagnostic[] GetSemanticDiagnostics(FilePath fileName, ITypeScriptOptions options)
		{
			host.UpdateCompilerSettings(options);
			host.UpdateFileName(fileName);
			context.Run(scriptLoader.GetSemanticDiagnosticsScript());
			
			return host.SemanticDiagnosticsResult.result;
		}
	}
}
