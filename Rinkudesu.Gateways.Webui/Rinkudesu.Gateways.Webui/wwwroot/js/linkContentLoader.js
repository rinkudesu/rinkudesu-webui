let query;
const contentDiv = document.getElementById('content');
const returnUrlPath = "/links";
const linksContentBaseUrl = '/links/IndexContent?returnUrl=';
const translations = document.getElementById('translations');

window.addEventListener('load', _ => { getQuery(); loadContent(); });
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
    if (getCurrentLinkCount() < query.Take)
        return;

    query.Skip += query.Take;
    loadContent();
}

function getCurrentLinkCount() {
    return contentDiv.getElementsByClassName('row').length;
}

function handleWindowLocation() {
    let newUrl = new URL(window.location);
    newUrl.pathname = "/links";
    newUrl.search = getQueryAsString();
    window.history.pushState(structuredClone(query), '', newUrl);
}

function loadContent(moveLocation = true) {
    performHttpRequest(linksContentBaseUrl + getReturnUrl() + getQueryAsString('&'), "GET", null, setLinksContent, _ => alert(translations.getAttribute('data-load-failed')));

    if (moveLocation)
        handleWindowLocation();
}

function getQuery() {
    query = JSON.parse(document.getElementById('query').getAttribute('data-value'));
}

function setLinksContent(responseEvent) {
    if (responseEvent.currentTarget.status !== 200) {
        alert(translations.getAttribute('data-load-failed'));
        contentDiv.innerText = "";
        return;
    }

    contentDiv.innerHTML = responseEvent.currentTarget.responseText;
    getQuery();
    handlePageBtnState();
}

function handlePageBtnState() {
   document.getElementById('page-prev').hidden = query.Skip < query.Take;
   document.getElementById('page-next').hidden = getCurrentLinkCount() < query.Take;
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
