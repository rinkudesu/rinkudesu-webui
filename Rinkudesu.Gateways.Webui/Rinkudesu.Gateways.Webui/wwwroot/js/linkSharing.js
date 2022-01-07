let actionBtn = document.getElementById('shareKeyAction');
let keyBox = document.getElementById('shareKey');
let linkId = keyBox.getAttribute('data-link-id');
let sharedCheckDone = false;
let isShared = false;
let token = document.getElementById('token').value;

const startSharingText = "Share";
const stopSharingText = "Stop";

document.getElementById('showSharingOptions').addEventListener('click', checkShared);
actionBtn.addEventListener('click', performAction)

function setCurrentState() {
    if (isShared) {
        let request = new XMLHttpRequest();
        request.onreadystatechange = function () {
            if (request.readyState === XMLHttpRequest.DONE) {
                if (request.status === 200) {
                    keyBox.value = request.responseText;
                    actionBtn.innerText = stopSharingText
                }
                else {
                    genericError();
                }
            }
        }

        request.open("GET", "/api/sharedLinks/" + linkId, true);
        request.send(null);
    }
    else {
        keyBox.value = '';
        actionBtn.innerText = startSharingText;
    }
}

function checkShared() {
    if (sharedCheckDone) {
        return;
    }

    let request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                isShared = request.responseText.toLowerCase() === 'true';
                sharedCheckDone = true;
                setCurrentState();
            }
            else {
                genericError();
            }
        }
    }

    request.open("GET", "/api/sharedLinks/shared/" + linkId, true);
    request.send(null);
}

function performAction() {
    if (!sharedCheckDone) {
        return;
    }
    if (isShared) {
        unshare();
    }
    else {
        share();
    }
}

function share() {
    let request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                isShared = true;
                keyBox.value = request.responseText;
                actionBtn.innerText = stopSharingText;
            }
            else {
                genericError();
            }
        }
    }

    request.open("POST", "/api/sharedLinks/" + linkId, true);
    request.setRequestHeader('RequestVerificationToken', token);
    request.send(null);
}

function unshare() {
    let request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                isShared = false;
                keyBox.value = '';
                actionBtn.innerText = startSharingText;
            }
            else {
                genericError();
            }
        }
    }

    request.open("DELETE", "/api/sharedLinks/" + linkId, true);
    request.setRequestHeader('RequestVerificationToken', token);
    request.send(null);
}

function genericError() {
    alert("Something went wrong, please reload the site and try again");
}