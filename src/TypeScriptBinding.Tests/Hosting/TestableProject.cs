
using System;
using MonoDevelop.Projects;

namespace TypeScriptBinding.Tests.Hosting
{
	public class TestableProject : Project
	{
		public TestableProject()
		{
		}
		
		public override string ProjectType {
			get { return String.Empty; }
		}
	}
}
