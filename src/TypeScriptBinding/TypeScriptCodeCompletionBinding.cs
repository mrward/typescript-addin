// 
// TypeScriptCodeCompletionBinding.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013 Matthew Ward
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.SharpDevelop.Editor.CodeCompletion;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptCodeCompletionBinding : ICodeCompletionBinding
	{
		static TypeScriptContext context = new TypeScriptContext();
		
		public TypeScriptCodeCompletionBinding()
		{
		}
		
		public CodeCompletionKeyPressResult HandleKeyPress(ITextEditor editor, char ch)
		{
			if (ch == '.') {
				editor.Document.Insert(editor.Caret.Offset, ch.ToString());
				ShowCompletion(editor, true);
				return CodeCompletionKeyPressResult.EatKey;
			}
			return CodeCompletionKeyPressResult.None;
		}
		
		bool ShowCompletion(ITextEditor editor, bool memberCompletion)
		{
			context.UpdateFile(editor.FileName, editor.Document.Text);
				
			var completionProvider = new TypeScriptCompletionItemProvider(context);
			return completionProvider.ShowCompletion(editor, memberCompletion);
		}
		
		public bool CtrlSpace(ITextEditor editor)
		{
			return ShowCompletion(editor, false);
		}
	}
}
