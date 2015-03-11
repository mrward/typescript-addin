// 
// LanguageServiceHost.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013-2015 Matthew Ward
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

using TypeScriptLanguageService;


namespace TypeScriptHosting
{
    public class LanguageServiceHost : ILanguageServiceHost
	{
		Dictionary<string, Script> scripts = new Dictionary<string, Script>(StringComparer.OrdinalIgnoreCase);

		ILogger logger;

		string defaultLibScriptFileName;

        TypeScriptOptions compilerSettings = new TypeScriptOptions();
		
		public LanguageServiceHost(ILogger logger)
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
			if (!scripts.ContainsKey(fileName.FullPath)) {
				scripts.Add(fileName, new Script(fileName, text));
			}
		}
		
		Script FindScript(FilePath fileName)
		{
			Script script = null;
			if (scripts.TryGetValue(fileName, out script)) {
				return script;
			}
			return null;
		}
		
		internal void UpdateFile(FilePath fileName, string text)
		{
			scripts[fileName.FullPath].Update(text);
		}
		
		public int position { get; set; }
		public string fileName { get; set; }
		public bool isMemberCompletion { get; set; }
		public string completionEntry { get; set; }
		
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
		
		internal void UpdateCompilerSettings(ICompilerOptions options)
        {//@ Todo checkout compiler options becous it has no consturctors
            compilerSettings = new TypeScriptOptions(options);
		}
		
		internal void UpdateFileName(FilePath fileName)
		{
			this.fileName = fileName;
		}
		
		internal void RemoveFile(FilePath fileName)
		{
			scripts.Remove(fileName);
		}
		
		internal IEnumerable<string> GetFileNames()
		{
			return scripts.Keys.AsEnumerable();
		}
		
        #region ILanguageServiceHost


        public ICompilerOptions getCompilationSettings()
        {
            return this.compilerSettings;
        }
        public string getNewLine()
        {
            return Environment.NewLine;
        }
        public string[] getScriptFileNames()
        {
            return scripts.Select(keyPair => keyPair.Value.FileName).ToArray();
        }
        public string getScriptVersion(string fileName)
        {
            return scripts[fileName].Version.ToString();
        }
        public IScriptSnapshot getScriptSnapshot(string fileName)
        {
            Script script = scripts[fileName];
            return new ScriptSnapshot(logger, script);
        }
        public string getLocalizedDiagnosticMessages()
        {
            return "undefined";
        }
        public ICancellationToken getCancellationToken()
        {
            //@TODO fix this 
            return new LanguageServiceCancellationToken();
        }
        public string getCurrentDirectory()
        {
            //@ TODO is this correct
            return String.Empty;
        }
        public string getDefaultLibFileName(ICompilerOptions options)
        {
            return this.defaultLibScriptFileName;
        }

        public void log(string s)
        {
            logger.log(s);
        }

        public void trace(string s)
        {
            logger.log(s);
        }
        public void error(string s)
        {
            logger.log(s);
        }
        #endregion
	}
}
