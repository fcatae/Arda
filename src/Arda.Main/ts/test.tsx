interface ITaskLegacyItem {
    id?;
    title;
    dateStart?;
    dateEnd?;
    users?;
}

class TemplateTitle extends React.Component<ITaskLegacyItem,{}> {
   render() {
       return <p><span className="templateTitle">{this.props.title}</span></p>;
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

class App extends React.Component<{},{}> {
   render() {
       return <div>Hello World!</div>;
   }
}

// ReactDOM.render(<App/>, document.getElementById('app'));
