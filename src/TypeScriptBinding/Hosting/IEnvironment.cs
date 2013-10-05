// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public interface IEnvironment
	{
		bool supportsCodePage();
		FileInformation readFile(string path, int codepage);
		void writeFile(string path, string contents, bool writeByteOrderMark);
		void deleteFile(string path);
		bool fileExists(string path);
		bool directoryExists(string path);
		string[] listFiles(string path, object re, object options);

		string[] arguments { get; }
		ITextWriter standardOut { get; }

		string currentDirectory();
		string newLine { get; }
	}
}
