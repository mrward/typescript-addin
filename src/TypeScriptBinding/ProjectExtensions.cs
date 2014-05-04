// 
// ProjectExtensions.cs
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
using MonoDevelop.Core.Serialization;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace ICSharpCode.TypeScriptBinding
{
	public static class ProjectExtensions
	{
		public static string GetProperty(this Project project, string name)
		{
			SolutionItemConfiguration config = GetActiveConfiguration(project);
			return project.GetProperty(config, name);
		}
		
		static SolutionItemConfiguration GetActiveConfiguration(Project project)
		{
			return project.GetConfiguration(IdeApp.Workspace.ActiveConfiguration);
		}
		
		public static string GetProperty(this Project project, SolutionItemConfiguration config, string name)
		{
			DataItem rawData = GetRawData(config);
			
			DataNode node = rawData.Extract(name);
			if (node == null)
				return null;
			
			return node.ToString();
		}
		
		static DataItem GetRawData(SolutionItemConfiguration config)
		{
			var dataItem = config.ExtendedProperties["__raw_data"] as DataItem;
			if (dataItem != null) {
				return dataItem;
			}
			return new DataItem();
		}
		
		public static void SetProperty(this Project project, string name, string value)
		{
			SolutionItemConfiguration config = GetActiveConfiguration(project);
			DataItem rawData = GetRawData(config);
			rawData.ItemData.Add(new DataValue(name, value));
		}
	}
}
