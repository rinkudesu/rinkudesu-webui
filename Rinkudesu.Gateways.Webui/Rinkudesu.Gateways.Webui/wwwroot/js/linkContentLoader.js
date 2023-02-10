let query;
const contentDiv = document.getElementById('content');
const returnUrlPath = encodeURIComponent("/links");
const linksContentBaseUrl = '/links/IndexContent?returnUrlBase=' + returnUrlPath;

window.addEventListener('load', _ => { getQuery(); loadContent(); });
document.getElementById('page-prev').addEventListener('click', prevPage);
document.getElementById('page-next').addEventListener('click', nextPage);

function prevPage() {
    if (query.Skip < query.Take)
        return;

    query.Skip -= query.Take;
    loadContent();
}

//todo: figure out when to stop allowing this
function nextPage() {
    query.Skip += query.Take;
    loadContent();
}

//todo: this needs to be localised
function loadContent() {
    performHttpRequest(linksContentBaseUrl + getQueryAsString('&'), "GET", null, setLinksContent, _ => alert("loading failed"))
}

function getQuery() {
    query = JSON.parse(document.getElementById('query').getAttribute('data-value'));
}

function setLinksContent(responseEvent) {
    if (responseEvent.currentTarget.status !== 200) {
        alert("loading failed")
        //alert(document.getElementById('translations').getAttribute('data-load-failed'));
        contentDiv.innerText = "";
        return;
    }

    contentDiv.innerHTML = responseEvent.currentTarget.responseText;
    getQuery();
}

function getQueryAsString(prefix) {
    const queryString = new URLSearchParams(query).toString();
    if (prefix) {
        return prefix + queryString;
    }
    return queryString;
}