var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
function gettasklist(callback) {
    httpCall('GET', '/Workload/ListWorkloadsByUser', null, callback, null);
}
function update(task, callback) {
    httpCall('PUT', '/Workload/UpdateStatus?id=' + task.Id + '&status=' + task.State, task, callback, null);
}
function addWorkloadSimpleV3HttpCall(data, callback, error) {
    httpCall('POST', '/Workload2/AddSimpleV3', data, callback, error);
}
function updateWorkloadSimpleV3HttpCall(data, callback, error) {
    httpCall('PUT', '/Workload2/UpdateSimpleV3', data, callback, error);
}
function loadWorkload(workloadID, callback) {
    httpCall('GET', '/Workload/GetWorkload?=' + workloadID, null, callback, null);
}
function deleteWorkloadHttpCall(workloadID, callback, error) {
    httpCall('DELETE', '/Workload/Delete?=' + workloadID, null, callback, error);
}
function httpCall(action, url, data, callback, error) {
    $.ajax({
        type: action,
        url: url,
        data: JSON.stringify(data),
        cache: false,
        contentType: 'application/json',
        dataType: 'json',
        success: callback,
        error: error,
        processData: false
    });
}
var TemplateHeader = (function (_super) {
    __extends(TemplateHeader, _super);
    function TemplateHeader() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TemplateHeader.prototype.render = function () {
        return React.createElement("div", { className: "folder-header" },
            React.createElement("p", null,
                React.createElement("span", { className: "templateTitle" }, this.props.title)));
    };
    return TemplateHeader;
}(React.Component));
var TemplateBody = (function (_super) {
    __extends(TemplateBody, _super);
    function TemplateBody() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TemplateBody.prototype.formatDate = function (dateStr) {
        // HACK
        if (dateStr.length <= 10)
            return dateStr;
        var date = new Date(dateStr);
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        var str = month + '/' + day + '/' + year;
        return str;
    };
    TemplateBody.prototype.render = function () {
        return React.createElement("div", { className: "folder-body" },
            React.createElement("p", null,
                React.createElement("i", { className: "fa fa-calendar fa-task-def", "aria-hidden": "true" }),
                React.createElement("span", { className: "templateStart" }, this.formatDate(this.props.dateStart)),
                React.createElement("i", { className: "fa fa-calendar-check-o fa-task-def", "aria-hidden": "true" }),
                React.createElement("span", { className: "templateEnd" }, this.formatDate(this.props.dateEnd))));
    };
    return TemplateBody;
}(React.Component));
var TemplateFooter = (function (_super) {
    __extends(TemplateFooter, _super);
    function TemplateFooter() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TemplateFooter.prototype.render = function () {
        return React.createElement("div", { className: "folder-footer" }, (this.props.users) ?
            this.props.users.map(function (email) { return React.createElement("img", { key: email, className: "user", src: '/users/photo/' + email }); })
            : null);
    };
    return TemplateFooter;
}(React.Component));
var TemplateTask = (function (_super) {
    __extends(TemplateTask, _super);
    function TemplateTask() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    TemplateTask.prototype.dragstart = function (ev) {
        ev.dataTransfer.setData('text', this.props.id);
    };
    TemplateTask.prototype.onclick = function () {
        taskedit(this.props.id);
    };
    TemplateTask.prototype.render = function () {
        return React.createElement("div", { className: "task", id: '_' + this.props.id, key: this.props.id, draggable: true, "data-toggle": "modal", "data-target": "#WorkloadModal", onDragStart: this.dragstart.bind(this), onClick: this.onclick.bind(this) },
            React.createElement("div", { className: "folder-tasks" },
                React.createElement(TemplateHeader, { title: this.props.title }),
                React.createElement(TemplateBody, __assign({}, this.props)),
                React.createElement(TemplateFooter, { users: this.props.users })));
    };
    return TemplateTask;
}(React.Component));
var DashboardFolderHeader = (function (_super) {
    __extends(DashboardFolderHeader, _super);
    function DashboardFolderHeader() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DashboardFolderHeader.prototype.render = function () {
        return React.createElement("div", { className: "row" },
            React.createElement("div", { className: "col-xs-12 col-md-3" },
                React.createElement("div", { className: "row" },
                    React.createElement("h3", { className: "dashboard-panel-title dashboard-panel-title--todo" }, "todo"),
                    React.createElement("button", { id: "btnNewSimple", className: "ds-button-update", "data-toggle": "modal", "data-target": "#WorkloadModal", onClick: newWorkloadStateSimple },
                        React.createElement("i", { className: "fa fa-plus", "aria-hidden": "true" }),
                        " Quick Create"))),
            React.createElement("div", { className: "col-xs-12 col-md-3" },
                React.createElement("div", { className: "row" },
                    React.createElement("h3", { className: "dashboard-panel-title dashboard-panel-title--doing" }, "doing"))),
            React.createElement("div", { className: "col-xs-12 col-md-3" },
                React.createElement("div", { className: "row" },
                    React.createElement("h3", { className: "dashboard-panel-title dashboard-panel-title--done" }, "done"))),
            React.createElement("div", { className: "col-xs-12 col-md-3" },
                React.createElement("div", { className: "row" },
                    React.createElement("h3", { className: "dashboard-panel-title dashboard-panel-title--approved" }, "archive"))),
            React.createElement("div", { className: "clearfix" }));
    };
    return DashboardFolderHeader;
}(React.Component));
var FolderModel = (function () {
    function FolderModel() {
        this.tasks = [];
    }
    FolderModel.prototype.add = function (task) {
        this.tasks.push(task);
    };
    FolderModel.prototype.remove = function (task) {
        var index = this.tasks.indexOf(task);
        (index >= 0) && this.tasks.splice(index, 1);
    };
    return FolderModel;
}());
var folderM = [new FolderModel(), new FolderModel(), new FolderModel(), new FolderModel()];
var dictM = {};
var Folder = (function (_super) {
    __extends(Folder, _super);
    function Folder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Folder.prototype.allowDrop = function (ev) {
        ev.preventDefault();
    };
    Folder.prototype.drop = function (ev) {
        ev.preventDefault();
        var numstate = this.props.state;
        var taskId = ev.dataTransfer.getData('text');
        ;
        (this.props.callback) && this.props.callback(taskId, numstate);
    };
    Folder.prototype.render = function () {
        return React.createElement("div", { className: "col-xs-12 col-md-3 dashboard-panel", style: { overflowY: 'scroll' } },
            React.createElement("div", { className: "folder", onDragOver: this.allowDrop, onDrop: this.drop.bind(this) }, (this.props.tasks) ?
                this.props.tasks.map(function (t) { return React.createElement(TemplateTask, __assign({ key: t.id }, t)); })
                : null));
    };
    return Folder;
}(React.Component));
var DashboardFolders = (function (_super) {
    __extends(DashboardFolders, _super);
    function DashboardFolders() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DashboardFolders.prototype.moveTask = function (id, nextState) {
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
    };
    DashboardFolders.prototype.render = function () {
        return React.createElement("div", null,
            React.createElement(Folder, { state: 0, tasks: folderM[0].tasks, callback: this.moveTask.bind(this) }),
            React.createElement(Folder, { state: 1, tasks: folderM[1].tasks, callback: this.moveTask.bind(this) }),
            React.createElement(Folder, { state: 2, tasks: folderM[2].tasks, callback: this.moveTask.bind(this) }),
            React.createElement(Folder, { state: 3, tasks: folderM[3].tasks, callback: this.moveTask.bind(this) }));
    };
    return DashboardFolders;
}(React.Component));
// Show/hide modal dialog
// Scope: modal
//Workloads:
function newWorkloadStateSimple() {
    EnableWorkloadFields();
    var opts = {
        fields: true,
        cancel: true,
        reset: true,
        add: true
    };
    var workload = {};
    renderModal('New Workload:', workload, opts);
}
function detailsWorkloadState(ev, guid) {
    DisableWorkloadFields();
    var opts = {
        fields: false,
        cancel: true,
        edit: true,
        newAppoint: true
    };
    renderModal('Workload Details:', {}, {});
    //Load Workload:
    loadWorkload(guid, function (data) {
        var workload = { wbid: data.WBID, title: data.WBTitle, description: data.WBDescription };
        renderModal('Workload Details:', workload, opts);
    });
}
function editWorkloadState() {
    var opts = {
        fields: true,
        cancel: true,
        update: true,
        delete: true
    };
    renderModal('Editing Workload:', null, opts);
    EnableWorkloadFields();
}
//Workloads Modal:
function resetWorkloadForm() {
    $('#msg').text('');
    $('#WBTitle').val('');
    $('#WBDescription').val('');
    // $('#WBStartDate').val('');
    // $('#WBEndDate').val('');
    clearValidate();
}
function DisableWorkloadFields() {
    $('#WBTitle').attr("disabled", "disabled");
    $('#WBDescription').attr("disabled", "disabled");
    // $('#WBStartDate').attr("disabled", "disabled");
    // $('#WBEndDate').attr("disabled", "disabled");
    //Disabled all buttons:
    // $("#btnWorkloadCancel").attr("disabled", "disabled");
    // $("#btnWorkloadReset").attr("disabled", "disabled");
    // $("#btnWorkloadEdit").attr("disabled", "disabled");
    // $("#btnWorkloadDelete").attr("disabled", "disabled");
    // $("#btnWorkloadSendUpdate").attr("disabled", "disabled");
    // $("#btnWorkloadSendAdd").attr("disabled", "disabled");    
}
function EnableWorkloadFields() {
    //Fields:
    $('#WBTitle').removeAttr("disabled");
    $('#WBDescription').removeAttr("disabled");
    // $('#WBStartDate').removeAttr("disabled");
    // $('#WBEndDate').removeAttr("disabled");
}
// function HideAllButtons() {
//     $("#btnWorkloadCancel").addClass('hidden');
//     $("#btnWorkloadReset").addClass('hidden');
//     $("#btnWorkloadEdit").addClass('hidden');
//     $("#btnWorkloadDelete").addClass('hidden');
//     $("#btnWorkloadSendAdd").addClass('hidden');
//     $("#btnWorkloadSendUpdate").addClass('hidden');
// }
function Initialize() {
    ReactDOM.render(React.createElement(ModalButtonList, null), document.querySelector('.appmodal'));
    ReactDOM.render(React.createElement(ModalForm, null), document.querySelector('.appmodalform'));
    // $('#WBStartDate').datepicker({
    //     format: "mm/dd/yyyy",
    //     autoclose: true,
    //     todayHighlight: true
    // });
    // $('#WBEndDate').datepicker({
    //     format: "mm/dd/yyyy",
    //     autoclose: true,
    //     todayHighlight: true
    // });        
}
function renderModal(windowTitle, workload, showopts) {
    $('#ModalTitle').text(windowTitle);
    if (workload != null) {
        ReactDOM.render(React.createElement(ModalForm, workload), document.querySelector('.appmodalform'));
    }
    ReactDOM.render(React.createElement(ModalButtonList, { show: showopts }), document.querySelector('.appmodal'));
}
// function loadWorkloadCallback(data) {
//         $('#WBID').val(data.WBID);
//         // Title and Description
//         $('#WBTitle').val(data.WBTitle);
//         $('#WBDescription').val(data.WBDescription);
//         //Dates
//         // formatDate(data.WBStartDate, function (str) {
//         //     $('#WBStartDate').val(str);
//         // });
//         // formatDate(data.WBEndDate, function (str) {
//         //     $('#WBEndDate').val(str);
//         // });
// }
function submitAddWorkloadSimple(e) {
    e.preventDefault();
    if (validateFormSimple()) {
        var workloadV3 = { title: this.WBTitle.value, description: this.WBDescription.value, status: 0 };
        DisableWorkloadFields();
        $('#msg').text('Wait...');
        $('#msg').fadeIn();
        addWorkloadSimpleV3HttpCall(workloadV3, function (response) {
            // success
            $('#WorkloadModal').modal('hide');
            createTaskSimple(response.WBID, response.WBTitle, response.WBStatus, response.WBStartDate, response.WBEndDate, response.WBUsers);
            updateDashboard();
        }, function () {
            // error
            $('#msg').text('Error!');
            EnableWorkloadFields();
            newWorkloadStateSimple();
        });
    }
}
function submitUpdateWorkloadSimple(e) {
    e.preventDefault();
    if (validateFormSimple()) {
        var workloadV3 = { id: this.WBID.value, title: this.WBTitle.value, description: this.WBDescription.value };
        DisableWorkloadFields();
        $('#msg').text('Wait...');
        $('#msg').fadeIn();
        updateWorkloadSimpleV3HttpCall(workloadV3, function (response) {
            // success
            $('#WorkloadModal').modal('hide');
            updateTaskInFolder(response.WBID, response.WBTitle, response.WBStatus, response.WBStartDate, response.WBEndDate, response.WBUsers);
            updateDashboard();
        }, function () {
            // error
            $('#msg').text('Error!');
            EnableWorkloadFields();
            editWorkloadState();
        });
    }
}
function deleteWorkloadSimple(ev) {
    ev.preventDefault();
    var workloadID = $('#WBID').val();
    $('#msg').text('Wait...');
    deleteWorkloadHttpCall(workloadID, function (response) {
        $('#WorkloadModal').modal('hide');
        deleteTaskInFolder(workloadID);
        updateDashboard();
    }, function () {
        // error
        $('#msg').text('Error!');
        EnableWorkloadFields();
        editWorkloadState();
    });
}
function validateFormSimple() {
    var ctrl = true;
    var textFields = ['#WBTitle'];
    for (var i = 0; i < textFields.length; i++) {
        var field = $(textFields[i]);
        if (field.val() == "") {
            ctrl = false;
            //field.addClass('error'); // css is not working
        }
        else {
            //field.removeClass('error');
        }
    }
    var msg = $('#msg');
    if (!ctrl) {
        msg.fadeIn();
        msg.text('Please, fill the mandatory fields.');
    }
    return ctrl;
}
function clearValidate() {
    var textFields = ['#WBTitle'];
    for (var i = 0; i < textFields.length; i++) {
        var field = $(textFields[i]);
        field.removeClass('error');
    }
}
var ModalButton = (function (_super) {
    __extends(ModalButton, _super);
    function ModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ModalButton.prototype.render = function () {
        var classButtonType = this.props.className;
        var pictureClass = (this.props.picture) ? ("fa " + this.props.picture) : null;
        return (!this.props.display) ? null :
            React.createElement("div", { className: "data-sorting-buttons" },
                React.createElement("button", { className: classButtonType, onClick: this.props.onClick },
                    (pictureClass) && (React.createElement("i", { className: pictureClass, "aria-hidden": "true" })),
                    this.props.children));
    };
    return ModalButton;
}(React.Component));
function cancelButton() {
    $('#WorkloadModal').modal('hide');
}
var CancelModalButton = (function (_super) {
    __extends(CancelModalButton, _super);
    function CancelModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CancelModalButton.prototype.cancel = function () {
        $('#WorkloadModal').modal('hide');
    };
    CancelModalButton.prototype.render = function () {
        return React.createElement("div", { className: "data-sorting-buttons" },
            React.createElement("button", { type: "button", className: "ds-button-reset", id: "btnWorkloadCancel", onClick: this.cancel }, "Cancel"));
    };
    return CancelModalButton;
}(React.Component));
var ResetModalButton = (function (_super) {
    __extends(ResetModalButton, _super);
    function ResetModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ResetModalButton.prototype.render = function () {
        return React.createElement("div", { className: "data-sorting-buttons" },
            React.createElement("button", { type: "button", className: "ds-button-reset", id: "btnWorkloadReset", onClick: resetWorkloadForm }, "Reset"));
    };
    return ResetModalButton;
}(React.Component));
var EditModalButton = (function (_super) {
    __extends(EditModalButton, _super);
    function EditModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    EditModalButton.prototype.render = function () {
        return React.createElement("div", { className: "data-sorting-buttons" },
            React.createElement("button", { type: "button", className: "ds-button-update", id: "btnWorkloadEdit", onClick: editWorkloadState },
                React.createElement("i", { className: "fa fa-retweet", "aria-hidden": "true" }),
                "Edit"));
    };
    return EditModalButton;
}(React.Component));
var DeleteModalButton = (function (_super) {
    __extends(DeleteModalButton, _super);
    function DeleteModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    DeleteModalButton.prototype.render = function () {
        if (this.props.display) {
            return React.createElement("div", { className: "data-sorting-buttons" },
                React.createElement("button", { type: "button", className: "btn btn-warning", id: "btnWorkloadDelete", onClick: deleteWorkloadSimple },
                    React.createElement("i", { className: "fa fa-trash-o", "aria-hidden": "true" }),
                    "Delete"));
        }
    };
    return DeleteModalButton;
}(React.Component));
var SendAddModalButton = (function (_super) {
    __extends(SendAddModalButton, _super);
    function SendAddModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SendAddModalButton.prototype.render = function () {
        return React.createElement("div", { className: "data-sorting-buttons" },
            React.createElement("button", { type: "button", className: "btn btn-success", id: "btnWorkloadSendAdd", onClick: submitAddWorkloadSimple }, "Add"));
    };
    return SendAddModalButton;
}(React.Component));
var SendUpdateModalButton = (function (_super) {
    __extends(SendUpdateModalButton, _super);
    function SendUpdateModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SendUpdateModalButton.prototype.render = function () {
        return React.createElement("div", { className: "data-sorting-buttons" },
            React.createElement("button", { type: "button", className: "btn btn-success", id: "btnWorkloadSendUpdate", onClick: submitUpdateWorkloadSimple }, "Update"));
    };
    return SendUpdateModalButton;
}(React.Component));
var AddAppointmentModalButton = (function (_super) {
    __extends(AddAppointmentModalButton, _super);
    function AddAppointmentModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    AddAppointmentModalButton.prototype.addAppointment = function (ev) {
        var wbid = this.props.wbid;
        if (wbid != '') {
            window.open('/appointment/addsimple?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no');
        }
    };
    AddAppointmentModalButton.prototype.render = function () {
        return React.createElement("div", { className: "data-sorting-buttons" },
            React.createElement("button", { type: "button", className: "btn btn-success", id: "btnWorkloadAddAppointment", onClick: this.addAppointment },
                React.createElement("i", { className: "fa fa-retweet", "aria-hidden": "true" }),
                "New Appointment"));
    };
    return AddAppointmentModalButton;
}(React.Component));
var LogHistoryModalButton = (function (_super) {
    __extends(LogHistoryModalButton, _super);
    function LogHistoryModalButton() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    LogHistoryModalButton.prototype.showAppointments = function (ev) {
        var wbid = this.props.wbid;
        if (wbid != '') {
            window.open('/appointment/work?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no');
        }
    };
    LogHistoryModalButton.prototype.render = function () {
        return React.createElement("div", { className: "data-sorting-buttons" },
            React.createElement("button", { type: "button", className: "btn btn-success", id: "btnWorkloadShowAppointment", onClick: this.showAppointments },
                React.createElement("i", { className: "fa fa-retweet", "aria-hidden": "true" }),
                "Log History"));
    };
    return LogHistoryModalButton;
}(React.Component));
var ModalButtonList = (function (_super) {
    __extends(ModalButtonList, _super);
    function ModalButtonList() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ModalButtonList.prototype.render = function () {
        var wbid = $('#WBID').val();
        var show = this.props.show || {};
        function getfun(wbid) {
            return function () { window.open('/appointment/work?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no'); };
        }
        ;
        function getfun2(wbid) {
            return function () { window.open('/appointment/addsimple?wbid=' + wbid, '_blank', 'height=700, width=600, status=no, toolbar=no, menubar=no, location=no'); };
        }
        return React.createElement("div", null,
            React.createElement(ModalButton, { display: show['cancel'], className: "ds-button-reset", onClick: cancelButton }, "CANCEL"),
            React.createElement(ModalButton, { display: show['reset'], className: "ds-button-reset", onClick: resetWorkloadForm }, "RESET"),
            React.createElement(ModalButton, { display: show['edit'], className: "ds-button-update", onClick: editWorkloadState, picture: "fa-retweet" }, "EDIT"),
            React.createElement(ModalButton, { display: show['delete'], className: "btn btn-warning", onClick: deleteWorkloadSimple, picture: "fa-trash-o" }, "DELETE"),
            React.createElement(ModalButton, { display: show['add'], className: "btn btn-success", onClick: submitAddWorkloadSimple }, "SENDADD"),
            React.createElement(ModalButton, { display: show['update'], className: "btn btn-success", onClick: submitUpdateWorkloadSimple }, "SENDUPD"),
            React.createElement(ModalButton, { display: show['newlog'], className: "btn btn-success", onClick: getfun, picture: "fa-retweet" }, "NEWLOG"),
            React.createElement(ModalButton, { display: show['history'], className: "btn btn-success", onClick: getfun, picture: "fa-retweet" }, "HISTORY"));
    };
    return ModalButtonList;
}(React.Component));
var ModalButtonList2 = (function (_super) {
    __extends(ModalButtonList2, _super);
    function ModalButtonList2() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ModalButtonList2.prototype.render = function () {
        var wbid = $('#WBID').val();
        var show = this.props.show || {};
        return React.createElement("div", null,
            React.createElement(CancelModalButton, null),
            React.createElement(ResetModalButton, null),
            React.createElement(EditModalButton, null),
            React.createElement(DeleteModalButton, { display: show['delete'] }),
            React.createElement(SendAddModalButton, null),
            React.createElement(SendUpdateModalButton, null),
            React.createElement(AddAppointmentModalButton, { wbid: wbid }),
            React.createElement(LogHistoryModalButton, { wbid: wbid }));
    };
    return ModalButtonList2;
}(React.Component));
var ModalDialog = (function (_super) {
    __extends(ModalDialog, _super);
    function ModalDialog() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    ModalDialog.prototype.render = function () {
        return React.createElement("div", { className: "folder-header" },
            React.createElement("p", null,
                React.createElement("span", { className: "templateTitle" }, this.props.title)));
    };
    return ModalDialog;
}(React.Component));
var ModalForm = (function (_super) {
    __extends(ModalForm, _super);
    function ModalForm(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            wbid: '',
            startDate: '',
            endDate: '',
            title: '',
            description: ''
        };
        _this.handleChange = _this.handleChange.bind(_this);
        return _this;
    }
    ModalForm.prototype.componentWillReceiveProps = function (props) {
        this.setState({
            wbid: props.wbid || '',
            startDate: props.startDate || '',
            endDate: props.endDate || '',
            title: props.title || '',
            description: props.description || ''
        });
    };
    ModalForm.prototype.handleChange = function (ev) {
        var name = ev.target.name;
        var value = ev.target.value;
        this.setState((_a = {}, _a[name] = ev.target.value, _a));
        var _a;
    };
    ModalForm.prototype.onclick = function (e) {
        alert(JSON.stringify(this.state));
        e.preventDefault();
    };
    ModalForm.prototype.render = function () {
        return React.createElement("div", null,
            React.createElement("div", { className: "row", hidden: true },
                React.createElement("div", { id: "#formWBID", className: "col-md-9" },
                    React.createElement("div", { className: "form-group" },
                        React.createElement("input", { type: "text", className: "form-control", id: "WBID", name: "WBID", value: this.state.wbid })))),
            React.createElement("div", { className: "row", hidden: true },
                React.createElement("div", { className: "col-md-4" },
                    React.createElement("div", { className: "form-group" },
                        React.createElement("label", { htmlFor: "WBStartDate" }, "Start date:"),
                        React.createElement("div", { className: "input-group date" },
                            React.createElement("input", { type: "text", id: "WBStartDate", name: "startDate", className: "form-control", autoComplete: "off", onChange: this.handleChange }),
                            React.createElement("span", { className: "input-group-addon" },
                                React.createElement("i", { className: "fa fa-calendar-check-o", "aria-hidden": "true" }))))),
                React.createElement("div", { className: "col-md-4" },
                    React.createElement("div", { className: "form-group" },
                        React.createElement("label", { htmlFor: "WBEndDate" }, "End date:"),
                        React.createElement("div", { className: "input-group date" },
                            React.createElement("input", { type: "text", id: "WBEndDate", name: "endDate", className: "form-control", autoComplete: "off", onChange: this.handleChange }),
                            React.createElement("span", { className: "input-group-addon" },
                                React.createElement("i", { className: "fa fa-calendar-check-o", "aria-hidden": "true" })))))),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-md-12" },
                    React.createElement("div", { className: "form-group" },
                        React.createElement("label", { htmlFor: "WBTitle" }, "Title:"),
                        React.createElement("input", { type: "text", className: "form-control", id: "WBTitle", name: "title", autoComplete: "off", value: this.state.title, onChange: this.handleChange, autoFocus: true })))),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-md-12" },
                    React.createElement("div", { className: "form-group" },
                        React.createElement("label", { htmlFor: "WBDescription" }, "Description:"),
                        React.createElement("textarea", { className: "form-control", id: "WBDescription", name: "description", rows: 3, value: this.state.description, onChange: this.handleChange })))),
            React.createElement("button", { onClick: this.onclick.bind(this) }, "DebugShow"));
    };
    return ModalForm;
}(React.Component));
// initialize
function InitializeKanban() {
    ReactDOM.render(React.createElement(DashboardFolderHeader, null), document.getElementById('dashboard-folder-header'));
    gettasklist(function (tasklist) {
        tasklist.map(function (task) {
            createTask(task.id, task.title, task.start, task.end, task.hours, task.attachments, task.tag, task.status, task.users /* , task.description */);
        });
        ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders'));
    });
}
function createTask(id, title, start, end, hours, attachments, tag, state, users) {
    var userArray = users.map(function (item) { return item.Item1; });
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
    ReactDOM.render(React.createElement(DashboardFolders, null), document.getElementById('dashboard-folders'));
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
