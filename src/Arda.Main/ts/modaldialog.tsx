class ModalButton extends React.Component<{className:string, display?:boolean, picture?:string, onClick: (ev?)=>void},{}> {
   render() {
       var classButtonType = this.props.className;
       var pictureClass = (this.props.picture) ? ("fa " + this.props.picture) : null;

       return   (!this.props.display) ? null :
                    <div className="data-sorting-buttons">                        
                        <button className={classButtonType} onClick={this.props.onClick}>
                            { (pictureClass) && (<i className={pictureClass} aria-hidden="true"></i>) }
                            {this.props.children}
                        </button>
                    </div>;
   }
}

function cancelButton() {
    $('#WorkloadModal').modal('hide');
}

class CancelModalButton extends React.Component<{},{}> {

    cancel() {
        $('#WorkloadModal').modal('hide');
    }

   render() { return   <div className="data-sorting-buttons">
        <button type="button" className="ds-button-reset" id="btnWorkloadCancel" onClick={this.cancel}>Cancel</button>
    </div>;}
}

class ResetModalButton extends React.Component<{},{}> {
   render() { return   <div className="data-sorting-buttons">
        <button type="button" className="ds-button-reset" id="btnWorkloadReset" onClick={resetWorkloadForm}>Reset</button>
    </div>;}
}
class EditModalButton extends React.Component<{},{}> {
   render() { return   <div className="data-sorting-buttons">
        <button type="button" className="ds-button-update" id="btnWorkloadEdit" onClick={editWorkloadState}><i className="fa fa-retweet" aria-hidden="true"></i>Edit</button>
    </div>;}
}
class DeleteModalButton extends React.Component<{display?:boolean},{}> {
   render() { 
        if(this.props.display) {
            return   <div className="data-sorting-buttons">
                        <button type="button" className="btn btn-warning" id="btnWorkloadDelete" onClick={deleteWorkloadSimple}>
                            <i className="fa fa-trash-o" aria-hidden="true"></i>Delete</button>
                    </div>;
        }
}
}
class SendAddModalButton extends React.Component<{},{}> {
   render() { return   <div className="data-sorting-buttons">
        <button type="button" className="btn btn-success" id="btnWorkloadSendAdd" onClick={submitAddWorkloadSimple}>Add</button>
    </div>;}
}
class SendUpdateModalButton extends React.Component<{},{}> {
   render() { return   <div className="data-sorting-buttons">
        <button type="button" className="btn btn-success" id="btnWorkloadSendUpdate" onClick={submitUpdateWorkloadSimple}>Update</button>
    </div>;}
}
class AddAppointmentModalButton extends React.Component<{wbid: string},{}> {
    addAppointment (ev) {
        var wbid = this.props.wbid;

        if (wbid != '') {
            window.open('/appointment/addsimple?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no');
        }
    }

   render() { return   <div className="data-sorting-buttons">
        <button type="button" className="btn btn-success" id="btnWorkloadAddAppointment" onClick={this.addAppointment}>
            <i className="fa fa-retweet" aria-hidden="true"></i>New Appointment</button>
    </div>;}
}
class LogHistoryModalButton extends React.Component<{wbid: string},{}> {
    
    showAppointments (ev) {
        var wbid = this.props.wbid;

        if (wbid != '') {
            window.open('/appointment/work?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no');
        }
    }
    
   render() { return   <div className="data-sorting-buttons">
        <button type="button" className="btn btn-success" id="btnWorkloadShowAppointment" onClick={this.showAppointments}>
            <i className="fa fa-retweet" aria-hidden="true"></i>Log History</button>
    </div>;}
}

class ModalButtonList extends React.Component<{show: any},{}> {
   render() {
       var wbid = $('#WBID').val();
       var show = this.props.show || {};
       
       function getfun(wbid) {
           return ()=>{ window.open('/appointment/work?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no'); }
       };
       function getfun2(wbid) {
           return ()=>{ window.open('/appointment/addsimple?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no'); }
       }

       return   <div>
                    <ModalButton display={show['cancel']} className="ds-button-reset" onClick={cancelButton}>CANCEL</ModalButton>
                    <ModalButton display={show['reset']} className="ds-button-reset" onClick={resetWorkloadForm}>RESET</ModalButton>
                    <ModalButton display={show['edit']} className="ds-button-update" onClick={editWorkloadState} picture="fa-retweet">EDIT</ModalButton>
                    <ModalButton display={show['delete']} className="btn btn-warning" onClick={deleteWorkloadSimple} picture="fa-trash-o">DELETE</ModalButton>

                    <ModalButton display={show['add']} className="btn btn-success" onClick={submitAddWorkloadSimple}>SENDADD</ModalButton>
                    <ModalButton display={show['update']} className="btn btn-success" onClick={submitUpdateWorkloadSimple}>SENDUPD</ModalButton>

                    <ModalButton display={show['newlog']} className="btn btn-success" onClick={getfun} picture="fa-retweet">NEWLOG</ModalButton>
                    <ModalButton display={show['history']} className="btn btn-success" onClick={getfun} picture="fa-retweet">HISTORY</ModalButton>
                </div>;
   }
}

class ModalButtonList2 extends React.Component<{show: any},{}> {
   render() {
       var wbid = $('#WBID').val();
       var show = this.props.show || {};

       return   <div>
                    <CancelModalButton></CancelModalButton>
                    <ResetModalButton></ResetModalButton>
                    <EditModalButton></EditModalButton>
                    <DeleteModalButton display={show['delete']}></DeleteModalButton>
                    <SendAddModalButton></SendAddModalButton>
                    <SendUpdateModalButton></SendUpdateModalButton>
                    <AddAppointmentModalButton wbid={wbid}></AddAppointmentModalButton>
                    <LogHistoryModalButton wbid={wbid}></LogHistoryModalButton>
                </div>;
   }
}

class ModalDialog extends React.Component<{title},{}> {
   render() {
       return   <div className="folder-header">
                    <p><span className="templateTitle">{this.props.title}</span></p>
                </div>;
   }
}

interface IModalFormState {
    wbid: string;
    startDate?: string;
    endDate?: string;
    title: string;
    description: string;
}

class ModalForm extends React.Component<IModalFormState,IModalFormState> {
    constructor(props: IModalFormState) {
        super(props);
        this.state = {
            wbid: '',
            startDate: '',
            endDate: '',
            title: '',
            description: ''
        };

        this.handleChange = this.handleChange.bind(this);
    }

    componentWillReceiveProps(props) {
        this.setState ( {
            wbid: props.wbid || '',
            startDate: props.startDate || '',
            endDate: props.endDate || '',
            title: props.title || '',
            description: props.description || ''
        });
    }

    handleChange(ev) {
        const name = ev.target.name;
        const value = ev.target.value;
        this.setState({[name]: ev.target.value});
    }

    onclick(e) {
        alert(JSON.stringify(this.state));
        e.preventDefault();
    }

   render() {       
                return  <div>
                        <div className="row" hidden>
                            <div id="#formWBID" className="col-md-9">
                                <div className="form-group">
                                    <input type="text" className="form-control" id="WBID" name="WBID" value={this.state.wbid}></input>
                                </div>
                            </div>
                        </div>
                        
                        <div className="row" hidden>
                            <div className="col-md-4">
                                <div className="form-group">
                                    <label htmlFor="WBStartDate">Start date:</label>
                                    <div className="input-group date">
                                        <input type="text" id="WBStartDate" name="startDate" className="form-control" autoComplete="off" onChange={this.handleChange}></input>
                                        <span className="input-group-addon"><i className="fa fa-calendar-check-o" aria-hidden="true"></i></span>
                                    </div>
                                </div>
                            </div>
                            <div className="col-md-4">
                                <div className="form-group">
                                    <label htmlFor="WBEndDate">End date:</label>
                                    <div className="input-group date">
                                        <input type="text" id="WBEndDate" name="endDate" className="form-control" autoComplete="off" onChange={this.handleChange}></input>
                                        <span className="input-group-addon"><i className="fa fa-calendar-check-o" aria-hidden="true"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-md-12">
                                <div className="form-group">
                                    <label htmlFor="WBTitle">Title:</label>
                                    <input type="text" className="form-control" id="WBTitle" name="title" autoComplete="off" value={this.state.title} onChange={this.handleChange} autoFocus></input>
                                </div>
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-md-12">
                                <div className="form-group">
                                    <label htmlFor="WBDescription">Description:</label>
                                    <textarea className="form-control" id="WBDescription" name="description" rows={3} value={this.state.description} onChange={this.handleChange}></textarea>
                                </div>
                            </div>
                        </div>

                        <button onClick={this.onclick.bind(this)}>DebugShow</button>
                    </div>;
   }
}

