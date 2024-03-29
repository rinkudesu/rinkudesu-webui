﻿let actionBtn = document.getElementById('shareKeyAction');
let keyBox = document.getElementById('shareKey');
let linkId = keyBox.getAttribute('data-link-id');
let sharedCheckDone = false;
let isShared = false;
let token = document.getElementById('token').value;

const startSharingText = actionBtn.getAttribute('data-text-share');
const stopSharingText = actionBtn.getAttribute('data-text-stop');

document.getElementById('showSharingOptions').addEventListener('click', checkShared);
actionBtn.addEventListener('click', performAction)

function setCurrentState() {
    actionBtn.disabled = true;
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
                actionBtn.disabled = false;
            }
        }

        request.open("GET", "/api/sharedLinks/" + linkId, true);
        request.send(null);
    }
    else {
        keyBox.value = '';
        actionBtn.innerText = startSharingText;
        actionBtn.disabled = false;
    }
}

function checkShared() {
    if (sharedCheckDone) {
        return;
    }

    actionBtn.disabled = true;
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
            actionBtn.disabled = false;
        }
    }

    request.open("GET", "/api/sharedLinks/shared/" + linkId, true);
    request.send(null);
}

function performAction() {
    if (!sharedCheckDone) {
        return;
    }
    actionBtn.disabled = true;
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
            actionBtn.disabled = false;
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
            actionBtn.disabled = false;
        }
    }

    request.open("DELETE", "/api/sharedLinks/" + linkId, true);
    request.setRequestHeader('RequestVerificationToken', token);
    request.send(null);
}

function genericError() {
    alert("Something went wrong, please reload the site and try again");
}
