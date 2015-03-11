// 
// ScriptSnapshotShim.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013-2014 Matthew Ward
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
using Newtonsoft.Json;
using TypeScriptLanguageService;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public class ScriptSnapshot : IScriptSnapshot
	{
		ILogger logger;
		Script script;
		
		public ScriptSnapshot(ILogger logger, Script script)
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
		
        public int[] getLineStartPositions()
		{
			Log("ScriptSnapshotShim.getLineStartPositions");
			
			int[] positions = script.GetLineStartPositions();
			
            return positions;
		}
		
        public TextChangeRange getChangeRange(IScriptSnapshot oldSnapshot)
		{
			Log("ScriptSnapshotShim.getChangeRange");
			
			var old = oldSnapshot as ScriptSnapshot;
			if ((old != null) && (script.Version == old.script.Version)) {
				return null;
			}
			
			TextChangeRange textChangeRange = script.GetTextChangeRange(oldSnapshot);
		
            return textChangeRange;
		}
		
		void Log(string format, params object[] args)
		{
			logger.log(String.Format(format, args));
		}
	}
}
