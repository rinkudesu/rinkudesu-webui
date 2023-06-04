let query;
const contentDiv = document.getElementById('content');
const translations = document.getElementById('translations');

window.addEventListener('load', () => { getQuery(); loadContent(); });
document.getElementById('page-prev').addEventListener('click', prevPage);
document.getElementById('page-next').addEventListener('click', nextPage);


onpopstate = (event) => {
    query = event.state;
    loadContent(false);
}

function prevPage() {
    if (query.Skip < query.Take)
        return;

    query.Skip -= query.Take;
    loadContent();
}

function nextPage() {
    if (getCurrentItemCount() < query.Take)
        return;

    query.Skip += query.Take;
    loadContent();
}

function getCurrentItemCount() {
    return contentDiv.getElementsByClassName('index-data-row').length;
}

function handleWindowLocation() {
    let newUrl = new URL(window.location);
    newUrl.pathname = returnUrlPath;
    newUrl.search = getQueryAsString();
    window.history.pushState(structuredClone(query), '', newUrl);
}

function loadContent(moveLocation = true) {
    performHttpRequest(contentBaseUrl + getReturnUrl() + getQueryAsString('&'), "GET", null, setContent, () => alert(translations.getAttribute('data-load-failed')));

    if (moveLocation)
        handleWindowLocation();
}

function setContent(responseEvent) {
    if (responseEvent.currentTarget.status !== 200) {
        alert(translations.getAttribute('data-load-failed'));
        contentDiv.innerText = "";
        return;
    }

    contentDiv.innerHTML = responseEvent.currentTarget.responseText;
    getQuery();
    handlePageBtnState();
    applyBgColourToAll();
}

function getQuery() {
    query = JSON.parse(document.getElementById('query').getAttribute('data-value'));
}

function handlePageBtnState() {
    document.getElementById('page-prev').hidden = query.Skip < query.Take;
    document.getElementById('page-next').hidden = getCurrentItemCount() < query.Take;
}

function getReturnUrl() {
    return encodeURIComponent(returnUrlPath + getQueryAsString('?'));
}

function getQueryAsString(prefix) {
    const queryString = new URLSearchParams(query).toString();
    if (prefix) {
        return prefix + queryString;
    }
    return queryString;
}
