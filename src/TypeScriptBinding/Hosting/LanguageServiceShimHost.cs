// 
// LanguageServiceShimHost.cs
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
using ICSharpCode.Core;
using ICSharpCode.TypeScriptBinding;
using ICSharpCode.TypeScriptBinding.Hosting;
using Newtonsoft.Json;

namespace TypeScriptHosting
{
	public class LanguageServiceShimHost : ILanguageServiceShimHost
	{
		List<Script> scripts = new List<Script>();
		int defaultLibScriptIndex = -1;
		ILogger logger;
		
		public LanguageServiceShimHost(ILogger logger)
		{
			this.logger = logger;
		}
		
		internal void AddDefaultLibScript(FileName fileName, string text)
		{
			AddFile(fileName, text);
			defaultLibScriptIndex = scripts.Count - 1;
		}
		
		internal void AddFile(FileName fileName, string text)
		{
			scripts.Add(new Script(fileName.ToLower(), text));
		}
		
		internal void UpdateFile(FileName fileName, string text)
		{
			Script script = FindScript(fileName);
			if (script != null) {
				script.Update(text);
			} else {
				AddFile(fileName, text);
			}
		}
		
		Script FindScript(FileName fileName)
		{
			string matchFileName = fileName.ToLower();
			return scripts.Find(s => s.Id == matchFileName);
		}
		
		internal void UpdateFile(int index, string text)
		{
			scripts[index].Update(text);
		}
		
		public int position { get; set; }
		public string fileName { get; set; }
		public bool isMemberCompletion { get; set; }
		
		public void updateCompletionInfoAtCurrentPosition(string completionInfo)
		{
			LogDebug(completionInfo);
			CompletionResult = JsonConvert.DeserializeObject<CompletionResult>(completionInfo);
		}
		
		internal CompletionResult CompletionResult { get; private set; }
		
		public void updateSignatureAtPosition(string signature)
		{
			LogDebug(signature);
			SignatureResult = JsonConvert.DeserializeObject<SignatureResult>(signature);
		}
		
		internal SignatureResult SignatureResult { get; private set; }
		
		public void updateReferencesAtPosition(string references)
		{
			LogDebug(references);
			ReferenceInfo = CreateReferenceInfo(references);
		}
		
		ReferenceInfo CreateReferenceInfo(string references)
		{
			var referenceInfo = new ReferenceInfo(references);
			foreach (ReferenceEntry entry in referenceInfo.entries) {
				entry.FileName = GetFileName(entry.unitIndex);
			}
			return referenceInfo;
		}
		
		string GetFileName(int index)
		{
			if ((index >= 0) && (index < scripts.Count)) {
				return scripts[index].Id;
			}
			return null;
		}
		
		internal ReferenceInfo ReferenceInfo { get; private set; }
		
		public void updateDefinitionAtPosition(string definition)
		{
			LogDebug(definition);
			DefinitionInfo = new DefinitionInfo(definition);
			DefinitionInfo.FileName = GetFileName(DefinitionInfo.unitIndex);
		}
		
		internal DefinitionInfo DefinitionInfo { get; private set; }
		
		public void updateLexicalStructure(string structure)
		{
			LogDebug(structure);
			LexicalStructure = new NavigationInfo(structure);
		}
		
		internal NavigationInfo LexicalStructure { get; private set; }
		
		public void updateOutliningRegions(string regions)
		{
			LogDebug(regions);
			OutlingRegions = new NavigationInfo(regions);
		}
		
		internal NavigationInfo OutlingRegions { get; private set; }
		
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
			return null;
		}
		
		public int getScriptCount()
		{
			LogDebug("Host.getScriptCount. Count={0}", scripts.Count);
			return scripts.Count;
		}
		
		public string getScriptId(int scriptIndex)
		{
			LogDebug("Host.getScriptId: " + scriptIndex);
			return scripts[scriptIndex].Id;
		}
		
		public string getScriptSourceText(int scriptIndex, int start, int end)
		{
			LogDebug("Host.getScriptSourceText: index={0}, start={1}, end={2}", scriptIndex, start, end);
			Script script = scripts[scriptIndex];
			return script.Source.Substring(start, end - start);
		}
		
		public int getScriptSourceLength(int scriptIndex)
		{
			LogDebug("Host.getScriptId: " + scriptIndex);
			return scripts[scriptIndex].Source.Length;
		}
		
		public bool getScriptIsResident(int scriptIndex)
		{
			LogDebug("Host.getScriptIsResident: " + scriptIndex);
			return scriptIndex == defaultLibScriptIndex;
		}
		
		public int getScriptVersion(int scriptIndex)
		{
			LogDebug("Host.getScriptVersion: " + scriptIndex);
			return scripts[scriptIndex].Version;
		}
		
		public string getScriptEditRangeSinceVersion(int scriptIndex, int scriptVersion)
		{
			LogDebug("Host.getScriptId: index={0}, version={1}", scriptIndex, scriptVersion);
			Script script = scripts[scriptIndex];
			if (script.Version == scriptVersion)
				return null;
			
			return "{ \"minChar\": -1, \"limChar\": -1, \"delta\": -1}";
		}
		
		internal void UpdateFileName(FileName fileName)
		{
			this.fileName = fileName.ToLower();
		}
		
		internal void RemoveFile(FileName fileName)
		{
			Script script = FindScript(fileName);
			if (script != null) {
				scripts.Remove(script);
			}
		}
	}
}
