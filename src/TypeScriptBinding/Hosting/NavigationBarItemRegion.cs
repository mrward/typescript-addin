using System;

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

using TypeScriptLanguageService;

namespace ICSharpCode.TypeScriptBinding
{
    public class NavigationBarItemRegion
    {
        NavigationBarItem navItem = null;

        public NavigationBarItemRegion(NavigationBarItem navItem)
        {
            this.navItem = navItem;
        }

        internal int minChar {
            get {
                if (HasSpans) {
                    return navItem.spans[0].start;
                }
                return 0;
            }
        }

        internal bool HasSpans {
            get { return (navItem.spans != null) && (navItem.spans.Length > 0); }
        }

        internal int limChar {
            get {
                if (HasSpans) {
                    TextSpan span = navItem.spans[0];
                    return span.start + span.length;
                }
                return 0;
            }
        }

        internal DomRegion ToRegionStartingFromOpeningCurlyBrace(IDocument document)
        {
            int startOffset = GetOpeningCurlyBraceOffsetForRegion(document);
            TextLocation start = document.GetLocation(startOffset);
            TextLocation end = document.GetLocation(limChar-1);
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

    }
}

