let tagsDiv;

window.addEventListener('load', function () {
    tagsDiv = document.getElementById('tags');
    loadTags();
});


function onTagsLoad(responseEvent) {
    disableWithChildren(tagsDiv, true);
    if (responseEvent.currentTarget.status !== 200) {
        alert(document.getElementById('translations').getAttribute('data-load-failed'));
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

    initialiseTagsAutocompletion();

    // make sure tomselect inputs have proper class applied
    for (const tagNameInput of document.getElementById('tag-assignment').children) {
        for (const tsInputDiv of tagNameInput.getElementsByClassName('ts-control')) {
            if (!tsInputDiv.classList.contains('tag-name-entry-assign')) {
                tsInputDiv.classList.add('tag-name-entry-assign');
            }
        }
    }
}

function onTagDelete(button) {
    const tagId = button.getAttribute('data-tag');
    const linkId = button.getAttribute('data-link');
    let formData = new FormData();
    formData.set('linkId', linkId);
    formData.set('tagId', tagId);
    performHttpRequest(`/api/LinkTags/delete`, "POST", formData, _ => loadTags(), _ => alert(document.getElementById('translations').getAttribute('data-load-failed')));
    disableWithChildren(tagsDiv);
}

function loadTags() {
    performHttpRequest(`/api/LinkTags/getTagsForLink/${tagsDiv.getAttribute('linkId')}`, "GET", null, onTagsLoad, _ => alert(document.getElementById('translations').getAttribute('data-load-failed')));
}

function addTag(submitEvent) {
    submitEvent.preventDefault();

    let data = new FormData(document.getElementById('assignTag'));
    performHttpRequest('/api/LinkTags', "POST", data, _ => loadTags(), _ => alert("Failed to assign tag!"));
    disableWithChildren(tagsDiv);
}
