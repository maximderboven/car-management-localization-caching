window.onload = function () {
    setTimeout(loadCars, 100);
    setTimeout(getDropdown, 250);
};
const urlParams = new URLSearchParams(window.location.search)
const driverid = parseInt(urlParams.get("socialnumber"))

//TABEL CARS VULLEN
function loadCars() {
    fetch('../api/Drivers/cars/' + driverid, {
        method: 'GET',
        headers: {'Accept': 'application/json'}
    })
        .then(response => {
            if (response.ok) return response.json();
        })
        .then(data => showCars(data))
        .catch(error => alert('Error :('));
}

function showCars(responses) {
    document.querySelector('#cars tbody').innerHTML = '';
    setTimeout(() => {
        responses.forEach(response => addCars(response));
    }, 20); //gewoon om even een flikker effect te krijgen (zeker te zijn dat da werkt)
}

function addCars(response) {
    const garagesTable = document.querySelector('#cars tbody');
    garagesTable.insertAdjacentHTML('beforeend', `<tr>
<td>${response.brand}</td>
<td>${response.fuel}</td>
<td>${response.purchaseprice != null ? response.purchaseprice : 'Not Known'}</td>
<td>${response.seats}</td>
<td>${response.mileage}</td>
</tr>`);
}


//DROPDOWN VULLEN
function getDropdown() {
    fetch('../api/Drivers/dropdown/' + driverid, {
        method: 'GET',
        headers: {'Accept': 'application/json'}
    }).then(response => {
        if (response.status === 200) {
            document.getElementById("car").innerHTML = "";
            return response.json().then(data => filldropdown(data));
        } else {
            document.getElementById('car').innerHTML = "";
            document.getElementById('car').disabled = true;
            document.getElementById('from').disabled = true;
            document.getElementById('from').value = "";
            document.getElementById('until').disabled = true;
            document.getElementById('until').value = "";
        }
    }).catch(error => alert(error.message));
}

function filldropdown(responses) {
    responses.forEach(response => {
        makeSelect(response)
    });

}

function makeSelect(response) {
    var el = document.createElement("option");
    el.textContent = response.numberPlate + " - " + response.brand;
    el.value = response.numberPlate;
    document.getElementById("car").appendChild(el);
}


//CAR TOEVOEGEN MET KNOP
const addCar = document.getElementById('addCar');
addCar.addEventListener('click', () => addRental());

function addRental() {
    const car = parseInt(document.getElementById('car').value);
    const price = parseFloat(document.getElementById('price').value);
    const from = document.getElementById('from').value;
    const until = document.getElementById('until').value;

    const newResponse = {
        Price: price,
        StartDate: from,
        EndDate: until,
        Socialnumber: driverid,
        NumberPlate: car
    };
    fetch('../api/Drivers', {
        method: 'POST',
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

                setTimeout(loadCars, 100);
                setTimeout(getDropdown, 250);
            } else {
                document.getElementById('successfully').classList.remove('visible');
                document.getElementById('successfully').classList.add('invisible');

                document.getElementById('error').classList.add('visible');
                document.getElementById('error').classList.remove('invisible');
            }
        })
        .catch(error => {
                alert("Error na adden" + error.message)
            }
        );
}

/* event listener */
document.getElementById("from").addEventListener('change', calculate);
document.getElementById("until").addEventListener('change', calculate);

/* function */
function calculate() {
    //vaste prijs van 15.5 administratie + 45€ per dag
    const totaaldagen = Math.floor((Date.parse(document.getElementById("until").value) - Date.parse(document.getElementById("from").value)) / 86400000);
    document.getElementById("price").value = ((totaaldagen * 45.0) + 15.5);
}
