//Initialize:
declare var $ : any

//Global Variables:
var folders = $('.folder');

//Kanban:

// initialize
function InitializeKanban() {
    ReactDOM.render(React.createElement(DashboardFolderHeader, null), document.getElementById('dashboard-folder-header') );
    ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders') );
}


function RefreshTaskList() {
    loadTaskList();
}

function loadTaskList() {

    gettasklist(function (tasklist) {
        tasklist.map(function (task) {
            createTask(task.id, task.title, task.start, task.end, task.hours, task.attachments, task.tag, task.status, task.users /* , task.description */);
        });

        ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders') );            
    });

}

// task
function dragstart(ev) {
    ev.dataTransfer.setData('text', ev.target.id);
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
    // fix problems
    var validIdName = '_' + id; // avoid issues when taskId starts with numbers
    var userArray = users.map( item => item.Item1 );
    var taskProp = { id: validIdName, title: title, dateStart: start, dateEnd: end, users: userArray };

    folderM[state].tasks.push(taskProp);

    // var task_state = '.state' + state;
    // createTaskInFolder(id, title, start, end, hours, attachments, tag, task_state, users);
}

// task
// function createTaskInFolder(taskId, taskTitle, start, end, hours, attachments, tag, folderSelector, users) {

//     // <div id="templateId" class="task" draggable="true" data-toggle="modal" data-target="#WorkloadModal"></div>
//     var elemTask = document.querySelector('#templateTask') as HTMLTemplateElement;
//     var content = elemTask.content; 
//     var clone = document.importNode(content, true);
    
//     var folder = document.querySelector(folderSelector);

//     clone.querySelector('.task').id = taskId;

//     clone.querySelector('.task').addEventListener('dragstart', dragstart);
//     clone.querySelector('.task').addEventListener('click', function () { taskedit(taskId) });

//     // fix problems
//     var validIdName = '_' + taskId; // avoid issues when taskId starts with numbers
//     var userArray = users.map( item => item.Item1 );

//     clone.querySelector('.task').id = validIdName;

//     folder.appendChild(clone, true);

//     var taskProp = { id: validIdName, title: taskTitle, dateStart: start, dateEnd: end, users: userArray };

//     ReactDOM.render(React.createElement(TemplateTask, taskProp), document.getElementById(validIdName) );
// }

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

