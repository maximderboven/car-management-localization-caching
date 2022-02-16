const loadResponsesButton = document.getElementById('loadGarages');
loadResponsesButton.addEventListener('click', () => loadGarages());
window.onload = function() {loadGarages();};

function loadGarages() {
    fetch('api/Garages', {
        method: 'GET',
        headers: { 'Accept': 'application/json' }
    })
        .then(response => { if (response.ok) return response.json(); })
        .then(data  => showGarages(data))
        .catch(error => alert('Oeps, something went wrong!'));
}

function showGarages(responses) {
    document.querySelector('#garages tbody').innerHTML = '';
    setTimeout(() => {
        responses.forEach(response => addGarages(response));
    },20); //gewoon om even een flikker effect te krijgen (zeker te zijn dat da werkt)
}

function addGarages(response) {
    const garagesTable = document.querySelector('#garages  tbody');
    garagesTable.insertAdjacentHTML('beforeend',`<tr>
<td>${response.name}</td>
<td>${response.adress}</td>
<td>${response.telnr}</td>
<td><a href="/Garage/Details?garage=${response.id}">Details</a></td>
</tr>`);
}