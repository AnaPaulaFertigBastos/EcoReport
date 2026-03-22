
var map = L.map('map').setView([-27.1, -52.6], 13);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap'
}).addTo(map);

let latSelecionada;
let lonSelecionada;
function abrirModal(lat, lon) {
    latSelecionada = lat;
    lonSelecionada = lon;

    const inputLat = document.getElementById('lat');
    inputLat.value = lat;

    const inputLon = document.getElementById('lon');
    inputLon.value = lon;

    const modal = new bootstrap.Modal(document.getElementById('modal'));
    modal.show();

}

function fecharModal() {
    
    const modalElement = document.getElementById('modal');
    const modal = bootstrap.Modal.getInstance(modalElement);

    if (modal) {
        modal.hide();
        form.reset();
    }

}

const checkOutroTipo = document.getElementById("checkOutroTipo");
const inputOutroTipo = document.getElementById("inputOutroTipo");

checkOutroTipo.addEventListener("change", function () {
    if (this.checked) {
        inputOutroTipo.style.display = "block";
    }
    else {
        inputOutroTipo.style.display = "none";
        inputOutroTipo.value = "";
    }
})

navigator.geolocation.getCurrentPosition(function(position) {

    var lat = position.coords.latitude;
    var lng = position.coords.longitude;

    map.setView([lat, lng], 15);

    var currentPosition = L.marker([lat, lng])
        .addTo(map)
        .bindPopup("Sua localização")
        .openPopup();

    currentPosition._icon.classList.add('current-location-icon');
});

var marker;

map.on('click', function(e) {

    if(marker){
        map.removeLayer(marker);
        
    }

    marker = L.marker(e.latlng).addTo(map);

    console.log("Latitude:", e.latlng.lat);
    console.log("Longitude:", e.latlng.lng);

    abrirModal(e.latlng.lat, e.latlng.lng)

});

const containerErro = document.getElementById("containerErro");
const mensagemErro = document.getElementById("mensagemErro");

function exibirErro(message) {
    containerErro.style.display = "flex";
    mensagemErro.textContent = message;
}

const form = document.getElementById("FormPonto");

document.getElementById("FormPonto").addEventListener("submit", function (e) {
    e.preventDefault();

    
    const formData = new FormData(form);

    fetch('/Ponto/Criar', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Erro ao enviar");
            }
            return response.json();
        })
        .then(data => {
            if (!data.success) {
                exibirErro(data.message);
                return;
            }

            fecharModal();
            
        })
        .catch(error => {
            console.error("Erro: ", error);
            exibirErro("Erro ao enviar ponto. Verifique os campos obrigatórios e tente novamente.")
        });
})

