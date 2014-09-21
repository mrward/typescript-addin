// 
// TypeScriptCodeCompletionBinding.cs
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
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.SharpDevelop.Editor.CodeCompletion;
using ICSharpCode.SharpDevelop.Editor.Search;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Refactoring;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptCodeCompletionBinding : ICodeCompletionBinding
	{
		TypeScriptInsightWindowHandler insightHandler = new TypeScriptInsightWindowHandler();
		
		public CodeCompletionKeyPressResult HandleKeyPress(ITextEditor editor, char ch)
		{
			if (ch == '.') {
				InsertCharacter(editor, ch);
				ShowCompletion(editor, true);
				return CodeCompletionKeyPressResult.EatKey;
			} else if (ch == '(') {
				InsertCharacter(editor, ch);
				ShowMethodInsight(editor);
				return CodeCompletionKeyPressResult.EatKey;
			}
			return CodeCompletionKeyPressResult.None;
		}
		
		void InsertCharacter(ITextEditor editor, char ch)
		{
			editor.Document.Insert(editor.Caret.Offset, ch.ToString());
		}
		
		bool ShowCompletion(ITextEditor editor, bool memberCompletion)
		{
			TypeScriptContext context = GetContext(editor);
			UpdateContext(context, editor);
			
			var completionProvider = new TypeScriptCompletionItemProvider(context);
			return completionProvider.ShowCompletion(editor, memberCompletion);
		}
		
		static TypeScriptContext GetContext(ITextEditor editor)
		{
			return TypeScriptService.ContextProvider.GetContext(editor.FileName);
		}
		
		static void UpdateContext(TypeScriptContext context, ITextEditor editor)
		{
			if (IsFileInsideProject(editor.FileName)) {
				UpdateAllOpenFiles(context);
			} else {
				context.UpdateFile(editor.FileName, editor.Document.Text);
			}
		}
		
		static bool IsFileInsideProject(FileName fileName)
		{
			return TypeScriptService.ContextProvider.IsFileInsideProject(fileName);
		}
		
		static void UpdateAllOpenFiles(TypeScriptContext context)
		{
			foreach (IViewContent view in WorkbenchSingleton.Workbench.ViewContentCollection) {
				if (TypeScriptParser.IsTypeScriptFileName(view.PrimaryFileName)) {
					if (IsFileInsideProject(view.PrimaryFileName)) {
						UpdateContext(context, view.PrimaryFileName);
					}
				}
			}
		}
		
		static void UpdateContext(TypeScriptContext context, FileName fileName)
		{
			ITextBuffer fileContent = GetFileContent(fileName);
			context.UpdateFile(fileName, fileContent.Text);
		}
		
		static ITextBuffer GetFileContent(string fileName)
		{
			return ParserService.GetParseableFileContent(fileName);
		}
		
		public bool CtrlSpace(ITextEditor editor)
		{
			return ShowCompletion(editor, false);
		}
		
		void ShowMethodInsight(ITextEditor editor)
		{
			TypeScriptContext context = GetContext(editor);
			UpdateContext(context, editor);
			
			var provider = new TypeScriptFunctionInsightProvider(context);
			IInsightItem[] items = provider.ProvideInsight(editor);
			IInsightWindow insightWindow = editor.ShowInsightWindow(items);
			if (insightWindow != null) {
				insightHandler.InitializeOpenedInsightWindow(editor, insightWindow);
				insightHandler.HighlightParameter(insightWindow, 0);
			}
		}
		
		public static List<Reference> GetReferences(ITextEditor editor)
		{
			TypeScriptContext context = GetContext(editor);
			UpdateContext(context, editor);
			
			ReferenceEntry[] entries = context.FindReferences(editor.FileName, editor.Caret.Offset);
			
			return entries
				.Select(entry => CreateReference(entry))
				.ToList();
		}
		
		static Reference CreateReference(ReferenceEntry entry)
		{
			ITextBuffer fileContent = GetFileContent(entry.fileName);
			string expression = fileContent.GetText(entry.minChar, entry.length);
			return new Reference(entry.fileName, entry.minChar, entry.length, expression, null);
		}
		
		static void ShowSearchResults(List<SearchResultMatch> searchResults)
		{
			SearchResultsPad.Instance.ShowSearchResults("References", searchResults);
			SearchResultsPad.Instance.BringToFront();
		}
		
		public static void GoToDefinition(ITextEditor editor)
		{
			TypeScriptContext context = GetContext(editor);
			UpdateContext(context, editor);
			
			DefinitionInfo[] definitions = context.GetDefinition(editor.FileName, editor.Caret.Offset);
			if ((definitions != null) && (definitions.Length > 0)) {
				GoToDefinition(definitions[0]);
			}
		}
		
		static void GoToDefinition(DefinitionInfo definition)
		{
			if (!definition.HasFileName())
				return;
			
			var provider = FileService.OpenFile(definition.fileName) as ITextEditorProvider;
			if (provider != null) {
				Location location = provider.TextEditor.Document.OffsetToPosition(definition.minChar);
				FileService.JumpToFilePosition(definition.fileName, location.Line, location.Column);
			}
		}
	}
}
