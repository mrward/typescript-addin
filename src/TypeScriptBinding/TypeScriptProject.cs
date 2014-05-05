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

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptProject : ITypeScriptOptions
	{
		IProject project;
		MSBuildBasedProject msbuildProject;
		
		public static readonly string CompileOnSavePropertyName = "TypeScriptCompileOnSaveEnabled";
		public static readonly string CompileOnBuildPropertyName = "TypeScriptCompileOnBuildEnabled";
		public static readonly string RemoveCommentsPropertyName = "TypeScriptRemoveComments";
		public static readonly string GenerateSourceMapPropertyName = "TypeScriptSourceMap";
		public static readonly string ModuleKindPropertyName = "TypeScriptModuleKind";
		public static readonly string TargetPropertyName = "TypeScriptTarget";
		
		static readonly string DefaultEcmaScriptVersion = "ES5";
		static readonly string DefaultModuleKind = "none";
		
		public TypeScriptProject(IProject project)
		{
			this.project = project;
			this.msbuildProject = (MSBuildBasedProject)project;
		}
		
		public string Name {
			get { return project.Name; }
		}
		
		public void AddMissingFiles(IEnumerable<GeneratedTypeScriptFile> filesGenerated)
		{
			foreach (GeneratedTypeScriptFile file in filesGenerated) {
				AddMissingFile(file);
			}
			project.Save();
		}
		
		void AddMissingFile(GeneratedTypeScriptFile file)
		{
			if (IsFileInProject(file.FileName) || !File.Exists(file.FileName)) {
				return;
			}
			
			var projectItem = new FileProjectItem(project, ItemType.None);
			projectItem.FileName = file.FileName;
			projectItem.DependentUpon = file.GetDependentUpon();
			
			ProjectService.AddProjectItem(project, projectItem);
		}
		
		public bool IsFileInProject(FileName fileName)
		{
			return project.IsFileInProject(fileName);
		}
		
		public bool HasTypeScriptFiles()
		{
			return project
				.Items
				.Any(item => TypeScriptParser.IsTypeScriptFileName(item.FileName));
		}
		
		public IEnumerable<FileName> GetTypeScriptFileNames()
		{
			return project
				.Items
				.Where(item => TypeScriptParser.IsTypeScriptFileName(item.FileName))
				.Select(item => new FileName(item.FileName));
		}
		
		string GetStringProperty(BuildConfiguration buildConfig, string name, string defaultValue)
		{
			string propertyValue = msbuildProject.GetProperty(buildConfig.Configuration, buildConfig.Platform, name);
			if (!String.IsNullOrEmpty(propertyValue)) {
				return propertyValue;
			}
			return defaultValue;
		}
		
		bool GetBooleanProperty(BuildConfiguration buildConfig, string name, bool defaultValue)
		{
			string propertyValue = msbuildProject.GetProperty(buildConfig.Configuration, buildConfig.Platform, name);
			return ConvertBooleanValue(propertyValue, defaultValue);
		}
		
		bool ConvertBooleanValue(string propertyValue, bool defaultValue)
		{
			bool convertedValue = false;
			if (Boolean.TryParse(propertyValue, out convertedValue)) {
				return convertedValue;
			}
			return defaultValue;
		}
		
		void SetBooleanProperty(BuildConfiguration buildConfig, string name, bool value)
		{
			SetStringProperty(buildConfig, name, value.ToString());
		}
		
		void SetStringProperty(BuildConfiguration buildConfig, string name, string value)
		{
			msbuildProject.SetProperty(
				buildConfig.Configuration,
				buildConfig.Platform,
				name,
				value,
				PropertyStorageLocations.ConfigurationSpecific,
				false);
		}
		
		bool GetBooleanProperty(string name, bool defaultValue)
		{
			string propertyValue = msbuildProject.GetEvaluatedProperty(name);
			return ConvertBooleanValue(propertyValue, defaultValue);
		}
		
		string GetStringProperty(string name, string defaultValue)
		{
			string propertyValue = msbuildProject.GetEvaluatedProperty(name);
			if (!String.IsNullOrEmpty(propertyValue)) {
				return propertyValue;
			}
			return defaultValue;
		}
		
		public bool CompileOnSave {
			get { return GetBooleanProperty(CompileOnSavePropertyName, false); }
		}
		
		public bool GetCompileOnSave(BuildConfiguration buildConfig)
		{
			return GetBooleanProperty(buildConfig, CompileOnSavePropertyName, false);
		}
		
		public void SetCompileOnSave(BuildConfiguration buildConfig, bool value)
		{
			SetBooleanProperty(buildConfig, CompileOnSavePropertyName, value);
		}
		
		public bool CompileOnBuild {
			get { return GetBooleanProperty(CompileOnBuildPropertyName, true); }
		}
		
		public bool GetCompileOnBuild(BuildConfiguration buildConfig)
		{
			return GetBooleanProperty(buildConfig, CompileOnBuildPropertyName, true);
		}
		
		public void SetCompileOnBuild(BuildConfiguration buildConfig, bool value)
		{
			SetBooleanProperty(buildConfig, CompileOnBuildPropertyName, value);
		}
		
		public bool RemoveComments {
			get { return GetBooleanProperty(RemoveCommentsPropertyName, true); }
		}
		
		public bool GetRemoveComments(BuildConfiguration buildConfig)
		{
			return GetBooleanProperty(buildConfig, RemoveCommentsPropertyName, false);
		}
		
		public void SetRemoveComments(BuildConfiguration buildConfig, bool value)
		{
			SetBooleanProperty(buildConfig, RemoveCommentsPropertyName, value);
		}
		
		public bool GenerateSourceMap {
			get { return GetBooleanProperty(GenerateSourceMapPropertyName, true); }
		}
		
		public bool GetGenerateSourceMap(BuildConfiguration buildConfig)
		{
			return GetBooleanProperty(buildConfig, GenerateSourceMapPropertyName, false);
		}
		
		public void SetGenerateSourceMap(BuildConfiguration buildConfig, bool value)
		{
			SetBooleanProperty(buildConfig, GenerateSourceMapPropertyName, value);
		}
		
		public string EcmaScriptVersion {
			get { return GetStringProperty(TargetPropertyName, DefaultEcmaScriptVersion); }
		}
		
		public string GetEcmaScriptVersion(BuildConfiguration buildConfig)
		{
			return GetStringProperty(buildConfig, TargetPropertyName, DefaultEcmaScriptVersion);
		}
		
		public void SetEcmaScriptVersion(BuildConfiguration buildConfig, string value)
		{
			SetStringProperty(buildConfig, TargetPropertyName, value);
		}
		
		public string ModuleKind {
			get { return GetStringProperty(ModuleKindPropertyName, DefaultModuleKind); }
		}
		
		public string GetModuleKind(BuildConfiguration buildConfig)
		{
			return GetStringProperty(buildConfig, ModuleKindPropertyName, DefaultModuleKind);
		}
		
		public void SetModuleKind(BuildConfiguration buildConfig, string value)
		{
			SetStringProperty(buildConfig, ModuleKindPropertyName, value);
		}
	}
}
