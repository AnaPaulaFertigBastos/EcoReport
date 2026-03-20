
var map = L.map('map').setView([-27.1, -52.6], 13);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap'
}).addTo(map);

navigator.geolocation.getCurrentPosition(function(position) {

    var lat = position.coords.latitude;
    var lng = position.coords.longitude;

    map.setView([lat, lng], 15);

    L.marker([lat, lng])
        .addTo(map)
        .bindPopup("Sua localização")
        .openPopup();

});

var marker;

map.on('click', function(e) {

    if(marker){
        map.removeLayer(marker);
    }

    marker = L.marker(e.latlng).addTo(map);

    console.log("Latitude:", e.latlng.lat);
    console.log("Longitude:", e.latlng.lng);

});

