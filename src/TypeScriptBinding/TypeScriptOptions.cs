// 
// TypeScriptOptions.cs
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
using MonoDevelop.Core;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptOptions
	{
		Properties properties;
		
		public TypeScriptOptions()
			: this(PropertyService.Get("TypeScriptBinding.Options", new Properties()))
		{
		}
		
		public TypeScriptOptions(Properties properties)
		{
			this.properties = properties;
		}
		
		public bool CompileOnSave {
			get { return properties.Get("CompileOnSave", true); }
			set { properties.Set("CompileOnSave", value); }
		}
		
		public bool CompileOnBuild {
			get { return properties.Get("CompileOnBuild", false); }
			set { properties.Set("CompileOnBuild", value); }
		}
		
		public bool IncludeComments {
			get { return properties.Get("IncludeComments", false); }
			set { properties.Set("IncludeComments", value); }
		}
		
		public bool GenerateSourceMap {
			get { return properties.Get("GenerateSourceMap", false); }
			set { properties.Set("GenerateSourceMap", value); }
		}
		
		public string ModuleKind {
			get { return properties.Get("ModuleKind", "AMD"); }
			set { properties.Set("ModuleKind", value); }
		}
		
		public string EcmaScriptTargetVersion {
			get { return properties.Get("EcmaScriptTargetVersion", "ES3"); }
			set { properties.Set("EcmaScriptTargetVersion", value); }
		}
	}
}
