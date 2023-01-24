var loadMore = true;

function uploadEditionCover(event, obj) {

    var file = event.target.files[0];

    if (file === null) return;
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


    var formData = new FormData();
    formData.append("file", file);
    formData.append('name', "Editions/" + $("#Edition_ISBN13").val() /*+ "." + file.name.split('.').pop()*/)

    $.ajax({
        url: "/Edition/SaveCover",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            $("#img-loader-container").addClass("loading");
        },
        success: function (url) {
            $("#form-img-preview").attr("src", url);
            $("#Edition_Cover").val(url);
            setTimeout(function () { $("#img-loader-container").removeClass("loading"); }, 300);

        },
        error: function (data) {
            alert("Ajax Called Failed");
        }
    });
}

function uploadCopyVisual(event, obj) {

    var file = event.target.files[0];

    if (file === null) return;
    var fileName = $(obj).val();
    $(obj).next('.custom-file-label').html(fileName);

    var formData = new FormData();
    formData.append("file", file);

    formData.append('name', "Copies/" + $("#Copy_Edition_ISBN13").val() + "_" + Date.now())

    $.ajax({
        url: "/Edition/SaveCover",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            $("#img-loader-container").addClass("loading");
        },
        success: function (url) {
            $("#form-img-preview").attr("src", url);
            $("#Copy_Visuals").val(url);
            setTimeout(function () { $("#img-loader-container").removeClass("loading"); }, 300);

        },
        error: function (data) {
            alert("Ajax Called Failed");
        }
    });
}

/*function ExternalLogin(provider, returnUrl) {

    var urlExtern = "/Account/_ExternalLogin?provider=" + provider + "&&returnUrl=" + returnUrl;

    $.ajax({
        url: urlExtern,
        type: 'POST',
        processData: false,
        contentType: false,

        error: function (err) {
        }
    });
}*/
function preventEnter(e) {
    e = e || window.event;

    if (e.keyCode == 13) {
        e.preventDefault();
    }
}

function removeTag(obj) {

    var currentIndex = parseInt($(obj).closest(".single-tag-container").prev().val());

    var nTags = $(".tag-name").length;

    for (i = currentIndex; i < nTags - 1; i++) {

        var nextVal = $($(".tag-name")[i + 1]).val();

        $($(".tag-name")[i]).val(nextVal);
    }
    $(".single-tag-container").last().remove();

    $.ajax({
        async: true,
        data: $("#form").serialize(),
        type: "POST",
        url: "/Edition/_UpdateTags",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
        }
    });
    return false;
}

function addTag(obj) {

    var emptyTag = false;

    $(".tag-name").each(function (index) {
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
        url: "/Edition/_AddTag",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
        }
    });
}

function removeAuthor(obj) {

    var currentIndex = parseInt($(obj).closest(".single-author-container").prev().val());

    var nAuthors = $(".author-name").length;

    for (i = currentIndex; i < nAuthors - 1; i++) {

        var nextVal = $($(".author-name")[i + 1]).val();

        $($(".author-name")[i]).val(nextVal);
    }
    $(".single-author-container").last().remove();

    $.ajax({
        async: true,
        data: $("#form").serialize(),
        type: "POST",
        url: "/Edition/_UpdateAuthors",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
        }
    });
    return false;
}

function addAuthor(obj) {

    var emptyAuthor = false;

    $(".author-name").each(function (index) {
        if ($(this).val().trim().length == 0) {
            emptyAuthor = true;
            return false;
        }
    });

    if (emptyAuthor == true) return false;

    $.ajax({
        async: true,
        data: $("#form").serialize(),
        type: "POST",
        url: "/Edition/_AddAuthor",
        cache: false,
        success: function (response) {
            $(response.targetId).html(response.responseText);
        }
    });
}

function openAdvancedSearch() {
    $("#main-modal-content").html($("#containerSearchEditionForm").children());
}

function onFindAdvanced() {
    var search = "";

    if ($("#ISBN").val() != "") search += "isbn:" + $("#ISBN").val();

    if ($("#Title").val() != "") {
        if (search.length > 0) {
            search += "+"
        }
        search += "intitle:" + $("#Title").val();
    }

    if ($("#Author").val() != "") {
        if (search.length > 0) {
            search += "+"
        }
        search += "inauthor:" + $("#Author").val();
    }

    if ($("#Editor").val() != "") {
        if (search.length > 0) {
            search += "+"
        }
        search += "inpublisher:" + $("#Editor").val();
    }

    //var sWidth = Math.max(Math.round(search.length / 1.7), 10);

    $("#advanced-search-input").val(search);
    //$("#advanced-search-input").css({ width: sWidth + "em" });
    //readMoreLoader("#containerReadMore");
    $("#containerReadMore").html('<div class="readmore-loader-container"><div class="readmore-loader"></div></div>');

    if ($("#advanced-search-input").val().length > 0) $("#clear-advanced-search-button").show();
    $('#main-modal').modal('hide');
    $('.main-search-container').addClass('sticky-top');
    document.activeElement.blur();
}

function onFindSimple() {
    //readMoreLoader("#containerReadMore");
    $("#containerReadMore").html('<div class="readmore-loader-container"><div class="readmore-loader"></div></div>');
    if ($("#advanced-search-input").val().length > 0) $("#clear-advanced-search-button").show();
    $('.main-search-container').addClass('sticky-top');
    document.activeElement.blur();
}

function onRadioRepoChange() {
    var repo = $('input[name=Repo]:checked', '#advanced-search-form').val();

    $('.main-search-container').find("#sRepo").val(repo);

    $("#select-repo").val(repo);
    updateSelect($("#select-repo"));
}

function onRadioAvailableChange() {
    var available = $('input[name=Available]:checked', '#advanced-search-form').val();

    $('.main-search-container').find("#sAvailable").val(available);

    $("#select-available").val(available);
    updateSelectAvailable($("#select-available"));
}

function onClearAdvancedSearch() {
    $("#containerSearchEditionForm").find("input[type=text], textarea").not("#Repo").val("");
    $("#advanced-search-input").val("");
    $("#clear-advanced-search-button").hide();
    $(".main-search-container").not(".no-clear").removeClass("sticky-top");
}

function clearSearch() {
    $("#containerSearchEditionForm").find("input[type=text], textarea").val("");
    $("#advanced-search-input").val("");
    $("#clear-advanced-search-button").hide();
    $(".main-search-container").not(".no-clear").removeClass("sticky-top");
    $('#main-search-button').trigger('click');
}

$(document).ready(function () {
    $('#main-modal').on('hide.bs.modal', function () {
        if ($('#main-modal-content').find(".modal-header").hasClass("advanced-search")) {
            $("#containerSearchEditionForm").append($("#main-modal-content").children());
        }
    });

    if ($("#containerReadMore").length > 0) {
        if (loadMore === false) {
            $("#read-more-btn").trigger('click');
        }
        else {
            initInfiniteScrollObserver();
        }
    }
});

function initInfiniteScrollObserver() {
    infScrollObserver = new IntersectionObserver(function (observables) {
        observables.forEach(function (observable) {
            if (observable.intersectionRatio > 0.25) {

                if (loadMore === true) {
                    $("#read-more-btn").trigger('click');
                    console.log("scroll");
                }
            }
        });
    }, { threshold: [0.25] });

    infScrollObserver.observe(document.querySelector("#containerReadMore"));
}