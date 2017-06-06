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
       var userImages = this.props.users.map( email => <img key={email} className="user" src={'/users/photo/' + email}></img>)

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

class Folder extends React.Component<{taskState: number},{}> {
   render() {       
       var className = "folder state" + this.props.taskState.toString();
       var state = this.props.taskState;

       return   <div className="col-xs-12 col-md-3 dashboard-panel" data-simplebar-direction="vertical">
                    <div className={className} data-state={state}></div>
                </div>
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


class DashboardFolders extends React.Component<{},{}> {
   render() {       
       return   <div>
                    <Folder taskState={0}></Folder>
                    <Folder taskState={1}></Folder>
                    <Folder taskState={2}></Folder>
                    <Folder taskState={3}></Folder>
                </div>
   }
}

