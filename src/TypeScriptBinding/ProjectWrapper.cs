// 
// ProjectWrapper.cs
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
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace ICSharpCode.TypeScriptBinding
{
	public class ProjectWrapper : IProject
	{
		Project project;
		
		public ProjectWrapper(Project project)
		{
			this.project = project;
		}
		
		public SolutionItemConfiguration GetConfiguration(ConfigurationSelector configuration)
		{
			return project.GetConfiguration(configuration);
		}
		
		public void Save()
		{
			IdeApp.ProjectOperations.Save(project);
		}
		
		public ProjectItemCollection Items {
			get { return project.Items; }
		}
		
		public string Name {
			get { return project.Name; }
		}
		
		public bool IsFileInProject(FilePath fileName)
		{
			return project.IsFileInProject(fileName);
		}
		
		public void AddFile(ProjectFile file)
		{
			project.AddFile(file);
		}
	}
}
