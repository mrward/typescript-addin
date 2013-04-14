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
using ICSharpCode.TypeScriptBinding.Hosting;
using Newtonsoft.Json;

namespace TypeScriptHosting
{
	public class LanguageServiceShimHost : ILanguageServiceShimHost
	{
		List<Script> scripts = new List<Script>();
		int defaultLibScriptIndex = -1;
		
		public LanguageServiceShimHost()
		{
		}
		
		internal void AddDefaultLibScript(string fileName, string text)
		{
			AddFile(fileName, text);
			defaultLibScriptIndex = scripts.Count - 1;
		}
		
		internal void AddFile(string fileName, string text)
		{
			scripts.Add(new Script(fileName, text));
		}
		
		internal void UpdateFile(string fileName, string text)
		{
			Script script = scripts.Find(s => s.Id == fileName);
			if (script != null) {
				script.Update(text);
			} else {
				AddFile(fileName, text);
			}
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
			log(completionInfo);
			CompletionResult = JsonConvert.DeserializeObject<CompletionResult>(completionInfo);
		}
		
		internal CompletionResult CompletionResult { get; private set; }
		
		public void updateSignatureAtPosition(string signature)
		{
			log(signature);
			SignatureResult = JsonConvert.DeserializeObject<SignatureResult>(signature);
		}
		
		internal SignatureResult SignatureResult { get; private set; }
		
		public void updateReferencesAtPosition(string references)
		{
			log(references);
			ReferenceInfo = new ReferenceInfo(references);
		}
		
		internal ReferenceInfo ReferenceInfo { get; private set; }
		
		public void updateDefinitionAtPosition(string definition)
		{
			log(definition);
			DefinitionInfo = new DefinitionInfo(definition);
		}
		
		internal DefinitionInfo DefinitionInfo { get; private set; }
		
		public void updateLexicalStructure(string structure)
		{
			log(structure);
			LexicalStructure = new NavigationInfo(structure);
		}
		
		internal NavigationInfo LexicalStructure { get; private set; }
		
		public void updateOutliningRegions(string regions)
		{
			log(regions);
			OutlingRegions = new NavigationInfo(regions);
		}
		
		internal NavigationInfo OutlingRegions { get; private set; }
		
		public bool information()
		{
			return true;
		}
		
		public bool debug()
		{
			return true;
		}
		
		public bool warning()
		{
			return true;
		}
		
		public bool error()
		{
			return true;
		}
		
		public bool fatal()
		{
			return true;
		}
		
		public void log(string s)
		{
			Console.WriteLine(s);
		}
		
		void LogFormat(string format, params object[] args)
		{
			log(String.Format(format, args));
		}
		
		public string getCompilationSettings()
		{
			log("Host.getCompilationSettings");
			return null;
		}
		
		public int getScriptCount()
		{
			LogFormat("Host.getScriptCount. Count={0}", scripts.Count);
			return scripts.Count;
		}
		
		public string getScriptId(int scriptIndex)
		{
			log("Host.getScriptId: " + scriptIndex);
			return scripts[scriptIndex].Id;
		}
		
		public string getScriptSourceText(int scriptIndex, int start, int end)
		{
			LogFormat("Host.getScriptSourceText: index={0}, start={1}, end={2}", scriptIndex, start, end);
			Script script = scripts[scriptIndex];
			return script.Source.Substring(start, end - start);
		}
		
		public int getScriptSourceLength(int scriptIndex)
		{
			log("Host.getScriptId: " + scriptIndex);
			return scripts[scriptIndex].Source.Length;
		}
		
		public bool getScriptIsResident(int scriptIndex)
		{
			log("Host.getScriptIsResident: " + scriptIndex);
			return scriptIndex == defaultLibScriptIndex;
		}
		
		public int getScriptVersion(int scriptIndex)
		{
			log("Host.getScriptVersion: " + scriptIndex);
			return scripts[scriptIndex].Version;
		}
		
		public string getScriptEditRangeSinceVersion(int scriptIndex, int scriptVersion)
		{
			LogFormat("Host.getScriptId: index={0}, version={1}", scriptIndex, scriptVersion);
			Script script = scripts[scriptIndex];
			if (script.Version == scriptVersion)
				return null;
			
			return "{ \"minChar\": -1, \"limChar\": -1, \"delta\": -1}";
		}
	}
}
