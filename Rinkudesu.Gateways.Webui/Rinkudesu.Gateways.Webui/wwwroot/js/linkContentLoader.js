let query;
const contentDiv = document.getElementById('content');
const returnUrlPath = encodeURIComponent("/links");
const linksContentBaseUrl = '/links/IndexContent?returnUrlBase=' + returnUrlPath;

window.addEventListener('load', _ => { getQuery(); loadContent(); });

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
