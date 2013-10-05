// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using Newtonsoft.Json;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class ScriptSnapshotShim : IScriptSnapshotShim
	{
		ILogger logger;
		Script script;
		
		public ScriptSnapshotShim(ILogger logger, Script script)
		{
			this.logger = logger;
			this.script = script;
		}
		
		public string getText(int start, int end)
		{
			return script.Source.Substring(start, end - start);
		}
		
		public int getLength()
		{
			return script.Source.Length;
		}
		
		public string getLineStartPositions()
		{
			Log("ScriptSnapshotShim.getLineStartPositions");
			
			int[] positions = script.GetLineStartPositions();
			string json = JsonConvert.SerializeObject(positions);
			
			//Log("ScriptSnapshotShim.getLineStartPositions: {0}", json);
			
			return json;
		}
		
		public string getTextChangeRangeSinceVersion(int scriptVersion)
		{
			Log("ScriptSnapshotShim.getTextChangeRangeSinceVersion: version={0}", scriptVersion);
			if (script.Version == scriptVersion)
				return null;
			
			TextChangeRange textChangeRange = script.GetTextChangeRangeSinceVersion(scriptVersion);
			string json = JsonConvert.SerializeObject(textChangeRange);
			
			Log("ScriptSnapshotShim.getTextChangeRangeSinceVersion: json: {0}", json);
			
			return json;
		}
		
		void Log(string format, params object[] args)
		{
			logger.log(String.Format(format, args));
		}
	}
}
