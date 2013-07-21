// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class FormalTypeParameterInfo
	{
		public string name { get; set; }
		public string docComment { get; set; }
		public int minChar { get; set; }
		public int limChar { get; set; }
	}
}
