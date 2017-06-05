//Initialize:
declare var $ : any
declare var folders : any;

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

    ReactDOM.render(React.createElement(App, null), document.querySelector('#' + id + ' .app') );

    var task = { Id: id, State: state };
    update(task);
}

function createTask(id, title, start, end, hours, attachments, tag, state, users) {
    var task_state = '.state' + state;
    createTaskInFolder(id, title, start, end, hours, attachments, tag, task_state, users);
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
    var img = $('<img class="user">').attr('src', url);
    $('#' + taskId + ' .folder-tasks .folder-footer').append(img);

}

function createTaskInFolder(taskId, taskTitle, start, end, hours, attachments, tag, folderSelector, users) {
    var elemTask = document.querySelector('#templateTask') as HTMLTemplateElement;
    var content = elemTask.content;
    var clone = document.importNode(content, true);
    var folder = document.querySelector(folderSelector);

    clone.querySelector('.task').id = taskId;

    clone.querySelector('.task').addEventListener('dragstart', dragstart);
    clone.querySelector('.task').addEventListener('click', function () { taskedit(taskId) });

    // fix problems
    var validIdName = '_' + taskId; // avoid issues when taskId starts with numbers
    var userArray = users.map( item => item.Item1 );

    clone.querySelector('.task').id = validIdName;

    folder.appendChild(clone, true);

    var taskProp = { id: validIdName, title: taskTitle, dateStart: start, dateEnd: end, users: userArray };

    ReactDOM.render(React.createElement(TemplateTask, taskProp), document.getElementById(validIdName) );
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

    var filter_user = user ? '?user=' + user : '';
    var filter_type = type ? '/ListBacklogsByUser' : '/ListWorkloadsByUser';

    httpCall('GET', '/Workload' + filter_type + filter_user, null, callback, null);
}

function update(task) {
    httpCall('PUT', '/Workload/UpdateStatus?id=' + task.Id + '&status=' + task.State, task, function (data) {
        // done
    }, null)
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
            createTask(task.id, task.title, task.start, task.end, task.hours, task.attachments, task.tag, task.status, task.users /* , task.description */);
        });
    },
        filter_type,
        filter_user
    );
}

