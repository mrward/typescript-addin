// 
// CompileTypeScriptAction.cs
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
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.TypeScriptBinding.Hosting;
using Mono.TextEditor.Utils;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace ICSharpCode.TypeScriptBinding
{
	public abstract class CompileTypeScriptAction
	{
		protected void Report(string message)
		{
			progressMonitor.Log.WriteLine(message);
		}

		protected IProgressMonitor progressMonitor;
		
		protected IProgressMonitor GetRunProcessMonitor()
		{
			progressMonitor = IdeApp.Workbench.ProgressMonitors.GetOutputProgressMonitor(
				"TypeScript",
				Stock.RunProgramIcon,
				false,
				true);
			
			return progressMonitor;
		}
		
		protected void Report(string format, params object[] args)
		{
			string message = String.Format(format, args);
			Report(message);
		}
		
		protected void ReportError(string error)
		{
			progressMonitor.ReportError(error, null);
		}
		
		protected void ReportCompileFinished(bool error)
		{
			if (error) {
				ReportError("TypeScript compilation failed.");
			} else {
				Report("TypeScript compilation finished successfully.");
			}
		}
		
		protected void UpdateFile(TypeScriptContext context, FilePath fileName)
		{
			context.UpdateFile(fileName, TypeScriptService.GetFileContents(fileName));
		}
	}
}
