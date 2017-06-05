// Initialize functions
// Scope: Modal, Dashboard, Kanban

function Initialize() {
    //Click events:
    //New Workload:
    $('#btnNew').click(newWorkloadState);
    $('#btnNewSimple').click(newWorkloadStateSimple);

    //Workload Details:
    $('#btnDetails').click(detailsWorkloadState);
    //Reset Button:
    $('#btnWorkloadReset').click(resetWorkloadForm);
    //Delete Button:
    $('#btnWorkloadDelete').click(deleteWorkload);
    //Cancel Button:
    $('#btnWorkloadCancel').click(function () {
        $('#WorkloadModal').modal('hide');
    });


    //Other events:
    $('#WBComplexity').on('change', changeComplexity);

    //Search:
    $('#search-box').on('keyup', function () {
        var matcher = new RegExp($(this).val(), 'gi');
        $('.task').show().not(function () {
            return matcher.test($(this).find('.templateTitle').text())
        }).hide();
    })

    //Components:
    $("#WBIsWorkload").bootstrapSwitch();

    $('#WBStartDate').datepicker({
        format: "mm/dd/yyyy",
        autoclose: true,
        todayHighlight: true
    });

    $('#WBEndDate').datepicker({
        format: "mm/dd/yyyy",
        autoclose: true,
        todayHighlight: true
    });

    $("#WBComplexity").ionRangeSlider({
        min: 1,
        max: 5,
        hide_min_max: true,
        hide_from_to: true,
        grid: false,
        keyboard: true
    });
}

function InitializeFields() {
    //Load values:
    //Get All Activities:
    $.getJSON('/activity/GetActivities', null, callbackGetActivities);
    //Get User Technologies:
    $.getJSON('/technology/GetTechnologies', null, callbackGetTechnologies);
    //Get User Metrics:
    $.getJSON('/metric/GetMetrics', null, callbackGetMetrics);
    //Get User Users:
    $.getJSON('/users/GetUsers', null, callbackGetUsers);
}

function InitializeKanban() {
    //Board Initialization
    folders.map(function (i, folder) {
        folder.addEventListener('dragover', dragover);
        folder.addEventListener('drop', drop.bind(folder));
    });

    $('.dashboard-filter-field').change(function () {
        RefreshTaskList();
    });

    if (window["hackIsAdmin"] != null) {
        GetUserList();
        $('#filter-assign').change(function () {
            RefreshTaskList();
        });
    }
    else {
        $('#filter-assign').css('visibility', 'hidden');
    }
}
