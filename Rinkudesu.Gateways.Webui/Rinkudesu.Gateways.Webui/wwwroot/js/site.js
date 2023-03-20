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
    for (const tomselect of document.getElementsByClassName('tomselect')) {
        new TomSelect(tomselect, {});
    }
}

function initialiseTagsAutocompletion() {
    for (const tomselect of document.getElementsByClassName('tags-autocompletion')) {
        const allowCreate = tomselect.getAttribute('data-allow-create') != null;
        let select = new TomSelect(tomselect, {
            valueField: 'id',
            labelField: 'data',
            searchField: 'data',
            create: allowCreate,
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
        select.on("option_add", createTagInAutocompletion);
    }
}

function createTagInAutocompletion(value, data) {
    const onTagsCreate = function (localData, select, response) {
        if (response.currentTarget.status !== 201) {
            // As you'll see below, option management in this is a nightmare, so when something goes wrong just give up and don't even try to handle anything.
            alert("Failed to create tags!");
            select.unlock();
            return;
        }

        // Story time:
        // Item first needs to be added to tomselect, and only after sent to API to be created.
        // Initially, it's created with id equal to text provided, which is not how the back-end stores it.
        // For that reason, to avoid issues with misaligned ids, remove this option entirely first...
        select.removeOption(localData.id);
        // ...then parse the returned object and set the id properly...
        let responseJson = JSON.parse(response.currentTarget.responseText);
        localData.id = responseJson.id;
        // ...then add the new option and select id...
        select.addOption(localData);
        select.addItem(localData.id, true);
        // ...and then rage-quit JavaScript.
        // So for whatever reason, tomselect fails to properly update the underlying select control.
        // The issue is that the "value" of the "option" in this control is set to the display value, instead of the id.
        // Another issue is that this option is not even removed when "removeOption" is called.
        // For that reason, to avoid further issues, just forcibly change the "value" property of the relevant option to what it's supposed to be.
        // This "works" and submits properly. It's definitely not pretty, but that's JS development for you, I guess.
        for (const option of select.input.options) {
            if (option.value === localData.data) {
                option.value = localData.id;
                break;
            }
        }
        select.unlock();
    }

    this.lock();
    let newTagData = new FormData();
    newTagData.append('name', value);
    performHttpRequest('/api/autocompletion/TagsAutocompletionApi', 'POST', newTagData, response => onTagsCreate(data, this, response), _ => alert("Failed to create tag!"));
}
