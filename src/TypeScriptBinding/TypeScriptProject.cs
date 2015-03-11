// 
// TypeScriptProject.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013-2014 Matthew Ward
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

using TypeScriptLanguageService;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptProject : ICompilerOptions
	{
		Project _project;
		
		public static readonly string CompileOnSavePropertyName = "TypeScriptCompileOnSaveEnabled";
		public static readonly string CompileOnBuildPropertyName = "TypeScriptCompileOnBuildEnabled";

        public static readonly string AllowNonTsExtensionsPropertyName = "TypeScriptAllowNonTsExtensions";
        public static readonly string CharsetPropertyName = "TypeScriptCharset";
        public static readonly string CodepagePropertyName = "TypeScriptCodepage";
        public static readonly string DeclarationPropertyName = "TypeScriptDeclaration";
        public static readonly string DiagnosticsPropertyName = "TypeScriptDiagnostics";
        public static readonly string EmitBOMPropertyName = "TypeScriptEmitBOM";
        public static readonly string HelpPropertyName = "TypeScriptHelp";
        public static readonly string ListFilesPropertyName = "TypeScriptListFiles";
        public static readonly string LocalePropertyName = "TypeScriptLocale";
        public static readonly string MapRootPropertyName = "TypeScriptMapRoot";
        public static readonly string ModulePropertyName = "TypeScriptModule";
        public static readonly string NoEmitPropertyName = "TypeScriptNoEmit";
        public static readonly string NoEmitOnErrorPropertyName = "TypeScriptNoEmitOnError";
        public static readonly string NoErrorTruncationPropertyName = "TypeScriptNoErrorTruncation";
        public static readonly string NoImplicitAnyPropertyName = "TypeScriptNoImplicitAny";
        public static readonly string NoLibPropertyName = "TypeScriptNoLib";
        public static readonly string NoLibCheckPropertyName = "TypeScriptNoLibCheck";
        public static readonly string NoResolvePropertyName = "TypeScriptNoResolve";
        public static readonly string TsOutPropertyName = "TypeScriptTsOut";
        public static readonly string OutDirPropertyName = "TypeScriptOutDir";
        public static readonly string PreserveConstEnumsPropertyName = "TypeScriptPreserveConstEnums";
        public static readonly string ProjectPropertyName = "TypeScriptProject";
        public static readonly string RemoveCommentsPropertyName = "TypeScriptRemoveComments";
        public static readonly string SourceMapPropertyName = "TypeScriptSourceMap";
        public static readonly string SourceRootPropertyName = "TypeScriptSourceRoot";
        public static readonly string SuppressImplicitAnyIndexErrorsPropertyName = "TypeScriptSuppressImplicitAnyIndexErrors";
        public static readonly string TargetPropertyName = "TypeScriptTarget";
        public static readonly string VersionPropertyName = "TypeScriptVersion";
        public static readonly string WatchPropertyName = "TypeScriptWatch";
        public static readonly string StripInternalPropertyName = "TypeScriptStripInternal";

        static readonly bool DefaultAllowNonTsExtensions = false;
        static readonly string DefaultCharset = "";
        static readonly int DefaultCodepage = 0;
        static readonly bool DefaultDeclaration = false;
        static readonly bool DefaultDiagnostics = false;
        static readonly bool DefaultEmitBOM = false;
        static readonly bool DefaultHelp = false;
        static readonly bool DefaultListFiles = false;
        static readonly string DefaultLocale = "";
        static readonly string DefaultMapRoot = "";
        static readonly ModuleKind DefaultModule = ModuleKind.None;
        static readonly bool DefaultNoEmit = false;
        static readonly bool DefaultNoEmitOnError = false;
        static readonly bool DefaultNoErrorTruncation = false;
        static readonly bool DefaultNoImplicitAny = false;
        static readonly bool DefaultNoLib = false;
        static readonly bool DefaultNoLibCheck = false;
        static readonly bool DefaultNoResolve = false;
        static readonly string DefaultTsOut = "";
        static readonly string DefaultOutDir = "";
        static readonly bool DefaultPreserveConstEnums = false;
        static readonly string DefaultProject = "";
        static readonly bool DefaultRemoveComments = false;
        static readonly bool DefaultSourceMap = false;
        static readonly string DefaultSourceRoot = "";
        static readonly bool DefaultSuppressImplicitAnyIndexErrors = false;
        static readonly ScriptTarget DefaultTarget = ScriptTarget.ES6;
        static readonly bool DefaultVersion = false;
        static readonly bool DefaultWatch = false;
        static readonly bool DefaultStripInternal = false;


        #region TypeScriptProject implementation
		public TypeScriptProject(Project project)
		{
            this._project = project;
		}
		
		public ICompilerOptions GetOptions()
		{
			return new TypeScriptOptions(this);
		}
		
		public string Name {
			get { return _project.Name; }
		}
		
		
		public bool HasTypeScriptFiles()
		{
			return _project
				.Items
				.OfType<ProjectFile>()
				.Any(item => TypeScriptParser.IsTypeScriptFileName(item.FilePath));
		}
		
		public IEnumerable<FilePath> GetTypeScriptFileNames()
		{
			return _project
				.Items
				.OfType<ProjectFile>()
				.Where(item => TypeScriptParser.IsTypeScriptFileName(item.FilePath))
				.Select(item => item.FilePath);
		}
		
		string GetStringProperty(ProjectConfiguration config, string name, string defaultValue)
		{
			string propertyValue = _project.GetProperty(config, name);
			if (!String.IsNullOrEmpty(propertyValue)) {
				return propertyValue;
			}
			return defaultValue;
		}
		
		bool GetBooleanProperty(ProjectConfiguration config, string name, bool defaultValue)
		{
			string propertyValue = _project.GetProperty(config, name);
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
			_project.SetProperty(config, name, value);
		}
		
		bool GetBooleanProperty(string name, bool defaultValue)
		{
			string propertyValue = _project.GetProperty(name);
			return ConvertBooleanValue(propertyValue, defaultValue);
		}
		
		string GetStringProperty(string name, string defaultValue)
		{
			string propertyValue = _project.GetProperty(name);
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

        public ScriptTarget GetScriptTarget()
        {
            ScriptTarget target;
            if (Enum.TryParse(TargetPropertyName, true, out target)) {
                return target;
            }
            return ScriptTarget.ES5;
        }

        public ModuleKind GetModuleTarget()
        {
            if (String.Equals(module.ToString("G"), "amd", StringComparison.OrdinalIgnoreCase)) {
                return TypeScriptLanguageService.ModuleKind.AMD;
            } else if (String.Equals(module.ToString("G"), "commonjs", StringComparison.OrdinalIgnoreCase)) {
                return TypeScriptLanguageService.ModuleKind.CommonJS;
            }
            return TypeScriptLanguageService.ModuleKind.None;
        }
            
        public IEnumerable<TypeScriptFile> GetTypeScriptFiles()
        {
            return GetTypeScriptFileNames()
                .Select(fileName => new TypeScriptFile(fileName, TypeScriptService.GetFileContents(fileName)));
        }

        void CreateOutputFileDirectory()
        {
            if (!String.IsNullOrEmpty(tsOut)) {
                string fullPath = GetOutputFileFullPath();
                string parentDirectory = Path.GetDirectoryName(fullPath);
                Directory.CreateDirectory(parentDirectory);
            }
        }

        public string GetOutputFileFullPath()
        {
            if (String.IsNullOrEmpty(tsOut)) {
                return String.Empty;
            }

            if (Path.IsPathRooted(tsOut)) {
                return tsOut;
            }
            return Path.Combine(_project.BaseDirectory, tsOut);
        }

        public void CreateOutputDirectory()
        {
            if (!String.IsNullOrEmpty(tsOut)) {
                CreateOutputFileDirectory();
            } else if (!String.IsNullOrEmpty(outDir)) {
                string fullPath = GetOutputDirectoryFullPath();
                Directory.CreateDirectory(fullPath);
            }
        }

        public string GetOutputDirectoryFullPath()
        {
            if (String.IsNullOrEmpty(outDir)) {
                return String.Empty;
            }

            if (Path.IsPathRooted(outDir)) {
                return outDir;
            }
            return Path.Combine(_project.BaseDirectory, outDir);
        }
        #endregion
		




        #region interface ICompilerOptions implementation


        #region allowNonTsExtensions
        public bool allowNonTsExtensions { 
            get { return GetBooleanProperty(AllowNonTsExtensionsPropertyName, DefaultAllowNonTsExtensions); }
            set { }
        }

        public bool GetAllowNonTsExtensions(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, AllowNonTsExtensionsPropertyName, DefaultAllowNonTsExtensions);
        }

        public void SetAllowNonTsExtensions(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, AllowNonTsExtensionsPropertyName, value);
        }
        #endregion 


        #region charset
        public string charset {
            get { return GetStringProperty(CharsetPropertyName, DefaultCharset); }
            set { }
        }

        public string GetCharset(ProjectConfiguration config)
        {
            return GetStringProperty(config, CharsetPropertyName, DefaultCharset);
        }

        public void SetCharset(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, CharsetPropertyName, value);
        }
        #endregion 


        #region codepage
        public int codepage {
            get { 
                int value;
                int.TryParse(GetStringProperty(CodepagePropertyName, DefaultCodepage.ToString()), out value);
                return value;
            }
            set { }
        }

        public string GetCodepage(ProjectConfiguration config)
        {
            return GetStringProperty(config, CodepagePropertyName, DefaultCodepage.ToString());
        }

        public void SetCodepage(ProjectConfiguration config, int value)
        {
            SetStringProperty(config, CodepagePropertyName, value.ToString());
        }
        #endregion 


        #region declaration
        public bool declaration { 
            get { return GetBooleanProperty(DeclarationPropertyName, DefaultDeclaration); }
            set { }
        }

        public bool GetDeclaration(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, DeclarationPropertyName, DefaultDeclaration);
        }

        public void SetDeclaration(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, DeclarationPropertyName, value);
        }
        #endregion 


        #region diagnostics
        public bool diagnostics { 
            get { return GetBooleanProperty(DiagnosticsPropertyName, DefaultDiagnostics); }
            set { }
        }

        public bool GetDiagnostics(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, DiagnosticsPropertyName, DefaultDiagnostics);
        }

        public void SetDiagnostics(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, DiagnosticsPropertyName, value);
        }
        #endregion 


        #region emitBOM
        public bool emitBOM { 
            get { return GetBooleanProperty(EmitBOMPropertyName, DefaultEmitBOM); }
            set { }
        }

        public bool GetEmitBOM(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, EmitBOMPropertyName, DefaultEmitBOM);
        }

        public void SetEmitBOM(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, EmitBOMPropertyName, value);
        }
        #endregion 


        #region help
        public bool help { 
            get { return GetBooleanProperty(HelpPropertyName, DefaultHelp); }
            set { }
        }

        public bool GetHelp(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, HelpPropertyName, DefaultHelp);
        }

        public void SetHelp(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, HelpPropertyName, value);
        }
        #endregion 


        #region listFiles
        public bool listFiles { 
            get { return GetBooleanProperty(ListFilesPropertyName, DefaultListFiles); }
            set { }
        }

        public bool GetListFiles(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, ListFilesPropertyName, DefaultListFiles);
        }

        public void SetListFiles(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, ListFilesPropertyName, value);
        }
        #endregion 


        #region locale
        public string locale {
            get { return GetStringProperty(LocalePropertyName, DefaultLocale); }
            set { }
        }

        public string GetLocale(ProjectConfiguration config)
        {
            return GetStringProperty(config, LocalePropertyName, DefaultLocale);
        }

        public void SetLocale(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, LocalePropertyName, value);
        }
        #endregion 


        #region mapRoot
        public string mapRoot {
            get { return GetStringProperty(MapRootPropertyName, DefaultMapRoot); }
            set { }
        }

        public string GetMapRoot(ProjectConfiguration config)
        {
            return GetStringProperty(config, MapRootPropertyName, DefaultMapRoot);
        }

        public void SetMapRoot(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, MapRootPropertyName, value);
        }
        #endregion 


        #region module
        public ModuleKind module {
            get { 
                ModuleKind value;
                Enum.TryParse(GetStringProperty(ModulePropertyName, DefaultModule.ToString()), out value);
                return value;
            }
            set { }
        }

        public string GetModule(ProjectConfiguration config)
        {
            return GetStringProperty(config, ModulePropertyName, DefaultModule.ToString());
        }

        public void SetModule(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, ModulePropertyName, value);
        }
        #endregion 


        #region noEmit
        public bool noEmit { 
            get { return GetBooleanProperty(NoEmitPropertyName, DefaultNoEmit); }
            set { }
        }

        public bool GetNoEmit(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, NoEmitPropertyName, DefaultNoEmit);
        }

        public void SetNoEmit(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, NoEmitPropertyName, value);
        }
        #endregion 


        #region noEmitOnError
        public bool noEmitOnError { 
            get { return GetBooleanProperty(NoEmitOnErrorPropertyName, DefaultNoEmitOnError); }
            set { }
        }

        public bool GetNoEmitOnError(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, NoEmitOnErrorPropertyName, DefaultNoEmitOnError);
        }

        public void SetNoEmitOnError(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, NoEmitOnErrorPropertyName, value);
        }
        #endregion 


        #region noErrorTruncation
        public bool noErrorTruncation { 
            get { return GetBooleanProperty(NoErrorTruncationPropertyName, DefaultNoErrorTruncation); }
            set { }
        }

        public bool GetNoErrorTruncation(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, NoErrorTruncationPropertyName, DefaultNoErrorTruncation);
        }

        public void SetNoErrorTruncation(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, NoErrorTruncationPropertyName, value);
        }
        #endregion 


        #region noImplicitAny
        public bool noImplicitAny { 
            get { return GetBooleanProperty(NoImplicitAnyPropertyName, DefaultNoImplicitAny); }
            set { }
        }

        public bool GetNoImplicitAny(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, NoImplicitAnyPropertyName, DefaultNoImplicitAny);
        }

        public void SetNoImplicitAny(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, NoImplicitAnyPropertyName, value);
        }
        #endregion 


        #region noLib
        public bool noLib { 
            get { return GetBooleanProperty(NoLibPropertyName, DefaultNoLib); }
            set { }
        }

        public bool GetNoLib(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, NoLibPropertyName, DefaultNoLib);
        }

        public void SetNoLib(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, NoLibPropertyName, value);
        }
        #endregion 


        #region noLibCheck
        public bool noLibCheck { 
            get { return GetBooleanProperty(NoLibCheckPropertyName, DefaultNoLibCheck); }
            set { }
        }

        public bool GetNoLibCheck(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, NoLibCheckPropertyName, DefaultNoLibCheck);
        }

        public void SetNoLibCheck(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, NoLibCheckPropertyName, value);
        }
        #endregion 


        #region noResolve
        public bool noResolve { 
            get { return GetBooleanProperty(NoResolvePropertyName, DefaultNoResolve); }
            set { }
        }

        public bool GetNoResolve(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, NoResolvePropertyName, DefaultNoResolve);
        }

        public void SetNoResolve(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, NoResolvePropertyName, value);
        }
        #endregion 


        #region tsOut
        public string tsOut {
            get { return GetStringProperty(TsOutPropertyName, DefaultTsOut); }
            set { }
        }

        public string GetTsOut(ProjectConfiguration config)
        {
            return GetStringProperty(config, TsOutPropertyName, DefaultTsOut);
        }

        public void SetTsOut(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, TsOutPropertyName, value);
        }
        #endregion 


        #region outDir
        public string outDir {
            get { return GetStringProperty(OutDirPropertyName, DefaultOutDir); }
            set { }
        }

        public string GetOutDir(ProjectConfiguration config)
        {
            return GetStringProperty(config, OutDirPropertyName, DefaultOutDir);
        }

        public void SetOutDir(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, OutDirPropertyName, value);
        }
        #endregion 


        #region preserveConstEnums
        public bool preserveConstEnums { 
            get { return GetBooleanProperty(PreserveConstEnumsPropertyName, DefaultPreserveConstEnums); }
            set { }
        }

        public bool GetPreserveConstEnums(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, PreserveConstEnumsPropertyName, DefaultPreserveConstEnums);
        }

        public void SetPreserveConstEnums(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, PreserveConstEnumsPropertyName, value);
        }
        #endregion 


        #region project
        public string project {
            get { return GetStringProperty(ProjectPropertyName, DefaultProject); }
            set { }
        }

        public string GetProject(ProjectConfiguration config)
        {
            return GetStringProperty(config, ProjectPropertyName, DefaultProject);
        }

        public void SetProject(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, ProjectPropertyName, value);
        }
        #endregion 


        #region removeComments
        public bool removeComments { 
            get { return GetBooleanProperty(RemoveCommentsPropertyName, DefaultRemoveComments); }
            set { }
        }

        public bool GetRemoveComments(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, RemoveCommentsPropertyName, DefaultRemoveComments);
        }

        public void SetRemoveComments(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, RemoveCommentsPropertyName, value);
        }
        #endregion 


        #region sourceMap
        public bool sourceMap { 
            get { return GetBooleanProperty(SourceMapPropertyName, DefaultSourceMap); }
            set { }
        }

        public bool GetSourceMap(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, SourceMapPropertyName, DefaultSourceMap);
        }

        public void SetSourceMap(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, SourceMapPropertyName, value);
        }
        #endregion 


        #region sourceRoot
        public string sourceRoot {
            get { return GetStringProperty(SourceRootPropertyName, DefaultSourceRoot); }
            set { }
        }

        public string GetSourceRoot(ProjectConfiguration config)
        {
            return GetStringProperty(config, SourceRootPropertyName, DefaultSourceRoot);
        }

        public void SetSourceRoot(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, SourceRootPropertyName, value);
        }
        #endregion 


        #region suppressImplicitAnyIndexErrors
        public bool suppressImplicitAnyIndexErrors { 
            get { return GetBooleanProperty(SuppressImplicitAnyIndexErrorsPropertyName, DefaultSuppressImplicitAnyIndexErrors); }
            set { }
        }

        public bool GetSuppressImplicitAnyIndexErrors(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, SuppressImplicitAnyIndexErrorsPropertyName, DefaultSuppressImplicitAnyIndexErrors);
        }

        public void SetSuppressImplicitAnyIndexErrors(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, SuppressImplicitAnyIndexErrorsPropertyName, value);
        }
        #endregion 


        #region target
        public ScriptTarget target {
            get { 
                ScriptTarget value;
                Enum.TryParse(GetStringProperty(TargetPropertyName, DefaultTarget.ToString()), out value);
                return value;
            }
            set { }
        }

        public string GetTarget(ProjectConfiguration config)
        {
            return GetStringProperty(config, TargetPropertyName, DefaultTarget.ToString());
        }

        public void SetTarget(ProjectConfiguration config, string value)
        {
            SetStringProperty(config, TargetPropertyName, value);
        }
        #endregion 


        #region version
        public bool version { 
            get { return GetBooleanProperty(VersionPropertyName, DefaultVersion); }
            set { }
        }

        public bool GetVersion(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, VersionPropertyName, DefaultVersion);
        }

        public void SetVersion(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, VersionPropertyName, value);
        }
        #endregion 


        #region watch
        public bool watch { 
            get { return GetBooleanProperty(WatchPropertyName, DefaultWatch); }
            set { }
        }

        public bool GetWatch(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, WatchPropertyName, DefaultWatch);
        }

        public void SetWatch(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, WatchPropertyName, value);
        }
        #endregion 


        #region stripInternal
        public bool stripInternal { 
            get { return GetBooleanProperty(StripInternalPropertyName, DefaultStripInternal); }
            set { }
        }

        public bool GetStripInternal(ProjectConfiguration config)
        {
            return GetBooleanProperty(config, StripInternalPropertyName, DefaultStripInternal);
        }

        public void SetStripInternal(ProjectConfiguration config, bool value)
        {
            SetBooleanProperty(config, StripInternalPropertyName, value);
        }
        #endregion 


        #endregion		

	}
}
