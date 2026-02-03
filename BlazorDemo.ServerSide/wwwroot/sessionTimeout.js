// wwwroot/js/sessionTimeout.js
let timeoutId;
let warningId;
let dotNetHelper;

window.sessionTimeout = {
    initialize: (dotNetRef, timeoutMilliseconds) => {
        dotNetHelper = dotNetRef;
        
        // Set initial timeout
        setTimers(timeoutMilliseconds);
        
        // Add event listeners for user activity
        document.addEventListener('mousemove', resetTimer);
        document.addEventListener('keypress', resetTimer);
        document.addEventListener('click', resetTimer);
        document.addEventListener('scroll', resetTimer);
        document.addEventListener('touchstart', resetTimer);
    },
    
    reset: () => {
        clearTimeout(timeoutId);
        clearTimeout(warningId);
        setTimers();
    }
};

function setTimers(timeoutMilliseconds = 20 * 60 * 1000) { // Default 20 minutes
    const warningTime = 60 * 1000; // 1 minute warning
    
    // Set warning timer
    warningId = setTimeout(() => {
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync('ShowTimeoutWarning');
        }
    }, timeoutMilliseconds - warningTime);
    
    // Set logout timer
    timeoutId = setTimeout(() => {
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync('TimeoutUser');
        }
    }, timeoutMilliseconds);
}

function resetTimer() {
    if (dotNetHelper) {
        dotNetHelper.invokeMethodAsync('UserIsActive');
    }
}