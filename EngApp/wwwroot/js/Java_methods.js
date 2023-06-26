var create_get = function (url_get_c) {
    var url = url_get_c;
    $("#content").load(url, function () {
        $("#mymodal").modal("show");
    })
}



var edit_get = function (id, url) {
    $("#content").empty();
    var url = url + "?id=" + id;
    $("#content").load(url, function () {
        $("#mymodal").modal("show");
    })
}


var post = function (url_get, url_post_e) {
    $("#error").empty();
    if ($("#mname").val() == "") {
        alert("ادخل قيمه ");
        return
    }
    var data = $("#create").serialize();
    $.ajax({
        url: url_get,
        type: "POST",
        data: data,
        success: function (data) {
            if (data == false) {
                $("#error").append("<p> يوجد خطا من فضلك تاكد من ادخال الحقول </p>")
            } else {
                $("#mymodal").modal("hide");
                location.reload();
            }
        }
    });
}

var idfordel = 0;
var del_get = function (id, name) {
    $("#content2").empty();
    $("#content2").append("<p> هل تريد حذف " + name + "?</p>")
    $("#mymodal2").modal("show");
    idfordel = id
}
var del_post = function (url_post_d) {
    $.ajax({
        type: "POST",
        url: url_post_d + "?id=" + idfordel,
        success: function () {
            $("#row_" + idfordel).remove();
            $("#mymodal2").modal("hide");
        }
    })
}

var check = function (id) {

    if ($("#" + id).is(":checked")) {
        if ($("#_" + id).val() == 0)
            $("#_" + id).val(1);
    }
    else
        $("#_" + id).val(0);
}

var post_cd = function (url_post, url_desti, id) {
    $("#error").empty();
    var data = $("#myform").serializeArray();
    var drink_val = [];
    var counts = [];
    for (var i = 1; i < id; i++) {
        drink_val.push($("#" + i).is(":checked"));
        counts.push($("#_" + i).val());
    }
    $.ajax({
        type: "POST",
        data: data,
        url: url_post + "?values=" + drink_val + "&&" + "counts=" + counts,
        success: function (ok) {
            if (ok) {
                $("#mymodal").modal("hide");
                window.location.href = url_desti;
            }
            else {
                $("#error").append("<p> يوجد خطا من فضلك تاكد من ادخال الحقول </p>");
            }
        }
    });
}