// ✅ Log when script is loaded to help debug
console.log("✅ cookieHelper.js loaded");

// ✅ Read a cookie by name
window.getCookie = function (name) {
    const nameEQ = name + "=";
    const ca = document.cookie.split(';');

    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];

        // Trim whitespace
        while (c.charAt(0) === ' ') {
            c = c.substring(1, c.length);
        }

        // If cookie name matches
        if (c.indexOf(nameEQ) === 0) {
            return c.substring(nameEQ.length, c.length);
        }
    }

    return null; // Cookie not found
};

// ✅ Set a cookie (non-HttpOnly, accessible by JS)
window.setCookie = function (name, value, days) {
    let expires = "";
    
    if (days) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
};
