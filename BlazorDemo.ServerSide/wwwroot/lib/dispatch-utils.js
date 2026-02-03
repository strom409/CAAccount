// ============================================
// DISPATCH PAGE - JAVASCRIPT UTILITIES
// Smooth scrolling, responsive helpers, and UX enhancements
// ============================================

window.dispatchUtils = {
    /**
     * Smooth scroll to a specific element
     * @param {string} elementId - The ID of the element to scroll to
     * @param {number} offset - Offset from top in pixels (default: 0)
     */
    scrollToElement: function(elementId, offset = 0) {
        const element = document.getElementById(elementId);
        if (element) {
            const top = element.getBoundingClientRect().top + window.pageYOffset - offset;
            window.scrollTo({
                top: top,
                behavior: 'smooth'
            });
        }
    },

    /**
     * Smooth scroll to top of page
     */
    scrollToTop: function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    },

    /**
     * Check if element is currently in viewport
     * @param {string} elementId - The ID of the element to check
     * @returns {boolean} True if element is in viewport
     */
    isInViewport: function(elementId) {
        const element = document.getElementById(elementId);
        if (!element) return false;
        
        const rect = element.getBoundingClientRect();
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    },

    /**
     * Get current scroll position
     * @returns {number} Current scroll position in pixels
     */
    getScrollPosition: function() {
        return window.pageYOffset || document.documentElement.scrollTop;
    },

    /**
     * Initialize sticky header behavior
     * @param {string} headerSelector - CSS selector for header element
     * @param {number} offset - Scroll offset to trigger sticky (default: 0)
     * @returns {function} Cleanup function to remove event listener
     */
    initStickyHeader: function(headerSelector, offset = 0) {
        const header = document.querySelector(headerSelector);
        if (!header) return () => {};

        const handleScroll = () => {
            if (window.pageYOffset > offset) {
                header.classList.add('sticky-active');
            } else {
                header.classList.remove('sticky-active');
            }
        };

        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    },

    /**
     * Debounce function to limit execution rate
     * @param {function} func - Function to debounce
     * @param {number} wait - Wait time in milliseconds
     * @returns {function} Debounced function
     */
    debounce: function(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    },

    /**
     * Throttle function to limit execution frequency
     * @param {function} func - Function to throttle
     * @param {number} limit - Minimum time between executions
     * @returns {function} Throttled function
     */
    throttle: function(func, limit) {
        let inThrottle;
        return function(...args) {
            if (!inThrottle) {
                func.apply(this, args);
                inThrottle = true;
                setTimeout(() => inThrottle = false, limit);
            }
        };
    },

    /**
     * Check if device is mobile
     * @returns {boolean} True if mobile device
     */
    isMobile: function() {
        return window.innerWidth <= 768;
    },

    /**
     * Check if device is tablet
     * @returns {boolean} True if tablet device
     */
    isTablet: function() {
        return window.innerWidth > 768 && window.innerWidth <= 1024;
    },

    /**
     * Get current device type
     * @returns {string} 'mobile', 'tablet', or 'desktop'
     */
    getDeviceType: function() {
        if (this.isMobile()) return 'mobile';
        if (this.isTablet()) return 'tablet';
        return 'desktop';
    },

    /**
     * Show loading overlay
     * @param {string} message - Optional loading message
     */
    showLoading: function(message = '') {
        // Remove existing overlay if present
        this.hideLoading();

        const overlay = document.createElement('div');
        overlay.id = 'dispatch-loading';
        overlay.className = 'loading-overlay';
        
        const content = `
            <div class="spinner"></div>
            ${message ? `<p class="loading-message">${message}</p>` : ''}
        `;
        
        overlay.innerHTML = content;
        document.body.appendChild(overlay);
        document.body.style.overflow = 'hidden';
    },

    /**
     * Hide loading overlay
     */
    hideLoading: function() {
        const overlay = document.getElementById('dispatch-loading');
        if (overlay) {
            overlay.style.opacity = '0';
            setTimeout(() => {
                overlay.remove();
                document.body.style.overflow = '';
            }, 300);
        }
    },

    /**
     * Auto-hide element after timeout
     * @param {string} elementId - ID of element to hide
     * @param {number} timeout - Time in milliseconds before hiding
     */
    autoHideAlert: function(elementId, timeout = 3000) {
        setTimeout(() => {
            const element = document.getElementById(elementId);
            if (element) {
                element.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
                element.style.opacity = '0';
                element.style.transform = 'translateY(-20px)';
                setTimeout(() => element.remove(), 300);
            }
        }, timeout);
    },

    /**
     * Initialize responsive table handling
     * @param {string} tableSelector - CSS selector for tables
     */
    initResponsiveTable: function(tableSelector) {
        const tables = document.querySelectorAll(tableSelector);
        
        tables.forEach(table => {
            const container = table.closest('.grid-container');
            
            // Add touch scrolling for mobile
            if (this.isMobile() && container) {
                container.style.overflowX = 'auto';
                container.style.WebkitOverflowScrolling = 'touch';
                
                // Add scroll indicator
                this.addScrollIndicator(container);
            }
        });
    },

    /**
     * Add visual scroll indicator for horizontal scrolling
     * @param {HTMLElement} container - Container element
     */
    addScrollIndicator: function(container) {
        if (!container) return;

        const indicator = document.createElement('div');
        indicator.className = 'scroll-indicator';
        indicator.innerHTML = '<i class="fa fa-chevron-right"></i>';
        container.appendChild(indicator);

        const checkScroll = () => {
            const isScrollable = container.scrollWidth > container.clientWidth;
            const isAtEnd = container.scrollLeft + container.clientWidth >= container.scrollWidth - 5;
            
            if (!isScrollable || isAtEnd) {
                indicator.style.opacity = '0';
            } else {
                indicator.style.opacity = '1';
            }
        };

        container.addEventListener('scroll', this.debounce(checkScroll, 100));
        checkScroll();
    },

    /**
     * Animate element entrance
     * @param {string} elementId - ID of element to animate
     * @param {string} animation - Animation type ('fadeIn', 'slideUp', 'slideDown')
     */
    animateElement: function(elementId, animation = 'fadeIn') {
        const element = document.getElementById(elementId);
        if (!element) return;

        element.classList.add(animation);
    },

    /**
     * Copy text to clipboard
     * @param {string} text - Text to copy
     * @returns {Promise<boolean>} Success status
     */
    copyToClipboard: async function(text) {
        try {
            await navigator.clipboard.writeText(text);
            return true;
        } catch (err) {
            console.error('Failed to copy:', err);
            return false;
        }
    },

    /**
     * Format number with thousands separator
     * @param {number} num - Number to format
     * @param {string} separator - Thousands separator (default: ',')
     * @returns {string} Formatted number
     */
    formatNumber: function(num, separator = ',') {
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, separator);
    },

    /**
     * Validate form field
     * @param {string} fieldId - ID of form field
     * @param {string} validationType - Type of validation
     * @returns {boolean} Validation result
     */
    validateField: function(fieldId, validationType) {
        const field = document.getElementById(fieldId);
        if (!field) return false;

        const value = field.value.trim();
        let isValid = false;

        switch(validationType) {
            case 'required':
                isValid = value !== '';
                break;
            case 'email':
                isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
                break;
            case 'phone':
                isValid = /^\d{10}$/.test(value);
                break;
            case 'number':
                isValid = !isNaN(value) && value !== '';
                break;
            default:
                isValid = true;
        }

        // Add visual feedback
        if (isValid) {
            field.classList.remove('is-invalid');
            field.classList.add('is-valid');
        } else {
            field.classList.remove('is-valid');
            field.classList.add('is-invalid');
        }

        return isValid;
    },

    /**
     * Initialize keyboard shortcuts
     * @param {Object} shortcuts - Object mapping key combinations to functions
     */
    initKeyboardShortcuts: function(shortcuts) {
        document.addEventListener('keydown', (e) => {
            // Guard against undefined or null key values
            if (!e.key) return;
            
            const key = e.key.toLowerCase();
            const ctrl = e.ctrlKey || e.metaKey;
            const shift = e.shiftKey;
            const alt = e.altKey;

            const combo = `${ctrl ? 'ctrl+' : ''}${shift ? 'shift+' : ''}${alt ? 'alt+' : ''}${key}`;

            if (shortcuts[combo]) {
                e.preventDefault();
                shortcuts[combo]();
            }
        });
    },

    /**
     * Local storage helper with expiration
     * @param {string} key - Storage key
     * @param {any} value - Value to store
     * @param {number} expiryMinutes - Minutes until expiration
     */
    setWithExpiry: function(key, value, expiryMinutes) {
        const now = new Date();
        const item = {
            value: value,
            expiry: now.getTime() + (expiryMinutes * 60 * 1000)
        };
        localStorage.setItem(key, JSON.stringify(item));
    },

    /**
     * Get item from local storage with expiration check
     * @param {string} key - Storage key
     * @returns {any} Stored value or null if expired
     */
    getWithExpiry: function(key) {
        const itemStr = localStorage.getItem(key);
        if (!itemStr) return null;

        const item = JSON.parse(itemStr);
        const now = new Date();

        if (now.getTime() > item.expiry) {
            localStorage.removeItem(key);
            return null;
        }

        return item.value;
    }
};

// ============================================
// AUTO-INITIALIZATION
// ============================================

window.addEventListener('DOMContentLoaded', () => {
    console.log('Dispatch utilities loaded');

    // Initialize responsive tables
    window.dispatchUtils.initResponsiveTable('.dxbl-grid');
    
    // Create and initialize back-to-top button
    const backToTopBtn = document.createElement('button');
    backToTopBtn.id = 'backToTop';
    backToTopBtn.className = 'back-to-top-btn';
    backToTopBtn.innerHTML = '<i class="fa fa-arrow-up"></i>';
    backToTopBtn.style.display = 'none';
    backToTopBtn.setAttribute('aria-label', 'Back to top');
    backToTopBtn.setAttribute('title', 'Back to top');
    document.body.appendChild(backToTopBtn);

    // Show/hide back-to-top button based on scroll position
    const handleScroll = window.dispatchUtils.debounce(() => {
        if (window.pageYOffset > 300) {
            backToTopBtn.style.display = 'flex';
            backToTopBtn.style.opacity = '1';
        } else {
            backToTopBtn.style.opacity = '0';
            setTimeout(() => {
                if (window.pageYOffset <= 300) {
                    backToTopBtn.style.display = 'none';
                }
            }, 300);
        }
    }, 100);

    window.addEventListener('scroll', handleScroll);

    backToTopBtn.addEventListener('click', () => {
        window.dispatchUtils.scrollToTop();
    });

    // Initialize sticky header
    window.dispatchUtils.initStickyHeader('.dispatch-header', 60);

    // Handle window resize
    const handleResize = window.dispatchUtils.debounce(() => {
        window.dispatchUtils.initResponsiveTable('.dxbl-grid');
    }, 250);

    window.addEventListener('resize', handleResize);

    // Keyboard shortcuts (optional - won't break if elements don't exist)
    try {
        window.dispatchUtils.initKeyboardShortcuts({
            'ctrl+s': () => {
                const saveBtn = document.querySelector('.btn-save');
                if (saveBtn) {
                    saveBtn.click();
                    console.log('Save shortcut triggered');
                }
            },
            'ctrl+f': () => {
                const searchBox = document.querySelector('.dxbl-grid-search-box input');
                if (searchBox) {
                    searchBox.focus();
                    console.log('Search shortcut triggered');
                }
            },
            'escape': () => {
                // Close any open modals
                const closeButtons = document.querySelectorAll('.dxbs-popup-close-button');
                if (closeButtons.length > 0) {
                    closeButtons[0].click();
                    console.log('Escape shortcut triggered');
                }
            }
        });
        console.log('Keyboard shortcuts initialized');
    } catch (error) {
        console.warn('Keyboard shortcuts initialization failed:', error);
    }
});

// Export for use in modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.dispatchUtils;
}
