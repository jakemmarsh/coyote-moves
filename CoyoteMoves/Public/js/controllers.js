function IndexCtrl($scope, $routeParams) {
    $scope.currentFloor = 3;
    $scope.currentEmployee = 0;

    $scope.showBazookaChanges = false;
    $scope.showUltiproChanges = false;

    $scope.changeCurrentForm = function (index) {
        $scope.currentEmployee = index;
        $scope.$apply();
        console.log(index);
    }

    $scope.movedEmployees = [{
        name: "Jake"
    },
    {
        name: "Magic"
    },
    {
        name: "Brandon"
    },
    {
        name: "Ian"
    }];

    $scope.$watch('currentFloor', function () {
        window.setTimeout(function () {
                if($scope.currentFloor === 3) { 
                    google.maps.event.trigger($scope.thirdFloorMap, 'resize');
                }
                else if ($scope.currentFloor === 4) {
                    google.maps.event.trigger($scope.fourthFloorMap, 'resize');
                }
                else if ($scope.currentFloor === 5) {
                    google.maps.event.trigger($scope.fifthFloorMap, 'resize');
                }
            }, 100);

    });


    // THIRD FLOOR MAP
    $scope.thirdFloorMarkers = [];
    $scope.thirdFloorMapOptions = {
        center: new google.maps.LatLng(35.784, -78.670),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    $scope.addThirdFloorMarker = function ($event) {
        $scope.thirdFloorMarkers.push(new google.maps.Marker({
            map: $scope.thirdFloorMap,
            position: $event.latLng
        }));
    };
    $scope.setThirdFloorZoomMessage = function (zoom) {
        $scope.thirdFloorZoomMessage = 'You just zoomed to ' + zoom + '!';
        console.log(zoom, 'zoomed')
    };
    $scope.openThirdFloorMarkerInfo = function (marker) {
        $scope.currentThirdFloorMarker = marker;
        $scope.currentThirdFloorMarkerLat = marker.getPosition().lat();
        $scope.currentThirdFloorMarkerLng = marker.getPosition().lng();
        $scope.myInfoWindow.open($scope.thirdFloorMap, marker);
    };
    $scope.setThirdFloorMarkerPosition = function (marker, lat, lng) {
        marker.setPosition(new google.maps.LatLng(lat, lng));
    };

    // FOURTH FLOOR MAP
    $scope.fourthFloorMarkers = [];
    $scope.fourthFloorMapOptions = {
        center: new google.maps.LatLng(33.784, -81.670),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    $scope.addFourthFloorMarker = function ($event) {
        $scope.fourthFloorMarkers.push(new google.maps.Marker({
            map: $scope.fourthFloorMap,
            position: $event.latLng
        }));
    };
    $scope.setFourthFloorZoomMessage = function (zoom) {
        $scope.fourthFloorZoomMessage = 'You just zoomed to ' + zoom + '!';
        console.log(zoom, 'zoomed')
    };
    $scope.openFourthFloorMarkerInfo = function (marker) {
        $scope.currentFourthFloorMarker = marker;
        $scope.currentFourthFloorMarkerLat = marker.getPosition().lat();
        $scope.currentFourthFloorMarkerLng = marker.getPosition().lng();
        $scope.myInfoWindow.open($scope.fourthFloorMap, marker);
    };
    $scope.setFourthFloorMarkerPosition = function (marker, lat, lng) {
        marker.setPosition(new google.maps.LatLng(lat, lng));
    };

    // FIFTH FLOOR MAP
    $scope.fifthFloorMarkers = [];
    $scope.fifthFloorMapOptions = {
        center: new google.maps.LatLng(32.784, -79.670),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    $scope.addFifthFloorMarker = function ($event) {
        $scope.fifthFloorMarkers.push(new google.maps.Marker({
            map: $scope.fifthFloorMap,
            position: $event.latLng
        }));
    };
    $scope.setFifthFloorZoomMessage = function (zoom) {
        $scope.fifthFloorZoomMessage = 'You just zoomed to ' + zoom + '!';
        console.log(zoom, 'zoomed')
    };
    $scope.openFifthFloorMarkerInfo = function (marker) {
        $scope.currentFifthFloorMarker = marker;
        $scope.currentFifthFloorMarkerLat = marker.getPosition().lat();
        $scope.currentFifthFloorMarkerLng = marker.getPosition().lng();
        $scope.myInfoWindow.open($scope.fifthFloorMap, marker);
    };
    $scope.setFifthFloorMarkerPosition = function (marker, lat, lng) {
        marker.setPosition(new google.maps.LatLng(lat, lng));
    };
}