
ls.refresh(true);
var structure = ls.getScriptLexicalStructure(host.fileName);
var regions = ls.getOutliningRegions(host.fileName);
host.updateLexicalStructure(structure);
host.updateOutliningRegions(regions);