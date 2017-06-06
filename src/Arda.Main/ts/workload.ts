//Initialize:
declare var $ : any
declare var folders : any;

//Kanban:

// initialize
function RefreshTaskList() {
    var selected_user = $('select[name=filter-assign] option:selected').val();
    var selected_type = $('input[name=type]:checked').val();
    var filter_user = selected_user; // (selected_user.length > 0) ? el.target.selectedOptions[0].value : null;
    var filter_type = (selected_type == 2); // is BACKLOG?

    // alert('filter: ' + filter_type + ', user = ' + filter_user);

    loadTaskList(filter_type, filter_user);
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


// task
function dragstart(ev) {
    ev.dataTransfer.setData('text', ev.target.id);
}

function dragover(ev) {
    ev.preventDefault();
}

// folder
function drop(ev) {
    var target = this;
    ev.preventDefault();
    var data = ev.dataTransfer.getData('text');

    var elem = document.getElementById(data);
    target.appendChild(elem);

    var state = target.dataset['state'];
    var numstate = state | 0;

    var taskId = data;

    // remove the underscore
    if(taskId[0] == '_') {
        taskId = taskId.slice(1);
    }

    var task = { Id: taskId, State: numstate };

    update(task);
}

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

    var taskId = id;

    // remove the underscore
    if(taskId[0] == '_') {
        taskId = taskId.slice(1);
    }

    var task = { Id: taskId, State: state };
    update(task);
}

function createTask(id, title, start, end, hours, attachments, tag, state, users) {
    var task_state = '.state' + state;
    createTaskInFolder(id, title, start, end, hours, attachments, tag, task_state, users);
}

// task
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

    // fix problems
    var validIdName = '_' + taskId; // avoid issues when taskId starts with numbers
    var userArray = users.map( item => item.Item1 );

    var taskProp = { id: validIdName, title: taskTitle, dateStart: start, dateEnd: end, users: userArray };

    ReactDOM.render(React.createElement(TemplateTask, taskProp), document.getElementById(validIdName) );
}

// task callback
function taskedit(id) {
    detailsWorkloadState(this, id);
}

// task done -- can't find references to it
function taskdone(id) {
    moveTask(id, 2);
}

