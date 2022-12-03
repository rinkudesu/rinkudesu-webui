document.addEventListener('keydown', event => {
    if (event.ctrlKey && event.key === 'Enter')
        document.activeElement?.form?.submit();
});
