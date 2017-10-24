$(document).ready(function () {
    //Show toastr message when redirected
    showQueryMessage();

    GetTotalCount();

    //Set login popup html
    $(function () {
        $("#btn-popover-login").popover({
            html: true,
            content: function () {
                var form = $('<form id="form-login" class="form-login"></form>');

                form.html(`<div class="input-group">
						<span class ="input-group-addon"><i class ="glyphicon glyphicon-user"></i></span>
						<input type="text" class ="form-control" id="email" name="email" placeholder="E-mail">
						</div>
						<span class="help-block"></span>
						<div class="input-group">
						<span class ="input-group-addon"><i class ="glyphicon glyphicon-lock"></i></span>
						<input  type="password" class ="form-control" id="password" name="password" placeholder="Senha">
						</div>
						<span id="login-msg-box" class ="help-block text-center text-error"></span>
						<a class ="btn btn-sm btn-primary btn-block popup-login-btn" onclick="popupLogin()">Login</a>`);

                //Validations for login popup
                form.validate({
                    rules: {
                        email: {
                            required: true,
                            email: true
                        },
                        password: {
                            required: true
                        }
                    },
                    messages: {
                        email: {
                            required: "Informe seu e-mail",
                            email: "Por favor informe um email válido"
                        },
                        password: {
                            required: "Informe sua senha"
                        }
                    },
                    errorClass: "error-class",
                    validClass: "valid-class",
                    highlight: function (element) {
                        $(element).closest(".input-group").addClass("has-error");
                    },
                    unhighlight: function (element) {
                        $(element).closest(".input-group").removeClass("has-error");
                    },
                    errorPlacement: function (error, element) {
                        if (element.parent(".input-group").length) {
                            error.insertAfter(element.parent());
                        } else {
                            error.insertAfter(element);
                        }
                    }
                });

                return form;
            }
        });
    });

    //Buttons events
    $("#btnSalvar").click(function (e) {
        e.preventDefault();

        var form = $("#form-create-user");

        if (!form.valid())
            return;

        $.ajax({
            type: "POST",
            url: "/Auth/RegisterUser",
            data: $("#form-create-user").serialize(),
            dataType: "json",
            success: function (data) {
                if (data.Response === EResponse.Error) {
                    $("#register-msg-box").text(data.Message);
                    $("#register-msg-box").show();
                    return;
                }

                //Redirect to index page to update header
                location.href = "?msg=" + data.Message + "&msgType=success";
            }, error: function (data) {
                console.log("Erro ao executar requisição ao servidor.");
                console.log(data.Message);
            }
        });
    });

    //Search events
    $(".search-btn").click(function (e) {
        e.preventDefault();
        searchQuery($("#search-term").val());
    });

    //$(".header-search-btn").click(function (e) {
    //    e.preventDefault();
    //    var query = $("#header-search-term").val();
    //    searchQuery(query);
    //    $("#search-term").val(query);
    //});

    function searchQuery(query) {
        Request.get({
            url: "/Search/Search",
            data: "query=" + query,
            success: function (data) {
                if (data.Response === EResponse.Error) {
                    toastr.error(data.Message);
                    return;
                }

                showSearchResult(data);
            },
            error: function (data) {
                console.log("Erro ao executar requisição ao servidor.");
                console.log(data.Message);
            }
        });
    }

    function showSearchResult(data) {
        var json = JSON.parse(data);
        var divResult = $("#response-box-container");

        //show the result container
        divResult.removeClass("hidden");

        //remove old results
        divResult.html("");

        if (json.length <= 0) {
            divResult.append("<div class='col-md-11'><p class='no-result-response-text'>Nenhum resultado encontrado</div>");
            return;
        }

        //append new results
        $.each(json, function (key, result) {
            var author = result.Book.author;
            var title = result.Book.title;
            var total = json[key].Contents.length;
            var query = $("#search-term").val();

            divResult.append("<h2>Resultados da pesquisa por <span>" + query + "</span></h2><span><span id='result-total'>" + total + "</span> Resultados encontrados</span>");

            $.each(json[key].Contents, function (key, content) {
                divResult.append("<div class='result-item'><span class='result-title'>" + title + "</span><span class='result-author'><i>Autor</i>" + author + "</span><span class='result-page'><i>Página</i>" + content.page + "</span><span class='result-text'><i>" + content.texto + "</i></span></div>");
            });
        });
    }

    //Enter to send search
    $("#search-term").keypress(function (e) {
        if (e.which === 13) {
            $("#search-btn").click();
            return false;
        }
    });
});

function GetTotalCount() {
    $.ajax({
        type: "GET",
        cache: false,
        dataType: "json",
        url: "/Home/GetTotalDoctrines",
        success: function (data) {
            if (data.Response === EResponse.Error) {
                toastr.error(data.Message);
                return;
            }

            var result = JSON.parse(data);
            $("#total-doctrines").html(result.doutrinas);
        },
        error: function (data) {
            console.log("Erro ao executar requisição ao servidor.");
            console.log(data.Message);
        }
    });
}

//Login function
function popupLogin() {
    var form = $("#form-login");

    if (!form.valid())
        return;

    $.ajax({
        type: "POST",
        url: "/Auth/Login",
        data: form.serialize(),
        dataType: "json",
        success: function (data) {
            if (data.Response === EResponse.Error) {
                $("#login-msg-box").text(data.Message);
                return;
            }

            //Redirect to index page to update header
            location.href = "?msg=" + data.Message + "&msgType=success";
        },
        error: function (data) {
            alert(data.data);
        }
    });
}

//Validation register form
$("#form-create-user").validate({
    rules: {
        name: {
            required: true
        },
        email: {
            required: true,
            email: true
        },
        password: {
            required: true
        },
        confirmpassword: {
            required: true
        }
    },
    messages: {
        name: {
            required: "Informe seu nome"
        },
        email: {
            required: "Informe seu e-mail",
            email: "Por favor informe um email válido"
        },
        password: {
            required: "Informe sua senha"
        },
        confirmpassword: {
            required: "Confirme sua senha"
        }
    },
    errorClass: "error-class",
    validClass: "valid-class",
    highlight: function (element) {
        $(element).closest(".form-group").addClass("has-error").addClass("fix-modal-error-msg");
    },
    unhighlight: function (element) {
        $(element).closest(".form-group").removeClass("has-error").removeClass("fix-modal-error-msg");
    },
    errorPlacement: function (error, element) {
        if (element.parent(".form-group").length) {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});

//Show message after login with toastr
function showQueryMessage() {
    var msg = getUrlParameter("msg");
    var msgType = getUrlParameter("msgType");

    if (msg === "")
        return;

    if (msgType === "success")
        toastr.success(msg);

    if (msgType === "error")
        toastr.error(msg);

    //Change url to remove query string
    window.history.pushState("", "Doutrina Ágil - A doutrina que importa para você", "/");
}

//Get parameter from query string
function getUrlParameter(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
    var results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

//repositioning tooltip when window changes size
$(window).off("resize").on("resize", function () {
    $("[data-toggle='popover']").each(function () {
        var popover = $(this);
        if (popover.is(":visible")) {
            var ctrl = $(popover.context);
            ctrl.popover("show");
        }
    });
});