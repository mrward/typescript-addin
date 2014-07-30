//
// TypeScriptProjectOptionsPanelWidget.cs
//
// Author:
//       Matt Ward <ward.matt@gmail.com>
//
// Copyright (c) 2014 Matthew Ward
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;

using MonoDevelop.Projects;

namespace ICSharpCode.TypeScriptBinding
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class TypeScriptProjectOptionsPanelWidget : Gtk.Bin
	{
		bool compileOnSave;
		bool compileOnBuild;
		bool includeComments;
		bool generateSourceMap;
		bool allowImplicitAnyTypes;
		DisplayValue selectedEcmaScriptTargetVersion;
		List<DisplayValue> ecmaScriptTargetVersions = new List<DisplayValue>();
		DisplayValue selectedModuleKind;
		List<DisplayValue> moduleKinds = new List<DisplayValue>();
		Project project;
		bool dirty;
		DotNetProjectConfiguration configuration;
		
		public TypeScriptProjectOptionsPanelWidget (Project project)
		{
			this.project = project;
			this.Build ();
			
			CreateDropDownLists();
		}
		
		void CreateDropDownLists()
		{
			ecmaScriptTargetVersions.Add(new DisplayValue("ES3", "ECMAScript 3"));
			ecmaScriptTargetVersions.Add(new DisplayValue("ES5", "ECMAScript 5"));
			
			PopulateEcmaScriptVersionComboBox();
			
			moduleKinds.Add(new DisplayValue("none", "None"));
			moduleKinds.Add(new DisplayValue("amd", "AMD"));
			moduleKinds.Add(new DisplayValue("commonjs", "CommonJS"));
			
			PopulateModuleComboBox();
		}
		
		void PopulateEcmaScriptVersionComboBox()
		{
			for (int index = 0; index < ecmaScriptTargetVersions.Count; ++index) {
				DisplayValue displayValue = ecmaScriptTargetVersions[index];
				ecmaVersionComboBox.InsertText(index, displayValue.Value);
			}
			
			ecmaVersionComboBox.Active = 0;
		}
		
		void PopulateModuleComboBox()
		{
			for (int index = 0; index < moduleKinds.Count; ++index) {
				DisplayValue displayValue = moduleKinds[index];
				moduleComboBox.InsertText(index, displayValue.Value);
			}
			
			moduleComboBox.Active = 0;
		}
		
		public void Load(DotNetProjectConfiguration configuration)
		{
			this.configuration = configuration;
			
			var typeScriptProject = new TypeScriptProject(project);
			CompileOnSave = typeScriptProject.GetCompileOnSave(configuration);
			CompileOnBuild = typeScriptProject.GetCompileOnBuild(configuration);
			IncludeComments = !typeScriptProject.GetRemoveComments(configuration);
			GenerateSourceMap = typeScriptProject.GetGenerateSourceMap(configuration);
			AllowImplicitAnyTypes = !typeScriptProject.GetNoImplicitAny(configuration);
			SelectedEcmaScriptTargetVersion = GetEcmaScriptTargetVersion(typeScriptProject);
			SelectedModuleKind = GetModuleKind(typeScriptProject);
			
			dirty = false;
			
			UpdateUserInterface();
			AddEventHandlers();
		}
		
		DisplayValue GetEcmaScriptTargetVersion(TypeScriptProject project)
		{
			string value = project.GetEcmaScriptVersion(configuration);
			
			DisplayValue displayValue = ecmaScriptTargetVersions.FirstOrDefault(version => version.Id.Equals(value, StringComparison.OrdinalIgnoreCase));
			if (displayValue != null) {
				return displayValue;
			}
			
			return ecmaScriptTargetVersions[0];
		}
		
		DisplayValue GetModuleKind(TypeScriptProject project)
		{
			string value = project.GetModuleKind(configuration);
			
			DisplayValue displayValue = moduleKinds.FirstOrDefault(moduleKind => moduleKind.Id.Equals(value, StringComparison.OrdinalIgnoreCase));
			if (displayValue != null) {
				return displayValue;
			}
			
			return moduleKinds[0];
		}
		
		void UpdateDirtyFlag<T>(T oldValue, T newValue)
		{
			if (!Object.Equals(oldValue, newValue)) {
				dirty = true;
			}
		}
		
		public bool CompileOnSave {
			get { return compileOnSave; }
			set {
				UpdateDirtyFlag(compileOnSave, value);
				compileOnSave = value;
			}
		}
		
		public bool CompileOnBuild {
			get { return compileOnBuild; }
			set {
				UpdateDirtyFlag(compileOnBuild, value);
				compileOnBuild = value;
			}
		}
		
		public bool IncludeComments {
			get { return includeComments; }
			set {
				UpdateDirtyFlag(includeComments, value);
				includeComments = value;
			}
		}
		
		public bool GenerateSourceMap {
			get { return generateSourceMap; }
			set {
				UpdateDirtyFlag(generateSourceMap, value);
				generateSourceMap = value;
			}
		}
		
		public bool AllowImplicitAnyTypes {
			get { return allowImplicitAnyTypes; }
			set {
				UpdateDirtyFlag(allowImplicitAnyTypes, value);
				allowImplicitAnyTypes = value;
			}
		}
		
		public List<DisplayValue> EcmaScriptTargetVersions {
			get { return ecmaScriptTargetVersions; }
		}
		
		public DisplayValue SelectedEcmaScriptTargetVersion {
			get { return selectedEcmaScriptTargetVersion; }
			set {
				UpdateDirtyFlag(selectedEcmaScriptTargetVersion, value);
				selectedEcmaScriptTargetVersion = value;
			}
		}
		
		public List<DisplayValue> ModuleKinds {
			get { return moduleKinds; }
		}
		
		public DisplayValue SelectedModuleKind {
			get { return selectedModuleKind; }
			set {
				UpdateDirtyFlag(selectedModuleKind, value);
				selectedModuleKind = value;
			}
		}
		
		void UpdateUserInterface()
		{
			compileOnBuildCheckButton.Active = compileOnBuild;
			compileOnSaveCheckButton.Active = compileOnSave;
			includeCommentsCheckButton.Active = includeComments;
			generateSourceMapCheckButton.Active = generateSourceMap;
			
			UpdateSelectedEcmaScriptVersionComboBox();
			UpdateSelectedModuleComboBox();
		}
		
		void UpdateSelectedEcmaScriptVersionComboBox()
		{
			ecmaVersionComboBox.Active = ecmaScriptTargetVersions.IndexOf(selectedEcmaScriptTargetVersion);
		}
		
		void UpdateSelectedModuleComboBox()
		{
			moduleComboBox.Active = moduleKinds.IndexOf(selectedModuleKind);
		}
		
		void AddEventHandlers()
		{
			compileOnSaveCheckButton.Clicked += CompileOnSaveCheckButtonClicked;
			compileOnBuildCheckButton.Clicked += CompileOnBuildCheckButtonClicked;
			includeCommentsCheckButton.Clicked += (o, e) => IncludeComments = !IncludeComments;
			generateSourceMapCheckButton.Clicked += (o, e) => GenerateSourceMap = !GenerateSourceMap;
			allowImplicitAnyCheckButton.Clicked += (o, e) => AllowImplicitAnyTypes = !AllowImplicitAnyTypes;
			moduleComboBox.Changed += ModuleComboBoxChanged;
			ecmaVersionComboBox.Changed += EcmaScriptVersionComboBoxChanged;
		}
		
		void CompileOnSaveCheckButtonClicked(object sender, EventArgs e)
		{
			CompileOnSave = !CompileOnSave;
		}
		
		void CompileOnBuildCheckButtonClicked(object sender, EventArgs e)
		{
			CompileOnBuild = !CompileOnBuild;
		}
		
		void ModuleComboBoxChanged(object sender, EventArgs e)
		{
			SelectedModuleKind = moduleKinds[moduleComboBox.Active];
		}
		
		void EcmaScriptVersionComboBoxChanged(object sender, EventArgs e)
		{
			SelectedEcmaScriptTargetVersion = ecmaScriptTargetVersions[ecmaVersionComboBox.Active];
		}
		
		public void Store()
		{
			if (!dirty)
				return;
			
			var typeScriptProject = new TypeScriptProject(project);
			typeScriptProject.SetCompileOnSave(configuration, CompileOnSave);
			typeScriptProject.SetCompileOnBuild(configuration, CompileOnBuild);
			typeScriptProject.SetRemoveComments(configuration, !IncludeComments);
			typeScriptProject.SetGenerateSourceMap(configuration, GenerateSourceMap);
			typeScriptProject.SetNoImplicitAny(configuration, !AllowImplicitAnyTypes);
			typeScriptProject.SetEcmaScriptVersion(configuration, SelectedEcmaScriptTargetVersion.Id);
			typeScriptProject.SetModuleKind(configuration, SelectedModuleKind.Id);
		}
	}
}

