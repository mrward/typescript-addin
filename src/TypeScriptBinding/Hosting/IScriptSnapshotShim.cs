// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.TypeScriptBinding.Hosting
{
	public interface IScriptSnapshotShim
	{
		string getText(int start, int end);
		int getLength();
		string getLineStartPositions();
		string getTextChangeRangeSinceVersion(int scriptVersion);
	}
}
