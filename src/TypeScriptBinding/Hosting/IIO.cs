// 
// IIO.cs
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
	public interface IIO
	{
		FileInformation readFile(string path, int codepage);
		void writeFile(string path, string contents, bool writeByteOrderMark);
		void deleteFile(string path);
		string[] dir(string path, object re, object options);
		bool fileExists(string path);
		bool directoryExists(string path);
		void createDirectory(string path);
		string resolvePath(string path);
		string dirName(string path);
		IResolvedFile findFile(string rootPath, string partialFilePath);
		void print(string str);
		void printLine(string str);
		string[] arguments { get; set; }
		ITextWriter stderr { get; set; }
		ITextWriter stdout { get; set; }
		//watchFile(fileName: string, callback: (x:string) => void ): IFileWatcher;
		void run(string source, string filename);
		string getExecutingFilePath();
		void quit(int exitCode);
	}
}
