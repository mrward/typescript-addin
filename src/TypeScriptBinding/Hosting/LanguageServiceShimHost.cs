// 
// LanguageServiceShimHost.cs
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
using ICSharpCode.TypeScriptBinding;
using ICSharpCode.TypeScriptBinding.Hosting;
using MonoDevelop.Core;
using Newtonsoft.Json;

namespace TypeScriptHosting
{
	public class LanguageServiceShimHost : ILanguageServiceShimHost
	{
		Dictionary<string, Script> scripts = new Dictionary<string, Script>();
		ILogger logger;
		string defaultLibScriptFileName;
		CompilerSettings compilerSettings = new CompilerSettings();
		
		public LanguageServiceShimHost(ILogger logger)
		{
			this.logger = logger;
		}
		
		internal void AddDefaultLibScript(FilePath fileName, string text)
		{
			defaultLibScriptFileName = fileName;
			AddFile(fileName, text);
		}
		
		internal void AddFile(FilePath fileName, string text)
		{
			string lowercaseFileName = fileName.FullPath.ToLower();
			if (!scripts.ContainsKey(lowercaseFileName)) {
				scripts.Add(lowercaseFileName, new Script(lowercaseFileName, text));
			}
		}
		
		Script FindScript(FilePath fileName)
		{
			string matchFileName = fileName.ToLower();
			Script script = null;
			if (scripts.TryGetValue(matchFileName, out script)) {
				return script;
			}
			return null;
		}
		
		internal void UpdateFile(FilePath fileName, string text)
		{
			scripts[fileName.FullPath.ToLower()].Update(text);
		}
		
		public int position { get; set; }
		public string fileName { get; set; }
		public bool isMemberCompletion { get; set; }
		public string completionEntry { get; set; }
		
		public void updateCompletionInfoAtCurrentPosition(string completionInfo)
		{
			LogDebug(completionInfo);
			CompletionResult = JsonConvert.DeserializeObject<CompletionResult>(completionInfo);
		}
		
		internal CompletionResult CompletionResult { get; private set; }
		
		public void updateCompletionEntryDetailsAtCurrentPosition(string completionEntryDetails)
		{
			LogDebug(completionEntryDetails);
			CompletionEntryDetailsResult = JsonConvert.DeserializeObject<CompletionEntryDetailsResult>(completionEntryDetails);
		}
		
		internal CompletionEntryDetailsResult CompletionEntryDetailsResult { get; private set; }
		
		public void updateSignatureAtPosition(string signature)
		{
			LogDebug(signature);
			SignatureResult = JsonConvert.DeserializeObject<SignatureResult>(signature);
		}
		
		internal SignatureResult SignatureResult { get; private set; }
		
		public void updateReferencesAtPosition(string references)
		{
			LogDebug(references);
			ReferencesResult = JsonConvert.DeserializeObject<ReferencesResult>(references);
		}
		
		internal ReferencesResult ReferencesResult { get; private set; }
		
		public void updateDefinitionAtPosition(string definition)
		{
			LogDebug(definition);
			DefinitionResult = JsonConvert.DeserializeObject<DefinitionResult>(definition);
		}
		
		internal DefinitionResult DefinitionResult { get; private set; }
		
		public void updateNavigationBarItems(string items)
		{
			LogDebug(items);
			NavigationResult = JsonConvert.DeserializeObject<NavigationResult>(items);
		}
		
		internal NavigationResult NavigationResult { get; private set; }
		
		public bool information()
		{
			return logger.information();
		}
		
		public bool debug()
		{
			return logger.debug();
		}
		
		public bool warning()
		{
			return logger.warning();
		}
		
		public bool error()
		{
			return logger.error();
		}
		
		public bool fatal()
		{
			return logger.fatal();
		}
		
		public void log(string s)
		{
			logger.log(s);
		}
		
		void LogDebug(string format, params object[] args)
		{
			LogDebug(String.Format(format, args));
		}
		
		void LogDebug(string s)
		{
			if (debug()) {
				log(s);
			}
		}
		
		public string getCompilationSettings()
		{
			LogDebug("Host.getCompilationSettings");
			return JsonConvert.SerializeObject(compilerSettings);
		}
		
		internal void UpdateCompilerSettings(ITypeScriptOptions options)
		{
			compilerSettings = new CompilerSettings(options);
		}
		
		public string getScriptVersion(string fileName)
		{
			LogDebug("Host.getScriptVersion: " + fileName);
			return scripts[fileName.ToLowerInvariant()].Version.ToString();
		}
		
		internal void UpdateFileName(FilePath fileName)
		{
			this.fileName = fileName.ToLower();
		}
		
		internal void RemoveFile(FilePath fileName)
		{
			scripts.Remove(fileName.ToLower());
		}
		
		internal IEnumerable<string> GetFileNames()
		{
			return scripts.Keys.AsEnumerable();
		}
		
		public IScriptSnapshotShim getScriptSnapshot(string fileName)
		{
			log("Host.getScriptSnapshot: " + fileName);
			Script script = scripts[fileName];
			return new ScriptSnapshotShim(logger, script);
		}
		
		public bool getScriptIsOpen(string fileName)
		{
			log("Host.getScriptIsOpen: " + fileName);
			if (defaultLibScriptFileName.Equals(new FilePath(fileName))) {
				return false;
			}
			return true;
		}
		
		public string getScriptFileNames()
		{
			log("Host.getScriptFileNames");
			
			string json = JsonConvert.SerializeObject(scripts.Keys.ToArray());
			
			log("Host.getScriptFileNames: " + json);
			
			return json;
		}
		
		public string getCurrentDirectory()
		{
			log("Host.getCurrentDirectory");
			return String.Empty;
		}
		
		public string getLocalizedDiagnosticMessages()
		{
			log("Host.getLocalizedDiagnosticMessages");
			return null;
		}
		
		public string getDefaultLibFilename()
		{
			log("Host.getDefaultLibFilename");
			return String.Empty;
		}
		
		public ICancellationToken getCancellationToken()
		{
			log("Host.getCancellationToken");
			return new LanguageServiceCancellationToken();
		}
		
		public void updateCompilerResult(string result)
		{
			log(result);
			CompilerResult = JsonConvert.DeserializeObject<CompilerResult>(result);
		}
		
		internal CompilerResult CompilerResult { get; private set; }
		
		public void updateSemanticDiagnosticsResult(string result)
		{
			log(result);
			SemanticDiagnosticsResult = JsonConvert.DeserializeObject<SemanticDiagnosticsResult>(result);
		}
		
		internal SemanticDiagnosticsResult SemanticDiagnosticsResult { get; private set; }
	}
}
