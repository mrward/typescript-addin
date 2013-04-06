
ls.refresh(true);
var signature = ls.getSignatureAtPosition(host.fileName, host.position);
host.updateSignatureAtPosition(signature);