function addMethodUsersCodeExists() {
	jQuery.validator.addMethod("userscodeExists",
        function (value1, element, params) {
        	var usersexistsCode = false;
        	if (params) {
        		var dataId;
        		if (params.getDataId) {
        			dataId = params.getDataId();
        		}
        		else if (params.idInput) {
        			dataId = document.getElementById(params.idInput).value;
        		}
        		jQuery.ajax({
        			type: "post",
        			url: params.rootUrl + "/UsersAuxiliary/ExistsCode",
        			data: {
        				'info.Id': dataId,
        				'info.Code': value1
        			},
        			async: false,
        			success: function (responseText, b, c) {
        				var a = responseText ? responseText : '';
        				usersexistsCode = a.toLowerCase().indexOf("true") > -1;
        			}
        		});
        	}
        	return !usersexistsCode;
        },
        "此编码已经存在");
}
addMethodUsersCodeExists();