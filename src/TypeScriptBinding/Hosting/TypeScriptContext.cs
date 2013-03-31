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
using System.IO;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Project;
using Noesis.Javascript;
using TypeScriptHosting;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TypeScriptContext : IDisposable
	{
		JavascriptContext context = new JavascriptContext();
		LanguageServiceShimHost host = new LanguageServiceShimHost();
		ScriptLoader scriptLoader = new ScriptLoader();
		
		public TypeScriptContext()
		{
			context.SetParameter("host", host);
			context.Run(scriptLoader.GetTypeScriptServicesScript());
		}
		
		public void Dispose()
		{
			context.Dispose();
		}
		
		public void AddFile(FileName fileName, string text)
		{
			host.AddFile(fileName.ToString(), text);
			
			if (host.getScriptCount() == 1) {
				context.Run(scriptLoader.GetMainScript());
			}
		}
		
		public void UpdateFile(FileName fileName, string text)
		{
			if (host.getScriptCount() == 0) {
				AddFile(fileName, text);
			} else {
				host.UpdateFile(fileName.ToString(), text);
			}
		}
		
		public CompletionInfo GetCompletionItems(FileName fileName, int offset, string text, bool memberCompletion)
		{
			host.position = offset;
			host.fileName = fileName.ToString();
			host.isMemberCompletion = memberCompletion;
			
			context.Run(scriptLoader.GetMemberCompletionScript());
			
			Console.WriteLine("text=" + text);
			
			return host.CompletionResult.result;
		}
	}
}
