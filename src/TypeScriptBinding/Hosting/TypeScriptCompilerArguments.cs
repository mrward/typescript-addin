// 
// TypeScriptCompilerArguments.cs
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
using System.Linq;
using ICSharpCode.Core;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TypeScriptCompilerArguments
	{
		TypeScriptOptions options;
		List<FileName> fileNames = new List<FileName>();
		
		public TypeScriptCompilerArguments(TypeScriptOptions options)
		{
			this.options = options;
		}
		
		public string GetCommandLine()
		{
			string[] args = GetArguments();
			string[] quotedArgs = args
				.Select(arg => QuoteStringIfContainsSpace(arg))
				.ToArray();
			
			return "tsc " + String.Join(" ", quotedArgs);
		}
		
		string QuoteStringIfContainsSpace(string text)
		{
			if (text.Contains(" ")) {
				return String.Format("\"{0}\"", text);
			}
			return text;
		}
		
		public string[] GetArguments()
		{
			var args = new List<string>();
			
			AddCompilerArguments(args);
			AddFileNames(args);
			
			return args.ToArray();
		}
		
		void AddCompilerArguments(List<string> args)
		{
			if (options.IncludeComments) {
				args.Add("--comments");
			}
			
			if (options.GenerateSourceMap) {
				args.Add("--sourcemap");
			}
			
			if (!String.IsNullOrEmpty(options.ModuleKind)) {
				args.Add("--module");
				args.Add(options.ModuleKind);
			}
			
			if (!String.IsNullOrEmpty(options.EcmaScriptTargetVersion)) {
				args.Add("--target");
				args.Add(options.EcmaScriptTargetVersion);
			}
		}
		
		void AddFileNames(List<string> args)
		{
			foreach (FileName fileName in fileNames) {
				args.Add(fileName);
			}
		}
		
		public void AddTypeScriptFiles(params FileName[] fileNames)
		{
			this.fileNames.AddRange(fileNames);
		}
		
		public IList<FileName> Files {
			get { return fileNames; }
		}
	}
}
