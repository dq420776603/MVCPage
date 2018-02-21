
function queryParamsEr(params) {
    return params;
}
var bootstrapTableOption = {
    method: 'post',                      //请求方式（*）
    striped: false,                      //是否显示行间隔色,有条纹，选中变色看不出来
    cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
    pagination: true,                   //是否显示分页（*）
    sortable: true,                    //是否启用排序
    queryParams: queryParamsEr,         //传递参数（*）
    sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
    clickToSelect: true                //是否启用点击选中行
};
var pageOption = {
    pageNumber: 1,                      //初始化加载第一页，默认第一页
    pageSize: 10,                       //每页的记录行数（*）
    pageList: [10, 25, 50, 100]        //可供选择的每页的行数（*）
};
var showControl = {
    search: true,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
    strictSearch: true,
    showColumns: true,                  //是否显示所有的列
    showRefresh: true,                   //是否显示刷新按钮
    showToggle: true                   //是否显示详细视图和列表视图的切换按钮
};
var showView = {
    cardView: false,                    //是否显示详细视图
    detailView: false                   //是否显示父子表
};
var optionNumbers = {
    minimumCountColumns: 2              //最少允许的列数
};
//关于分页
var bootstrapTableAqua = jQuery.extend({},bootstrapTableOption, pageOption);
