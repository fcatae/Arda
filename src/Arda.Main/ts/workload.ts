//Initialize:
declare var $ : any

// initialize
function InitializeKanban() {
    ReactDOM.render(React.createElement(DashboardFolderHeader, null), document.getElementById('dashboard-folder-header') );
    ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders') );
}

function RefreshTaskList() {
    
    gettasklist(function (tasklist) {
        tasklist.map(function (task) {
            createTask(task.id, task.title, task.start, task.end, task.hours, task.attachments, task.tag, task.status, task.users /* , task.description */);
        });

        ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders') );            
    });

}

function createTask(id, title, start, end, hours, attachments, tag, state, users) {
    var validIdName = '_' + id; // avoid issues when taskId starts with numbers
    var userArray = users.map( item => item.Item1 );
    var taskProp = { id: validIdName, title: title, dateStart: start, dateEnd: end, users: userArray };

    folderM[state].tasks.push(taskProp);
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
    detailsWorkloadState(null, id);
}
