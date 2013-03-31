// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.IO;
using System.Text;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TextFileWriter : ITextWriter
	{
		StreamWriter writer;
		
		public TextFileWriter(string path, bool utf8)
		{
			writer = new StreamWriter(path, false, GetEncoding(utf8));
		}
		
		Encoding GetEncoding(bool utf8)
		{
			if (utf8)
				return Encoding.UTF8;
			
			return Encoding.ASCII;
		}
		
		public void Write(string s)
		{
			writer.Write(s);
		}
		
		public void WriteLine(string s)
		{
			writer.WriteLine(s);
		}
		
		public void Close()
		{
			writer.Close();
		}
	}
}
