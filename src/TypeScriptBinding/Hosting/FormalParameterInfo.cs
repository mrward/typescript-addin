// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class FormalParameterInfo
	{
		public string name { get; set; }
		public bool isVariable { get; set; } // true if parameter is var args
		public string docComment { get; set; }
		public int minChar { get; set; }
		public int limChar { get; set; }
	}
}
