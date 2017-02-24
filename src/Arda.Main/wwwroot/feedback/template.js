function init(feedback, form, url) {
    var parameters = document.location.search;
    form.action = url + parameters;
}
