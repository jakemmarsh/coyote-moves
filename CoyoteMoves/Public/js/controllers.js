function IndexCtrl($scope, $routeParams, desks) {
    $scope.currentFloor = 3;
    $scope.currentEmployee = 0;
    $scope.people = ["Jake", "Magic", "Brandon", "Ian"];

    $scope.changeCurrentForm = function (index) {
        $scope.currentEmployee = index;
        $scope.$apply();
    }

    $scope.searchForEmployee = function () {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if ($scope.currentFloorEmployees[i].name.toLowerCase() === $scope.employeeToSearchFor.toLowerCase()) {
                // zoom to employee's desk on map
                $scope.currentDeskNumber = $scope.currentFloorEmployees[i].deskNumber;
                $scope.currentDeskOccupant = $scope.currentFloorEmployees[i].name;
            }
        }
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

    // watch for change in current floor tab. reload desks, employees, and employee names
    $scope.$watch('currentFloor', function () {
        desks.getDesksByFloor($scope.currentFloor).then(function (data) {
            $scope.currentFloorDesks = data;
            $scope.currentFloorEmployees = [];
            $scope.currentFloorEmployeeNames = [];
            for (var i = 0; i < $scope.currentFloorDesks.length; i++) {
                var name = $scope.currentFloorDesks[i].currentTenant.firstName + ' ' + $scope.currentFloorDesks[i].currentTenant.lastName,
                    deskNumber = $scope.currentFloorDesks[i].deskNumber,
                    employee = {
                        name: name,
                        deskNumber: deskNumber
                    };
                $scope.currentFloorEmployeeNames.push(name);
                $scope.currentFloorEmployees.push(employee);
            }
        },
        function (errorMessage) {
            console.log(errorMessage);
        });
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
    $scope.setThirdFloorZoomMessage = function (zoom) {
        $scope.thirdFloorZoomMessage = 'You just zoomed to ' + zoom + '!';
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
    $scope.setFourthFloorZoomMessage = function (zoom) {
        $scope.fourthFloorZoomMessage = 'You just zoomed to ' + zoom + '!';
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
    $scope.setFifthFloorZoomMessage = function (zoom) {
        $scope.fifthFloorZoomMessage = 'You just zoomed to ' + zoom + '!';
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