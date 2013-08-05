

var mapModule = (function () {
    var DESK_CONSTANT_Y = 1.2291675;

    var DESK_CONSTANT_X = DESK_CONSTANT_Y / 2;

    // Note: this value is exact as the map projects a full 360 degrees of longitude
    var GALL_PETERS_RANGE_X = 800;

    // Note: this value is inexact as the map is cut off at ~ +/- 83 degrees.
    // However, the polar regions produce very little increase in Y range, so
    // we will use the tile size.
    var GALL_PETERS_RANGE_Y = 510;

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

    function makeDesk(xcoord, ycoord, deg, map, maptype) {

        var paths = null;

        var rad = (Math.PI / 180) * deg;

        var t0 = transformCoord(DESK_CONSTANT_Y, 0, rad);
        var t1 = transformCoord(0, 0, rad);
        var t2 = transformCoord(0, DESK_CONSTANT_X, rad);
        var t3 = transformCoord(DESK_CONSTANT_Y, DESK_CONSTANT_X, rad);

        var coord1 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t0[0], ycoord + t0[1]))
        var coord2 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t1[0], ycoord + t1[1]))
        var coord3 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t2[0], ycoord + t2[1]))
        var coord4 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t3[0], ycoord + t3[1]))

        paths = [
          coord1,
          coord2,
          coord3,
          coord4
        ]


        var desk = new google.maps.Polygon({
            paths: paths,
            strokeColor: '#000000',
            strokeOpacity: 0.8,
            strokeWeight: 1,
            fillColor: '#41FF23',
            draggable: true,
            fillOpacity: 1,
            id: null,
        });


        google.maps.event.addListener(desk, 'dblclick', function (evt) {
            giveFocus(desk);
        });

        google.maps.event.addListener(desk, 'click', function (evt) {
            console.log(desk);
        });

        google.maps.event.addListener(desk, "dragstart", function (evt) {
            console.log("dragstart" + evt.latLng);

        });

        google.maps.event.addListener(desk, "dragend", function (evt) {
            console.log("dragend" + evt.latLng);
        });

        desk.getPosition = function () {
            return coord1;
        };

        desk.modColor = function (color) {
            desk.setOptions({ fillColor: color });
        }
        desk.modContent = function (cont) {
            desk.setOptions({ info: cont });
        }
        desk.modID = function (eyedee) {
            desk.setOptions({ id: eyedee });
        }
        desk.setMap(map);

        return desk;

    };


    var focusDesk = null;

    function giveFocus(desk) {
        if (focusDesk != null)
            focusDesk.modColor('#41FF23');
        desk.modColor('#FF0000');
        focusDesk = desk;
    }

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

        var center = new google.maps.Point(47, 40);
        if (floor != 3) {
            center = new google.maps.Point(0, 9);
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
            }

        };
        gallPetersMap = new google.maps.Map(document.getElementById("map-canvas-" + floor),
            mapOptions);

        gallPetersMap.mapTypes.set('gallPetersMap', gallPetersMapType);
        gallPetersMap.setMapTypeId('gallPetersMap');



        google.maps.event.addListener(gallPetersMap, 'click', function (event) {
            console.log('Point.X.Y: ' + gallPetersMapType.projection.fromLatLngToPoint(event.latLng));
        });

        gallPetersMap.desks = {};

        gallPetersMap.addDesk = function (xpos, ypos, angle, id) {
            // :)
            // KEEP GOING FROM HERE.
            gallPetersMap.desks[id] = makeDesk(xpos, ypos, angle, gallPetersMap, gallPetersMapType);
        };


        return gallPetersMap;
    }

    return {
        initializeMap: initializeMap
    };
})();