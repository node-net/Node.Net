// JavaScript module for Maps component Leaflet integration
// This module assumes Leaflet is loaded globally as window.L

export function initializeMap(elementId, latitude, longitude, zoomLevel, mapType) {
    // Get the map element
    const mapElement = document.getElementById(elementId);
    if (!mapElement) {
        console.error(`Map element with id '${elementId}' not found`);
        return null;
    }

    // Check if Leaflet is available
    if (typeof window.L === 'undefined') {
        console.error('Leaflet library (window.L) is not loaded. Please ensure Leaflet CSS and JS are included in your application.');
        return null;
    }

    // Initialize Leaflet map
    const map = window.L.map(elementId).setView([latitude, longitude], zoomLevel);

    // Add tile layer based on map type
    let tileUrl;
    switch (mapType) {
        case 'satellite':
            // Use OpenStreetMap satellite-like tiles (actually using standard OSM tiles as fallback)
            // For true satellite, you would use a provider like Esri World Imagery
            tileUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
            break;
        case 'roadmap':
            tileUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
            break;
        case 'terrain':
            tileUrl = 'https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png';
            break;
        default:
            tileUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
    }

    window.L.tileLayer(tileUrl, {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        maxZoom: 19
    }).addTo(map);

    return map;
}

export function updateMapCenter(mapInstance, latitude, longitude) {
    if (mapInstance && typeof mapInstance.setView === 'function') {
        mapInstance.setView([latitude, longitude], mapInstance.getZoom());
    }
}

export function updateZoomLevel(mapInstance, zoomLevel) {
    if (mapInstance && typeof mapInstance.setZoom === 'function') {
        mapInstance.setZoom(zoomLevel);
    }
}

export function updateMapType(mapInstance, mapType) {
    if (!mapInstance) return;

    // Remove existing tile layers
    mapInstance.eachLayer((layer) => {
        if (layer instanceof window.L.TileLayer) {
            mapInstance.removeLayer(layer);
        }
    });

    // Add new tile layer based on map type
    let tileUrl;
    switch (mapType) {
        case 'satellite':
            tileUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
            break;
        case 'roadmap':
            tileUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
            break;
        case 'terrain':
            tileUrl = 'https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png';
            break;
        default:
            tileUrl = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
    }

    window.L.tileLayer(tileUrl, {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        maxZoom: 19
    }).addTo(mapInstance);
}
