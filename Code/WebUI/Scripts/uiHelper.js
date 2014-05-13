$(window).resize(function () {
    setMapDivSize();
});

var infowindow = null;
var marker = null;

function initialize(ipInfo) {
    var myLatlng = new google.maps.LatLng(ipInfo.Latitude, ipInfo.Longitude);
    
    var mapOptions = {
        zoom: 8,
        center: myLatlng
    }
    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        title: 'Ubicación IP'
    });
    
    var contentString = '<div style="width: 380px"><table class="table table-striped"><tbody><tr><td><b>IP</b></td><td>' + ipInfo.IpAddress +
                        '</td></tr><tr><td><b>IP Numerica</b></td><td>' + ipInfo.IpNumber +
                        '</td></tr><tr><td><b>Zona Horaria</b></td><td>' + ipInfo.TimeZone +
                        '</td></tr><tr><td><b>Pais</b></td><td>' + ipInfo.Country +
                        '</td></tr><tr><td><b>Ciudad</b></td><td>' + ipInfo.City +
                        '</td></tr></tbody></table></div>';

    infowindow = new google.maps.InfoWindow({
        content: contentString
    });
    
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });
}

function loadIpInformation(ipAddress, loadLocalInfo) {
    if (ipAddress == '::1' || ipAddress == '39.9612')
        ipAddress = '';
    var url = '/api/IpFinder/GetIpInformation?ipAddress=' + ipAddress;
    $.getJSON(url, null, function (ipInfo) {
        onLoadIpInformation(ipInfo, loadLocalInfo);
    });

}

function onLoadIpInformation(ipInfo, loadLocalInfo) {
    if (loadLocalInfo == 1) {
        $('#ipInput').val(ipInfo.IpAddress);
        $('#ipLocation').html(ipInfo.City + ', ' + ipInfo.Country);
    }
    $('#geoResult').text(ipInfo.Country + ', ' + ipInfo.City);
    initialize(ipInfo);
}

function initMap(ipAddress, loadLocalInfo) {
    setMapDivSize();
    loadIpInformation(ipAddress, loadLocalInfo);
}

function setMapDivSize() {
    var parentWidth = $('#map-canvas').parent().width();
    var parentHeight = Math.floor(window.innerHeight * 0.5);
    document.getElementById('map-canvas').style.width = parentWidth + 'px';
    document.getElementById('map-canvas').style.height = parentHeight + 'px';
    //$('#map-canvas').width(parentWidth);
}