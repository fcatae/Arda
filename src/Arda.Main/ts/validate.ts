//Workload Validation:

function validateForm(e, data, callback) {
    var ctrl = true;

    var textFields = ['#WBStartDate', '#WBEndDate', '#WBTitle'];
    for (var i = 0; i < textFields.length; i++) {
        var field = $(textFields[i]);
        if (field.val() == "") {
            ctrl = false;
            field.addClass('error');
        } else {
            field.removeClass('error');
        }
    }

    var selectFields = ['#WBExpertise', '#WBActivity'];
    for (var i = 0; i < selectFields.length; i++) {
        var field = $(selectFields[i]);
        if (field.val() == -1 || field.val() == null) {
            ctrl = false;
            field.addClass('error');
        } else {
            field.removeClass('error');
        }
    }

    var multiselectFields = ['#WBTechnologies', '#WBMetrics', '#WBUsers'];
    for (var i = 0; i < multiselectFields.length; i++) {
        var field = $(multiselectFields[i]);
        var selected = $(multiselectFields[i] + ' option:selected');
        if (selected.length == 0) {
            ctrl = false;
            field.siblings().children("button").addClass('error');
        } else {
            field.siblings().children("button").removeClass('error');
        }
    }

    var msg = $('#msg');
    if (ctrl) {
        callback(e, data);
    } else {
        msg.fadeIn();
        msg.text('Please, fill the mandatory fields.');
        e.preventDefault();
    }
}

function validateFormSimple(e, data, callback) {
    var ctrl = true;

    var textFields = [/*'#WBStartDate', '#WBEndDate',*/ '#WBTitle'];
    for (var i = 0; i < textFields.length; i++) {
        var field = $(textFields[i]);
        if (field.val() == "") {
            ctrl = false;
            field.addClass('error');
        } else {
            field.removeClass('error');
        }
    }

    //var selectFields = ['#WBExpertise', '#WBActivity'];
    //for (var i = 0; i < selectFields.length; i++) {
    //    var field = $(selectFields[i]);
    //    if (field.val() == -1 || field.val() == null) {
    //        ctrl = false;
    //        field.addClass('error');
    //    } else {
    //        field.removeClass('error');
    //    }
    //}

    //var multiselectFields = ['#WBTechnologies', '#WBMetrics', '#WBUsers'];
    //for (var i = 0; i < multiselectFields.length; i++) {
    //    var field = $(multiselectFields[i]);
    //    var selected = $(multiselectFields[i] + ' option:selected');
    //    if (selected.length == 0) {
    //        ctrl = false;
    //        field.siblings().children("button").addClass('error');
    //    } else {
    //        field.siblings().children("button").removeClass('error');
    //    }
    //}

    var msg = $('#msg');
    if (ctrl) {
        callback(e, data);
    } else {
        msg.fadeIn();
        msg.text('Please, fill the mandatory fields.');
        e.preventDefault();
    }
}

function clearValidate() {
    var textFields = ['#WBStartDate', '#WBEndDate', '#WBTitle', '#WBExpertise', '#WBActivity'];
    for (var i = 0; i < textFields.length; i++) {
        var field = $(textFields[i]);
        field.removeClass('error');
    }

    var multiselectFields = ['#WBTechnologies', '#WBMetrics', '#WBUsers'];
    for (var i = 0; i < multiselectFields.length; i++) {
        var field = $(multiselectFields[i]);
        field.siblings().children("button").removeClass('error');
    }
}
