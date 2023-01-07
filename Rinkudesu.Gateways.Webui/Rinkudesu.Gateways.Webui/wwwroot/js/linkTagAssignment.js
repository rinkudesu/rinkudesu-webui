let tagsDiv;
//todo: localise this file

window.addEventListener('load', function () {
    tagsDiv = document.getElementById('tags');
    loadTags();
});


function onTagsLoad(responseEvent) {
    disableWithChildren(tagsDiv, true);
    if (responseEvent.currentTarget.status !== 200) {
        alert("Failed to load tags, please try again!");
        tagsDiv.innerText = "";
        return;
    }

    tagsDiv.innerHTML = responseEvent.currentTarget.responseText;
    document.getElementById('assignTag')?.addEventListener('submit', addTag);

    for (const deleteButton of document.getElementsByClassName('delete-btn')) {
        if (deleteButton.getAttribute('data-action') !== "link-tag-delete")
            continue;

        deleteButton.addEventListener('click', _ => onTagDelete(deleteButton));
    }
}

function onTagDelete(button) {
    const tagId = button.getAttribute('data-tag');
    const linkId = button.getAttribute('data-link');
    let formData = new FormData();
    formData.set('linkId', linkId);
    formData.set('tagId', tagId);
    performHttpRequest(`/api/LinkTags/delete`, "POST", formData, _ => loadTags(), _ => alert("Failed to load tags, please try again."));
    disableWithChildren(tagsDiv);
}

function loadTags() {
    performHttpRequest(`/api/LinkTags/getTagsForLink/${tagsDiv.getAttribute('linkId')}`, "GET", null, onTagsLoad, _ => alert("Failed to load tags, please try again."));
}

function addTag(submitEvent) {
    submitEvent.preventDefault();

    let data = new FormData(document.getElementById('assignTag'));
    performHttpRequest('/api/LinkTags', "POST", data, _ => loadTags(), _ => alert("Failed to assign tag!"));
    disableWithChildren(tagsDiv);
}
