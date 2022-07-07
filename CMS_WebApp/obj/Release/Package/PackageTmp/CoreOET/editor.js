function InsertImg(url, editorID)
{
    var ed = tinyMCE.get(editorID);                // get editor instance
    var range = ed.selection.getRng();                  // get range
    var newNode = ed.getDoc().createElement("img");  // create img node
    newNode.src = url;                           // add src attribute
    range.insertNode(newNode);                          // insert Node
}