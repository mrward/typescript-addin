// 
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
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ICSharpCode.NRefactory.Editor;
using ICSharpCode.TypeScriptBinding.Hosting;
using MonoDevelop.Core;
using MonoDevelop.Ide;
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
		
		public TypeScriptParsedDocument Parse(string fileName, string content, ITypeScriptOptions options)
		{
			return Parse(fileName, content, options, new TypeScriptFile[0]);
		}
		
		public TypeScriptParsedDocument Parse(
			string fileName,
			string content,
			ITypeScriptOptions options,
			IEnumerable<TypeScriptFile> files)
		{
			try {
				using (TypeScriptContext context = contextFactory.CreateContext()) {
					var file = new FilePath(fileName);
					context.AddFile(file, content);
					context.RunInitialisationScript();
					
					NavigationBarItem[] navigation = context.GetNavigationInfo(file);
					var document = new ReadOnlyDocument(content);
					var parsedDocument = new TypeScriptParsedDocument(fileName);
					parsedDocument.AddNavigation(navigation, document);
					
					if (options != null) {
						context.AddFiles(files);
						Diagnostic[] diagnostics = context.GetSemanticDiagnostics(file, options);
						parsedDocument.AddDiagnostics(diagnostics, document);
					}

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
			ITypeScriptOptions options = null;
			List<TypeScriptFile> files = null;
			DispatchService.GuiSyncDispatch(() => {
				TypeScriptProject typeScriptProject = TypeScriptService.GetProjectForFile(fileName);
				if (typeScriptProject != null) {
					options = typeScriptProject.GetOptions();
					files = typeScriptProject.GetTypeScriptFiles().ToList();
				} else {
					files = new List<TypeScriptFile>();
				}
			});
			return Parse(fileName, content.ReadToEnd(), options, files);
		}
	}
}
