// Show/hide modal dialog
// Scope: modal

//Workloads:

function newWorkloadStateSimple() {
    EnableWorkloadFields();

    var opts = {
        fields: true,
        cancel: true,
        reset: true, 
        add: true
    };

    var workload = {};
    renderModal('New Workload:', workload, opts);    
}

function detailsWorkloadState(ev, guid) {
    DisableWorkloadFields();

    var opts = {
        fields: false,
        cancel: true,
        edit: true,
        newAppoint: true
    };

    renderModal('Workload Details:', {}, {});

    //Load Workload:
    loadWorkload(guid, function (data) {
        var workload = { wbid: data.WBID, title: data.WBTitle, description: data.WBDescription};

        renderModal('Workload Details:', workload, opts);
    });

}

function editWorkloadState() {

    var opts = {
        fields: true,
        cancel: true,
        update: true,
        delete: true
    };

    renderModal('Editing Workload:', null, opts);

    EnableWorkloadFields();
}

//Workloads Modal:

function resetWorkloadForm() {
    $('#msg').text('');
    $('#WBTitle').val('');
    $('#WBDescription').val('');
    
    // $('#WBStartDate').val('');
    // $('#WBEndDate').val('');

    clearValidate();
}

function DisableWorkloadFields() {
    $('#WBTitle').attr("disabled", "disabled");
    $('#WBDescription').attr("disabled", "disabled");

    // $('#WBStartDate').attr("disabled", "disabled");
    // $('#WBEndDate').attr("disabled", "disabled");

    //Disabled all buttons:
    // $("#btnWorkloadCancel").attr("disabled", "disabled");
    // $("#btnWorkloadReset").attr("disabled", "disabled");
    // $("#btnWorkloadEdit").attr("disabled", "disabled");
    // $("#btnWorkloadDelete").attr("disabled", "disabled");
    // $("#btnWorkloadSendUpdate").attr("disabled", "disabled");
    // $("#btnWorkloadSendAdd").attr("disabled", "disabled");    
}

function EnableWorkloadFields() {
    //Fields:
    $('#WBTitle').removeAttr("disabled");
    $('#WBDescription').removeAttr("disabled");

    // $('#WBStartDate').removeAttr("disabled");
    // $('#WBEndDate').removeAttr("disabled");
}

// function HideAllButtons() {
//     $("#btnWorkloadCancel").addClass('hidden');
//     $("#btnWorkloadReset").addClass('hidden');
//     $("#btnWorkloadEdit").addClass('hidden');
//     $("#btnWorkloadDelete").addClass('hidden');
//     $("#btnWorkloadSendAdd").addClass('hidden');
//     $("#btnWorkloadSendUpdate").addClass('hidden');
// }
