interface ITaskLegacyItem {
    id;
    title;
    dateStart?;
    dateEnd?;
    users?;
}

class TemplateHeader extends React.Component<{title},{}> {
   render() {
       return   <div className="folder-header">
                    <p><span className="templateTitle">{this.props.title}</span></p>
                </div>;
   }
}

class TemplateBody extends React.Component<ITaskLegacyItem,{}> {
   render() {
        return <div className="folder-body">
            <p>
                <i className="fa fa-calendar fa-task-def" aria-hidden="true">
                </i>
                <span className="templateStart">
                    {this.props.dateStart}   
                </span>
                <i className="fa fa-calendar-check-o fa-task-def" aria-hidden="true">
                </i>
                <span className="templateEnd">
                    {this.props.dateEnd}
                </span>
            </p>
        </div>;
   }
}

class TemplateFooter extends React.Component<{users: string[]},{}> {
   render() {       
       var userImages = null;
       
       if( this.props.users ) {
           userImages = this.props.users.map( email => <img key={email} className="user" src={'/users/photo/' + email}></img>)
       }

       return   <div className="folder-footer">
                    {userImages}
                </div>;
   }
}

class TemplateTask extends React.Component<ITaskLegacyItem,{}> {
   render() {
       var users = this.props.users;

       return   <div className="folder-tasks" id={this.props.id}>
                    <TemplateHeader title={this.props.title}></TemplateHeader>
                    <TemplateBody {...this.props}></TemplateBody>
                    <TemplateFooter users={users}></TemplateFooter>
                </div>;
   }
}

class TemplateTask2 extends React.Component<ITaskLegacyItem,{}> {

    dragstart(ev) {
        ev.dataTransfer.setData('text', this.props.id);
    }    

   render() {
       var users = this.props.users;
       var validIdName = '_' + this.props.id; // avoid issues when taskId starts with numbers
       var taskId = this.props.id;
       return   <div id={validIdName} className="task" draggable={true} data-toggle="modal" data-target="#WorkloadModal" 
                                                        onDragStart={this.dragstart.bind(this)} onClick={function () { taskedit(taskId) }}>
                    <div className="folder-tasks" id={this.props.id}>
                        <TemplateHeader title={this.props.title}></TemplateHeader>
                        <TemplateBody {...this.props}></TemplateBody>
                        <TemplateFooter users={users}></TemplateFooter>
                    </div>
                </div>;
   }
}

class DashboardFolderHeader extends React.Component<{},{}> {
    render() {
        return <div className="row">
                        <div className="col-xs-12 col-md-3">
                            <div className="row">
                                <h3 className="dashboard-panel-title dashboard-panel-title--todo">todo</h3>
                                <button id="btnNewSimple" className="ds-button-update" data-toggle="modal" data-target="#WorkloadModal"><i className="fa fa-plus" aria-hidden="true"></i> Quick Create</button>
                            </div>
                        </div>
                        <div className="col-xs-12 col-md-3">
                            <div className="row">
                                <h3 className="dashboard-panel-title dashboard-panel-title--doing">doing</h3>
                            </div>
                        </div>
                        <div className="col-xs-12 col-md-3">
                            <div className="row">
                                <h3 className="dashboard-panel-title dashboard-panel-title--done">done</h3>
                            </div>
                        </div>
                        <div className="col-xs-12 col-md-3">
                            <div className="row">
                                <h3 className="dashboard-panel-title dashboard-panel-title--approved">archive</h3>
                            </div>
                        </div>
                        <div className="clearfix"></div>
                    </div>;
    }
}

class FolderModel {
    constructor(state) {
        this.state = state;
        this.tasks = [];
    }
    public state: number;
    public tasks: ITaskLegacyItem[];
}
var folderM0 = new FolderModel(0);
var folderM1 = new FolderModel(1);
var folderM2 = new FolderModel(2);
var folderM3 = new FolderModel(3);
var folderM = [folderM0, folderM1, folderM2, folderM3];

class Folder extends React.Component<{taskState: number, model: FolderModel},{}> {

   allowDrop(ev) {
       ev.preventDefault();
   }
   drop(ev) {
    ev.preventDefault();
    
    // jquery-alike
    var data = ev.dataTransfer.getData('text');
    var elem = document.getElementById('_' + data);
    var target = document.querySelector('.folder.state' + this.props.taskState) as HTMLDivElement;
    target.appendChild(elem);

    // react
    var numstate = this.props.taskState; 

    var taskId = ev.dataTransfer.getData('text');;

    // remove the underscore
    if(taskId[0] == '_') {
        taskId = taskId.slice(1);
    }

    var task = { Id: taskId, State: numstate };

    update(task);        
   }

   render() {       
       var state = this.props.model.state;
       var className = "folder state" + this.props.model.state.toString();

       var tasks = null;
       if(this.props.model.tasks) {
           
           tasks = this.props.model.tasks.map( t => <TemplateTask2 key={t.id} {...t}></TemplateTask2>)
       }

       return   <div className="col-xs-12 col-md-3 dashboard-panel" data-simplebar-direction="vertical">
                    <div className={className} data-state={state} onDragOver={this.allowDrop} onDrop={this.drop.bind(this)}>
                        {tasks}
                    </div>
                </div>
   }
}

class DashboardFolders extends React.Component<{},{}> {
   render() {       
       return   <div>
                    <Folder taskState={0} model={folderM0}></Folder>
                    <Folder taskState={1} model={folderM1}></Folder>
                    <Folder taskState={2} model={folderM2}></Folder>
                    <Folder taskState={3} model={folderM3}></Folder>
                </div>
   }
}

