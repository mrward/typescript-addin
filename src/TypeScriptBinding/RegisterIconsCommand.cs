// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Reflection;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop;

namespace ICSharpCode.TypeScriptBinding
{
	public class RegisterIconsCommand : AbstractCommand
	{
		public override void Run()
		{
			Assembly assembly = typeof(RegisterIconsCommand).Assembly;
			ResourceService.RegisterImages("ICSharpCode.TypeScriptBinding.Resources.ImageResources", assembly);
		}
	}
}
