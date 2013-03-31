// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class StandardOutputTextWriter : ITextWriter
	{
		public void Write(string s)
		{
			Console.WriteLine("TextWriter.Write: {0}", s);
		}
		
		public void WriteLine(string s)
		{
			Console.WriteLine("TextWriter.WriteLine: {0}", s);
		}
		
		public void Close()
		{
			Console.WriteLine("TextWriter.Close");
		}
	}
}
