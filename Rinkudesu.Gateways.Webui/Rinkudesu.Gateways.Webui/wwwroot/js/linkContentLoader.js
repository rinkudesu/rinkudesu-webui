const contentDiv = document.getElementById('content');
const returnUrlPath = encodeURIComponent("/links");
const linksContentBaseUrl = '/links/IndexContent?returnUrlBase=' + returnUrlPath;

window.addEventListener('load', _ => loadContent())

//todo: this needs to be localised
function loadContent() {
    performHttpRequest(linksContentBaseUrl, "GET", null, setLinksContent, _ => alert("loading failed"))
}

function setLinksContent(responseEvent) {
    if (responseEvent.currentTarget.status !== 200) {
        alert("loading failed")
        //alert(document.getElementById('translations').getAttribute('data-load-failed'));
        contentDiv.innerText = "";
        return;
    }

    contentDiv.innerHTML = responseEvent.currentTarget.responseText;
}
