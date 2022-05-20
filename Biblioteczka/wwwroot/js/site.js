// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function InpuNumber(input) {
    if (input.value.length > input.maxLength) {
        input.value = input.value.slice(0, input.maxLength);
    }
}

function preview(input) {
    var preview = $('#ImagePreview');

    if (fileExtensionValidate(input) == false) {
        alert("Nieprawidłowy format pliku!");
        input.value = null;

        preview.attr("src", null);
        preview.attr("height", 0);
        preview.css({ marginTop: "00px" });
    }
    else if (fileSizeValidate(input) == false) {
        alert("Nieprawidłowy rozmiar pliku!");
        input.value = null;

        preview.attr("src", null);
        preview.attr("height", 0);
        preview.css({ marginTop: "0px" });
    }
    else {
        preview.attr("src", URL.createObjectURL(input.files[0]));
        preview.attr("height", 250);
        preview.css({marginTop: "20px"});
    }
}
var validExtension = ".png, .gif, .jpeg, .jpg";
function fileExtensionValidate(file) {
    var filePath = file.value;
    var getFileExtension = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
    var pos = validExtension.indexOf(getFileExtension);
    if (pos < 0) {
        return false;
    } else {
        return true;
    }
}
var maxSize = '2048'; //2 MB
function fileSizeValidate(file) {
    if (file.files && file.files[0]) {
        var fileSize = file.files[0].size / 1024;
        if (fileSize > maxSize) {
            return false;
        } else {
            return true;
        }
    }
}
