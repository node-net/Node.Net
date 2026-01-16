// JavaScript module for Maps component Leaflet integration
// This module assumes Leaflet is loaded globally as window.L

// Store map instances by element ID
const mapInstances = {};

export function initializeMap(elementId, latitude, longitude, zoomLevel, mapType) {
    // Get the map element
    const mapElement = document.getElementById(elementId);
    if (!mapElement) {
        console.error(`[Maps.razor.js] Map element with id '${elementId}' not found`);
        return null;
    }

    // Check if Leaflet is available - wait a bit if it's still loading
    if (typeof window.L === 'undefined') {
        console.error('[Maps.razor.js] Leaflet library (window.L) is not loaded. Please ensure Leaflet CSS and JS are included in your application.');
        console.error('[Maps.razor.js] Make sure Leaflet script is loaded before the Blazor framework script.');
        return null;
    }
    
    console.log(`[Maps.razor.js] Initializing map for element '${elementId}' at [${latitude}, ${longitude}] with zoom ${zoomLevel} and type '${mapType}'`);

    // Remove existing map if it exists
    if (mapInstances[elementId]) {
        mapInstances[elementId].remove();
        delete mapInstances[elementId];
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

    // Store map instance
    mapInstances[elementId] = map;

    // Return a reference object that can be serialized
    return { elementId: elementId };
}

export function updateMapCenter(mapRef, latitude, longitude) {
    if (!mapRef || !mapRef.elementId) return;
    const map = mapInstances[mapRef.elementId];
    if (map && typeof map.setView === 'function') {
        map.setView([latitude, longitude], map.getZoom());
    }
}

export function updateZoomLevel(mapRef, zoomLevel) {
    if (!mapRef || !mapRef.elementId) return;
    const map = mapInstances[mapRef.elementId];
    if (map && typeof map.setZoom === 'function') {
        map.setZoom(zoomLevel);
    }
}

export function updateMapType(mapRef, mapType) {
    if (!mapRef || !mapRef.elementId) return;
    const map = mapInstances[mapRef.elementId];
    if (!map) return;

    // Remove existing tile layers
    map.eachLayer((layer) => {
        if (layer instanceof window.L.TileLayer) {
            map.removeLayer(layer);
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
    }).addTo(map);
}

export function disposeMap(mapRef) {
    if (!mapRef || !mapRef.elementId) return;
    const map = mapInstances[mapRef.elementId];
    if (map) {
        map.remove();
        delete mapInstances[mapRef.elementId];
    }
}
