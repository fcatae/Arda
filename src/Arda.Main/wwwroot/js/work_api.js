//Initialize:

function Initialize() {
    //Click events:
    //New Workload:
    $('#btnNew').click(newWorkloadState);
    $('#btnNewSimple').click(newWorkloadStateSimple);

    //Workload Details:
    $('#btnDetails').click(detailsWorkloadState);
    //Reset Button:
    $('#btnWorkloadReset').click(resetWorkloadForm);
    //Delete Button:
    $('#btnWorkloadDelete').click(deleteWorkload);
    //Cancel Button:
    $('#btnWorkloadCancel').click(function () {
        $('#WorkloadModal').modal('hide');
    });


    //Other events:
    $('#WBComplexity').on('change', changeComplexity);

    //Search:
    $('#search-box').on('keyup', function () {
        var matcher = new RegExp($(this).val(), 'gi');
        $('.task').show().not(function () {
            return matcher.test($(this).find('.templateTitle').text())
        }).hide();
    })

    //Components:
    $("#WBIsWorkload").bootstrapSwitch();

    $('#WBStartDate').datepicker({
        format: "mm/dd/yyyy",
        autoclose: true,
        todayHighlight: true
    });

    $('#WBEndDate').datepicker({
        format: "mm/dd/yyyy",
        autoclose: true,
        todayHighlight: true
    });

    $("#WBComplexity").ionRangeSlider({
        min: 1,
        max: 5,
        hide_min_max: true,
        hide_from_to: true,
        grid: false,
        keyboard: true
    });
}

function InitializeFields() {
    //Load values:
    //Get All Activities:
    $.getJSON('/activity/GetActivities', null, callbackGetActivities);
    //Get User Technologies:
    $.getJSON('/technology/GetTechnologies', null, callbackGetTechnologies);
    //Get User Metrics:
    $.getJSON('/metric/GetMetrics', null, callbackGetMetrics);
    //Get User Users:
    $.getJSON('/users/GetUsers', null, callbackGetUsers);
}

function InitializeKanban() {
    //Board Initialization
    folders.map(function (i, folder) {
        folder.addEventListener('dragover', dragover);
        folder.addEventListener('drop', drop.bind(folder));
    });

    $('.dashboard-filter-field').change(function () {
        RefreshTaskList();
    });

    if (window.hackIsAdmin != null) {
        GetUserList();
        $('#filter-assign').change(function () {
            RefreshTaskList();
        });
    }
    else {
        $('#filter-assign').css('visibility', 'hidden');
    }
}

//Kanban:

function dragstart(ev) {
    ev.dataTransfer.setData('text', ev.target.id);
}

function dragover(ev) {
    ev.preventDefault();
}

function drop(ev) {
    var target = this;
    ev.preventDefault();
    var data = ev.dataTransfer.getData('text');
    var elem = document.getElementById(data);
    target.appendChild(elem);

    var state = target.dataset['state'];
    var numstate = state | 0;
    var task = { Id: elem.id, State: numstate };

    update(task);
}

//var tasks = $('.task');
//tasks.map(function (i, task) {
//    task.draggable = true;
//    task.addEventListener('dragstart', dragstart);
//});

function clearFolder(state) {
    var task_state = '.state' + state;
    var folder = document.querySelector(task_state);
    $(folder).empty();
}

function clearTasks() {
    clearFolder('0');
    clearFolder('1');
    clearFolder('2');
    clearFolder('3');
}

function moveTask(id, state) {
    var task_state = '.state' + state;
    var folder = document.querySelector(task_state);
    var taskElem = document.getElementById(id);

    folder.appendChild(taskElem);

    var task = { Id: id, State: state };
    update(task);
}

function createTask(id, title, start, end, hours, attachments, tag, state, users, description) {
    var task_state = '.state' + state;
    createTaskInFolder(id, title, start, end, hours, attachments, /*tag,*/ task_state, users, description);
}

function getUserImageTask(user, taskId) {
    //var url = '/users/GetUserPhoto?=' + user;
    //var url = '/users/photo/' + user;
    //$.ajax({
    //    url: url,
    //    type: "GET",
    //    cache: true,
    //    success: function (data) {
    //        img = $('<img class="user">').attr('src', data);
    //        $('#' + taskId + ' .folder-tasks .folder-footer').append(img);
    //    }
    //});

    var url = '/users/photo/' + user;
    img = $('<img class="user">').attr('src', url);
    $('#' + taskId + ' .folder-tasks .folder-footer').append(img);
}

function createTaskInFolder(taskId, taskTitle, start, end, hours, attachments, /*tag,*/ folderSelector, users, description) {
    var content = document.querySelector('#templateTask').content;
    var clone = document.importNode(content, true);
    var folder = document.querySelector(folderSelector);

    clone.querySelector('.task').id = taskId;
    // clone.querySelector('.task').classList.add(tag);

    clone.querySelector('.task .templateTitle').textContent = taskTitle;

    if (clone.querySelector('.hack-force-hide-template-layout') == null) {
        clone.querySelector('.task .templateStart').textContent = start;
        clone.querySelector('.task .templateEnd').textContent = end;
        clone.querySelector('.task .templateHours').textContent = hours;
        clone.querySelector('.task .templateAttachments').textContent = attachments;
        clone.querySelector('.task .templateDescription').innerHTML = description;
    } 
    else {
        // will not work
        //clone.querySelector('.task .templateDescription').textContent = description;
    }

    clone.querySelector('.task').addEventListener('dragstart', dragstart);
    clone.querySelector('.task').addEventListener('click', function () { taskedit(taskId) });

    folder.appendChild(clone, true);

    if (clone.querySelector('.hack-force-hide-template-layout') == null) {
        $.each(users, function (index, value) {
            getUserImageTask(value.Item1, taskId);
        });
    } 
}

function updateTaskInFolder(taskId, taskTitle, start, end, attachments, tag, users) {
    var task = $('#' + taskId);

    $('#' + taskId + ' .templateTitle').text(taskTitle);
    $('#' + taskId + ' .templateStart').text(start);
    $('#' + taskId + ' .templateEnd').text(end);
    $('#' + taskId + ' .templateAttachments').text(attachments);

    $('#' + taskId + ' .folder-tasks .folder-footer img').remove();

    $.each(users, function (index, value) {
        getUserImageTask(value.Item1, taskId);
    });
}

function httpCall(action, url, data, callback, error) {

    $.ajax({
        type: action, // GET POST PUT
        url: url,
        data: JSON.stringify(data),
        cache: false,
        contentType: 'application/json',
        dataType: 'json',
        success: callback,
        error: error,
        processData: false
    });

}

function taskedit(id) {
    detailsWorkloadState(this, id);
}

function taskdone(id) {
    moveTask(id, 2);
}

function gettasklist(callback, type, user) {

    var filter_user = user ? '?tag=' + user : '';
    var filter_type = type ? '/ListBacklogsByUser' : '/ListWorkloadsByUser';

    httpCall('GET', '/Workload' + filter_type + filter_user, null, callback);
}

function update(task) {
    httpCall('PUT', '/Workload/UpdateStatus?id=' + task.Id + '&status=' + task.State, task, function (data) {
        // done
    })
}

function RefreshTaskList() {
    var selected_user = $('select[name=filter-assign] option:selected').val();

    var selected_type = $('input[name=type]:checked').val();
    var filter_user = selected_user; // (selected_user.length > 0) ? el.target.selectedOptions[0].value : null;
    var filter_type = (selected_type == 2); // is BACKLOG?

    // alert('filter: ' + filter_type + ', user = ' + filter_user);

    loadTaskList(filter_type, filter_user);
}

function GetUserList() {
    var url = '/Users/ViewRestrictedUserList';
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        success: function (data, textStatus, jqXHR) {

            var userListElem = $('#filter-assign');

            data.map(function (user) {
                //alert(JSON.stringify(user));
                var name = user.Name;
                var id = user.Email;
                var opt = new Option(name, id);
                userListElem.append(opt);
            })
        }
    });
}

function loadTaskList(filter_type, filter_user) {
    //alert(filter_user);

    clearTasks();

    gettasklist(function (tasklist) {
        tasklist.map(function (task) {
            createTask(task.id, task.title, task.start, task.end, task.hours, task.attachments, task.tag, task.status, task.users, task.textual);
        });
    },
        filter_type,
        filter_user
    );
}


//Workloads:

//Workload Modal states:

function newWorkloadState() {
    //Clean values:
    resetWorkloadForm();
    EnableWorkloadFields();

    // force 0 to clean up
    $('#WBID').attr('value', '00000000-0000-0000-0000-000000000000');

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

    // force 0 to clean up
    $('#WBID').attr('value', '00000000-0000-0000-0000-000000000000');

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
    $('#btnWorkloadAddAppointment').removeClass('hidden');

    $("#btnWorkloadCancel").removeAttr("disabled");

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
    $('#btnWorkloadAddAppointment').removeClass('hidden');

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
    $('#btnWorkloadAddAppointment').addClass('hidden');

}

//Microsoft Graph API calls:

function GetImage(user, token) {
    var url = "https://graph.microsoft.com/v1.0/me/photo/$value";
    var elem = "userImg";
    var auth = 'bearer ' + token;
    GetImageBase64FromGraph(url, elem, auth);
}

function GetImageBase64FromGraph(url, element, token) {
    var request = new XMLHttpRequest;
    request.open("GET", url);
    request.setRequestHeader("Authorization", token);
    request.responseType = "blob";
    request.onload = function () {
        if (request.readyState === 4 && request.status === 200) {
            var image = document.getElementById(element);
            var reader = new FileReader();
            reader.onload = function () {
                image.src = reader.result;
                updateImgOnDatabase();
            };
            reader.readAsDataURL(request.response);
        }
    };
    request.send(null);
}

//Util:

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

function formatDate(dateStr, callback) {
    var date = new Date(dateStr);
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();
    var str = month + '/' + day + '/' + year;
    callback(str)
}

function getGUID(callback) {
    $.ajax({
        url: '/Workload/GetGuid',
        type: 'GET',
        processData: false,
        contentType: false,
        cache: false,
        success: function (data) {
            callback(data);
        }
    });
}

//Callbacks:

function callbackGetActivities(data) {
    var options = [];
    options.push('<option selected disabled value="-1">Select the activity</option>');
    for (var i = 0; i < data.length; i++) {
        var text = data[i].ActivityName;
        var key = data[i].ActivityID;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    $('#WBActivity').html(options.join(''));
}

function callbackGetMetrics(data) {
    var options = [];
    var select = $('#WBMetrics');
    for (var i = 0; i < data.length; i++) {
        var text = '[' + data[i].MetricCategory + '] ' + data[i].MetricName;
        var key = data[i].MetricID;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    select.html(options.join(''));

    select.multiselect({
        buttonWidth: '100%',
        numberDisplayed: 1,
        nonSelectedText: 'Click to select the metrics'
    });
}

function callbackGetTechnologies(data) {
    var options = [];
    var select = $('#WBTechnologies');
    for (var i = 0; i < data.length; i++) {
        var text = data[i].TechnologyName;
        var key = data[i].TechnologyID;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    select.html(options.join(''));

    select.multiselect({
        buttonWidth: '100%',
        numberDisplayed: 2,
        nonSelectedText: 'Click to select the technologies'
    });
}

function callbackGetUsers(data) {
    var options = [];
    var select = $('#WBUsers');
    for (var i = 0; i < data.length; i++) {
        var text = data[i].Name;
        var key = data[i].UniqueName;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    select.html(options.join(''));

    select.multiselect({
        buttonWidth: '100%',
        numberDisplayed: 2,
        nonSelectedText: 'Click to select the users'
    });
}

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
    var selected_work1 = $('#filter-assign-upskill').val();
    var selected_work2 = $('select[name=filter-assign] option:selected').val();

    var selected_work = (selected_work1 != null) ? selected_work1 : selected_work2;

    //Serializes form and append bootstrap-switch value:
    var data = new FormData(this);
    data.append('WBIsWorkload', value);
    data.append('Tag', selected_work);

    var selectedUsers = $('#WBUsers option:selected');
    var users = [];
    for (var i = 0; i < selectedUsers.length; i++) {
        var item = $(selectedUsers[i]);
        var user = { Item1: item.val(), Item2: item.text() };
        users.push(user);
    }


    var attachments = (this.WBFiles.files != null) ? this.WBFiles.files.length : 0;

    // this tag is used for CSS
    // var tag = this.WBExpertise.options[this.WBExpertise.selectedIndex].text;
    var tag = 'workload-task';

    var workload = { id: this.WBID.value, title: this.WBTitle.value, start: this.WBStartDate.value, end: this.WBEndDate.value, hours: 0, attachments: attachments, tag: tag, state: 0, users: users, description: '' };

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

                    // force 0 to clean up
                    $('#WBID').attr('value', '00000000-0000-0000-0000-000000000000');

                    //Get GUID:
                    getGUID(function (data) {
                        $('#WBID').attr('value', data);
                    });
                    createTask(workload.id, workload.title, workload.start, workload.end, workload.hours, workload.attachments, workload.tag, workload.state, workload.users, workload.description);
                } else {
                    $('#msg').text('Error!');

                    $('#WBID').attr('value', '00000000-0000-0000-0000-000000000000');
                    getGUID(function (data) {
                        $('#WBID').attr('value', data);
                    });
                }
            }
        });
        e.preventDefault();
    })
}

function addWorkloadSimple(e) {
    //Gets bootstrap-switch component value:
    var value = $('#WBIsWorkload').bootstrapSwitch('state');
    var selected_work1 = $('#filter-assign-upskill').val();
    var selected_work2 = $('select[name=filter-assign] option:selected').val();

    var selected_work = (selected_work1 != null) ? selected_work1 : selected_work2;

    //Serializes form and append bootstrap-switch value:
    var data = new FormData(this);
    data.append('WBIsWorkload', value);
    data.append('Tag', selected_work);

    var selectedUsers = $('#WBUsers option:selected');
    var users = [];
    for (var i = 0; i < selectedUsers.length; i++) {
        var item = $(selectedUsers[i]);
        var user = { Item1: item.val(), Item2: item.text() };
        users.push(user);
    }


    var attachments = (this.WBFiles.files != null) ? this.WBFiles.files.length : 0;
    // this tag is used for CSS
    // var tag = this.WBExpertise.options[this.WBExpertise.selectedIndex].text;
    var tag = 'workload-task';

    var workload = { id: this.WBID.value, title: this.WBTitle.value, start: this.WBStartDate.value, end: this.WBEndDate.value, hours: 0, attachments: attachments, tag: tag, state: 0, users: users, description: '' };

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

                    // force 0 to clean up
                    $('#WBID').attr('value', '00000000-0000-0000-0000-000000000000');

                    //Get GUID:
                    getGUID(function (data) {
                        $('#WBID').attr('value', data);
                    });

                    // hack
                    var user = { Item1: response.WBUsers[0], Item2: 'not-filled' };
                    var workload = { id: response.WBID, title: response.WBTitle, start: response.WBStartDate, end: response.WBEndDate, hours: 0, attachments: null, tag: response.WBExpertise, state: 0, users: [user], description: '' };
                    
                    createTask(workload.id, workload.title, workload.start, workload.end, workload.hours, workload.attachments, workload.tag, workload.state, workload.users);
                } else {
                    $('#msg').text('Error!');
                    $('#WBID').attr('value', '00000000-0000-0000-0000-000000000000');
                    getGUID(function (data) {
                        $('#WBID').attr('value', data);
                    });
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

function updateImgOnDatabase() {
    var img = $('#userImg').attr('src');

    var data = new FormData(this);
    data.append('img', img)

    $.ajax({
        url: '/Users/PhotoUpdate',
        type: 'PUT',
        data: data,
        processData: false,
        contentType: false,
        success: function (response) {

        }
    });
}

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
