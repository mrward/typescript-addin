// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class CompletionEntryDetails
	{
		public CompletionEntryDetails()
		{
			this.name = "";
			this.type = "";
			this.kind = "";
			this.fullSymbolName = "";
			this.docComment = "";
		}
		
		public string name { get; set; }
		public string type { get; set; }
		public string kind { get; set; }            // see ScriptElementKind
		public string kindModifiers { get; set; }   // see ScriptElementKindModifier, comma separated
		public string fullSymbolName { get; set; }
		public string docComment { get; set; }
		
		public override string ToString()
		{
			return string.Format("[CompletionEntryDetails Name={0}, Type={1}, Kind={2}, DocComment={3}]", name, type, kind, docComment);
		}
	}
}
