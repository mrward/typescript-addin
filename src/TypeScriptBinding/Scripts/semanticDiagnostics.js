ls.refresh(true);
var diagnostics = ls.getSemanticDiagnostics(host.fileName);
host.updateSemanticDiagnosticsResult(diagnostics);