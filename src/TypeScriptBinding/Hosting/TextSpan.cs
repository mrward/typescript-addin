// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class TextSpan
	{
		public TextSpan(int start, int length)
		{
			this.start = start;
			this.length = length;
		}
		
		public int start { get; set; }
		public int length { get; set; }
	}
}
