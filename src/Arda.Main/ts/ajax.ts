//Initialize:
declare var $ : any

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


function gettasklist(callback, type, user) {

    var filter_user = user ? '?user=' + user : '';
    var filter_type = type ? '/ListBacklogsByUser' : '/ListWorkloadsByUser';

    httpCall('GET', '/Workload' + filter_type + filter_user, null, callback, null);
}

function update(task) {
    httpCall('PUT', '/Workload/UpdateStatus?id=' + task.Id + '&status=' + task.State, task, function (data) {
        // done
    }, null)
}

function getGUID(callback) {
    $.ajax({
        url: '/Workload/GetGuid',
        type: 'GET',
        processData: false,
        contentType: false,
        cache: false,
        success: function (data) {
            callback(data);
        }
    });
}

