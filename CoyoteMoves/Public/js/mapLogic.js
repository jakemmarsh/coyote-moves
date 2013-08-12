var mapModule = (function () {
    var center,
        DESK_CONSTANT_Y = 0.41,
        DESK_CONSTANT_X = DESK_CONSTANT_Y / 2,
        // Note: this value is exact as the map projects a full 360 degrees of longitude
        GALL_PETERS_RANGE_X = 800,
        // Note: this value is inexact as the map is cut off at ~ +/- 83 degrees.
        // However, the polar regions produce very little increase in Y range, so
        // we will use the tile size.
        GALL_PETERS_RANGE_Y = 510,
        thisMap = null;

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
        var origin = this.worldOrigin_,
            x = origin.x + this.worldCoordinatePerLonDegree_ * latLng.lng(),
            // Note that latitude is measured from the world coordinate origin
            // at the top left of the map.
            latRadians = degreesToRadians(latLng.lat()),
            y = origin.y - this.worldCoordinateLatRange * Math.sin(latRadians);

        return new google.maps.Point(x, y);
    };

    GallPetersProjection.prototype.fromPointToLatLng = function (point, noWrap) {
        var y = point.y,
            x = point.x,
            origin,
            lng,
            latRadians,
            lat;

        if (y < 0) {
            y = 0;
        }
        if (y >= GALL_PETERS_RANGE_Y) {
            y = GALL_PETERS_RANGE_Y;
        }

        origin = this.worldOrigin_;
        lng = (x - origin.x) / this.worldCoordinatePerLonDegree_;
        latRadians = Math.asin((origin.y - y) / this.worldCoordinateLatRange);
        lat = radiansToDegrees(latRadians);
        return new google.maps.LatLng(lat, lng, noWrap);
    };    

    function transformCoord(xCoord, yCoord, rad) {
        return [xCoord * Math.cos(rad) - yCoord * Math.sin(rad), xCoord * Math.sin(rad) + yCoord * Math.cos(rad)];
    }

    function makeDesk(xcoord, ycoord, deg, map, maptype, employee, deskId, isAdmin) {
        var paths = null,
            rad,
            t0,
            t1,
            t2,
            t3,
            coord1,
            coord2,
            coord3,
            coord4,
            desk,
            labelText,
            marker;

        if (deg == 0 || deg == 180) {
            rad = (Math.PI / 180) * deg;
            t0 = transformCoord(-DESK_CONSTANT_Y / 2, -DESK_CONSTANT_X / 2, rad);
            t1 = transformCoord(DESK_CONSTANT_Y / 2, -DESK_CONSTANT_X / 2, rad);
            t2 = transformCoord(DESK_CONSTANT_Y / 2, DESK_CONSTANT_X / 2, rad);
            t3 = transformCoord(-DESK_CONSTANT_Y / 2, DESK_CONSTANT_X / 2, rad);
            coord1 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t0[0], ycoord + t0[1]));
            coord2 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t1[0], ycoord + t1[1]));
            coord3 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t2[0], ycoord + t2[1]));
            coord4 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + t3[0], ycoord + t3[1]));
        }

        else {
            // do different math here to account for desk being rotated upon placement
            rad = (Math.PI / 180) * deg;
            t0 = transformCoord(DESK_CONSTANT_Y / 2, DESK_CONSTANT_X / 2, rad);
            t1 = transformCoord(-DESK_CONSTANT_Y / 2, DESK_CONSTANT_X / 2, rad);
            t2 = transformCoord(-DESK_CONSTANT_Y / 2, -DESK_CONSTANT_X / 2, rad);
            t3 = transformCoord(DESK_CONSTANT_Y / 2, -DESK_CONSTANT_X / 2, rad);
            coord1 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + 0.1 + t0[0], ycoord + 0.2 + t0[1]));
            coord2 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + 0.1 + t1[0], ycoord + 0.2 + t1[1]));
            coord3 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + 0.1 + t2[0], ycoord + 0.2 + t2[1]));
            coord4 = maptype.projection.fromPointToLatLng(new google.maps.Point(xcoord + 0.1 + t3[0], ycoord + 0.2 + t3[1]));
        }

        paths = [coord1, coord2, coord3, coord4];

        if (isAdmin) {
            desk = new google.maps.Polygon({
                paths: paths,
                strokeColor: '#000000',
                strokeOpacity: 0.9,
                strokeWeight: 1,
                fillColor: '#f7f7f7',
                draggable: true,

                fillOpacity: 1,
                id: employee.id,
            });
        }
        else {
            desk = new google.maps.Polygon({
                paths: paths,
                strokeColor: '#000000',
                strokeOpacity: 0.9,
                strokeWeight: 1,
                fillColor: '#f7f7f7',
                draggable: false,

                fillOpacity: 1,
                id: employee.id,
            });
        }

        desk.getPoint = function() {
            var p0 = maptype.projection.fromLatLngToPoint(this.getPath().getAt(0)),
                p1 = maptype.projection.fromLatLngToPoint(this.getPath().getAt(1)),
                p2 = maptype.projection.fromLatLngToPoint(this.getPath().getAt(2)),
                p3 = maptype.projection.fromLatLngToPoint(this.getPath().getAt(3));

            return new google.maps.Point((p0.x + p1.x) / 2, (p1.y + p2.y) / 2);
        }

        desk.deskNumber = deskId;

        labelText = employee.name;

        marker = new MarkerWithLabel({
            position: new google.maps.LatLng(0,0),
            draggable: false,
            raiseOnDrag: false,
            map: map,
            labelContent: labelText,
            labelAnchor: new google.maps.Point(20, 10),
            labelClass: "desk-number-label", // the CSS class for the label
            labelStyle: {opacity: 1.0},
            icon: "http://placehold.it/1x1",
            visible: false
        });

        google.maps.event.addListener(desk, "mousemove", function(event) {
            var temp = desk.getPoint();
            temp.y = (temp.y - 0.8) - (7 - map.getZoom());
            temp.x -= 0.3;
            marker.setPosition(maptype.projection.fromPointToLatLng(temp));
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
        desk.modPath = function (exx, why, rot) {
            var newPaths = null,
            rad0 = (Math.PI / 180) * rot,
            p0 = transformCoord(-DESK_CONSTANT_Y / 2, -DESK_CONSTANT_X / 2, rad0),
            p1 = transformCoord(DESK_CONSTANT_Y / 2, -DESK_CONSTANT_X / 2, rad0),
            p2 = transformCoord(DESK_CONSTANT_Y / 2, DESK_CONSTANT_X / 2, rad0),
            p3 = transformCoord(-DESK_CONSTANT_Y / 2, DESK_CONSTANT_X / 2, rad0),

            c1 = maptype.projection.fromPointToLatLng(new google.maps.Point(exx + p0[0], why + p0[1])),
            c2 = maptype.projection.fromPointToLatLng(new google.maps.Point(exx + p1[0], why + p1[1])),
            c3 = maptype.projection.fromPointToLatLng(new google.maps.Point(exx + p2[0], why + p2[1])),
            c4 = maptype.projection.fromPointToLatLng(new google.maps.Point(exx + p3[0], why + p3[1]));

            newPaths = [c1, c2, c3, c4];

            desk.setOptions({ paths: newPaths });
        }

        desk.setMap(map);

        return desk;

    };

    var initializeMap = function(floor) {

        var gallPetersMap,
            gallPetersMapType = new google.maps.ImageMapType({
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
                minZoom: 4,
                maxZoom: 7,
                name: 'COYOTE'
            });

        gallPetersMapType.projection = new GallPetersProjection();

        function lookAt(desk) {
            gallPetersMap.panTo(desk.getPosition());
        }

        center = new google.maps.LatLng(71, -173);


        var mapOptions = {
            zoom: 4,
            panControl: true,
            zoomControl: true,
            mapTypeControl: false,
            scaleControl: false,
            streetViewControl: false,
            overviewMapControl: false,
            center: center,
            mapTypeControlOptions: {
                mapTypeIds: [google.maps.MapTypeId.ROADMAP, 'gallPetersMap']
            },
            disableDoubleClickZoom: true

        };
        gallPetersMap = new google.maps.Map(document.getElementById("map-canvas-" + floor),
            mapOptions);

        gallPetersMap.mapTypes.set('gallPetersMap', gallPetersMapType);
        gallPetersMap.setMapTypeId('gallPetersMap');

        gallPetersMap.desks = [];

        gallPetersMap.addDesk = function (xpos, ypos, angle, employee, deskId, isAdmin) {
            var desk = makeDesk(xpos, ypos, angle, gallPetersMap, gallPetersMapType, employee, deskId, isAdmin);
            gallPetersMap.desks.push(desk);
            return desk;
        };

        gallPetersMap.fromLatLngToPoint = function (latLng) {
            return gallPetersMapType.projection.fromLatLngToPoint(latLng);
        };

        gallPetersMap.fromPointToLatLng = function (point, noWrap) {
            return gallPetersMapType.projection.fromPointToLatLng(point, noWrap);
        };
        map = gallPetersMap;

        gallPetersMap.getDesk = function(deskNumber) {
            for (var i = 0; i < gallPetersMap.desks.length; i++) {
                if (gallPetersMap.desks[i].deskNumber == deskNumber) {
                    return gallPetersMap.desks[i];
                }
            }
            return null;
        }

        return gallPetersMap;
    }



    return {
        initializeMap: initializeMap
    };
})();