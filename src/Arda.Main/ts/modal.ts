
function Initialize() {
    
    ReactDOM.render(React.createElement(ModalButtonList, null), document.querySelector('.appmodal') );
    ReactDOM.render(React.createElement(ModalForm, null), document.querySelector('.appmodalform') );

    // $('#WBStartDate').datepicker({
    //     format: "mm/dd/yyyy",
    //     autoclose: true,
    //     todayHighlight: true
    // });

    // $('#WBEndDate').datepicker({
    //     format: "mm/dd/yyyy",
    //     autoclose: true,
    //     todayHighlight: true
    // });        
}

function renderModal(windowTitle, workload, showopts) {
    $('#ModalTitle').text(windowTitle);
    if( workload != null ) {
        ReactDOM.render(React.createElement(ModalForm, workload), document.querySelector('.appmodalform') );
    }

    ReactDOM.render(React.createElement(ModalButtonList, {show: showopts}), document.querySelector('.appmodal') );    
}


// function loadWorkloadCallback(data) {
        
//         $('#WBID').val(data.WBID);
        
//         // Title and Description
//         $('#WBTitle').val(data.WBTitle);
//         $('#WBDescription').val(data.WBDescription);

//         //Dates
//         // formatDate(data.WBStartDate, function (str) {
//         //     $('#WBStartDate').val(str);
//         // });
//         // formatDate(data.WBEndDate, function (str) {
//         //     $('#WBEndDate').val(str);
//         // });
// }

function submitAddWorkloadSimple(e) {
    e.preventDefault();
    if(validateFormSimple()) {
        var workloadV3 = { title: this.WBTitle.value, description: this.WBDescription.value, status: 0};

        DisableWorkloadFields();
        $('#msg').text('Wait...');
        $('#msg').fadeIn();

        addWorkloadSimpleV3HttpCall(workloadV3, function(response) {
            // success
            $('#WorkloadModal').modal('hide');

            createTaskSimple( response.WBID, response.WBTitle, response.WBStatus, response.WBStartDate, response.WBEndDate, response.WBUsers);
            updateDashboard();
        }, function() { 
            // error
            $('#msg').text('Error!'); 
            EnableWorkloadFields();
            newWorkloadStateSimple();
        });
    }

}

function submitUpdateWorkloadSimple(e) {
    e.preventDefault();
    if(validateFormSimple()) {
        var workloadV3 = { id: this.WBID.value, title: this.WBTitle.value, description: this.WBDescription.value};

        DisableWorkloadFields();
        $('#msg').text('Wait...');
        $('#msg').fadeIn();

        updateWorkloadSimpleV3HttpCall(workloadV3, function(response) {
            // success
            $('#WorkloadModal').modal('hide');

            updateTaskInFolder( response.WBID, response.WBTitle, response.WBStatus, response.WBStartDate, response.WBEndDate, response.WBUsers);
            updateDashboard();
        },
        function() { 
            // error
            $('#msg').text('Error!'); 
            EnableWorkloadFields();
            editWorkloadState();
        });
    }
    
}

function deleteWorkloadSimple(ev) {
    ev.preventDefault();
    var workloadID = $('#WBID').val();
    $('#msg').text('Wait...');

    deleteWorkloadHttpCall(workloadID, function (response) {
        $('#WorkloadModal').modal('hide');

            deleteTaskInFolder(workloadID);
            updateDashboard();
    },
        function() { 
            // error
            $('#msg').text('Error!'); 
            EnableWorkloadFields();
            editWorkloadState();
        });    
}

function validateFormSimple() {
    var ctrl = true;

    var textFields = ['#WBTitle'];
    for (var i = 0; i < textFields.length; i++) {
        var field = $(textFields[i]);
        if (field.val() == "") {
            ctrl = false;
            //field.addClass('error'); // css is not working
        } else {
            //field.removeClass('error');
        }
    }

    var msg = $('#msg');
    
    if (!ctrl) {
        msg.fadeIn();
        msg.text('Please, fill the mandatory fields.');        
    }

    return ctrl;
}

function clearValidate() {
    var textFields = ['#WBTitle' ]; 
    for (var i = 0; i < textFields.length; i++) {
        var field = $(textFields[i]);
        field.removeClass('error');
    }
}
