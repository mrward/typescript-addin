//// 
//// TypeScriptOptionsPanel.cs
//// 
//// Author:
////   Matt Ward <ward.matt@gmail.com>
//// 
//// Copyright (C) 2013 Matthew Ward
//// 
//// Permission is hereby granted, free of charge, to any person obtaining
//// a copy of this software and associated documentation files (the
//// "Software"), to deal in the Software without restriction, including
//// without limitation the rights to use, copy, modify, merge, publish,
//// distribute, sublicense, and/or sell copies of the Software, and to
//// permit persons to whom the Software is furnished to do so, subject to
//// the following conditions:
//// 
//// The above copyright notice and this permission notice shall be
//// included in all copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////
//
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Windows.Controls;
//
//using ICSharpCode.SharpDevelop.Gui;
//
//namespace ICSharpCode.TypeScriptBinding
//{
//	public partial class TypeScriptOptionsPanel : OptionPanel, INotifyPropertyChanged
//	{
//		TypeScriptOptions options;
//		bool compileOnSave;
//		bool compileOnBuild;
//		bool includeComments;
//		bool generateSourceMap;
//		string selectedEcmaScriptTargetVersion;
//		string[] ecmaScriptTargetVersions = new string[] { "ES3", "ES5" };
//		string selectedModuleKind;
//		string[] moduleKinds = new string[] { "AMD", "CommonJS" };
//		
//		public TypeScriptOptionsPanel()
//		{
//			InitializeComponent();
//			this.DataContext = this;
//			
//			options = TypeScriptService.Options;
//		}
//		
//		public bool CompileOnSave {
//			get { return compileOnSave; }
//			set {
//				compileOnSave = value;
//				RaisePropertyChanged(() => CompileOnSave);
//			}
//		}
//		
//		public bool CompileOnBuild {
//			get { return compileOnBuild; }
//			set {
//				compileOnBuild = value;
//				RaisePropertyChanged(() => CompileOnBuild);
//			}
//		}
//		
//		public bool IncludeComments {
//			get { return includeComments; }
//			set {
//				includeComments = value;
//				RaisePropertyChanged(() => IncludeComments);
//			}
//		}
//		
//		public bool GenerateSourceMap {
//			get { return generateSourceMap; }
//			set {
//				generateSourceMap = value;
//				RaisePropertyChanged(() => GenerateSourceMap);
//			}
//		}
//		
//		public string[] EcmaScriptTargetVersions {
//			get { return ecmaScriptTargetVersions; }
//		}
//		
//		public string SelectedEcmaScriptTargetVersion {
//			get { return selectedEcmaScriptTargetVersion; }
//			set {
//				selectedEcmaScriptTargetVersion = value;
//				RaisePropertyChanged(() => SelectedEcmaScriptTargetVersion);
//			}
//		}
//		
//		public string[] ModuleKinds {
//			get { return moduleKinds; }
//		}
//		
//		public string SelectedModuleKind {
//			get { return selectedModuleKind; }
//			set {
//				selectedModuleKind = value;
//				RaisePropertyChanged(() => SelectedModuleKind);
//			}
//		}
//		
//		public override void LoadOptions()
//		{
//			CompileOnSave = options.CompileOnSave;
//			CompileOnBuild = options.CompileOnBuild;
//			IncludeComments = options.IncludeComments;
//			GenerateSourceMap = options.GenerateSourceMap;
//			SelectedEcmaScriptTargetVersion = options.EcmaScriptTargetVersion;
//			SelectedModuleKind = options.ModuleKind;
//		}
//		
//		public override bool SaveOptions()
//		{
//			options.CompileOnSave = CompileOnSave;
//			options.CompileOnBuild = CompileOnBuild;
//			options.IncludeComments = IncludeComments;
//			options.GenerateSourceMap = GenerateSourceMap;
//			options.EcmaScriptTargetVersion = SelectedEcmaScriptTargetVersion;
//			options.ModuleKind = SelectedModuleKind;
//			return true;
//		}
//	}
//}