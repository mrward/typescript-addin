// 
// ScriptLoader.cs
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

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class ScriptLoader
	{
		string root;
		string typeScriptServicesFileName;
		string mainScriptFileName;
		string memberCompletionScriptFileName;
		
		public ScriptLoader()
		{
			root = Path.Combine(StringParser.Parse("${addinpath:ICSharpCode.TypeScriptBinding}"), "Scripts");
			typeScriptServicesFileName = Path.Combine(root, "typescriptServices.js");
			mainScriptFileName = Path.Combine(root, "main.js");
			memberCompletionScriptFileName = Path.Combine(root, "completion.js");
		}
		
		public string GetTypeScriptServicesScript()
		{
			return ReadScript(typeScriptServicesFileName);
		}
		
		string ReadScript(string fileName)
		{
			return File.ReadAllText(fileName);
		}
		
		public string GetMainScript()
		{
			return ReadScript(mainScriptFileName);
		}
		
		public string GetMemberCompletionScript()
		{
			return ReadScript(memberCompletionScriptFileName);
		}
	}
}
