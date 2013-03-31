
ls.refresh(true);
var items = ls.getCompletionsAtPosition(host.fileName, host.position, true);
host.updateCompletionInfoAtCurrentPosition(items);