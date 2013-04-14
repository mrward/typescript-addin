// 
// NavigateToItem.cs
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
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Editor;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class NavigateToItem
	{
		public NavigateToItem(string navigateToItem)
		{
			ParseNavigateToItem(navigateToItem);
		}
		
		void ParseNavigateToItem(string navigateToItem)
		{
			string[] parts = navigateToItem.Split('\t');
			if (parts.Length == 9) {
				name = parts[0];
				kind = parts[1];
				kindModifiers = parts[2];
				containerName = parts[3];
				containerKind = parts[4];
				matchKind = parts[5];
				unitIndex = ParseInt(parts[6]);
				minChar = ParseInt(parts[7]);
				limChar = ParseInt(parts[8]);
			}
		}
		
		int ParseInt(string text)
		{
			return Int32.Parse(text);
		}
		
		public string name { get; set; }
		public string kind { get; set; }
		public string kindModifiers { get; set; }
		public string matchKind { get; set; }
		public int unitIndex { get; set; }
		public int minChar { get; set; }
		public int limChar { get; set; }
		public string containerName { get; set; }
		public string containerKind { get; set; }
		
		internal int length {
			get { return limChar - minChar; }
		}
		
		public DomRegion ToRegion(IDocument document)
		{
			Location start = document.OffsetToPosition(minChar);
			Location end = document.OffsetToPosition(limChar);
			return DomRegion.FromLocation(start, end);
		}
	}
}
