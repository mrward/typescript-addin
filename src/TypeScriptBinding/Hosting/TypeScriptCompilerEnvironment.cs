// 
// TypeScriptCompilerEnvironment.cs
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
