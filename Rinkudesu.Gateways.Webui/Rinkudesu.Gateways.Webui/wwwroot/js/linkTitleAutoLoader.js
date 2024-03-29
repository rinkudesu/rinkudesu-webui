let titleHasFocus = false;

window.addEventListener('load', () => {
    let titleField = document.getElementById('Title');
    let urlField = document.getElementById('LinkUrl');

    titleField.addEventListener('focusin', () => titleHasFocus = true);
    titleField.addEventListener('focusout', () => titleHasFocus = false);
    urlField.addEventListener('focusout', () => getTitle(() => urlField.value, titleField));
});

function getTitle(urlValueGetter, titleField) {
    const url = urlValueGetter();
    if (!url || titleField.value || titleHasFocus)
        return;

    let onload = e => {
        if (titleField.value || titleHasFocus || e.currentTarget.status !== 200)
            return;

        titleField.value = e.currentTarget.responseText;
    }
    let onerror = () => console.log("Failed to fetch title for current url");

    let requestUrl = new URL(`${window.location.origin}/api/autocompletion/LinkTitleAutocompletion`);
    requestUrl.searchParams.append('url', url);
    performHttpRequest(requestUrl.toString(), 'GET', null, onload, onerror);
}
