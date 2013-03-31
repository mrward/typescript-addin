// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TypeScriptCompilerIOHost : IIO
	{
		string executingFilePath = new ScriptLoader().TypeScriptCompilerFileName;
		
		public TypeScriptCompilerIOHost(string fileName)
		{
			arguments = new string[] {
				fileName
			};
			stderr = new StandardOutputTextWriter();
			stdout = stderr;
		}
		
		public string[] arguments { get; set; }
		
		public ITextWriter stderr { get; set; }
		public ITextWriter stdout { get; set; }
		
		public string readFile(string path)
		{
			LogFormat("readFile() '{0}'", path);
			return File.ReadAllText(path);
		}
		
		public void writeFile(string path, string contents)
		{
			LogFormat("writeFile() '{0}'", path);
		}
		
		public ITextWriter createFile(string path, bool useUTF8)
		{
			LogFormat("createFile() '{0}', UTF8 {1}", path, useUTF8);
			return new TextFileWriter(path, useUTF8);
		}
		
		public void deleteFile(string path)
		{
			LogFormat("deleteFile() '{0}'", path);
		}
		
		public string[] dir(string path, object spec, object options)
		{
			LogFormat("dir() '{0}'", path);
			return new string[0];
		}
		
		public bool fileExists(string path)
		{
			LogFormat("fileExists() '{0}'", path);
			return System.IO.File.Exists(path);
		}
		
		public bool directoryExists(string path)
		{
			LogFormat("directoryExists() '{0}'", path);
			return System.IO.Directory.Exists(path);
		}
		
		public void createDirectory(string path)
		{
			LogFormat("createDirectory() '{0}'", path);
		}
		
		public string resolvePath(string path)
		{
			LogFormat("resolvePath() '{0}'", path);
			if (System.IO.Path.IsPathRooted(path)) {
				return path;
			}
			return System.IO.Path.Combine(path, getExecutingFilePath());
		}
		
		public string dirName(string path)
		{
			LogFormat("dirName '{0}'", path);
			
			if (String.IsNullOrEmpty(path))
				return path;
			
			return System.IO.Path.GetDirectoryName(path);
		}
		
		public IResolvedFile findFile(string rootPath, string partialFilePath)
		{
			LogFormat("findFile '{0}'", partialFilePath);
			return null;
		}
		
		public void print(string str)
		{
			Console.Write("print: " + str);
		}
		
		public void printLine(string str)
		{
			Console.WriteLine("printLine: " + str);
		}
		
		public void run(string source, string filename)
		{
			LogFormat("run filename: '{0}'", filename);
		}
		
		public string getExecutingFilePath()
		{
			LogFormat("getExecutingFilePath()");
			return executingFilePath;
		}
		
		public void quit(int exitCode)
		{
			LogFormat("quit {0}", exitCode);
		}
		
		void LogFormat(string format, params object[] args)
		{
			Console.WriteLine(format, args);
		}
	}
}
