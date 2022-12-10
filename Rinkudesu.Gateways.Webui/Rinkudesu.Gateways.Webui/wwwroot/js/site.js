document.addEventListener('keydown', event => {
    if (event.ctrlKey && event.key === 'Enter')
        document.activeElement?.form?.submit();
});

function performHttpRequest(url, method, body, onLoad, onFailure, onProgress) {
    const request = new XMLHttpRequest();

    request.onload = onLoad;
    request.onerror = onFailure;
    request.onprogress = onProgress;

    request.open(method, url);
    request.send(body);
}
