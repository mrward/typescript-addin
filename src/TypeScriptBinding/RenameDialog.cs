//
// RenameDialog.cs
//
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2015 Matthew Ward
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using MonoDevelop.Ide;

namespace ICSharpCode.TypeScriptBinding
{
	public partial class RenameDialog : Gtk.Dialog
	{
		string originalNewName;
		string newName;

		public RenameDialog()
		{
			TransientFor = IdeApp.Workbench.RootWindow;

			this.Build ();

			Name = "wizard_dialog";
			WindowPosition = Gtk.WindowPosition.CenterOnParent;

			okButton.GrabDefault();
			newNameEntry.GrabFocus();
			newNameEntry.Changed += NewNameChanged;
		}

		public bool HasNewName { get; private set; }

		public string NewName {
			get { return newName; }
			set {
				originalNewName = value;
				newNameEntry.Text = value;
				newNameEntry.SelectRegion(0, -1);
			}
		}

		void NewNameChanged(object sender, EventArgs e)
		{
			newName = newNameEntry.Text;
			HasNewName = HasNewNameChanged ();
			okButton.Sensitive = HasNewName;
		}

		bool HasNewNameChanged()
		{
			return (newName.Length > 0) && (newName != originalNewName);
		}
	}
}

