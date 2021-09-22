var isSystemDark = window.matchMedia('(prefers-color-scheme: dark)');//Is syst

function switchTheme(theme) {
    var themeInBarrel = localStorage.getItem('theme');
    var isThemeSystem = themeInBarrel.includes("System");

    if (theme === "System") {
        isSystemDark.addEventListener('change', OSChanged);
        theme = isSystemDark.matches ? "Dark" : "Light";
    }
    else if (!isThemeSystem) {
        isSystemDark.removeEventListener('change', OSChanged);
    }

    if (theme === "Dark") {
        document.documentElement.setAttribute('data-theme', 'dark');
    }
    else {
        document.documentElement.setAttribute('data-theme', 'light');
    }
}

function OSChanged(e) {
    var newTheme = isSystemDark.matches ? "Dark" : "Light";
    switchTheme(newTheme);
}

