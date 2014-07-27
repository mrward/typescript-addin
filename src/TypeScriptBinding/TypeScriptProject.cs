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
using MonoDevelop.Core.Serialization;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptProject : ITypeScriptOptions
	{
		Project project;
		
		public static readonly string CompileOnSavePropertyName = "TypeScriptCompileOnSaveEnabled";
		public static readonly string CompileOnBuildPropertyName = "TypeScriptCompileOnBuildEnabled";
		public static readonly string RemoveCommentsPropertyName = "TypeScriptRemoveComments";
		public static readonly string GenerateSourceMapPropertyName = "TypeScriptSourceMap";
		public static readonly string ModuleKindPropertyName = "TypeScriptModuleKind";
		public static readonly string TargetPropertyName = "TypeScriptTarget";
		
		static readonly string DefaultEcmaScriptVersion = "ES5";
		static readonly string DefaultModuleKind = "none";
		
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
			project.Save();
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
			project.Save();
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
		
		string GetStringProperty(ProjectConfiguration config, string name, string defaultValue)
		{
			string propertyValue = project.GetProperty(config, name);
			if (!String.IsNullOrEmpty(propertyValue)) {
				return propertyValue;
			}
			return defaultValue;
		}
		
		bool GetBooleanProperty(ProjectConfiguration config, string name, bool defaultValue)
		{
			string propertyValue = project.GetProperty(config, name);
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
		
		void SetBooleanProperty(ProjectConfiguration config, string name, bool value)
		{
			SetStringProperty(config, name, value.ToString());
		}
		
		void SetStringProperty(ProjectConfiguration config, string name, string value)
		{
			project.SetProperty(config, name, value);
		}
		
		bool GetBooleanProperty(string name, bool defaultValue)
		{
			string propertyValue = project.GetProperty(name);
			return ConvertBooleanValue(propertyValue, defaultValue);
		}
		
		string GetStringProperty(string name, string defaultValue)
		{
			string propertyValue = project.GetProperty(name);
			if (!String.IsNullOrEmpty(propertyValue)) {
				return propertyValue;
			}
			return defaultValue;
		}
		
		public bool CompileOnSave {
			get { return GetBooleanProperty(CompileOnSavePropertyName, false); }
		}
		
		public bool GetCompileOnSave(ProjectConfiguration config)
		{
			return GetBooleanProperty(config, CompileOnSavePropertyName, false);
		}
		
		public void SetCompileOnSave(ProjectConfiguration config, bool value)
		{
			SetBooleanProperty(config, CompileOnSavePropertyName, value);
		}
		
		public bool CompileOnBuild {
			get { return GetBooleanProperty(CompileOnBuildPropertyName, true); }
		}
		
		public bool GetCompileOnBuild(ProjectConfiguration config)
		{
			return GetBooleanProperty(config, CompileOnBuildPropertyName, true);
		}
		
		public void SetCompileOnBuild(ProjectConfiguration config, bool value)
		{
			SetBooleanProperty(config, CompileOnBuildPropertyName, value);
		}
		
		public bool RemoveComments {
			get { return GetBooleanProperty(RemoveCommentsPropertyName, true); }
		}
		
		public bool GetRemoveComments(ProjectConfiguration config)
		{
			return GetBooleanProperty(config, RemoveCommentsPropertyName, true);
		}
		
		public void SetRemoveComments(ProjectConfiguration config, bool value)
		{
			SetBooleanProperty(config, RemoveCommentsPropertyName, value);
		}
		
		public bool GenerateSourceMap {
			get { return GetBooleanProperty(GenerateSourceMapPropertyName, true); }
		}
		
		public bool GetGenerateSourceMap(ProjectConfiguration config)
		{
			return GetBooleanProperty(config, GenerateSourceMapPropertyName, true);
		}
		
		public void SetGenerateSourceMap(ProjectConfiguration config, bool value)
		{
			SetBooleanProperty(config, GenerateSourceMapPropertyName, value);
		}
		
		public string EcmaScriptVersion {
			get { return GetStringProperty(TargetPropertyName, DefaultEcmaScriptVersion); }
		}
		
		public string GetEcmaScriptVersion(ProjectConfiguration config)
		{
			return GetStringProperty(config, TargetPropertyName, DefaultEcmaScriptVersion);
		}
		
		public void SetEcmaScriptVersion(ProjectConfiguration config, string value)
		{
			SetStringProperty(config, TargetPropertyName, value);
		}
		
		public string ModuleKind {
			get { return GetStringProperty(ModuleKindPropertyName, DefaultModuleKind); }
		}
		
		public string GetModuleKind(ProjectConfiguration config)
		{
			return GetStringProperty(config, ModuleKindPropertyName, DefaultModuleKind);
		}
		
		public void SetModuleKind(ProjectConfiguration config, string value)
		{
			SetStringProperty(config, ModuleKindPropertyName, value);
		}
		
		public LanguageVersion GetLanguageVersion()
		{
			if (EcmaScriptVersion == "ES3") {
				return LanguageVersion.EcmaScript3;
			}
			return LanguageVersion.EcmaScript5;
		}
		
		public ModuleGenTarget GetModulearget()
		{
			if (String.Equals(ModuleKind, "amd", StringComparison.OrdinalIgnoreCase)) {
				return ModuleGenTarget.Asynchronous;
			} else if (String.Equals(ModuleKind, "commonjs", StringComparison.OrdinalIgnoreCase)) {
				return ModuleGenTarget.Synchronous;
			}
			return ModuleGenTarget.Unspecified;
		}
	}
}
