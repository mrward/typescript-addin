// 
// TypeScriptProjectOptionsPanel.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

using ICSharpCode.SharpDevelop.Gui.OptionPanels;
using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.TypeScriptBinding
{
	public partial class TypeScriptProjectOptionsPanel : ProjectOptionPanel
	{
		bool compileOnSave;
		bool compileOnBuild;
		bool includeComments;
		bool generateSourceMap;
		DisplayValue selectedEcmaScriptTargetVersion;
		List<DisplayValue> ecmaScriptTargetVersions = new List<DisplayValue>();
		DisplayValue selectedModuleKind;
		List<DisplayValue> moduleKinds = new List<DisplayValue>();

		public TypeScriptProjectOptionsPanel()
		{
			InitializeComponent();
			
			ecmaScriptTargetVersions.Add(new DisplayValue("ES3", "ECMAScript 3"));
			ecmaScriptTargetVersions.Add(new DisplayValue("ES5", "ECMAScript 5"));
			
			moduleKinds.Add(new DisplayValue("none", "None"));
			moduleKinds.Add(new DisplayValue("amd", "AMD"));
			moduleKinds.Add(new DisplayValue("commonjs", "CommonJS"));
			
			DataContext = this;
		}
		
		void UpdateDirtyFlag<T>(T oldValue, T newValue)
		{
			if (!Object.Equals(oldValue, newValue)) {
				IsDirty = true;
			}
		}
		
		public bool CompileOnSave {
			get { return compileOnSave; }
			set {
				UpdateDirtyFlag(compileOnSave, value);
				compileOnSave = value;
				compileOnBuild = !compileOnSave;
				RaisePropertyChanged(() => CompileOnBuild);
				RaisePropertyChanged(() => CompileOnSave);
			}
		}
		
		public bool CompileOnBuild {
			get { return compileOnBuild; }
			set {
				UpdateDirtyFlag(compileOnBuild, value);
				compileOnBuild = value;
				compileOnSave = !compileOnBuild;
				RaisePropertyChanged(() => CompileOnSave);
				RaisePropertyChanged(() => CompileOnBuild);
			}
		}
		
		public bool IncludeComments {
			get { return includeComments; }
			set {
				UpdateDirtyFlag(includeComments, value);
				includeComments = value;
				RaisePropertyChanged(() => IncludeComments);
			}
		}
		
		public bool GenerateSourceMap {
			get { return generateSourceMap; }
			set {
				UpdateDirtyFlag(generateSourceMap, value);
				generateSourceMap = value;
				RaisePropertyChanged(() => GenerateSourceMap);
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
				RaisePropertyChanged(() => SelectedEcmaScriptTargetVersion);
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
				RaisePropertyChanged(() => SelectedModuleKind);
			}
		}
		
		protected override void Load(MSBuildBasedProject project, string configuration, string platform)
		{
			base.Load(project, configuration, platform);
			
			var buildConfig = new BuildConfiguration(configuration, platform);
			
			var typeScriptProject = new TypeScriptProject(project);
			CompileOnSave = typeScriptProject.GetCompileOnSave(buildConfig);
			CompileOnBuild = typeScriptProject.GetCompileOnBuild(buildConfig);
			IncludeComments = !typeScriptProject.GetRemoveComments(buildConfig);
			GenerateSourceMap = typeScriptProject.GetGenerateSourceMap(buildConfig);
			SelectedEcmaScriptTargetVersion = GetEcmaScriptTargetVersion(typeScriptProject, buildConfig);
			SelectedModuleKind = GetModuleKind(typeScriptProject, buildConfig);
			
			IsDirty = false;
		}
		
		DisplayValue GetEcmaScriptTargetVersion(TypeScriptProject project, BuildConfiguration buildConfig)
		{
			string value = project.GetEcmaScriptVersion(buildConfig);
			
			DisplayValue displayValue = ecmaScriptTargetVersions.FirstOrDefault(version => version.Id.Equals(value, StringComparison.OrdinalIgnoreCase));
			if (displayValue != null) {
				return displayValue;
			}
			
			return ecmaScriptTargetVersions[0];
		}
		
		DisplayValue GetModuleKind(TypeScriptProject project, BuildConfiguration buildConfig)
		{
			string value = project.GetModuleKind(buildConfig);
			
			DisplayValue displayValue = moduleKinds.FirstOrDefault(moduleKind => moduleKind.Id.Equals(value, StringComparison.OrdinalIgnoreCase));
			if (displayValue != null) {
				return displayValue;
			}
			
			return moduleKinds[0];
		}
		
		protected override bool Save(MSBuildBasedProject project, string configuration, string platform)
		{
			var buildConfig = new BuildConfiguration(configuration, platform);
			
			var typeScriptProject = new TypeScriptProject(project);
			typeScriptProject.SetCompileOnSave(buildConfig, CompileOnSave);
			typeScriptProject.SetCompileOnBuild(buildConfig, CompileOnBuild);
			typeScriptProject.SetRemoveComments(buildConfig, !IncludeComments);
			typeScriptProject.SetGenerateSourceMap(buildConfig, GenerateSourceMap);
			typeScriptProject.SetEcmaScriptVersion(buildConfig, SelectedEcmaScriptTargetVersion.Id);
			typeScriptProject.SetModuleKind(buildConfig, SelectedModuleKind.Id);
			
			return base.Save(project, configuration, platform);
		}
	}
}