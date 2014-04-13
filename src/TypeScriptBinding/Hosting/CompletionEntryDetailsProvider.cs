// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using MonoDevelop.Core;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class CompletionEntryDetailsProvider
	{
		TypeScriptContext context;
		FilePath fileName;
		int offset;
		
		public CompletionEntryDetailsProvider(TypeScriptContext context, FilePath fileName, int offset)
		{
			this.context = context;
			this.fileName = fileName;
			this.offset = offset;
		}
		
		public CompletionEntryDetails GetCompletionEntryDetails(string entryName)
		{
			return context.GetCompletionEntryDetails(fileName, offset, entryName);
		}
	}
}
