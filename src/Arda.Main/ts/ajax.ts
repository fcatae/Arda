//Initialize:
declare var $ : any

function gettasklist(callback) {
    httpCall('GET', '/Workload/ListWorkloadsByUser' , null, callback, null);
}

function update(task, callback) {
    httpCall('PUT', '/Workload/UpdateStatus?id=' + task.Id + '&status=' + task.State, task, callback, null)
}

function addWorkloadSimpleV3HttpCall(data, callback, error) {
    httpCall('POST', '/Workload2/AddSimpleV3', data, callback, error);
}

function updateWorkloadSimpleV3HttpCall(data, callback, error) {
    httpCall('PUT', '/Workload2/UpdateSimpleV3', data, callback, error);
}

function loadWorkload(workloadID: string, callback) {
    httpCall('GET', '/Workload/GetWorkload?=' + workloadID, null, callback, null);
}

function deleteWorkloadHttpCall(workloadID, callback, error) {
    httpCall('DELETE', '/Workload/Delete?=' + workloadID, null, callback, error);
}

function httpCall(action, url, data, callback, error) {

    $.ajax({
        type: action, // GET POST PUT
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
