function switchTheme(theme) {

    const currentTheme = localStorage.getItem('theme') ? localStorage.getItem('theme') : null;
    theme = theme ? theme : currentTheme;

    if (theme == 'dark') {
        document.documentElement.setAttribute('data-theme', 'dark');
        localStorage.setItem('theme', 'dark');
    }
    else if (theme == 'light') {
        document.documentElement.setAttribute('data-theme', 'light');
        localStorage.setItem('theme', 'light');
    }
    else {
        document.documentElement.removeAttribute('data-theme');
    }

}

