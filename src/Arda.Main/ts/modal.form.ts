// Show/hide modal dialog
// Scope: modal

//Workloads:

//Workload Modal states:

function newWorkloadState() {
    //Clean values:
    resetWorkloadForm();
    EnableWorkloadFields();

    //Get GUID:
    getGUID(function (data) {
        $('#WBID').attr('value', data);
    });

    //Set submit event:
    $('#form-workload').unbind();
    $('#form-workload').submit(addWorkload);

    //Modal Title:
    $('#ModalTitle').text('New Workload:');

    //Buttons:
    $('#btnWorkloadSend').text('Add');

    $('#btnWorkloadEdit').addClass('hidden');
    $('#btnWorkloadDelete').addClass('hidden');
    $('#btnWorkloadAddAppointment').addClass('hidden');

    $("#btnWorkloadCancel").removeAttr("disabled");

    $('#btnWorkloadReset').removeClass('hidden');
    $('#btnWorkloadReset').removeAttr("disabled");

    $('#btnWorkloadSend').removeClass('hidden');
    $('#btnWorkloadSend').removeAttr("disabled");
}

function newWorkloadStateSimple() {
    //Clean values:
    resetWorkloadForm();
    EnableWorkloadFields();

    //Get GUID:
    getGUID(function (data) {
        $('#WBID').attr('value', data);
    });

    //Set submit event:
    $('#form-workload').unbind();
    $('#form-workload').submit(addWorkloadSimple);

    //Modal Title:
    $('#ModalTitle').text('New Workload:');

    //Buttons:
    $('#btnWorkloadSend').text('Add');

    $('#btnWorkloadEdit').addClass('hidden');
    $('#btnWorkloadDelete').addClass('hidden');
    $('#btnWorkloadAddAppointment').addClass('hidden');

    $("#btnWorkloadCancel").removeAttr("disabled");

    $('#btnWorkloadReset').removeClass('hidden');
    $('#btnWorkloadReset').removeAttr("disabled");

    $('#btnWorkloadSend').removeClass('hidden');
    $('#btnWorkloadSend').removeAttr("disabled");
}
function detailsWorkloadState(ev, openWorkloadGuid) {
    resetWorkloadForm();
    DisableWorkloadFields();

    //Modal Title:
    $('#ModalTitle').text('Workload Details:');

    //Set GUID:
    var guid = openWorkloadGuid || $('#_WBID').val();
    $('#WBID').attr('value', guid);

    //Load Workload:
    loadWorkload(guid);

    //Buttons:
    $('#btnWorkloadReset').addClass('hidden');
    $('#btnWorkloadDelete').addClass('hidden');
    $('#btnWorkloadSend').addClass('hidden');

    $("#btnWorkloadCancel").removeAttr("disabled");

    $('#btnWorkloadAddAppointment').removeClass('hidden');
    $('#btnWorkloadEdit').removeClass('hidden');
    $('#btnWorkloadEdit').removeClass('hidden');
    $('#btnWorkloadEdit').removeAttr("disabled");

    $('#btnWorkloadEdit').click(editWorkloadState);
}

function editWorkloadState() {
    //Set submit event:
    $('#form-workload').unbind();
    $('#form-workload').submit(updateWorkload);

    //Modal Title:
    $('#ModalTitle').text('Editing Workload:');

    EnableWorkloadFields();
    $('.fileDel').removeClass('hidden');

    //Buttons:
    $('#btnWorkloadSend').text('Update');

    $('#btnWorkloadReset').addClass('hidden');
    $('#btnWorkloadEdit').addClass('hidden');
    $('#btnWorkloadAddAppointment').addClass('hidden');

    $('#btnWorkloadSend').removeClass('hidden');
    $('#btnWorkloadSend').removeAttr("disabled");

    $('#btnWorkloadDelete').removeClass('hidden');
    $('#btnWorkloadDelete').removeAttr("disabled");
}

//Workloads Modal:

function resetWorkloadForm() {
    $('#msg').text('');
    $('#WBStartDate').val('');
    $('#WBEndDate').val('');
    if ($('#WBIsWorkload').bootstrapSwitch('disabled')) {
        $('#WBIsWorkload').bootstrapSwitch('toggleDisabled', true, true);
        $('#WBIsWorkload').bootstrapSwitch('state', true);
        $('#WBIsWorkload').bootstrapSwitch('toggleDisabled', true, true);
    } else {
        $('#WBIsWorkload').bootstrapSwitch('state', true);
    }

    $('#WBTitle').val('');
    $('#WBDescription').val('');
    $('#WBExpertise').val('-1');
    $('#WBActivity').val('-1');
    //Slider:
    var slider = $("#WBComplexity").data("ionRangeSlider");
    slider.update({
        from: 1
    });
    //Technologies Multiselect:
    var tech = [];
    $("#WBTechnologies option").each(function () {
        tech.push($(this).val());
    });
    $('#WBTechnologies').multiselect('deselect', tech);
    //Metrics Multiselect:
    var met = [];
    $("#WBMetrics option").each(function () {
        met.push($(this).val());
    });
    $('#WBMetrics').multiselect('deselect', met);
    //Users Multiselect:
    var users = [];
    $("#WBUsers option").each(function () {
        users.push($(this).val());
    });
    $('#WBUsers').multiselect('deselect', users);
    //Files:
    $('.fileinput').fileinput('clear');
    $('#filesList').html('');

    clearValidate();
}

function DisableWorkloadFields() {
    $('#WBStartDate').attr("disabled", "disabled");
    $('#WBEndDate').attr("disabled", "disabled");
    if (!($('#WBIsWorkload').bootstrapSwitch('disabled'))) {
        $('#WBIsWorkload').bootstrapSwitch('toggleDisabled', true, true);
    }
    $('#WBTitle').attr("disabled", "disabled");
    $('#WBDescription').attr("disabled", "disabled");
    $('#WBExpertise').attr("disabled", "disabled");
    $('#WBActivity').attr("disabled", "disabled");

    var slider = $("#WBComplexity").data("ionRangeSlider");
    slider.update({
        disable: true
    });

    $('.multiselect-container.dropdown-menu li a label input').attr("disabled", "disabled");
    $('.fileinput').attr("disabled", "disabled");

    //Disabled all buttons:
    $("#btnWorkloadCancel").attr("disabled", "disabled");
    $("#btnWorkloadReset").attr("disabled", "disabled");
    $("#btnWorkloadEdit").attr("disabled", "disabled");
    $("#btnWorkloadDelete").attr("disabled", "disabled");
    $("#btnWorkloadSend").attr("disabled", "disabled");
}

function EnableWorkloadFields() {
    //Fields:
    $('#WBStartDate').removeAttr("disabled");
    $('#WBEndDate').removeAttr("disabled");
    if ($('#WBIsWorkload').bootstrapSwitch('disabled')) {
        $('#WBIsWorkload').bootstrapSwitch('toggleDisabled', true, true);
    }
    $('#WBTitle').removeAttr("disabled");
    $('#WBDescription').removeAttr("disabled");
    $('#WBExpertise').removeAttr("disabled");
    $('#WBActivity').removeAttr("disabled");

    var slider = $("#WBComplexity").data("ionRangeSlider");
    slider.update({
        disable: false
    });

    $('.multiselect-container.dropdown-menu li a label input').removeAttr("disabled");
    $('.fileinput').removeClass('hidden');
}

function HideAllButtons() {
    $("#btnWorkloadCancel").addClass('hidden');
    $("#btnWorkloadReset").addClass('hidden');
    $("#btnWorkloadEdit").addClass('hidden');
    $("#btnWorkloadDelete").addClass('hidden');
    $("#btnWorkloadSend").addClass('hidden');
}
