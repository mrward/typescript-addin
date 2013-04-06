// 
// FormalSignatureInfo.cs
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
using System.Linq;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class FormalSignatureInfo
	{
		public FormalSignatureInfo()
		{
			signatureGroup = new FormalSignatureItemInfo[0];
		}
		
		public string name { get; set; }
		public bool isNew { get; set; }
		public string openParen { get; set; }
		public string closeParen { get; set; }
		public string docComment { get; set; }
		public FormalSignatureItemInfo[] signatureGroup { get; set; }
	}

	public class FormalSignatureItemInfo
	{
		public FormalSignatureItemInfo()
		{
			parameters = new FormalParameterInfo[0];
		}
		
		public FormalParameterInfo[] parameters { get; set; } // Array of parameters
		public string returnType { get; set; } // String representation of parameter type
		public string docComment { get; set; } // Help for the signature
		
		public override string ToString()
		{
			return String.Format(
				"({0}){1}",
				GetParameterInfo(),
				GetReturnTypeInfo());
		}
		
		string GetParameterInfo()
		{
			string[] text = parameters
				.Select(p => p.ToString())
				.ToArray();
			
			return String.Join(", ", text);
		}
		
		string GetReturnTypeInfo()
		{
			return String.Format(": {0}", returnType);
		}
	}

	public class FormalParameterInfo
	{
		public string name { get; set; }        // Parameter name
		public string type { get; set; }        // String representation of parameter type
		public bool isOptional { get; set; }    // true if parameter is optional
		public bool isVariable { get; set; }    // true if parameter is var args
		public string docComment { get; set; }  // Comments that contain help for the parameter
		
		public override string ToString()
		{
			return String.Format(
				"{0}{1}: {2}",
				name,
				GetOptionalText(),
				type);
		}
		
		string GetOptionalText()
		{
			if (isOptional) {
				return "?";
			}
			return String.Empty;
		}
	}
}
