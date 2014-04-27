// 
// TypeScriptCodeCompletionTextEditorExtension.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013 - 2014 Matthew Ward
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

using ICSharpCode.TypeScriptBinding.Hosting;
using Mono.TextEditor;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptCodeCompletionTextEditorExtension : CompletionTextEditorExtension
	{
		public override ICompletionDataList HandleCodeCompletion(CodeCompletionContext completionContext, char completionChar, ref int triggerWordLength)
		{
			if (completionChar == '.') {
				return GetCompletionData(completionContext, false);
			}
			return null;
		}
		
		ICompletionDataList GetCompletionData(CodeCompletionContext completionContext, bool memberCompletion)
		{
			TypeScriptContext context = GetContext(Editor);
			UpdateContext(context, Editor);
			
			var completionProvider = new TypeScriptCompletionItemProvider(context);
			return completionProvider.GenerateCompletionList(Editor, memberCompletion);
		}
		
		static TypeScriptContext GetContext(TextEditorData editor)
		{
			return TypeScriptService.ContextProvider.GetContext(editor.FileName);
		}
		
		static void UpdateContext(TypeScriptContext context, TextEditorData editor)
		{
			if (IsFileInsideProject(editor.FileName)) {
				UpdateAllOpenFiles(context);
			} else {
				context.UpdateFile(editor.FileName, editor.Document.Text);
			}
		}
		
		static bool IsFileInsideProject(FilePath fileName)
		{
			return TypeScriptService.ContextProvider.IsFileInsideProject(fileName);
		}
		
		static void UpdateAllOpenFiles(TypeScriptContext context)
		{
			foreach (Document document in IdeApp.Workbench.Documents.Where(doc => doc.IsFile)) {
				if (TypeScriptParser.IsTypeScriptFileName(document.FileName)) {
					if (IsFileInsideProject(document.FileName)) {
						context.UpdateFile(document.FileName, document.Editor.Text);
					}
				}
			}
		}
		
		public override ParameterDataProvider HandleParameterCompletion(CodeCompletionContext completionContext, char completionChar)
		{
			if (completionChar == '(') {
				return CreateParameterDataProvider(completionContext);
			}
			return null;
		}
		
		TypeScriptParameterDataProvider CreateParameterDataProvider(CodeCompletionContext completionContext)
		{
			TypeScriptContext context = GetContext(Editor);
			UpdateContext(context, Editor);
			
			var provider = new TypeScriptParameterDataProvider(context, completionContext.TriggerOffset);
			provider.GetSignatures(Editor);
			
			return provider;
		}
		
		public override ICompletionDataList CodeCompletionCommand(CodeCompletionContext completionContext)
		{
			return GetCompletionData(completionContext, false);
		}
		
//		public static List<Reference> GetReferences(ITextEditor editor)
//		{
//			TypeScriptContext context = GetContext(editor);
//			UpdateContext(context, editor);
//			
//			ReferenceEntry[] entries = context.FindReferences(editor.FileName, editor.Caret.Offset);
//			
//			return entries
//				.Select(entry => CreateReference(entry))
//				.ToList();
//		}
//		
//		static Reference CreateReference(ReferenceEntry entry)
//		{
//			ITextBuffer fileContent = GetFileContent(entry.fileName);
//			string expression = fileContent.GetText(entry.minChar, entry.length);
//			return new Reference(entry.fileName, entry.minChar, entry.length, expression, null);
//		}
//		
//		static void ShowSearchResults(List<SearchResultMatch> searchResults)
//		{
//			SearchResultsPad.Instance.ShowSearchResults("References", searchResults);
//			SearchResultsPad.Instance.BringToFront();
//		}
//		
		public static void GoToDefinition(TextEditorData editor)
		{
			TypeScriptContext context = GetContext(editor);
			UpdateContext(context, editor);
			
			DefinitionInfo[] definitions = context.GetDefinition(editor.FileName, editor.Caret.Offset);
			if (definitions.Length > 0) {
				GoToDefinition(definitions[0]);
			}
		}
		
		static void GoToDefinition(DefinitionInfo definition)
		{
			if (!definition.HasFileName())
				return;
			
			Document document = IdeApp.Workbench.OpenDocument(definition.fileName, OpenDocumentOptions.TryToReuseViewer);
			DocumentLocation location = document.Editor.OffsetToLocation(definition.minChar);
			IdeApp.Workbench.OpenDocument(definition.fileName, location.Line, location.Column, OpenDocumentOptions.Default);
		}
	}
}
