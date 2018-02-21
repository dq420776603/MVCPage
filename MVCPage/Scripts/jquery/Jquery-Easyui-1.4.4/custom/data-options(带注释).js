
var gridOptionsObj = {
    fit: true,
    rownumbers: true,
    singleSelect: true,
    autoRowHeight: false,
    pagination: true,
    pageSize: 10
};
//jQuery.extend用来深拷贝属性
var gridOptionsObjPageSize20 = jQuery.extend({}, gridOptionsObj);
gridOptionsObjPageSize20.pageSize = 20;