//Enumerador request type
EResponse = {
    Error: 0,
    Success: 1
}

//Encapsulated ajax methods
var Request = (function () {
    function showLoading() {
        $(".spinner").removeClass("hidden");
    };

    function hideLoading() {
        $(".spinner").addClass("hidden");
    };

    return {
        post: function (params) {
            return $.ajax({
                type: "POST",
                cache: false,
                dataType: "json",
                url: params.url,
                data: params.data,
                beforeSend: function () {
                    if (params.ignoreLoading === true)
                        return;

                    showLoading();
                },
                complete: function () {
                    hideLoading();
                },
                success: params.success,
                error: function (data) { AjaxUnhandlerError(data) }
            });
        },

        get: function (params) {
            return $.ajax({
                type: "GET",
                cache: false,
                dataType: "json",
                url: params.url,
                data: params.data,
                beforeSend: function () {
                    if (params.ignoreLoading === true)
                        return;

                    showLoading();
                },
                complete: function () {
                    hideLoading();
                },
                success: params.success,
                error: function (data) { AjaxUnhandlerError(data) }
            });
        }
    };
})();

AjaxUnhandlerError = function (data) {
    toastr.error("Erro ao executar requisição ao servidor");
    console.log("Erro ao executar requisição ao servidor");
    console.log(data.Message);
}

//Toastr configurations
toastr.options = {
    "preventDuplicates": true,
    "positionClass": "toast-top-right"
};

////Declare loading spinner methods to global scope
//$(function () {
//    $.loading_spinner_on = function () {
//        alert("loading");
//    };

//    $.loading_spinner_off = function () {
//        alert("unload");
//    };
//});