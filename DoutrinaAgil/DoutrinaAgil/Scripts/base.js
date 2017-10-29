//Enumerador request type
EResponse = {
    Error: 0,
    Success: 1
}

//abtn text functions
var abtnFunc = (function () {
    return {
        name: function (fullName) {
            var firstName = fullName.split(" ").slice(0, -1).join(" ");
            var lastName = fullName.split(" ").slice(-1).join(" ");
            return lastName.toUpperCase() + ", " + firstName + ".";
        }
    };
})();

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

//plugin to ignore an element inside another
$.fn.ignore = function (sel) {
    return this.clone().find(sel || ">*").remove().end();
};