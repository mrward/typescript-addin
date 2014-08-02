ls.refresh(true);
var hostAdapter = new TypeScript.Services.LanguageServiceShimHostAdapter(host);
var compiler = new TypeScript.Services.LanguageServiceCompiler(hostAdapter);
var emitResult = compiler.emit(host.fileName, host.ResolvePath);
var result = JSON.stringify({result: emitResult});
if (compiler.canEmitDeclarations(host.fileName)) {
	compiler.emitDeclarations(host.fileName, host.ResolvePath);
}
host.updateCompilerResult(result);