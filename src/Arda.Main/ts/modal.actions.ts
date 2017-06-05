
//Database Operations:

function loadWorkload(workloadID) {

    $.ajax({
        url: '/Workload/GetWorkload?=' + workloadID,
        type: 'GET',
        processData: false,
        contentType: false,
        cache: false,
        success: function (data) {
            //Hide File Input:
            $('.fileinput').addClass('hidden');

            $('#WBID').val(data.WBID);
            //Dates
            formatDate(data.WBStartDate, function (str) {
                $('#WBStartDate').val(str);
            });
            formatDate(data.WBEndDate, function (str) {
                $('#WBEndDate').val(str);
            });

            var isWorkload = $('#WBIsWorkload');
            isWorkload.bootstrapSwitch('toggleDisabled', true, true);
            isWorkload.bootstrapSwitch('state', data.WBIsWorkload);
            isWorkload.bootstrapSwitch('toggleDisabled', true, true);

            $('#WBTitle').val(data.WBTitle);
            $('#WBDescription').val(data.WBDescription);
            $('#WBExpertise').val(data.WBExpertise);
            $('#WBActivity').val(data.WBActivity);

            //Complexity
            var slider = $("#WBComplexity").data("ionRangeSlider");
            slider.update({
                from: data.WBComplexity,
                disable: true
            });
            var txt = '';
            switch (data.WBComplexity) {
                case 1:
                    txt = 'Very Low';
                    break;
                case 2:
                    txt = 'Low';
                    break;
                case 3:
                    txt = 'Medium';
                    break;
                case 4:
                    txt = 'High';
                    break;
                case 5:
                    txt = 'Very High';
                    break;
            }
            $('#ComplexityLevel').text(txt);

            //Multi-Select:
            $('#WBTechnologies').multiselect('select', data.WBTechnologies);
            $('#WBMetrics').multiselect('select', data.WBMetrics);
            $('#WBUsers').multiselect('select', data.WBUsers);

            //Files:
            var list = $('#filesList');
            $(data.WBFilesList).each(function () {
                var div = $('<div id="' + this.Item1 + '">');
                var a = $('<a class="filePrev" FileID="' + this.Item1 + '" href=' + this.Item2 + '>').text(this.Item3);
                var remove = $('<a class="fileDel hidden" style="padding-left: 5px;"/>').text('(remove)');
                remove.click(function () {
                    $(this).parent().remove();
                });
                div.append(a);
                div.append(remove);
                list.append(div);
            });

        }
    });

}

function addWorkload(e) {
    //Gets bootstrap-switch component value:
    var value = $('#WBIsWorkload').bootstrapSwitch('state');
    //Serializes form and append bootstrap-switch value:
    var data = new FormData(this);
    data.append('WBIsWorkload', value);

    var selectedUsers = $('#WBUsers option:selected');
    var users = [];
    for (var i = 0; i < selectedUsers.length; i++) {
        var item = $(selectedUsers[i]);
        var user = { Item1: item.val(), Item2: item.text() };
        users.push(user);
    }


    var attachments = (this.WBFiles.files != null) ? this.WBFiles.files.length : 0;
    var tag = this.WBExpertise.options[this.WBExpertise.selectedIndex].text;

    var workload = { id: this.WBID.value, title: this.WBTitle.value, start: this.WBStartDate.value, end: this.WBEndDate.value, hours: 0, attachments: attachments, tag: tag, state: 0, users: users };

    validateForm(e, data, function (e, data) {
        DisableWorkloadFields();
        $('#msg').text('Wait...');
        $('#msg').fadeIn();
        $.ajax({
            url: '/Workload/Add',
            type: 'POST',
            data: data,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.IsSuccessStatusCode) {
                    $('#WorkloadModal').modal('hide');
                    //Get GUID:
                    getGUID(function (data) {
                        $('#WBID').attr('value', data);
                    });
                    createTask(workload.id, workload.title, workload.start, workload.end, workload.hours, workload.attachments, workload.tag, workload.state, workload.users /* , workload.description */);
                } else {
                    $('#msg').text('Error!');
                }
            }
        });
        e.preventDefault();
    })
}

function addWorkloadSimple(e) {
    //Gets bootstrap-switch component value:
    var value = $('#WBIsWorkload').bootstrapSwitch('state');
    //Serializes form and append bootstrap-switch value:
    var data = new FormData(this);
    data.append('WBIsWorkload', value);

    var selectedUsers = $('#WBUsers option:selected');
    var users = [];
    for (var i = 0; i < selectedUsers.length; i++) {
        var item = $(selectedUsers[i]);
        var user = { Item1: item.val(), Item2: item.text() };
        users.push(user);
    }


    var attachments = (this.WBFiles.files != null) ? this.WBFiles.files.length : 0;
    var tag = this.WBExpertise.options[this.WBExpertise.selectedIndex].text;

    var workload = { id: this.WBID.value, title: this.WBTitle.value, start: this.WBStartDate.value, end: this.WBEndDate.value, hours: 0, attachments: attachments, tag: tag, state: 0, users: users };

    validateFormSimple(e, data, function (e, data) {
        DisableWorkloadFields();
        $('#msg').text('Wait...');
        $('#msg').fadeIn();
        $.ajax({
            url: '/Workload/AddSimple',
            type: 'POST',
            data: data,
            processData: false,
            contentType: false,
            success: function (response) {

                if (response) {
                    $('#WorkloadModal').modal('hide');
                    //Get GUID:
                    getGUID(function (data) {
                        $('#WBID').attr('value', data);
                    });

                    // hack
                    var user = { Item1: response.WBUsers[0], Item2: 'not-filled' };
                    var workload = { id: response.WBID, title: response.WBTitle, start: response.WBStartDate, end: response.WBEndDate, hours: 0, attachments: null, tag: response.WBExpertise, state: 0, users: [user] };
                    
                    createTask(workload.id, workload.title, workload.start, workload.end, workload.hours, workload.attachments, workload.tag, workload.state, workload.users);
                } else {
                    $('#msg').text('Error!');
                }
            }
        });
        e.preventDefault();
    })
}
function updateWorkload(e) {
    var id = this.WBID.value;

    //Gets bootstrap-switch component value:
    var value = $('#WBIsWorkload').bootstrapSwitch('state');
    //Serializes form and append bootstrap-switch value:
    var data = new FormData(this);
    data.append('WBIsWorkload', value)
    //Append previous files:
    var files = $('#filesList div a.filePrev');
    for (var i = 0; i < files.length; i++) {
        data.append('oldFiles', files[i].getAttribute("fileid") + '&' + files[i].href + '&' + files[i].text);
    }
    var selectedUsers = $('#WBUsers option:selected');
    var users = [];
    for (var i = 0; i < selectedUsers.length; i++) {
        var item = $(selectedUsers[i]);
        var user = { Item1: item.val(), Item2: item.text() };
        users.push(user);
    }
    var state = $('#' + id).parent().data('state');
    data.append('WBStatus', state);
    var attachments = (this.WBFiles.files != null) ? this.WBFiles.files.length : 0;
    var tag = this.WBExpertise.options[this.WBExpertise.selectedIndex].text;

    var workload = { id: this.WBID.value, title: this.WBTitle.value, start: this.WBStartDate.value, end: this.WBEndDate.value, attachments: attachments, tag: tag, users: users };

    // hack: disable validateForm for V2 (and for the legacy code as well)
    //validateForm(e, data, function (e, data) {
        DisableWorkloadFields();
        $('#msg').text('Wait...');
        $.ajax({
            url: '/Workload/Update',
            type: 'PUT',
            data: data,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.IsSuccessStatusCode) {
                    $('#WorkloadModal').modal('hide');
                    updateTaskInFolder(workload.id, workload.title, workload.start, workload.end, workload.attachments, workload.tag, workload.users);
                } else {
                    $('#msg').text('Error!');
                }
            }
        });
        e.preventDefault();
    //})
}
function deleteWorkload() {
    var workloadID = $('#WBID').val();
    $('#msg').text('Wait...');

    $.ajax({
        url: '/Workload/Delete?=' + workloadID,
        type: 'DELETE',
        success: function (response) {
            if (response.IsSuccessStatusCode) {
                $('#' + workloadID).remove();
                $('#WorkloadModal').modal('hide');
            } else {
                $('#msg').text('Error!');
            }
        }
    });
}


function changeComplexity(e) {
    var value = $(this).val();
    var txt = '';
    switch (value) {
        case '1':
            txt = 'Very Low';
            break;
        case '2':
            txt = 'Low';
            break;
        case '3':
            txt = 'Medium';
            break;
        case '4':
            txt = 'High';
            break;
        case '5':
            txt = 'Very High';
            break;
    }
    $('#ComplexityLevel').text(txt);
}