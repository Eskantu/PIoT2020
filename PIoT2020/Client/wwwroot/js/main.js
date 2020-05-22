

function saveAsFile(filename, bytesBase64) {
    if (navigator.msSaveBlob) {
        //Download document in Edge browser
        var data = window.atob(bytesBase64);
        var bytes = new Uint8Array(data.length);
        for (var i = 0; i < data.length; i++) {
            bytes[i] = data.charCodeAt(i);
        }
        var blob = new Blob([bytes.buffer], { type: "application/octet-stream" });
        navigator.msSaveBlob(blob, filename);
    }
    else {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:application/octet-stream;base64," + bytesBase64;
        document.body.appendChild(link); // Needed for Firefox
        link.click();
        document.body.removeChild(link);
    }
}

var morris;
function LineChart(idElement, data, XKey, yKeys, labels) {
    var divPadre = document.getElementById(idElement);
    if (divPadre.hasChildNodes()) {
        var antiguoHijo = document.getElementById("grafica");
        divPadre.removeChild(antiguoHijo);
    }
    else {
    }
    var divHijo = document.createElement("div");
    divHijo.id = "grafica";
    divPadre.appendChild(divHijo);

    morris = new Morris.Line({
        // ID of the element in which to draw the chart.
        element: "grafica",
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: data,
        // The name of the data record attribute that contains x-values.
        xkey: XKey,
        // A list of names of data record attributes that contain y-values.
        ykeys: yKeys,
        // Labels for the ykeys -- will be displayed when you hover over the
        // chart.
        labels: labels
    });
    console.log(divHijo);
    

}

function Load() {
    Swal.fire({
        title: '',
        width: 0,
        padding: '3em',
        showConfirmButton: false,
        background: 'rgba(211,84,0,0)',
        backdrop: `
          rgba(0,0,0,0.2)
          url("../img/Load.gif")
          center
          no-repeat
        `
    })
}

function LoadFinished(titulo, text) {
    Swal.fire({
        position: 'center',
        icon: 'success',
        title: titulo,
        text: text,
        footer: 'PIoT2020',
        showConfirmButton: true,
    })
}

async function ExportOptions() {
    const { value: fruit } = await Swal.fire({
        title: 'Select field validation',
        input: 'select',
        inputOptions: {
            apples: 'Apples',
            bananas: 'Bananas',
            grapes: 'Grapes',
            oranges: 'Oranges'
        },
        inputPlaceholder: 'Select a fruit',
        showCancelButton: true,
        inputValidator: (value) => {
            return new Promise((resolve) => {
                if (value === 'oranges') {
                    resolve()
                } else {
                    resolve('You need to select oranges :)')
                }
            })
        }
    })

    if (fruit) {
        Swal.fire(`You selected: ${fruit}`)
    }
}
