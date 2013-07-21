
ls.refresh(true);
var structure = ls.getScriptLexicalStructure(host.fileName);
host.updateLexicalStructure(structure);