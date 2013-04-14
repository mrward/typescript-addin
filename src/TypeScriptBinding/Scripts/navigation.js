
ls.refresh(true);
var regions = ls.getOutliningRegions(host.fileName);
host.updateOutliningRegions(regions);