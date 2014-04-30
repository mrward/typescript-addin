// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding
{
	public class DisplayValue
	{
		public DisplayValue(string id, string value)
		{
			Id = id;
			Value = value;
		}
		
		public string Id { get; private set; }
		public string Value { get; private set; }
		
		public override string ToString()
		{
			return Value;
		}
	}
}
