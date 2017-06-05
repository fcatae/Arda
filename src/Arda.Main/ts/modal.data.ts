// Callback to fill form
// Scope: Modal

//Callbacks:

function callbackGetActivities(data) {
    var options = [];
    options.push('<option selected disabled value="-1">Select the activity</option>');
    for (var i = 0; i < data.length; i++) {
        var text = data[i].ActivityName;
        var key = data[i].ActivityID;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    $('#WBActivity').html(options.join(''));
}

function callbackGetMetrics(data) {
    var options = [];
    var select = $('#WBMetrics');
    for (var i = 0; i < data.length; i++) {
        var text = '[' + data[i].MetricCategory + '] ' + data[i].MetricName;
        var key = data[i].MetricID;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    select.html(options.join(''));

    select.multiselect({
        buttonWidth: '100%',
        numberDisplayed: 1,
        nonSelectedText: 'Click to select the metrics'
    });
}

function callbackGetTechnologies(data) {
    var options = [];
    var select = $('#WBTechnologies');
    for (var i = 0; i < data.length; i++) {
        var text = data[i].TechnologyName;
        var key = data[i].TechnologyID;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    select.html(options.join(''));

    select.multiselect({
        buttonWidth: '100%',
        numberDisplayed: 2,
        nonSelectedText: 'Click to select the technologies'
    });
}

function callbackGetUsers(data) {
    var options = [];
    var select = $('#WBUsers');
    for (var i = 0; i < data.length; i++) {
        var text = data[i].Name;
        var key = data[i].UniqueName;
        options.push('<option value="' + key + '">' + text + '</option>');
    }
    select.html(options.join(''));

    select.multiselect({
        buttonWidth: '100%',
        numberDisplayed: 2,
        nonSelectedText: 'Click to select the users'
    });
}
