// Write your JavaScript code.
function isJsonFormat(data) {
    return typeof (data) == "object"
        && Object.prototype.toString.call(data).toLowerCase() == "[object object]"
        && !data.length;     
}