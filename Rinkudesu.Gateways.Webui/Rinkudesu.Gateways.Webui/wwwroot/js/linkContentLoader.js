let query;
const contentDiv = document.getElementById('content');
const returnUrlPath = "/links";
const linksContentBaseUrl = '/links/IndexContent?returnUrl=';
const translations = document.getElementById('translations');

window.addEventListener('load', _ => { getQuery(); loadContent(); });
document.getElementById('page-prev').addEventListener('click', prevPage);
document.getElementById('page-next').addEventListener('click', nextPage);

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
    //todo: support for "back" and "forward" browser button also needs to be added, as this breaks it completely
    //current query can probably be stored in the "data" parameter below, but this needs some investigation
    window.history.pushState({}, '', newUrl);
}

function loadContent() {
    performHttpRequest(linksContentBaseUrl + getReturnUrl() + getQueryAsString('&'), "GET", null, setLinksContent, _ => alert(translations.getAttribute('data-load-failed')));
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
    handleWindowLocation();
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
