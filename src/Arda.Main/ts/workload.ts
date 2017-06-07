//Initialize:
declare var $ : any

// initialize
function InitializeKanban() {
    ReactDOM.render(React.createElement(DashboardFolderHeader, null), document.getElementById('dashboard-folder-header') );
        
    gettasklist(function (tasklist) {
        tasklist.map(function (task) {
            createTask(task.id, task.title, task.start, task.end, task.hours, task.attachments, task.tag, task.status, task.users /* , task.description */);
        });

        ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders') );            
    });

}

function createTask(id, title, start, end, hours, attachments, tag, state, users) {
    var userArray = users.map( item => item.Item1 );
    var taskProp = { id: id, title: title, dateStart: start, dateEnd: end, users: userArray, state: state };

    folderM[state].tasks.push(taskProp);
    dictM[id] = taskProp;
}


function createTaskSimple(id, title, state, start, end, users) {
    var taskProp = { id: id, title: title, dateStart: start, dateEnd: end, users: users, state: state };

    folderM[state].tasks.unshift(taskProp);
    dictM[id] = taskProp;
}

function updateDashboard() {
    ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders') ); 
}

function updateTaskInFolder(id, title, state, start, end, users) {

    var task = dictM[id];

    task.title = title;
    task.dateStart = start;
    task.dateEnd = end;
    task.users = users;
    task.state = state;

}

function deleteTaskInFolder(id) {

    var task = dictM[id];
    var curState = task.state;
    folderM[curState].remove(task);
    dictM[id] = null;

}

// task callback
function taskedit(id) {
    detailsWorkloadState(this, id);
}
