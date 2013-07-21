// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class LanguageServicesDiagnostics : ILanguageServicesDiagnostics
	{
		ILogger logger;
		
		public LanguageServicesDiagnostics(ILogger logger)
		{
			this.logger = logger;
		}
		
		public void log(string content)
		{
			logger.log("Diagnostics: " + content);
		}
	}
}
