

// Mapa

var map = L.map('map').setView([-27.1, -52.6], 13);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap'
}).addTo(map);

let pontosCarregados;

let markers = [];

async function carregarPontos() {
    try {
        const response = await fetch('/Ponto/PontosSalvos');

        if (!response.ok) {
            throw new Error('Erro na requisição');
        }

        pontosCarregados = await response.json();

        console.log(pontosCarregados);

        pontosCarregados.forEach(p => {
            const markedPin = L.marker([parseFloat(p.lat), parseFloat(p.lon)]).addTo(map);
            markedPin.pontoId = p.id;
            markedPin._icon.classList.add('marked-pin');

            markers.push(markedPin);
            console.log(markedPin);
        });
    }
    catch (erro) {
        console.error('Erro: ', erro);
    }
}

carregarPontos();

// Localizacao atual

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

let latSelecionada;
let lonSelecionada;

const checkOutroTipo = document.getElementById("checkOutroTipo");
const inputOutroTipo = document.getElementById("inputOutroTipo");

const containerErro = document.getElementById("containerErro");
const mensagemErro = document.getElementById("mensagemErro");
function abrirCadastrarModal(lat, lon) {

    inputOutroTipo.classList.add("display-none-imp");

    latSelecionada = lat;
    lonSelecionada = lon;

    const inputLat = document.getElementById('lat');
    inputLat.value = lat;

    const inputLon = document.getElementById('lon');
    inputLon.value = lon;

    const modal = new bootstrap.Modal(document.getElementById('cadastroModal'));
    modal.show();

}

function fecharCadastrarModal() {

    const modalElement = document.getElementById('cadastroModal');
    const modal = bootstrap.Modal.getInstance(modalElement);

    if (modal) {
        modal.hide();
        form.reset();
        containerErro.classList.add("display-none-imp");
    }

}

checkOutroTipo.addEventListener("change", function () {
    if (this.checked) {
        inputOutroTipo.classList.remove("display-none-imp");
    }
    else {
        inputOutroTipo.classList.add("display-none-imp");
        inputOutroTipo.value = "";
    }
})

var marker;

map.on('click', function(e) {

    if(marker){
        map.removeLayer(marker);
        
    }

    marker = L.marker(e.latlng).addTo(map);

    console.log("Latitude:", e.latlng.lat);
    console.log("Longitude:", e.latlng.lng);

    abrirCadastrarModal(e.latlng.lat, e.latlng.lng)

});


function exibirErro(message) {
    containerErro.classList.remove("display-none-imp");
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

            fecharCadastrarModal();
            
        })
        .catch(error => {
            console.error("Erro: ", error);
            exibirErro("Erro ao enviar ponto. Verifique os campos obrigatórios e tente novamente.")
        });
})

