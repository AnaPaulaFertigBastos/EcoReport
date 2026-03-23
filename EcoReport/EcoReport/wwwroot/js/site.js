

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

            configurarMarker(markedPin, p.id);
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

// Cadastrar um ponto

let latSelecionada;
let lonSelecionada;

const checkOutroTipo = document.getElementById("checkOutroTipo");
const inputOutroTipo = document.getElementById("inputOutroTipo");

const containerErro = document.getElementById("containerErro");
const mensagemErro = document.getElementById("mensagemErro");
function abrirCadastrarModal(lat, lon) {

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

function novoPonto(lat, lon, id) {
    const markedPin = L.marker([parseFloat(lat), parseFloat(lon)]).addTo(map);
    configurarMarker(markedPin, id);
}

const form = document.getElementById("FormPonto");

// Envio do ponto

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

            const latNovoPonto = parseFloat(formData.get("Lat"));
            const lonNovoPonto = parseFloat(formData.get("Lon"))
            novoPonto(latNovoPonto, lonNovoPonto, data.id)
            
        })
        .catch(error => {
            console.error("Erro: ", error);
            exibirErro("Erro ao enviar ponto. Verifique os campos obrigatórios e tente novamente.")
        });
})

// Modal para visualizacao do ponto

function abrirVisualizarModal(pontoId) {

    const modal = new bootstrap.Modal(document.getElementById('visualizarPonto'));
    modal.show();

}

function fecharVisualizarModal() {

    const modalElement = document.getElementById('visualizarPonto');
    const modal = bootstrap.Modal.getInstance(modalElement);

    if (modal) {
        modal.hide();
        form.reset();
    }

}


function configurarMarker(marker, id) {
    marker.pontoId = id;

    marker.on('click', function (e) {
        L.DomEvent.stopPropagation(e);
        abrirVisualizarModal(this.pontoId);
    });

    marker._icon.classList.add('marked-pin');

    markers.push(marker);
}