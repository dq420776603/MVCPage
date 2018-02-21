
function bootstrapDateRenderer(value, row, index) {
    //if (value.length == 22 || value.length == 21) {
    var reg = /\/Date\(([-]?\d+)\)\//gi;
    var returnVal = '';
    if (reg.test(value)) {
        var msec = value.toString().replace(reg, "$1");
        var dateVal = new Date(parseInt(msec));
        returnVal = Ext.Date.format(dateVal, "Y-m-d");
    }
    //}
    return returnVal;
}
//把form代表的参数和 分页需要的参数合起来，返回
function extendFormParams(params, formId) {
    var formSerializeObj = $(formId).serializeJSON();
    var requestObj = jQuery.extend({}, params, formSerializeObj);
    return requestObj;
}

//把form代表的参数和 分页需要的参数合起来，返回
function extendFormParamsAndFormJsonStr(params, formId) {
    return jQuery.extend({ formJson:JSON.stringify($(formId).serializeJSON()) }, extendFormParams(params, formId));
}