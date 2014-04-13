// 
// TypeScriptProject.cs
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
using System.IO;
using System.Linq;
using ICSharpCode.TypeScriptBinding.Hosting;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptProject
	{
		Project project;
		
		public TypeScriptProject(Project project)
		{
			this.project = project;
		}
		
		public string Name {
			get { return project.Name; }
		}
		
		public void AddMissingFiles(IEnumerable<GeneratedTypeScriptFile> filesGenerated)
		{
			foreach (GeneratedTypeScriptFile file in filesGenerated) {
				AddMissingFile(file);
			}
			IdeApp.ProjectOperations.Save(project);
		}
		
		void AddMissingFile(GeneratedTypeScriptFile file)
		{
			if (IsFileInProject(file.FileName) || !File.Exists(file.FileName)) {
				return;
			}
			
			ProjectFile fileItem = new ProjectFile(file.FileName) {
				BuildAction = BuildAction.None,
				DependsOn = file.GetDependentUpon()
			};
			project.AddFile(fileItem);
			IdeApp.ProjectOperations.Save(project);
		}
		
		public bool IsFileInProject(FilePath fileName)
		{
			return project.IsFileInProject(fileName);
		}
		
		public bool HasTypeScriptFiles()
		{
			return project
				.Items
				.OfType<ProjectFile>()
				.Any(item => TypeScriptParser.IsTypeScriptFileName(item.FilePath));
		}
		
		public IEnumerable<FilePath> GetTypeScriptFileNames()
		{
			return project
				.Items
				.OfType<ProjectFile>()
				.Where(item => TypeScriptParser.IsTypeScriptFileName(item.FilePath))
				.Select(item => item.FilePath);
		}
	}
}
