ls.refresh(true);
var hostAdapter = new TypeScript.Services.LanguageServiceShimHostAdapter(host);
var compiler = new TypeScript.Services.LanguageServiceCompiler(hostAdapter);
var diagnostics = compiler.getSemanticDiagnostics(host.fileName);
var diagnosticsResult = JSON.stringify({result: diagnostics});
host.updateSemanticDiagnosticsResult(diagnosticsResult);