window.TourPlannerMap = (function () {
    let map, routeGroup, markers = {}, markerClusterGroup, drawnItems, drawControl;

    const DEFAULT_VIEW = [48.2082, 16.3738];
    const DEFAULT_ZOOM = 5;
    const TILE_LAYER_URL = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
    const TILE_LAYER_ATTRIBUTION = '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors';

    function createIcon(color) {
        return L.icon({
            iconUrl: `https://cdn.rawgit.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-${color}.png`,
            shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
            iconSize: [25, 41],
            iconAnchor: [12, 41],
            popupAnchor: [1, -34],
            shadowSize: [41, 41]
        });
    }

    function initializeMap(element) {
        if (!element) {
            console.error("Map element not found");
            return;
        }
        try {
            map = L.map(element).setView(DEFAULT_VIEW, DEFAULT_ZOOM);
            L.tileLayer(TILE_LAYER_URL, {attribution: TILE_LAYER_ATTRIBUTION}).addTo(map);
            routeGroup = L.featureGroup().addTo(map);
            markerClusterGroup = L.markerClusterGroup();
            map.addLayer(markerClusterGroup);
            drawnItems = new L.FeatureGroup();
            map.addLayer(drawnItems);

            addCustomControls();
            addLayerControl();
            addDrawingControls();
            addEventListeners();
        } catch (error) {
            console.error("Error initializing map:", error);
        }
    }

    function addCustomControls() {
        L.control.zoom({position: 'topright'}).addTo(map);

        L.Control.LocationButton = L.Control.extend({
            onAdd: function (map) {
                const btn = L.DomUtil.create('button', 'leaflet-bar leaflet-control leaflet-control-custom');
                btn.innerHTML = '🎯';
                btn.style.fontSize = '20px';
                btn.style.padding = '5px 10px';
                btn.style.cursor = 'pointer';
                btn.title = 'Center on your location';
                btn.onclick = centerOnUserLocation;
                return btn;
            }
        });
        new L.Control.LocationButton({position: 'topleft'}).addTo(map);
    }

    function addLayerControl() {
        const baseLayers = {
            "OpenStreetMap": L.tileLayer(TILE_LAYER_URL, {attribution: TILE_LAYER_ATTRIBUTION}),
            "Satellite": L.tileLayer('https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', {attribution: 'Map data: &copy; OpenStreetMap contributors'})
        };
        L.control.layers(baseLayers).addTo(map);
    }

    function addDrawingControls() {
        const drawOptions = {
            position: 'topright',
            draw: {
                polyline: {shapeOptions: {color: '#f357a1', weight: 10}},
                polygon: {
                    allowIntersection: false,
                    drawError: {color: '#e1e100', message: '<strong>Oh snap!</strong> you can\'t draw that!'},
                    shapeOptions: {color: '#bada55'}
                },
                circle: false,
                rectangle: {shapeOptions: {clickable: false}},
                marker: {icon: createIcon('red')}
            },
            edit: {featureGroup: drawnItems, remove: true}
        };
        drawControl = new L.Control.Draw(drawOptions);
        map.addControl(drawControl);
    }

    function addEventListeners() {
        map.on(L.Draw.Event.CREATED, function (e) {
            const layer = e.layer;
            drawnItems.addLayer(layer);
            if (e.layerType === 'marker') {
                layer.bindPopup('A custom popup!');
            }
        });
    }

    function centerOnUserLocation() {
        map.locate({setView: true, maxZoom: 16});
    }

    function setRoute(fromLat, fromLng, toLat, toLng) {
        if (!map) {
            console.error("Map not initialized. Attempting to initialize...");
            initializeMap(document.getElementById('map'));
            if (!map) {
                console.error("Failed to initialize map. Cannot set route.");
                return;
            }
        }

        if (!routeGroup) {
            routeGroup = L.featureGroup().addTo(map);
        }

        try {
            clearMap();
            addMarker('start', fromLat, fromLng, createIcon('green'));
            addMarker('end', toLat, toLng, createIcon('red'));
            drawRouteLine();
            updateRouteInfo();
            animatePanToRoute();
            addDistanceCircles(fromLat, fromLng);
        } catch (error) {
            console.error("Error setting route:", error);
        }
    }

    function addMarker(type, lat, lng, icon) {
        try {
            markers[type] = L.marker([lat, lng], {icon: icon, draggable: true})
                .addTo(routeGroup)
                .bindPopup(createPopupContent(type, lat, lng))
                .bindTooltip(type.charAt(0).toUpperCase() + type.slice(1))
                .on('dragend', updateRoute);
        } catch (error) {
            console.error(`Error adding marker: ${error.message}`);
        }
    }

    function createPopupContent(type, lat, lng) {
        return `<b>${type.charAt(0).toUpperCase() + type.slice(1)}</b><br>Coordinates: ${lat.toFixed(4)}, ${lng.toFixed(4)}`;
    }

    function drawRouteLine() {
        if (markers.start && markers.end) {
            const start = markers.start.getLatLng();
            const end = markers.end.getLatLng();
            L.polyline([start, end], {color: 'blue', weight: 3, opacity: 0.7, smoothFactor: 1}).addTo(routeGroup);
        }
    }

    function updateRoute() {
        routeGroup.clearLayers();
        drawRouteLine();
        updateRouteInfo();
    }

    function updateRouteInfo() {
        if (markers.start && markers.end) {
            const start = markers.start.getLatLng();
            const end = markers.end.getLatLng();
            const distance = calculateDistance(start.lat, start.lng, end.lat, end.lng);
            const duration = estimateDuration(distance);
            const info = `<br>Distance: ${distance.toFixed(2)} km<br>Estimated Duration: ${duration}`;

            markers.start.setPopupContent(createPopupContent('start', start.lat, start.lng) + info);
            markers.end.setPopupContent(createPopupContent('end', end.lat, end.lng) + info);
        }
    }

    function animatePanToRoute() {
        const bounds = routeGroup.getBounds();
        if (bounds.isValid()) {
            map.flyToBounds(bounds, {padding: [50, 50], duration: 0.5});
        }
    }

    function addDistanceCircles(lat, lng) {
        [10, 20, 50].forEach(radius => {
            L.circle([lat, lng], {radius: radius * 1000, fill: false, color: 'red', weight: 1}).addTo(routeGroup);
        });
    }

    function clearMap() {
        try {
            if (routeGroup) {
                routeGroup.clearLayers();
            }
            if (drawnItems) {
                drawnItems.clearLayers();
            }
            markers = {};
        } catch (error) {
            console.error(`Error clearing map: ${error.message}`);
        }
    }

    function calculateDistance(lat1, lon1, lat2, lon2) {
        const R = 6371;
        const dLat = deg2rad(lat2 - lat1);
        const dLon = deg2rad(lon2 - lon1);
        const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.cos(deg2rad(lat1)) * Math.cos(deg2rad(lat2)) *
            Math.sin(dLon / 2) * Math.sin(dLon / 2);
        const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        return R * c;
    }

    function deg2rad(deg) {
        return deg * (Math.PI / 180);
    }

    function estimateDuration(distance) {
        const hours = distance / 60;
        const minutes = Math.round(hours * 60);
        return `${Math.floor(hours)}h ${minutes % 60}m`;
    }

    return {
        initializeMap: initializeMap,
        setRoute: setRoute,
        clearMap: clearMap,
        centerOnUserLocation: centerOnUserLocation
    };
})();
