interface ITaskLegacyItem {
    id;
    title;
    dateStart?;
    dateEnd?;
    users?;
}

interface IFolderModel {
    state: number;
    tasks: ITaskLegacyItem[];
    callback: any;    
}

class TemplateHeader extends React.Component<{title},{}> {
   render() {
       return   <div className="folder-header">
                    <p><span className="templateTitle">{this.props.title}</span></p>
                </div>;
   }
}

class TemplateBody extends React.Component<ITaskLegacyItem,{}> {
    
    formatDate(dateStr: string) {
        // HACK
        if(dateStr.length<=10) return dateStr;
        
        var date = new Date(dateStr);
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        var str = month + '/' + day + '/' + year;
        return str;
    }

   render() {
        return <div className="folder-body">
                    <p>
                        <i className="fa fa-calendar fa-task-def" aria-hidden="true"></i>
                        <span className="templateStart">
                            {this.formatDate(this.props.dateStart)}   
                        </span>

                        <i className="fa fa-calendar-check-o fa-task-def" aria-hidden="true"></i>
                        <span className="templateEnd">
                            {this.formatDate(this.props.dateEnd)}
                        </span>
                    </p>
                </div>;
   }
}

class TemplateFooter extends React.Component<{users: string[]},{}> {
   render() {
       return   <div className="folder-footer">
                    {
                        ( this.props.users ) ? 
                            this.props.users.map( email => <img key={email} className="user" src={'/users/photo/' + email}></img>) 
                            : null
                    }
                </div>;
   }
}

class TemplateTask extends React.Component<ITaskLegacyItem,{}> {

    dragstart(ev) {
        ev.dataTransfer.setData('text', this.props.id);
    }    

    onclick() {
        taskedit(this.props.id);
    }

   render() {
       return   <div className="task"      
                        id={'_'+this.props.id}                   
                        key={this.props.id}
                        draggable={true} 
                        data-toggle="modal" data-target="#WorkloadModal"
                        onDragStart={this.dragstart.bind(this)} 
                        onClick={this.onclick.bind(this)}>

                    <div className="folder-tasks">
                        <TemplateHeader title={this.props.title}></TemplateHeader>
                        <TemplateBody {...this.props}></TemplateBody>
                        <TemplateFooter users={this.props.users}></TemplateFooter>
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
                                <button id="btnNewSimple" className="ds-button-update" data-toggle="modal" data-target="#WorkloadModal" onClick={newWorkloadStateSimple}><i className="fa fa-plus" aria-hidden="true"></i> Quick Create</button>
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
    constructor() {
        this.tasks = [];
    }
    public tasks: ITaskLegacyItem[];

    add(task: ITaskLegacyItem) {
        this.tasks.push(task);
    }

    remove(task: ITaskLegacyItem) {
        let index = this.tasks.indexOf(task);
        
        (index >=0) && this.tasks.splice(index, 1);
    }
}

var folderM = [new FolderModel(), new FolderModel(), new FolderModel(), new FolderModel()];
var dictM = {};

class Folder extends React.Component<IFolderModel,{}> {

   allowDrop(ev) {
       ev.preventDefault();
   }
   drop(ev) {
        ev.preventDefault();
        
        var numstate = this.props.state; 
        var taskId = ev.dataTransfer.getData('text');;

        (this.props.callback) && this.props.callback(taskId, numstate); 
   }

   render() {
       return   <div className="col-xs-12 col-md-3 dashboard-panel" style={ {overflowY: 'scroll'} }>
                    <div className="folder" onDragOver={this.allowDrop} onDrop={this.drop.bind(this)}>
                        {
                            (this.props.tasks) ? 
                                this.props.tasks.map( t => <TemplateTask key={t.id} {...t}></TemplateTask>)
                                : null
                        }
                    </div>
                </div>
   }
}

class DashboardFolders extends React.Component<{},{}> {
    moveTask(id, nextState) {
        var task = dictM[id];
        var curState = task.state;
        
        var callback = null;
        // call the update API
        update({ Id: id, State: nextState }, callback);    

        // update the folder states
        folderM[curState].remove(task);
        folderM[nextState].add(task);

        // update the task state
        task.state = nextState;

        // update react
        this.forceUpdate();
    }

   render() {       
       return   <div>
                    <Folder state={0} tasks={folderM[0].tasks} callback={this.moveTask.bind(this)}></Folder>
                    <Folder state={1} tasks={folderM[1].tasks} callback={this.moveTask.bind(this)}></Folder>
                    <Folder state={2} tasks={folderM[2].tasks} callback={this.moveTask.bind(this)}></Folder>
                    <Folder state={3} tasks={folderM[3].tasks} callback={this.moveTask.bind(this)}></Folder>
                </div>
   }
}
