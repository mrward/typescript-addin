// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TypeScriptCompilerEnvironment : IEnvironment
	{
		IIO host;
		
		public TypeScriptCompilerEnvironment(IIO host)
		{
			this.host = host;
		}
		
		public string[] arguments {
			get { return host.arguments; }
		}
		
		public ITextWriter standardOut {
			get { return host.stdout; }
		}
		
		public string newLine {
			get { return Environment.NewLine; }
		}
		
		public bool supportsCodePage()
		{
			return false;
		}
		
		public FileInformation readFile(string path, int codepage)
		{
			return host.readFile(path, codepage);
		}
		
		public void writeFile(string path, string contents, bool writeByteOrderMark)
		{
			host.writeFile(path, contents, writeByteOrderMark);
		}
		
		public void deleteFile(string path)
		{
			host.deleteFile(path);
		}
		
		public bool fileExists(string path)
		{
			return host.fileExists(path);
		}
		
		public bool directoryExists(string path)
		{
			return host.directoryExists(path);
		}
		
		public string[] listFiles(string path, object re, object options)
		{
			return host.dir(path, re, options);
		}
		
		public string currentDirectory()
		{
			return host.getExecutingFilePath();
		}
	}
}
