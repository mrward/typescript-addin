// 
// TypeScriptParsedDocument.cs
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
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.TypeScriptBinding.Hosting;
using MonoDevelop.Ide.TypeSystem;

using TypeScriptLanguageService;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptParsedDocument : ParsedDocument
	{
		List<Error> errors = new List<Error>();
		
		public TypeScriptParsedDocument(string fileName)
			: base(fileName)
		{
			Flags = ParsedDocumentFlags.NonSerializable;
		}
		
		public void AddNavigation(NavigationBarItem[] navigation, IDocument document)
		{
			foreach (NavigationBarItem item in navigation) {
				switch (item.kind) {
					case "class":
					case "interface":
						AddClass(item, document);
						break;
					case "module":
						AddModule(item, document);
						break;
				}
			}
		}

		void AddModule(NavigationBarItem item, IDocument document)
		{
			if (IsGlobalModule(item)) {
				return;
			}

			AddClass(item, document);
		}

		static bool IsGlobalModule(NavigationBarItem item)
		{
			return item.text == "<global>";
		}
		
		void AddClass(NavigationBarItem item, IDocument document)
		{
   var navItem = new NavigationBarItemRegion(item);
			DomRegion region = navItem.ToRegionStartingFromOpeningCurlyBrace(document);
			var folding = new FoldingRegion(region, FoldType.Type);
			Add(folding);
			AddMethods(item.childItems, document);
		}

		void AddMethods(NavigationBarItem[] childItems, IDocument document)
		{
			foreach (NavigationBarItem item in childItems) {
				switch (item.kind) {
					case "method":
					case "constructor":
						AddMethod(item, document);
						break;
				}
			}
		}
		
		void AddMethod(NavigationBarItem item, IDocument document)
		{
   var navItem = new NavigationBarItemRegion(item);
			DomRegion region = navItem.ToRegionStartingFromOpeningCurlyBrace(document);
			var folding = new FoldingRegion(region, FoldType.Member);
			Add(folding);
		}
		
		public void AddDiagnostics(Diagnostic[] diagnostics, IDocument document)
		{
			errors = diagnostics
				.Select(diagnostic => CreateError(diagnostic, document))
				.ToList();
		}
		
		Error CreateError(Diagnostic diagnostic, IDocument document)
		{
			TextLocation location = document.GetLocation(diagnostic.start);
			return new Error(
				GetErrorType(diagnostic.category),
				diagnostic.messageText,
				location);
		}

		ErrorType GetErrorType(DiagnosticCategory category)
		{
			switch (category) {
				case DiagnosticCategory.Warning:
					return ErrorType.Warning;
				case DiagnosticCategory.Error:
					return ErrorType.Error;
				case DiagnosticCategory.Message:
					return ErrorType.Unknown;
				default:
					return ErrorType.Error;
			}
		}
		
		public override IList<Error> Errors {
			get { return errors; }
		}
	}
}
