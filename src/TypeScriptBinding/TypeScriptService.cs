// 
// TypeScriptService.cs
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
using System.ComponentModel;
using System.Linq;

using ICSharpCode.TypeScriptBinding.Hosting;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace ICSharpCode.TypeScriptBinding
{
	public static class TypeScriptService
	{
		static readonly TypeScriptOptions options = new TypeScriptOptions();
		//static readonly TypeScriptParserService parserService = new TypeScriptParserService();
		static readonly TypeScriptContextProvider contextProvider = new TypeScriptContextProvider();
		static TypeScriptWorkbenchMonitor workbenchMonitor;
		static TypeScriptProjectMonitor projectMonitor;
		
		public static TypeScriptOptions Options {
			get { return options; }
		}
		
		public static TypeScriptContextProvider ContextProvider {
			get { return contextProvider; }
		}
		
		public static void Initialize()
		{
			if (IdeApp.IsInitialized) {
				OnIdeInitialized();
			} else {
				IdeApp.Initialized += (sender, e) => OnIdeInitialized();
			}
		}
		
		static void OnIdeInitialized()
		{
			IdeApp.Exiting += OnIdeExiting;
			workbenchMonitor = new TypeScriptWorkbenchMonitor(IdeApp.Workbench, contextProvider);
			projectMonitor = new TypeScriptProjectMonitor(contextProvider);
//			parserService.Start();
		}
		
		static void OnIdeExiting(object sender, ExitEventArgs e)
		{
//			parserService.Stop();
		}
		
		public static TypeScriptProject GetProjectForFile(FilePath fileName)
		{
			if (IdeApp.ProjectOperations.CurrentSelectedSolution == null)
				return null;
			
			return IdeApp
				.ProjectOperations
				.CurrentSelectedSolution
				.GetAllProjects()
				.Where(project => project.IsFileInProject(fileName))
				.Select(project => new TypeScriptProject(project))
				.FirstOrDefault();
		}
	}
}
