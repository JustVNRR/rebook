function updateSelect(obj) {

    var placeholder = "search for a book";

    switch ($(obj).val()) {
        case "google":
            placeholder = "Search for a reference on google books";
            break;
        case "local":
            placeholder = "Search for a reference on rebook";
            break;
        case "copies":
            placeholder = "Search for books available for trade";
            break;
        case "wishes":
            placeholder = "Search for books people are looking for";
            break;
    }
    $("#advanced-search-input").attr('placeholder', placeholder);

    $("#advanced-search-form").find(".form-check-label").each(function () {
        if ($(this).text() == $(obj).val()) {
            $(this).prev().prop("checked", true);
            return false;
        }
    });
    $('#main-search-button').trigger('click');
}

function updateSelectAvailable(obj) {

    var placeholder = "Search for books in your library";

    switch ($(obj).val()) {
        case "Availables":
            placeholder = "Search for availables books for trade in your library";
            break;
        case "Privates":
            placeholder = "Search for private books in your library";
            break;
    }
    $("#advanced-search-input").attr('placeholder', placeholder);

    $("#advanced-search-form").find(".form-check-label").each(function () {
        console.log("check for each ");
        if ($(this).text() == $(obj).val()) {
            console.log("check : " + $(obj).val());
            $(this).prev().prop("checked", true);
            return false;
        }
    });
    $('#main-search-button').trigger('click');
}

onBegin = function () {
    $("#main-modal-content").html($(".containerLoader").html());
};

function onBeginTarget(target) {
    readMoreLoader($("#" + target));
}
onError = function () {
    alert("Ajax Called Failed");
};

onRefresh = function () {
    $.ajax({
        async: true,
        type: "Get",
        url: "Home/_IndexPartial",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
        }
    });
};

function onLink(obj) {

    var sWidth = Math.max(Math.round(obj.length / 1.7), 10);
    $("#advanced-search-input").val(obj);
    $("#advanced-search-input").css({ width: sWidth + "em" });
    $("#clear-advanced-search-button").fadeIn();
}

replaceContent = function (response) {
    $(response.targetId).html(response.responseText);
};

replaceAndNotif = function (response) {
    $(response.targetId).html(response.responseText);

    if (response.notification) {
        eval(response.notification);
    }
};

onNotif = function (response) {

    if (response.notification) {
        eval(response.notification);
    }
};
replaceAndReadMore = function (response) {
    $('#main-modal').modal('hide');
    $(response.targetId).html(response.responseText);
    displayOrHideReadMoreButton();
};

replaceAndReadMoreNoClose = function (response) {
    $(response.targetId).html(response.responseText);
    displayOrHideReadMoreButton();
};

appendReadMore = function (response) {
    $(response.targetId).append(response.responseText);
    displayOrHideReadMoreButton();
};

replaceAndCallback = function (response) {

    $(response.targetId).html(response.responseText);
    if (response.callback != null) {

        if (response.callback.indexOf("_") != -1) {
            $.ajax({
                async: true,
                type: "Get",
                url: response.callback,
                cache: false,
                success: function (response2) {
                    $(response2.targetId).html(response2.responseText);
                    displayOrHideReadMoreButton();
                }
            });
        }
        else {
            console.log("eval callback");
            eval(response.callback);
        }
    }
};

replaceCallbackAndNotif = function (response) {

    $(response.targetId).html(response.responseText);

    if (response.notification) {
        console.log(response.notification);
        eval(response.notification);
    }

    if (response.callback != null) {
        if (response.callback.indexOf("_") != -1) {
            $.ajax({
                async: true,
                type: "Get",
                url: response.callback,
                cache: false,
                success: function (response2) {
                    $(response2.targetId).html(response2.responseText);
                    displayOrHideReadMoreButton();
                }
            });
        }
        else {
            console.log("eval callback");
            eval(response.callback);
        }

    }
};

replaceRefreshAndNotif = function (response) {

    $(response.targetId).html(response.responseText);

    if (response.notification) {
        console.log(response.notification);
        eval(response.notification);
    }

    $("#main-search-button").trigger("click");
};

onSuccessLoginRegister = function (response) {

    if (response.success == true && response.responseText == null) {
        $('#main-modal').modal('hide');
        $('#main-modal').on('hidden.bs.modal', function () {
            updateNavBar();
        });
    }
    else if (response.success == false) {
        $(response.targetId).html(response.responseText);
    }
    else {
        if (typeof response.targetId !== 'undefined') {
            $(response.targetId).html(response.responseText);
            updateNavBar();
        }
        else {
            const urlParams = new URLSearchParams(window.location.search);
            const myParam = urlParams.get('ReturnUrl');
            window.location.href = myParam;
        }
    }
};

onSuccessExternalLogin = function (response) {
    if (response.success == true && response.responseText == null) {
        //window.location.href = "/";
        setTimeout(function () { $('#main-modal').modal('hide'); }, 500);

        $('#main-modal').on('hidden.bs.modal', function () {
            updateNavBar();
        });
    }
    else if (response.success == false) {
        $(response.targetId).html(response.responseText);
    }
    else {
        if (typeof response.targetId !== 'undefined') {
            $(response.targetId).html(response.responseText);
            updateNavBar();
        }
        else {
            const urlParams = new URLSearchParams(window.location.search);
            const myParam = urlParams.get('ReturnUrl');
            window.location.href = myParam;
        }
    }
};

updateNavBar = function () {
    $.ajax({
        async: true,
        type: "Get",
        url: "/Account/_UpdateNavBar",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
            checkNotifications();
        }
    });
};

function usersRole(obj) {
    $(".text-success").remove();
    if ($(obj).html() == "Edit") {
        $("#usersRoleModalBody").find(".d-none").removeClass("d-none");
        $(obj).html("Done");
        $(".table-bordered").addClass("table-striped");
    }
    else {
        $(obj).html("Edit");
        $(".table-striped").removeClass("table-striped");
        $(".form-check-input").addClass("d-none");
        $(".form-check-input").each(function () {
            if ($(this).prop("checked") == false) {
                $(this).closest("tr").addClass("d-none");
            }
        });
    }
}

function updateUserRole(obj) {

    var role = $("#usersRole").val();
    var user = $(obj).closest(".form-check").children().first().val();
    var action = "remove";

    if ($(obj).prop("checked")) {
        action = "add";
    }

    $(".text-success").remove();
    $(obj).addClass("userInRoleUpdate");
    $.ajax({
        async: true,
        data: { 'roleName': role, 'userId': user, 'action': action },
        type: "POST",
        url: "/Administration/AddOrRemoveUserRole",
        cache: false,
        success: function (partialView) {
            $(".userInRoleUpdate").closest(".form-check").append(partialView);
            $(".userInRoleUpdate").removeClass("userInRoleUpdate");
        }
    });
}

function displayOrHideReadMoreButton() {

    $("#containerReadMore").html($("#read-more-embed").html());
    $(".tricky").remove();

    var cp = parseInt($("#sCurrentPage").val());
    $("#sCurrentPage").val(cp + 1);
}

function readMoreLoader(obj) {
    $(obj).html('<div class="readmore-loader-container"><div class="readmore-loader"></div></div>');
}

function uploadAvatar(event, obj, userId, navbar) {

    if (userId.length == 0) userId = "undefined";

    var file = event.target.files[0];

    if (file === null) return;
    var fileName = $(obj).val();

    $(obj).next('.custom-file-label').html(fileName);

    var formData = new FormData();
    formData.append("file", file);

    if (typeof $("#form-img-preview").attr("src") != "undefined" && $("#form-img-preview").attr("src").indexOf("anonymous_placeholder") == -1) {
        var currentFileName = $("#form-img-preview").attr("src").substring($("#form-img-preview").attr("src").indexOf("/Avatars/") + 1).split(".")[0];
        formData.append('name', currentFileName);
        formData.append('userId', userId);
    }
    else {
        formData.append('name', "Avatars/" + Date.now());
        formData.append('userId', userId);
    }

    $.ajax({
        url: "/Administration/SaveAvatar",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            $("#avatar-loader-container").addClass("loading");
        },
        success: function (url) {
            $("#form-img-preview").attr("src", url);
            $("#Avatar").val(url);
            setTimeout(function () { $("#avatar-loader-container").removeClass("loading"); }, 300);

            if (navbar === true) {
                $.ajax({
                    async: true,
                    type: "Get",
                    url: "/Account/_UpdateNavBar",
                    cache: false,
                    success: function (response) {
                        $(response.targetId).html(response.responseText);
                    }
                });
            }
        },
        error: function () {
            alert("Ajax Called Failed");
        }
    });
}

function mainSearchObserver() {
    mainSearchObserver = new IntersectionObserver(function (observables) {
        observables.forEach(function (observable) {
            if (observable.intersectionRatio < 1) {

                $(".main-search-container").addClass('box-shad');
            }
            else if (observable.intersectionRatio == 1) {

                $(".main-search-container").removeClass('box-shad');
            }
        });
    }, { threshold: [1] });

    mainSearchObserver.observe(document.querySelector(".main-search-container"));
}

function submitOnEnter(event) {

    if (!event) { var event = window.event; }

    if (event.keyCode == 13) {
        $("#submit-advanced-search").trigger('click');
    }
}

function resetPassword(email, token) {
    //console.log(token);

    $.ajax({
        async: true,
        data: { 'email': email, 'token': token },
        type: 'Get',
        url: "/Account/_ResetPassword",
        cache: false,
        beforeSend: function () {
            $('#main-modal').modal('show');
            onBegin;
        },
        success: function (response) {
            replaceAndCallback(response);
            resetUrl();
        },
        error: function (data) {
            alert("Ajax Called Failed");
        }
    });
}

function confirmEmail(userID, token) {
    console.log(token);

    $.ajax({
        async: true,
        data: { 'userId': userID, 'token': token },
        type: 'Get',
        url: "/Account/_ConfirmEmail",
        cache: false,
        beforeSend: function () {
            $('#main-modal').modal('show');
            onBegin;
        },
        success: function (response) {
            replaceAndCallback(response);
            resetUrl();
        },
        error: function (data) {
            alert("Ajax Called Failed");
        }
    });
}

function externalLogin(returnUrl, remoteError) {

    console.log("RETURN URL LEN : " + returnUrl.length);

    if (returnUrl.length == 0) {

        $.ajax({
            async: true,
            data: { 'returnUrl': returnUrl, 'remoteError': remoteError },
            type: 'Get',
            url: "/Account/_ExternalLoginCallback",
            cache: false,
            beforeSend: function () {
            },
            success: function (response) {
                updateNavBar();
                resetUrl();
            },
            error: function (data) {
                alert("Ajax Called Failed");
            }
        });
    }
    else {
        $.ajax({
            async: true,
            data: { 'returnUrl': returnUrl, 'remoteError': remoteError },
            type: 'Get',
            url: "/Account/_ExternalLoginCallback",
            cache: false,
            beforeSend: function () {
                $('#main-modal').modal('show');
                onBegin;
            },
            success: function (response) {
                onSuccessLoginRegister(response);
                resetUrl();
            },
            error: function (data) {
                alert("Ajax Called Failed");
            }
        });
    }
}

function loginFilter(returnUrl) {

    $.ajax({
        async: true,
        data: { 'returnUrl': returnUrl, 'obstrusive': true },
        type: 'Get',
        url: "/Account/_LoginRegisterGet",
        cache: false,
        beforeSend: function () {
            $('#main-modal').modal('show');
            onBegin;
        },
        success: function (response) {
            onSuccessLoginRegister(response);
        },
        error: function (data) {
            alert("Ajax Called Failed");
        }
    });
}

function resetUrl() {
    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];
    //window.location.replace(baseUrl);
    window.history.replaceState("", "", baseUrl);
}
function readNotification(obj) {
    if ($("#notification-count").length > 0 && $("#notification-count").html() > 0) {
        $("#notification-count").html($("#notification-count").html() - 1);
        if ($("#notification-count").html() == 0) {
            $("#notification-count-container").fadeOut().children().fadeOut();
        }
    }
    hideNotification(obj);
}

function readNotificationWithoutClosingModal(obj) {
    if ($("#notification-count").length > 0 && $("#notification-count").html() > 0) {
        $("#notification-count").html($("#notification-count").html() - 1);
        if ($("#notification-count").html() == 0) {
            $("#notification-count-container").fadeOut().children().fadeOut();
        }
    }
}

function hideNotification(obj) {
    if ($("#notification-count").html() == 0) {
        $('#main-modal').modal('hide');
    }
    $('#' + obj).fadeOut();
}

function checkNotifications() {
    $.ajax({
        async: true,
        type: "Get",
        url: "/Notification/CheckNotifications",
        cache: false,
        success: function (response) {
            if (response.count > 0) {
                $("#notification-count").html(response.count);
                $("#notification-count-container").fadeIn().children().fadeIn();
            }
            else
                $("#notification-count-container").fadeOut().children().fadeOut();
        }
    });
}
$(document).ready(function () {
    if ($(".main-search-container").length > 0) mainSearchObserver();

    connection = new signalR.HubConnectionBuilder().withUrl("/signalServer").build();

    connection.start().then(function () {
        connection.on('ReceiveNotification', (count) => {
            console.log("Notifications count : " + count);

            $("#notification-count").html(count);

            if (count > 0) {
                $("#notification-count-container").fadeIn().children().fadeIn();
            }
            else {
                $("#notification-count-container").fadeOut().children().fadeOut();
            }
        });

        checkNotifications();

    }).catch(function (err) {
        return console.error(err.toString());
    });
});
var connection;