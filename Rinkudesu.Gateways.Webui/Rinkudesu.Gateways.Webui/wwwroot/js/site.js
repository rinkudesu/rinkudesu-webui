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
