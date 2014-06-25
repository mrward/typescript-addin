﻿// 
// TypeScriptParser.cs
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
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.TypeScriptBinding.Hosting;
using MonoDevelop.Core;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptParser : TypeSystemParser
	{
		ITypeScriptContextFactory contextFactory;
		
		public TypeScriptParser()
			: this(new LanguageServiceNullLogger())
		{
		}
		
		public TypeScriptParser(ILogger logger)
			: this(new TypeScriptContextFactory(new ScriptLoader(), logger))
		{
		}
		
		public TypeScriptParser(ITypeScriptContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}
		
		public TypeScriptParsedDocument Parse(string fileName, string content)
		{
			try {
				using (TypeScriptContext context = contextFactory.CreateContext()) {
					var file = new FilePath(fileName);
					context.AddFile(file, content);
					context.RunInitialisationScript();
					
					NavigateToItem[] navigation = context.GetLexicalStructure(file);
					var document = new ReadOnlyDocument(content);
					var parsedDocument = new TypeScriptParsedDocument(fileName);
					parsedDocument.AddNavigation(navigation, document);
					return parsedDocument;
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
				LoggingService.LogDebug(ex.ToString());
			}
			return new TypeScriptParsedDocument(fileName);
		}
		
		public static bool IsTypeScriptFileName(FilePath fileName)
		{
			return String.Equals(".ts", fileName.Extension, StringComparison.OrdinalIgnoreCase);
		}
		
		public static bool IsTypeScriptFileName(string fileName)
		{
			return IsTypeScriptFileName(new FilePath(fileName));
		}
		
		public override ParsedDocument Parse(bool storeAst, string fileName, TextReader content, Project project)
		{
			return Parse(fileName, content.ReadToEnd());
		}
	}
}
