// 
// TypeScriptOptions.cs
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
using ICSharpCode.TypeScriptBinding.Hosting;
using TypeScriptLanguageService;

namespace ICSharpCode.TypeScriptBinding
{
	public class TypeScriptOptions : ICompilerOptions
	{
        ICompilerOptions cOpt = null;


        public TypeScriptOptions()
        {
            this.cOpt = new CompilerOptions();
            this.cOpt.allowNonTsExtensions=false;
            this.cOpt.charset="UTF-8";
            this.cOpt.codepage=0;
            this.cOpt.declaration=false;
            this.cOpt.diagnostics=false;
            this.cOpt.emitBOM=false;
            this.cOpt.help=false;
            this.cOpt.listFiles=false;
            this.cOpt.locale="";
            this.cOpt.mapRoot="";
            this.cOpt.module=0;
            this.cOpt.noEmit=false;
            this.cOpt.noEmitOnError=false;
            this.cOpt.noErrorTruncation=false;
            this.cOpt.noImplicitAny=false;
            this.cOpt.noLib=false;
            this.cOpt.noLibCheck=false;
            this.cOpt.noResolve=false;
            this.cOpt.tsOut="";
            this.cOpt.outDir="";
            this.cOpt.preserveConstEnums=false;
            this.cOpt.project="";
            this.cOpt.removeComments=false;
            this.cOpt.sourceMap=false;
            this.cOpt.sourceRoot="";
            this.cOpt.suppressImplicitAnyIndexErrors=false;
            this.cOpt.target=ScriptTarget.ES6;
            this.cOpt.version=false;
            this.cOpt.watch=false;
            this.cOpt.stripInternal=false;
        }

        public TypeScriptOptions(ICompilerOptions cOpt)
		{
            this.cOpt = cOpt;
		}
		

        #region ICompilerOptions
        public bool allowNonTsExtensions { get{ return this.cOpt.allowNonTsExtensions;}  set{this.cOpt.allowNonTsExtensions = value;}}
        public string charset { get{ return this.cOpt.charset;}  set{this.cOpt.charset = value;}}
        public int codepage { get{ return this.cOpt.codepage;}  set{this.cOpt.codepage = value;}}
        public bool declaration { get{ return this.cOpt.declaration;}  set{this.cOpt.declaration = value;}}
        public bool diagnostics { get{ return this.cOpt.diagnostics;}  set{this.cOpt.diagnostics = value;}}
        public bool emitBOM { get{ return this.cOpt.emitBOM;}  set{this.cOpt.emitBOM = value;}}
        public bool help { get{ return this.cOpt.help;}  set{this.cOpt.help = value;}}
        public bool listFiles { get{ return this.cOpt.listFiles;}  set{this.cOpt.listFiles = value;}}
        public string locale { get{ return this.cOpt.locale;}  set{this.cOpt.locale = value;}}
        public string mapRoot { get{ return this.cOpt.mapRoot;}  set{this.cOpt.mapRoot = value;}}
        public ModuleKind module { get{ return this.cOpt.module;}  set{this.cOpt.module = value;}}
        public bool noEmit { get{ return this.cOpt.noEmit;}  set{this.cOpt.noEmit = value;}}
        public bool noEmitOnError { get{ return this.cOpt.noEmitOnError;}  set{this.cOpt.noEmitOnError = value;}}
        public bool noErrorTruncation { get{ return this.cOpt.noErrorTruncation;}  set{this.cOpt.noErrorTruncation = value;}}
        public bool noImplicitAny { get{ return this.cOpt.noImplicitAny;}  set{this.cOpt.noImplicitAny = value;}}
        public bool noLib { get{ return this.cOpt.noLib;}  set{this.cOpt.noLib = value;}}
        public bool noLibCheck { get{ return this.cOpt.noLibCheck;}  set{this.cOpt.noLibCheck = value;}}
        public bool noResolve { get{ return this.cOpt.noResolve;}  set{this.cOpt.noResolve = value;}}
        public string tsOut { get{ return this.cOpt.tsOut;}  set{this.cOpt.tsOut = value;}}
        public string outDir { get{ return this.cOpt.outDir;}  set{this.cOpt.outDir = value;}}
        public bool preserveConstEnums { get{ return this.cOpt.preserveConstEnums;}  set{this.cOpt.preserveConstEnums = value;}}
        public string project { get{ return this.cOpt.project;}  set{this.cOpt.project = value;}}
        public bool removeComments { get{ return this.cOpt.removeComments;}  set{this.cOpt.removeComments = value;}}
        public bool sourceMap { get{ return this.cOpt.sourceMap;}  set{this.cOpt.sourceMap = value;}}
        public string sourceRoot { get{ return this.cOpt.sourceRoot;}  set{this.cOpt.sourceRoot = value;}}
        public bool suppressImplicitAnyIndexErrors { get{ return this.cOpt.suppressImplicitAnyIndexErrors;}  set{this.cOpt.suppressImplicitAnyIndexErrors = value;}}
        public ScriptTarget target { get{ return this.cOpt.target;}  set{this.cOpt.target = value;}}
        public bool version { get{ return this.cOpt.version;}  set{this.cOpt.version = value;}}
        public bool watch { get{ return this.cOpt.watch;}  set{this.cOpt.watch = value;}}
        public bool stripInternal { get{ return this.cOpt.stripInternal;}  set{this.cOpt.stripInternal = value;}}
        #endregion


        public ScriptTarget GetScriptTarget()
        {
            return cOpt.target;
        }

        public ModuleKind GetModuleTarget()
        {
            return cOpt.module;
        }

        public string GetOutputFileFullPath()
        {
            return cOpt.tsOut;
        }

        public string GetOutputDirectoryFullPath()
        {
            return cOpt.outDir;
        }

	}
}
