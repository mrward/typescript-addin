// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using ICSharpCode.TypeScriptBinding.Hosting;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptUnresolvedFile : IUnresolvedFile
	{
		List<IUnresolvedTypeDefinition> typeDefinitions = new List<IUnresolvedTypeDefinition>();
		List<Error> errors = new List<Error>();
		
		public TypeScriptUnresolvedFile(FileName fileName)
		{
			FileName = fileName;
		}
		
		public string FileName { get; private set; }
		
		public DateTime? LastWriteTime { get; set; }
		
		public IList<IUnresolvedTypeDefinition> TopLevelTypeDefinitions {
			get { return typeDefinitions; }
		}
		
		public IList<IUnresolvedAttribute> AssemblyAttributes {
			get { return EmptyList<IUnresolvedAttribute>.Instance; }
		}
		
		public IList<IUnresolvedAttribute> ModuleAttributes {
			get { return EmptyList<IUnresolvedAttribute>.Instance; }
		}
		
		public IList<Error> Errors {
			get { return errors; }
		}
		
		public IUnresolvedTypeDefinition GetTopLevelTypeDefinition(TextLocation location)
		{
			return null;
		}
		
		public IUnresolvedTypeDefinition GetInnermostTypeDefinition(TextLocation location)
		{
			return null;
		}
		
		public IUnresolvedMember GetMember(TextLocation location)
		{
			return null;
		}
		
		public void AddNavigation(NavigateToItem[] navigation, ITextSource textSource)
		{
			IDocument document = new TextDocument(textSource);
			AddNavigationInfo(navigation, document);
		}
		
		public void AddNavigationInfo(NavigateToItem[] navigation, IDocument document)
		{
			foreach (NavigateToItem item in navigation) {
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
					case "method":
					case "constructor":
						AddMethod(item, document);
						break;
				}
			}
		}
		
		DefaultUnresolvedTypeDefinition AddClass(NavigateToItem item, IDocument document)
		{
			var defaultClass = new DefaultUnresolvedTypeDefinition(item.GetFullName()) {
				UnresolvedFile = this
			};
			defaultClass.BodyRegion = item.ToRegionStartingFromOpeningCurlyBrace(document);
			defaultClass.Region = defaultClass.BodyRegion;
			
			if (item.HasContainer()) {
				DefaultUnresolvedTypeDefinition parentClass = FindParentClass(item);
				parentClass.NestedTypes.Add(defaultClass);
			} else {
				typeDefinitions.Add(defaultClass);
			}
			return defaultClass;
		}
		
		void AddInterface(NavigateToItem item, IDocument document)
		{
			DefaultUnresolvedTypeDefinition c = AddClass(item, document);
			c.Kind = TypeKind.Interface;
		}
		
		void AddModule(NavigateToItem item, IDocument document)
		{
			DefaultUnresolvedTypeDefinition c = AddClass(item, document);
			c.Kind = TypeKind.Module;
		}
		
		void AddMethod(NavigateToItem item, IDocument document)
		{
			DefaultUnresolvedTypeDefinition c = FindParentClass(item);
			var method = new DefaultUnresolvedMethod(c, item.name);
			UpdateMethodRegions(method, item, document);
			c.Members.Add(method);
		}
		
		void UpdateMethodRegions(DefaultUnresolvedMethod method, NavigateToItem item, IDocument document)
		{
			DomRegion region = item.ToRegionStartingFromOpeningCurlyBrace(document);
			method.Region = new DomRegion(
				region.BeginLine,
				region.BeginColumn,
				region.BeginLine,
				region.BeginColumn);
			method.BodyRegion = region;
		}
		
		DefaultUnresolvedTypeDefinition FindParentClass(NavigateToItem item)
		{
			string containerParentName = item.GetContainerParentName();
			if (containerParentName != null) {
				DefaultUnresolvedTypeDefinition parentClass = FindClass(containerParentName);
				return FindClass(parentClass.NestedTypes, item.containerName);
			} else {
				return FindClass(item.containerName);
			}
		}
		
		DefaultUnresolvedTypeDefinition FindClass(string name)
		{
			return FindClass(typeDefinitions, name);
		}
		
		DefaultUnresolvedTypeDefinition FindClass(IList<IUnresolvedTypeDefinition> classes, string name)
		{
			return classes
				.Where(c => c.FullName == name)
				.Select(c => (DefaultUnresolvedTypeDefinition)c)
				.FirstOrDefault();
		}
	}
}
