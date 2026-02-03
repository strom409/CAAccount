// ✅ Log when script is loaded to help debug
console.log("✅ report-zoom.js loaded");

// Report zoom functionality
window.reportZoom = {
    currentZoom: 100,

    // Set zoom level for report viewing
    setZoom: function (zoomLevel) {
        this.currentZoom = zoomLevel;
        const reportContainer = document.querySelector('.report-container');
        if (reportContainer) {
            reportContainer.style.zoom = zoomLevel + '%';
        }
    },

    // Increase zoom
    zoomIn: function () {
        if (this.currentZoom < 200) {
            this.currentZoom += 10;
            this.setZoom(this.currentZoom);
        }
    },

    // Decrease zoom
    zoomOut: function () {
        if (this.currentZoom > 50) {
            this.currentZoom -= 10;
            this.setZoom(this.currentZoom);
        }
    },

    // Reset to 100%
    resetZoom: function () {
        this.currentZoom = 100;
        this.setZoom(100);
    }
};
