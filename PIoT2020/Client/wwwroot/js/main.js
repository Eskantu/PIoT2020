

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
