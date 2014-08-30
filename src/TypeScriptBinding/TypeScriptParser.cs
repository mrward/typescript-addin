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
using System.Threading;

using ICSharpCode.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.SharpDevelop.Editor.Search;
using ICSharpCode.SharpDevelop.Parser;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptParser : IParser
	{
		ITypeScriptContextFactory contextFactory;
		
		public TypeScriptParser(ILogger logger)
			: this(new TypeScriptContextFactory(new ScriptLoader(), logger))
		{
		}
		
		public TypeScriptParser(ITypeScriptContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}
		
		public IReadOnlyList<string> TaskListTokens {
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}
		
		public bool CanParse(string fileName)
		{
			return true;
		}
//		
//		public ICompilationUnit Parse(IProjectContent projectContent, string fileName, ITextBuffer fileContent)
//		{
//			return Parse(projectContent, fileName, fileContent, new TypeScriptFile[0]);
//		}
//		
//		public ICompilationUnit Parse(
//			IProjectContent projectContent,
//			string fileName,
//			ITextBuffer fileContent,
//			IEnumerable<TypeScriptFile> files)
//		{
//			try {
//				using (TypeScriptContext context = contextFactory.CreateContext()) {
//					var file = new FileName(fileName);
//					context.AddFile(file, fileContent.Text);
//					context.RunInitialisationScript();
//					
//					NavigateToItem[] navigation = context.GetLexicalStructure(file);
//					var unit = new TypeScriptCompilationUnit(projectContent) {
//						FileName = fileName
//					};
//					unit.AddNavigation(navigation, fileContent);
//					
//					var typeScriptProjectContent = projectContent as TypeScriptProjectContent;
//					if (typeScriptProjectContent != null) {
//						context.AddFiles(files);
//						IDocument document = DocumentUtilitites.LoadReadOnlyDocumentFromBuffer(fileContent);
//						Diagnostic[] diagnostics = context.GetSemanticDiagnostics(file, typeScriptProjectContent.Options);
//						TypeScriptService.TaskService.Update(diagnostics, file, document);
//					}
//					
//					return unit;
//				}
//			} catch (Exception ex) {
//				Console.WriteLine(ex.ToString());
//				LoggingService.Debug(ex.ToString());
//			}
//			return new DefaultCompilationUnit(projectContent);
//		}
		
		public static bool IsTypeScriptFileName(FileName fileName)
		{
			if (fileName == null)
				return false;
			
			return String.Equals(".ts", fileName.GetExtension(), StringComparison.OrdinalIgnoreCase);
		}
		
		public ITextSource GetFileContent(FileName fileName)
		{
			throw new NotImplementedException();
		}
		
		public ParseInformation Parse(FileName fileName, ITextSource fileContent, bool fullParseInformationRequested, IProject parentProject, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
		
		public ResolveResult Resolve(ParseInformation parseInfo, TextLocation location, ICompilation compilation, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
		
		public ICodeContext ResolveContext(ParseInformation parseInfo, ICSharpCode.NRefactory.TextLocation location, ICSharpCode.NRefactory.TypeSystem.ICompilation compilation, System.Threading.CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
		
		public ResolveResult ResolveSnippet(ParseInformation parseInfo, TextLocation location, string codeSnippet, ICompilation compilation, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
		
		public void FindLocalReferences(ParseInformation parseInfo, ITextSource fileContent, IVariable variable, ICompilation compilation, Action<SearchResultMatch> callback, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
		
		public ICompilation CreateCompilationForSingleFile(FileName fileName, IUnresolvedFile unresolvedFile)
		{
			throw new NotImplementedException();
		}
	}
}
