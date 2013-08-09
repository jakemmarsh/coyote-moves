

var mapModule = (function () {
    var center,
        DESK_CONSTANT_Y = 0.41,
        DESK_CONSTANT_X = DESK_CONSTANT_Y / 2,
        // Note: this value is exact as the map projects a full 360 degrees of longitude
        GALL_PETERS_RANGE_X = 800,
        // Note: this value is inexact as the map is cut off at ~ +/- 83 degrees.
        // However, the polar regions produce very little increase in Y range, so
        // we will use the tile size.
        GALL_PETERS_RANGE_Y = 510;

    function degreesToRadians(deg) {
        return deg * (Math.PI / 180);
    }

    function radiansToDegrees(rad) {
        return rad / (Math.PI / 180);
    }

    /** @constructor */
    function GallPetersProjection() {

        // Using the base map tile, denote the lat/lon of the equatorial origin.
        this.worldOrigin_ = new google.maps.Point(GALL_PETERS_RANGE_X * 400 / 800,
            GALL_PETERS_RANGE_Y / 2);

        // This projection has equidistant meridians, so each longitude degree is a linear
        // mapping.
        this.worldCoordinatePerLonDegree_ = GALL_PETERS_RANGE_X / 360;

        // This constant merely reflects that latitudes vary from +90 to -90 degrees.
        this.worldCoordinateLatRange = GALL_PETERS_RANGE_Y / 2;
    };

    GallPetersProjection.prototype.fromLatLngToPoint = function (latLng) {

        var origin = this.worldOrigin_;
        var x = origin.x + this.worldCoordinatePerLonDegree_ * latLng.lng();

        // Note that latitude is measured from the world coordinate origin
        // at the top left of the map.
        var latRadians = degreesToRadians(latLng.lat());
        var y = origin.y - this.worldCoordinateLatRange * Math.sin(latRadians);

        return new google.maps.Point(x, y);
    };

    GallPetersProjection.prototype.fromPointToLatLng = function (point, noWrap) {

        var y = point.y;
        var x = point.x;

        if (y < 0) {
            y = 0;
        }
        if (y >= GALL_PETERS_RANGE_Y) {
            y = GALL_PETERS_RANGE_Y;
        }

        var origin = this.worldOrigin_;
        var lng = (x - origin.x) / this.worldCoordinatePerLonDegree_;
        var latRadians = Math.asin((origin.y - y) / this.worldCoordinateLatRange);
        var lat = radiansToDegrees(latRadians);
        return new google.maps.LatLng(lat, lng, noWrap);
    };    

    function transformCoord(xCoord, yCoord, rad) {
        return [xCoord * Math.cos(rad) - yCoord * Math.sin(rad), xCoord * Math.sin(rad) + yCoord * Math.cos(rad)];
    }

    function makeDesk(xcoord, ycoord, deg, map, maptype, employee, deskId) {

        var paths = null,
            rad = (Math.PI / 180) * deg,
            t0 = transformCoord(DESK_CONSTANT_Y, 0, rad),
            t1 = transformCoord(0, 0, rad),
            t2 = transformCoord(0, DESK_CONSTANT_X, rad),
            t3 = transformCoord(DESK_CONSTANT_Y, DESK_CONSTANT_X, rad),
            coord1 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t0[0], ycoord + t0[1])),
            coord2 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t1[0], ycoord + t1[1])),
            coord3 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t2[0], ycoord + t2[1])),
            coord4 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t3[0], ycoord + t3[1]));

        paths = [coord1, coord2, coord3, coord4];


        var desk = new google.maps.Polygon({
            paths: paths,
            strokeColor: '#000000',
            strokeOpacity: 0.9,
            strokeWeight: 1,
            fillColor: '#f7f7f7',
            draggable: true,
            fillOpacity: 1,
            id: employee.id,
        });

        var labelText = deskId + "<br />(CO) " + employee.name;

        var marker = new MarkerWithLabel({
            position: new google.maps.LatLng(0,0),
            draggable: false,
            raiseOnDrag: false,
            map: map,
            labelContent: labelText,
            labelAnchor: new google.maps.Point(30, 20),
            labelClass: "desk-number-label", // the CSS class for the label
            labelStyle: {opacity: 1.0},
            icon: "http://placehold.it/1x1",
            visible: false
        });

        google.maps.event.addListener(desk, "mousemove", function(event) {
            marker.setPosition(new google.maps.LatLng(event.latLng.lat() + 2, event.latLng.lng()));
            marker.setVisible(true);
        });
        google.maps.event.addListener(desk, "mouseout", function(event) {
            marker.setVisible(false);
        });

        desk.getPosition = function () {
            return coord1;
        };

        desk.modColor = function (color) {
            desk.setOptions({ fillColor: color });
        }
        desk.modBorder = function (color) {
            desk.setOptions({ strokeColor: color });
        }

        desk.setMap(map);

        return desk;

    };

    var initializeMap = function(floor) {

        var gallPetersMap;

        var gallPetersMapType = new google.maps.ImageMapType({
            getTileUrl: function (coord, zoom) {
                var numTiles = 1 << zoom;

                // Don't wrap tiles vertically.
                if (coord.y < 0 || coord.y >= numTiles) {
                    return null;
                }

                var baseURL = 'coyotemoves/public/img/floor-' + floor + '/';
                baseURL += zoom + '_' + coord.x + '_' + coord.y + '.gif';
                return baseURL;
            },
            tileSize: new google.maps.Size(256, 256),
            isPng: false,
            minZoom: 3,
            maxZoom: 7,
            name: 'COYOTE'
        });

        gallPetersMapType.projection = new GallPetersProjection();

        function lookAt(desk) {
            gallPetersMap.panTo(desk.getPosition());
        }

        if (floor === 3) {
            center = new google.maps.Point(47, 40);
        }
        else if (floor === 4) {
            center = new google.maps.Point(11.5, 12.65);
        }
        else if (floor === 5) {
            center = new google.maps.Point(15.13, 30);
        }


        var mapOptions = {
            zoom: 3,
            panControl: true,
            zoomControl: true,
            mapTypeControl: false,
            scaleControl: false,
            streetViewControl: false,
            overviewMapControl: false,
            center: gallPetersMapType.projection.fromPointToLatLng(center),
            mapTypeControlOptions: {
                mapTypeIds: [google.maps.MapTypeId.ROADMAP, 'gallPetersMap']
            },
            disableDoubleClickZoom: true

        };
        gallPetersMap = new google.maps.Map(document.getElementById("map-canvas-" + floor),
            mapOptions);

        gallPetersMap.mapTypes.set('gallPetersMap', gallPetersMapType);
        gallPetersMap.setMapTypeId('gallPetersMap');



        google.maps.event.addListener(gallPetersMap, 'click', function (event) {
            console.log('Point.X.Y: ' + gallPetersMapType.projection.fromLatLngToPoint(event.latLng));
        });

        gallPetersMap.desks = [];

        gallPetersMap.addDesk = function (xpos, ypos, angle, employee, deskId) {
            var desk = makeDesk(xpos, ypos, angle, gallPetersMap, gallPetersMapType, employee, deskId);
            gallPetersMap.desks.push(desk);
            return desk;
        };

        gallPetersMap.fromLatLngToPoint = function (latLng) {
            return gallPetersMapType.projection.fromLatLngToPoint(latLng);
        }

        // limit bounds for panning
        var swlat = gallPetersMapType.projection.fromPointToLatLng(new google.maps.Point(6, 69)).lat();
        var swlng = gallPetersMapType.projection.fromPointToLatLng(new google.maps.Point(6, 69)).lng();
        var nelat = gallPetersMapType.projection.fromPointToLatLng(new google.maps.Point(86, 11)).lat();
        var nelng = gallPetersMapType.projection.fromPointToLatLng(new google.maps.Point(86, 11)).lng();

        var allowedBounds = new google.maps.LatLngBounds(
          new google.maps.LatLng(swlat, swlng),
          new google.maps.LatLng(nelat, nelng)
        );

        // Listen for the dragend event
        google.maps.event.addListener(gallPetersMap, 'dragend', function () { checkBounds(); });

        function checkBounds() {
            if (!allowedBounds.contains(gallPetersMap.getCenter())) {
                var C = gallPetersMap.getCenter();
                var X = C.lng();
                var Y = C.lat();

                var AmaxX = allowedBounds.getNorthEast().lng();
                var AmaxY = allowedBounds.getNorthEast().lat();
                var AminX = allowedBounds.getSouthWest().lng();
                var AminY = allowedBounds.getSouthWest().lat();

                if (X < AminX) { X = AminX; }
                if (X > AmaxX) { X = AmaxX; }
                if (Y < AminY) { Y = AminY; }
                if (Y > AmaxY) { Y = AmaxY; }

                gallPetersMap.setCenter(new google.maps.LatLng(Y, X));


            }
        }


        return gallPetersMap;
    }

    return {
        initializeMap: initializeMap
    };
})();