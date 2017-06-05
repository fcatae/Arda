
//Microsoft Graph API calls:

function GetImage(user, token) {
    var url = "https://graph.microsoft.com/v1.0/me/photo/$value";
    var elem = "userImg";
    var auth = 'bearer ' + token;
    GetImageBase64FromGraph(url, elem, auth);
}

function GetImageBase64FromGraph(url, element, token) {
    var request = new XMLHttpRequest;
    request.open("GET", url);
    request.setRequestHeader("Authorization", token);
    request.responseType = "blob";
    request.onload = function () {
        if (request.readyState === 4 && request.status === 200) {
            var image = document.getElementById(element) as HTMLImageElement;
            var reader = new FileReader();
            reader.onload = function () {
                image.src = reader.result;
                updateImgOnDatabase();
            };
            reader.readAsDataURL(request.response);
        }
    };
    request.send(null);
}

function updateImgOnDatabase() {
    var img = $('#userImg').attr('src');

    var data = new FormData(this);
    data.append('img', img)

    $.ajax({
        url: '/Users/PhotoUpdate',
        type: 'PUT',
        data: data,
        processData: false,
        contentType: false,
        success: function (response) {

        }
    });
}

//Util:

function formatDate(dateStr, callback) {
    var date = new Date(dateStr);
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();
    var str = month + '/' + day + '/' + year;
    callback(str)
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

