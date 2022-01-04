



$(document).ready(function() {

    console.log("ready")

    $.ajax({
        url: "https://api.publicapis.org/entries",
        data: {},
        success: function (data, status, jqXHR) {
            alert("request to https://api.publicapis.org/entries: " + status);
        },
        dataType: 'html'
    });

});