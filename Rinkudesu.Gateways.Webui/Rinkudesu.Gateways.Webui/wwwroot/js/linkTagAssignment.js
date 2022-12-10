let tagsDiv;

window.addEventListener('load', function () {
    tagsDiv = document.getElementById('tags');
    performHttpRequest(`/api/LinkTags/getTagsForLink/${tagsDiv.getAttribute('linkId')}`, "GET", null, onTagsLoad, _ => alert("Failed to load tags, please try again."));
});

function onTagsLoad(responseEvent) {
    if (responseEvent.currentTarget.status !== 200) {
        alert("Failed to load tags, please try again!");
        tagsDiv.innerText = "";
        return;
    }

    tagsDiv.innerHTML = responseEvent.currentTarget.responseText;
}
