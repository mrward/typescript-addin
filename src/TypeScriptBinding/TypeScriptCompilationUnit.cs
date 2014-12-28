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
		
		public void AddNavigation(NavigationBarItem[] navigation, ITextBuffer textBuffer)
		{
			IDocument document = DocumentUtilitites.LoadReadOnlyDocumentFromBuffer(textBuffer);
			AddNavigationInfo(navigation, document);
		}
		
		public void AddNavigationInfo(NavigationBarItem[] navigation, IDocument document)
		{
			foreach (NavigationBarItem item in navigation) {
				switch (item.kind) {
					case "class":
						AddClass(item, document);
						break;
					case "interface":
						AddInterface(item, document);
						break;
					case "module":
						AddModule(item, document);
						break;
				}
			}
		}
		
		DefaultClass AddClass(NavigationBarItem item, IDocument document)
		{
			var defaultClass = new DefaultClass(this, item.text);
			defaultClass.BodyRegion = item.ToRegionStartingFromOpeningCurlyBrace(document);
			defaultClass.Region = defaultClass.BodyRegion;
			
			IClass parentClass = FindParentClass(defaultClass);
			if (parentClass != null) {
				defaultClass.FullyQualifiedName = parentClass.FullyQualifiedName + "." + defaultClass.FullyQualifiedName;
				parentClass.InnerClasses.Add(defaultClass);
			} else {
				Classes.Add(defaultClass);
			}
			AddMethods(defaultClass, item.childItems, document);
			return defaultClass;
		}
		
		void AddMethods(IClass parent, NavigationBarItem[] childItems, IDocument document)
		{
			foreach (NavigationBarItem item in childItems) {
				switch (item.kind) {
					case "method":
					case "constructor":
						AddMethod(parent, item, document);
						break;
				}
			}
		}
		
		void AddInterface(NavigationBarItem item, IDocument document)
		{
			DefaultClass c = AddClass(item, document);
			c.ClassType = ClassType.Interface;
		}
		
		void AddModule(NavigationBarItem item, IDocument document)
		{
			DefaultClass c = AddClass(item, document);
			c.ClassType = ClassType.Module;
		}
		
		void AddMethod(IClass parent, NavigationBarItem item, IDocument document)
		{
			var method = new DefaultMethod(parent, item.text);
			UpdateMethodRegions(method, item, document);
			parent.Methods.Add(method);
		}
		
		void UpdateMethodRegions(DefaultMethod method, NavigationBarItem item, IDocument document)
		{
			DomRegion region = item.ToRegionStartingFromOpeningCurlyBrace(document);
			method.Region = new DomRegion(
				region.BeginLine,
				region.BeginColumn,
				region.BeginLine,
				region.BeginColumn);
			method.BodyRegion = region;
		}
		
		IClass FindParentClass(IClass c)
		{
			return Classes.FirstOrDefault(parent => IsInside(c.BodyRegion, parent.BodyRegion));
		}
		
		bool IsInside(DomRegion childBodyRegion, DomRegion parentBodyRegion)
		{
			return (childBodyRegion.BeginLine >= parentBodyRegion.BeginLine) &&
				(childBodyRegion.EndLine <= parentBodyRegion.EndLine);
		}
	}
}
