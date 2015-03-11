using System;
using TypeScriptLanguageService;

namespace ICSharpCode.TypeScriptBinding
{
    public class V8TypescriptProvider
    {
        static TypeScriptLanguageServices tslService = null;

        public V8TypescriptProvider(ILanguageServiceHost host, string typescript)
        {
            tslService = new TypeScriptLanguageServices(host, typescript);
        }

        public static TypeScriptLanguageServices TService(){
            return tslService;
        }
    }
}

