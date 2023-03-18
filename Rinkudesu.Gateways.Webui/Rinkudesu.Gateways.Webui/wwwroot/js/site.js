﻿document.addEventListener('keydown', event => {
    if (event.ctrlKey && event.key === 'Enter')
        document.activeElement?.form?.submit();
});

function performHttpRequest(url, method, body, onLoad, onFailure, onProgress) {
    const request = new XMLHttpRequest();

    request.onload = onLoad;
    request.onerror = onFailure;
    request.onprogress = onProgress;

    request.open(method, url);

    const antiforgery = document.getElementById('antiforgeryToken')?.getAttribute('token-value');
    if (antiforgery) {
        request.setRequestHeader('RequestVerificationToken', antiforgery);
    }

    request.send(body);
}

function disableWithChildren(node, status = false) {
    node.disabled = !status;
    for (const nodeChild of node.getElementsByTagName('*')) {
        nodeChild.disabled = !status;
    }
}

function setBgColour(element) {
    const colour = element.getAttribute('data-bg-colour');
    if (colour == null)
        return;

    element.style.background = colour;
}

function applyBgColourToAll() {
    for (const element of document.getElementsByClassName('has-bg-colour')) {
        setBgColour(element);
    }
}

window.addEventListener('load', _ => initialiseTagsAutocompletion());
window.addEventListener('load', _ => initialiseGenericTomselect());

function initialiseGenericTomselect() {
    new TomSelect('.tomselect', {});
}

function initialiseTagsAutocompletion() {
    new TomSelect('.tags-autocompletion', {
        valueField: 'id',
        labelField: 'data',
        searchField: 'data',
        load: function (query, callback) {
            const url = '/api/autocompletion/TagsAutocompletionApi?name=' + encodeURIComponent(query);
            fetch(url)
                .then(response => response.json())
                .then(json => {
                    callback(json);
                }).catch(() => {
                callback();
            });
        }
    });
}
