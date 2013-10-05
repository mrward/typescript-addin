// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class FileInformation
	{
		public FileInformation(string contents, ByteOrderMark byteOrderMark) {
			this.contents = contents;
			this.byteOrderMark = byteOrderMark;
		}
		
		public string contents { get; set; }
		public ByteOrderMark byteOrderMark { get; set; }
	}
}
