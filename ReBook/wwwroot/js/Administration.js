var loadMore = true;

function uploadEditionCover(event, obj) {
    var fileName = $(obj).val();
    $(obj).next('.custom-file-label').html(fileName);

    //var CLOUDINARY_URL = "https://api.cloudinary.com/v1_1/dngz4sjc2/image/upload";
    //var CLOUDINARY_UPLOAD_PRESET = "ml_Rebookdefault";
    //formData.append("upload_preset", CLOUDINARY_UPLOAD_PRESET);

    //$.ajax({
    //    url: CLOUDINARY_URL,
    //    type: 'POST',
    //    data: formData,
    //    processData: false,
    //    contentType: false,
    //    success: function (res) {
    //        $("#edition-cover-preview").attr("src", res.secure_url);
    //        $("#googleAPIPic").val(res.secure_url);
    //    },
    //    error: function (err) {
    //        $("#tagsListContainer").html(err);
    //    }
    //});

    var file = event.target.files[0];
    var formData = new FormData();
    formData.append("file", file);
    formData.append('name', "Editions/" + $("#Edition_ISBN13").val() /*+ "." + file.name.split('.').pop()*/)

    $.ajax({
        url: "/Edition/SaveCover",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function (partialView) {
            //$("#containerEditionModal").html($(".containerLoader").html());
            $("#cover-loader-container").addClass("loading");
        },
        success: function (url) {
            $("#edition-cover-preview").attr("src", url);
            $("#Edition_Cover").val(url);
            setTimeout(function () { $("#cover-loader-container").removeClass("loading"); }, 300);

        },
        error: function (data) {
            alert("Ajax Called Failed");
        }
    });
}

function openAdvancedSearch() {
    $("#main-modal-content").html($("#containerSearchEditionForm").children());
}

function onFindAdvanced() {
    var search = "";

    if ($("#Id") != "") search += $("#Id").val();

    if ($("#Pseudo").val() != "") {
        if (search.length > 0) {
            search += " - "
        }
        search += $("#Pseudo").val();
    }

    if ($("#Login").val() != "") {
        if (search.length > 0) {
            search += " - "
        }
        search += $("#Login").val();
    }

    if ($("#Role").val() != "") {
        if (search.length > 0) {
            search += " - "
        }
        search += $("#Role").val();
    }
    var sWidth = Math.max(Math.round(search.length / 1.7), 10);

    $("#advanced-search-input").val(search);
    $("#advanced-search-input").css({ width: sWidth + "em" });

    $("#clear-advanced-search-button").fadeIn();
}

function onClearAdvancedSearch() {
    $("#containerSearchEditionForm").find("input[type=text], textarea").val("");
    $("#clear-advanced-search-button").fadeOut();
}
$(document).ready(function () {
    $('#main-modal').on('hide.bs.modal', function () {
        if ($('#main-modal-content').find(".modal-header").hasClass("advanced-search")) {
            $("#containerSearchEditionForm").append($("#main-modal-content").children());
        }
    });

    if (loadMore === false) {
        $("#read-more-btn").trigger('click');
    }
    else {
        initInfiniteScrollObserver();
    }
});

function initInfiniteScrollObserver() {
    infScrollObserver = new IntersectionObserver(function (observables) {
        observables.forEach(function (observable) {
            if (observable.intersectionRatio > 0.25) {

                if (loadMore === true) {
                    $("#read-more-btn").trigger('click');
                    console.log("CLIC");
                }
            }
        });
    }, { threshold: [0.25] });

    infScrollObserver.observe(document.querySelector("#containerReadMore"));
}

function preventEnter(e) {
    e = e || window.event;

    if (e.keyCode == 13) {
        e.preventDefault();
    }
}

function removeRole(obj) {

    var currentIndex = parseInt($(obj).closest(".single-role-container").prev().val());

    var nRoles = $(".role-name").length;

    for (i = currentIndex; i < nRoles - 1; i++) {

        var nextVal = $($(".role-name")[i + 1]).val();

        $($(".role-name")[i]).val(nextVal);
    }
    $(".single-role-container").last().remove();

    $.ajax({
        async: true,
        data: $("#form").serialize(),
        type: "POST",
        url: "/Administration/_UpdateRolesInForm",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
        }
    });
    return false;
}

function addRole(obj) {

    var emptyTag = false;

    $(".role-name").each(function (index) {
        if ($(this).val().trim().length == 0) {
            emptyTag = true;
            return false;
        }
    });

    if (emptyTag == true) return false;

    $.ajax({
        async: true,
        data: $("#form").serialize(),
        type: "POST",
        url: "/Administration/_AddRoleInForm",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
        }
    });
}