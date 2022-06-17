jQuery.validator.addMethod("extension", function (value, element, param) {
    var validExtension = ".png, .jpeg, .jpg";

    var file = element;
    var filePath = file.value;
    var getFileExtension = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
    var position = validExtension.indexOf(getFileExtension);
    if (position < 0) {
        return false;
    } else {
        return value;
    }
});
jQuery.validator.unobtrusive.adapters.addBool("extension");

jQuery.validator.addMethod("size", function (value, element, param) {
    var maxSize = '2048';

    var file = element;
    if (file.files && file.files[0]) {
        var fileSize = file.files[0].size / 1024;
        if (fileSize > maxSize) {
            return false;
        } else {
            return value;
        }
    }
});
jQuery.validator.unobtrusive.adapters.addBool("size");