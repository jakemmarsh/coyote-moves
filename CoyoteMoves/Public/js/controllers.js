function IndexCtrl($scope, $routeParams, desks, requestForm) {
    $scope.currentFloor = 3;
    $scope.currentEmployee = 0;
    $scope.allMovesForms = [];
    $scope.currentDeskOrientation = 0;
    $scope.movedEmployees = [{
        name: "Test Employee"
    }];


    requestForm.getAllJobTitles().then(function (data) {
        $scope.jobTitles = data;
    })

    requestForm.getAllGroups().then(function (data) {
        $scope.groups = data;
    })

    requestForm.getAllDepartments().then(function (data) {
        
        $scope.departments = data;
    })

    $scope.changeCurrentForm = function (index) {
        $scope.currentEmployee = index;
    }

    $scope.searchForEmployee = function () {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if ($scope.currentFloorEmployees[i].name.toLowerCase() === $scope.employeeToSearchFor.toLowerCase()) {
                // zoom to employee's desk on map
                $scope.currentDeskNumber = $scope.currentFloorEmployees[i].current.deskInfo.deskNumber;
                $scope.currentDeskOccupant = $scope.currentFloorEmployees[i].name;
                $scope.currentDeskOrientation = $scope.currentFloorEmployees[i].deskOrientation;

                //set current employee
                $scope.selectedDeskEmployee = $scope.currentFloorEmployees[i];
            }
        }
    }

    $scope.createMoveForm = function () {
        $scope.movedEmployees.push($scope.selectedDeskEmployee);
        $scope.changeCurrentForm($scope.movedEmployees.length - 1);
    }

    $scope.sendAllForms = function () {
        
        for (var i = 0; i < $scope.movedEmployees.length; i++) {
            $scope.allMovesForms.push($scope.movedEmployees[i]);
        }
        console.log($scope.allMovesForms);
    }

    $scope.cancelSingleMove = function (index) {
        console.log(index);
        $scope.movedEmployees.splice(index, 1);
    }

    $scope.cancelAllMoves = function () {
        $scope.movedEmployees = [];
    }

    // watch for change in current floor tab. reload desks, employees, and employee names
    $scope.$watch('currentFloor', function () {
        desks.getDesksByFloor($scope.currentFloor).then(function (data) {
            $scope.currentFloorDesks = data;
            $scope.currentFloorEmployees = [];
            $scope.currentFloorEmployeeNames = [];

            for (var i = 0; i < $scope.currentFloorDesks.length; i++) {
                var name = $scope.currentFloorDesks[i].currentTenant.firstName + ' ' + $scope.currentFloorDesks[i].currentTenant.lastName,
                    employee = {
                        name: name,
                        deskOrientation: $scope.currentFloorDesks[i].location.orientation,
                        current: {

                            bazookaInfo: {
                                jobTitle: $scope.currentFloorDesks[i].currentTenant.jobTitle,
                                department: $scope.currentFloorDesks[i].currentTenant.department,
                                group: $scope.currentFloorDesks[i].currentTenant.group,
                                managerId: $scope.currentFloorDesks[i].currentTenant.managerName,
                                jobTemplate: $scope.currentFloorDesks[i].currentTenant.department + " " + $scope.currentFloorDesks[i].currentTenant.jobTitle,
                            },

                            ultiproInfo: {
                                jobTitle: $scope.currentFloorDesks[i].currentTenant.jobTitle,
                                department: $scope.currentFloorDesks[i].currentTenant.department,
                                group: $scope.currentFloorDesks[i].currentTenant.group,
                                supervisor: $scope.currentFloorDesks[i].currentTenant.managerName,
                                other: ""
                            },

                            deskInfo: {
                                deskNumber: $scope.currentFloorDesks[i].deskNumber
                            },

                            phoneInfo: {
                                phoneNumber: $scope.currentFloorDesks[i].currentTenant.phoneNumber
                            }

                        }
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