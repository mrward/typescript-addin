
using System;
using System.Collections;
using ICSharpCode.TypeScriptBinding;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects;

namespace TypeScriptBinding.Tests.Hosting
{
	public class TestableProject : IProject
	{
		public SolutionItemConfiguration SolutionItemConfiguration;
		
		public TestableProject()
		{
			Name = String.Empty;
			Items = new ProjectItemCollection();
			
			SolutionItemConfiguration = new SolutionItemConfiguration("Debug");
			SolutionItemConfiguration.ExtendedProperties.Add("__raw_data", new DataItem());
		}
		
		public SolutionItemConfiguration GetConfiguration(ConfigurationSelector configuration)
		{
			return SolutionItemConfiguration;
		}
		
		public string Name { get; set; }
		
		public bool IsFileInProject(FilePath fileName)
		{
			return false;
		}
		
		public void Save()
		{
		}
		
		public void AddFile(ProjectFile fileItem)
		{
		}
		
		public ProjectItemCollection Items { get; private set; }
	}
}
