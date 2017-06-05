interface ITaskLegacyItem {
    id?;
    title;
    dateStart?;
    dateEnd?;
    hours?;
    attachments?;
    tag?;
    users?;
}

class TemplateTitle extends React.Component<ITaskLegacyItem,{}> {
   render() {
       return <p><span className="templateTitle">{this.props.title}</span></p>;
   }
}
class App extends React.Component<{},{}> {
   render() {
       return <div>Hello World!</div>;
   }
}

// ReactDOM.render(<App/>, document.getElementById('app'));
