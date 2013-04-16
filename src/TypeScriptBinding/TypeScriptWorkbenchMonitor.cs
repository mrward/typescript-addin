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
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptWorkbenchMonitor
	{
		TypeScriptContextProvider provider;
		
		public TypeScriptWorkbenchMonitor(
			IWorkbench workbench,
			TypeScriptContextProvider provider)
		{
			workbench.ViewOpened += ViewOpened;
			workbench.ViewClosed += ViewClosed;
			this.provider = provider;
		}
		
		void ViewOpened(object sender, ViewContentEventArgs e)
		{
			if (StandaloneTypeScriptFileOpened(e)) {
				CreateTypeScriptContext(e.Content);
			}
		}
		
		void CreateTypeScriptContext(IViewContent view)
		{
			provider.CreateContext(view.PrimaryFileName, GetText(view));
		}
		
		string GetText(IViewContent view)
		{
			var provider = view as ITextEditorProvider;
			return provider.TextEditor.Document.Text;
		}
		
		bool StandaloneTypeScriptFileOpened(ViewContentEventArgs e)
		{
			return StandaloneTypeScriptFileOpened(e.Content.PrimaryFileName);
		}
		
		bool StandaloneTypeScriptFileOpened(FileName fileName)
		{
			return TypeScriptParser.IsTypeScriptFileName(fileName) &&
				!TypeScriptFileInOpenProject(fileName);
		}
		
		bool TypeScriptFileInOpenProject(FileName fileName)
		{
			if (TypeScriptService.IsProjectOpen) {
				return TypeScriptService.GetCurrentTypeScriptProject().IsFileInProject(fileName);
			}
			return false;
		}
		
		void ViewClosed(object sender, ViewContentEventArgs e)
		{
			if (StandaloneTypeScriptFileOpened(e)) {
				provider.DisposeContext(e.Content.PrimaryFileName);
			}
		}
	}
}
