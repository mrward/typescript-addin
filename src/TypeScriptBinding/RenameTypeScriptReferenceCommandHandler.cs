// 
// RenameTypeScriptReferenceCommand.cs
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

using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Refactoring;

namespace ICSharpCode.TypeScriptBinding
{
	public class RenameTypeScriptReferenceCommandHandler : TypeScriptCommandHandler
	{
		protected override void Run(Document document)
		{
			List<SearchResult> references = TypeScriptCodeCompletionTextEditorExtension.GetReferences(document.Editor);
			
			if (references.Count == 0) {
				ShowReferenceError();
			} else {
				RenameAllReferences(references);
			}
		}
		
		static void ShowReferenceError()
		{
			MessageService.ShowMessage("Unable to find any items to rename.");
		}
		
		void RenameAllReferences(List<SearchResult> references)
		{
			string name = GetExistingName(references);
			string newName = GetNewName(name);
			if (ShouldRenameReference(newName, name)) {
				RenameAllReferences(references, name, newName);
			}
		}
		
		string GetExistingName(List<SearchResult> references)
		{
			SearchResult firstReference = references.First();
			string content = firstReference.FileProvider.ReadString();
			return content.Substring(firstReference.Offset, firstReference.Length);
		}
		
		string GetNewName(string name)
		{
			return MessageService.GetTextResponse("Enter the new name", "Rename", name);
		}
		
		bool ShouldRenameReference(string newName, string name)
		{
			return (newName != null) && (newName != name);
		}
		
		void RenameAllReferences(List<SearchResult> references, string oldName, string newName)
		{
			using (IProgressMonitor monitor = IdeApp.Workbench.ProgressMonitors.GetBackgroundProgressMonitor("Renaming", null)) {
				List<Change> changes = GetRenameChanges(references, oldName, newName);
				RefactoringService.AcceptChanges (monitor, changes);
			}
		}
		
		List<Change> GetRenameChanges(List<SearchResult> references, string oldName, string newName)
		{
			return references
				.Select(reference => CreateTextChange(reference, oldName, newName))
				.ToList();
		}
		
		Change CreateTextChange(SearchResult reference, string oldName, string newName)
		{
			return new TextReplaceChange {
				Description = String.Format (GettextCatalog.GetString ("Replace '{0}' with '{1}'"), oldName, newName),
				FileName = reference.FileName,
				InsertedText = newName,
				Offset = reference.Offset,
				RemovedChars = reference.Length
			};
		}
	}
}
