

jQuery.validator.addMethod("validNoSwedish", function(value, element, param) {
    console.log('validNoSwedish' + value);


    if (value != null &&
    (value.toLowerCase().includes("å") ||
        value.toLowerCase().includes("ä") ||
        value.toLowerCase().includes("ö")))
        return false;

    return true;



    //var d = new Date();
    //var n = d.getHours();
    //if (parseInt(value) == parseInt(n)) {
    //    return false;
    //} else {
    //    return true;
    //}

});

jQuery.validator.unobtrusive.adapters.addBool('validNoSwedish');