var templateUrl;

function initForm(user, path) {
    var baseUrl = document.location.protocol + '//' + document.location.host
    var redirectUrl = baseUrl + path;
    templateUrl = '/feedback/template.html?user=' + user + '&redirect=' + redirectUrl;
}

function openForm() {
    feedbackForm.src = templateUrl;
    feedbackForm.style.display = 'block'
}
function closeForm() {
    feedbackForm.src = '';
    feedbackForm.style.display = 'none';
}
function checkForm(url) {
    var isTemplate = url.indexOf(templateUrl) >= 0;
    var isValidPage = url.startsWith('http');

    if (isValidPage && (!isTemplate)) {
        setTimeout(function () {
            closeForm();
        }, 800)
        
    }
}

