// 
// TypeScriptWorkbenchMonitor.cs
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
using System.Text;

using ICSharpCode.TypeScriptBinding.Hosting;
using Mono.TextEditor;
using Mono.TextEditor.Utils;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptWorkbenchMonitor
	{
		TypeScriptContextProvider provider;
		
		public TypeScriptWorkbenchMonitor(
			Workbench workbench,
			TypeScriptContextProvider provider)
		{
			workbench.DocumentOpened += DocumentOpened;
			workbench.DocumentClosed += DocumentClosed;
			this.provider = provider;
		}
		
		void DocumentOpened(object sender, DocumentEventArgs e)
		{
			if (StandaloneTypeScriptFileOpened(e)) {
				CreateTypeScriptContext(e.Document);
			}
		}
		
		void CreateTypeScriptContext(Document document)
		{
			TextEditorData textEditor = TextFileProvider.Instance.GetTextEditorData(document.FileName);
			provider.CreateContext(document.FileName, textEditor.Text);
		}
		
		bool StandaloneTypeScriptFileOpened(DocumentEventArgs e)
		{
			return StandaloneTypeScriptFileOpened(e.Document.FileName);
		}
		
		bool StandaloneTypeScriptFileOpened(FilePath fileName)
		{
			return TypeScriptParser.IsTypeScriptFileName(fileName) &&
				!TypeScriptFileInAnyProject(fileName);
		}
		
		bool TypeScriptFileInAnyProject(FilePath fileName)
		{
			return TypeScriptService.ContextProvider.IsFileInsideProject(fileName);
		}
		
		void DocumentClosed(object sender, DocumentEventArgs e)
		{
			FilePath fileName = e.Document.FileName;
			if (TypeScriptParser.IsTypeScriptFileName(fileName)) {
				if (TypeScriptFileInAnyProject(fileName)) {
					UpdateTypeScriptContextWithFileContentFromDisk(fileName);
				} else {
					provider.DisposeContext(fileName);
				}
			}
		}
		
		void UpdateTypeScriptContextWithFileContentFromDisk(FilePath fileName)
		{
			if (File.Exists(fileName)) {
				TypeScriptContext context = TypeScriptService.ContextProvider.GetContext(fileName);
				string fileContent = ReadFileContent(fileName);
				context.UpdateFile(fileName, fileContent);
			}
		}
		
		string ReadFileContent(FilePath filePath)
		{
			bool hadBom;
			Encoding encoding;
			return TextFileUtility.ReadAllText(filePath, out hadBom, out encoding);
		}
	}
}
