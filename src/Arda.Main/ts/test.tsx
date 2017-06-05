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

            <p>
                <i className="fa fa-clock-o fa-task-def" aria-hidden="true">
                </i>
                <span className="templateHours">
                    {/* ignore */}
                </span>
                <i className="fa fa-paperclip fa-task-def" aria-hidden="true">
                </i>
                <span className="templateAttachments">
                    {/* ignore */}
                </span>
            </p>
        </div>;
   }
}

class TemplateFooter extends React.Component<{users: string[]},{}> {
   render() {       
       var userImages = this.props.users.map( email => <img className="user" src={'/users/photo/' + email}></img>)

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

class App extends React.Component<{},{}> {
   render() {
       return <div>Hello World!</div>;
   }
}

// ReactDOM.render(<App/>, document.getElementById('app'));
