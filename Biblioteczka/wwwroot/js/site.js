// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function preview(input) {
    var preview = $('#ImagePreview');

    if (input.value == null) {
        preview.attr("src", null);
        preview.attr("height", 0);
        preview.css({ marginTop: "00px" });
    }
    else {
        preview.attr("src", URL.createObjectURL(input.files[0]));
        preview.attr("height", 250);
        preview.css({ marginTop: "20px" });
    }
}

$(".selected-value option:selected").css({ "background-color": "#F2545B", color: "white" });