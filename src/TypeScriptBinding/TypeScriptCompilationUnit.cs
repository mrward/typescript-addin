// 
// TypeScriptCompilationUnit.cs
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
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptCompilationUnit : DefaultCompilationUnit
	{
		public TypeScriptCompilationUnit(IProjectContent projectContent)
			: base(projectContent)
		{
		}
		
		public void AddNavigationInfo(NavigationInfo navigationInfo, ITextBuffer textBuffer)
		{
			IDocument document = DocumentUtilitites.LoadReadOnlyDocumentFromBuffer(textBuffer);
			AddNavigationInfo(navigationInfo, document);
		}
		
		public void AddNavigationInfo(NavigationInfo navigationInfo, IDocument document)
		{
			foreach (NavigateToItem item in navigationInfo.items) {
				switch (item.kind) {
					case "class":
						AddClass(item, document);
						break;
					case "method":
						AddMethod(item, document);
						break;
				}
			}
		}
		
		void AddClass(NavigateToItem item, IDocument document)
		{
			var defaultClass = new DefaultClass(this, item.name);
			defaultClass.BodyRegion = item.ToRegion(document);
			Classes.Add(defaultClass);
		}
		
		void AddMethod(NavigateToItem item, IDocument document)
		{
			IClass c = FindClass(item);
			var method = new DefaultMethod(c, item.name);
			UpdateMethodRegions(method, item, document);
			c.Methods.Add(method);
		}
		
		void UpdateMethodRegions(DefaultMethod method, NavigateToItem item, IDocument document)
		{
			DomRegion region = item.ToRegion(document);
			method.Region = new DomRegion(
				region.BeginLine,
				region.BeginColumn,
				region.BeginLine,
				region.BeginColumn);
			method.BodyRegion = region;
		}
		
		IClass FindClass(NavigateToItem item)
		{
			return Classes
				.Where(c => c.Name == item.containerName)
				.FirstOrDefault();
		}
	}
}
