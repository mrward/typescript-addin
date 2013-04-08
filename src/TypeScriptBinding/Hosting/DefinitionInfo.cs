// 
// TypeScriptContext.cs
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

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class DefinitionInfo
	{
		public DefinitionInfo(string definition)
		{
			ParseDefinition(definition);
		}
		
		internal bool IsValid { get; private set; }
		
		void ParseDefinition(string definition)
		{
			string[] parts = definition.Split('\t');
			if (parts.Length == 7) {
				unitIndex = ParseInt(parts[0]);
				minChar = ParseInt(parts[1]);
				limChar = ParseInt(parts[2]);
				kind = parts[3];
				name = parts[4];
				containerKind = parts[5];
				containerName = parts[6];
				
				IsValid = true;
			}
		}
		
		int ParseInt(string text)
		{
			return Int32.Parse(text);
		}
		
		public int unitIndex { get; set; }
		public int minChar { get; set; }
		public int limChar { get; set; }
		public string kind { get; set; }
		public string name { get; set; }
		public string containerKind { get; set; }
		public string containerName { get; set; }
	}
}
