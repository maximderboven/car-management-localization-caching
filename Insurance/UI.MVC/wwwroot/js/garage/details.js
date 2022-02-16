//KRIJG GARAGE ID
const urlParams = new URLSearchParams(window.location.search)
const garageID = parseInt(urlParams.get("garage"))


//INFO LADEN IN FIELDS
window.onload = function () {
    loadGaragesDetails();
};

function loadGaragesDetails() {
    fetch('../api/Garages/' + garageID, {
        method: 'GET',
        headers: {'Accept': 'application/json'}
    })
        .then(response => {
            if (response.ok) return response.json();
        })
        .then(data => addData(data))
        .catch(error => alert(error.message));
}


function addData(response) {
    document.getElementById('id').value = garageID;
    document.getElementById('name').value = response.name;
    document.getElementById('telnr').value = response.telnr;
    document.getElementById('address').value = response.adress;
}

//update knop

const editbutton = document.getElementById('editgarage');
editbutton.addEventListener('click', () => editGarage());

function editGarage() {
    const name = document.getElementById('name').value;
    const telnr = document.getElementById('telnr').value;
    const adress = document.getElementById('address').value;

    const newResponse = {
        id: garageID,
        name: name,
        adress: adress,
        telnr: telnr
    };
    fetch('../api/Garages', {
        method: 'PUT',
        body: JSON.stringify(newResponse),
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                document.getElementById('successfully').classList.add('visible');
                document.getElementById('successfully').classList.remove('invisible');

                document.getElementById('error').classList.remove('visible');
                document.getElementById('error').classList.add('invisible');
            } else {
                document.getElementById('successfully').classList.remove('visible');
                document.getElementById('successfully').classList.add('invisible');

                document.getElementById('error').classList.add('visible');
                document.getElementById('error').classList.remove('invisible');
            }
        }).catch(error => {
            alert(error.message)
            }
        );
}
