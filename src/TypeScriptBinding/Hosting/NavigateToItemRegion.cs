using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

using TypeScriptLanguageService;

namespace ICSharpCode.TypeScriptBinding
{
    public class NavigateToItemRegion
    {
        NavigateToItem navItem = null;

        public NavigateToItemRegion(NavigateToItem navItem)
        {
            this.navItem = navItem;
        }

        internal int minChar {
            get { return navItem.textSpan.start; }
        }

        internal int limChar {
            get { return minChar + length; }
        }

        internal int length {
            get { return navItem.textSpan.length; }
        }

        public DomRegion ToRegion(IDocument document)
        {
            TextLocation start = document.GetLocation(minChar);
            TextLocation end = document.GetLocation(limChar);
            return new DomRegion(start, end);
        }

        public DomRegion ToRegionStartingFromOpeningCurlyBrace(IDocument document)
        {
            int startOffset = GetOpeningCurlyBraceOffsetForRegion(document);
            TextLocation start = document.GetLocation(startOffset);
            TextLocation end = document.GetLocation(limChar);
            return new DomRegion(start, end);
        }

        int GetOpeningCurlyBraceOffsetForRegion(IDocument document)
        {
            int offset = minChar;
            while (offset < limChar) {
                if (document.GetCharAt(offset) == '{') {
                    return offset - 1;
                }
                ++offset;
            }

            return minChar;
        }

        public bool HasContainer()
        {
            return !String.IsNullOrEmpty(navItem.containerName);
        }

        public string GetFullName()
        {
            if (HasContainer()) {
                return String.Format("{0}.{1}", navItem.containerName, navItem.name);
            }
            return navItem.name;
        }

        public string GetContainerParentName()
        {
            int dotPosition = navItem.containerName.IndexOf('.');
            if (dotPosition > 0) {
                return navItem.containerName.Substring(0, dotPosition);
            }
            return null;
        }
    }
}