function addMethodCodeExists() {
    jQuery.validator.addMethod("codeExists",
        function (value1, element, params) {
            var existsCode = false;
            if (params) {
                var dataId;
                if (params.getDataId)
                {
                    dataId = params.getDataId();
                }
                else if (params.idInput)
                {
                    dataId = document.getElementById(params.idInput).value;
                }
                jQuery.ajax({
                    type: "post",
                    url: params.rootUrl + "/Common/ExistsCode",
                    data: {
                        'info.Id': dataId,
                        'info.Code': value1,
                        tablaName: params.tablaName
                    },
                    async: false,
                    success: function (responseText, b, c) {
                        var a = responseText ? responseText : '';
                        existsCode = a.toLowerCase().indexOf("true") > -1;
                    }
                });
            }
            return !existsCode;
        },
        "此编码已经存在");
}
addMethodCodeExists();