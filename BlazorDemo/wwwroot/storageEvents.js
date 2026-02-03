let storageListenerRegistered = false;
let dotNetRef = null;

export function registerStorageListener(dotNetReference) {
    if (storageListenerRegistered) return;
    
    dotNetRef = dotNetReference;
    
    const storageEventListener = function(event) {
        if (event.key === 'Username' || event.key === 'UserGroup' || 
            event.key === 'UnitName' || event.key === 'UnitId') {
            console.log('Storage change detected:', event.key, event.newValue);
            
            if (dotNetRef) {
                try {
                    dotNetRef.invokeMethodAsync('OnStorageChangedAsync', event.key);
                } catch (error) {
                    console.error('Error calling .NET method:', error);
                }
            }
        }
    };

    window.addEventListener('storage', storageEventListener);
    storageListenerRegistered = true;
    
    console.log('Storage listener registered');
}

export function unregisterStorageListener() {
    window.removeEventListener('storage', window.storageEventListener);
    storageListenerRegistered = false;
    dotNetRef = null;
    console.log('Storage listener unregistered');
}

// Global function that can be called from any tab
window.forceUserDataReload = function() {
    if (dotNetRef) {
        dotNetRef.invokeMethodAsync('OnStorageChangedAsync', 'forceReload');
    }
};