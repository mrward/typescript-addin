// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class FileInformation
	{
		private string fileContents;
		private ByteOrderMark byteOrder;
		
		public FileInformation(string contents, ByteOrderMark byteOrderMark) {
			this.fileContents = contents;
			this.byteOrder = byteOrderMark;
		}
		
		public string contents() {
			return this.fileContents;
		}
		
		public ByteOrderMark byteOrderMark() {
			return this.byteOrder;
		}
	}
}
