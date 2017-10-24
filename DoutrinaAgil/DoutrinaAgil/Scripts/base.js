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
                    showLoading();
                },
                complete: function () {
                    hideLoading();
                },
                success: params.success,
                error: params.error
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
                    showLoading();
                },
                complete: function () {
                    hideLoading();
                },
                success: params.success,
                error: params.error
            });
        }
    };
})();

AjaxUnhandlerError = function (data) {
    alert(data);
}

//Toastr configurations
toastr.options = {
    "preventDuplicates": true,
    "positionClass": "toast-top-right"
};

//Declare loading spinner methods to global scope
$(function () {
    $.loading_spinner_on = function () {
        alert("loading");
    };

    $.loading_spinner_off = function () {
        alert("unload");
    };
});