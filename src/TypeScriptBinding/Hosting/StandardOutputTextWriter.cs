// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class StandardOutputTextWriter : ITextWriter
	{
		public void Write(string s)
		{
//			BuildMessageView.AppendText(s);
		}
		
//		MessageViewCategory BuildMessageView {
//			get { return TaskService.BuildMessageViewCategory; }
//		}
		
		public void WriteLine(string s)
		{
//			BuildMessageView.AppendLine(s);
		}
		
		public void Close()
		{
		}
	}
}
