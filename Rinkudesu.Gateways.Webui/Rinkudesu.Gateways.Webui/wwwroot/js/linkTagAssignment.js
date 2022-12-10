let tagsDiv;
//todo: localise this file

window.addEventListener('load', function () {
    tagsDiv = document.getElementById('tags');
    loadTags();
});


function onTagsLoad(responseEvent) {
    if (responseEvent.currentTarget.status !== 200) {
        alert("Failed to load tags, please try again!");
        tagsDiv.innerText = "";
        return;
    }

    tagsDiv.innerHTML = responseEvent.currentTarget.responseText;
    document.getElementById('assignTag')?.addEventListener('submit', addTag);
}

function loadTags() {
    performHttpRequest(`/api/LinkTags/getTagsForLink/${tagsDiv.getAttribute('linkId')}`, "GET", null, onTagsLoad, _ => alert("Failed to load tags, please try again."));
}

function addTag(submitEvent) {
    submitEvent.preventDefault();

    let data = new FormData(document.getElementById('assignTag'));
    performHttpRequest('/api/LinkTags', "POST", data, null, _ => alert("Failed to assign tag!"));
    loadTags();
}
