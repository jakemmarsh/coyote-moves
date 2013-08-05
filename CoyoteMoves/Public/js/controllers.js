function IndexCtrl($scope, $routeParams, desks, user, requestForm) {

    
    $scope.maps = {
        3: mapModule.initializeMap(3),
        4: mapModule.initializeMap(4),
        5: mapModule.initializeMap(5)
    };

    $scope.currentFloor = 3;
    $scope.currentFormTab = 0;
    $scope.isAdmin = false;
    $scope.movedEmployees = [];
    $scope.displacedEmployees = [];
    $scope.moves = [];
    $scope.showSidebar = false;

    var initialize = function () {
        user.getUserRoles().then(function (data) {
            $scope.userRoles = data;
            console.log(data);
            // check all roles that the logged in user has
            for (var i = 0; i < $scope.userRoles.length; i++) {
                // if user is an admin, set variable to true (TODO)
                if ($scope.userRoles[i].toLowerCase() == 'users') {
                    $scope.isAdmin = true;
                }
            }
        },
        function (errorMessage) {
            console.log(errorMessage);
        });
    };

    initialize();

    requestForm.getAllJobTitles().then(function (data) {
        $scope.jobTitles = data;
    },
    function (errorMessage) {
        console.log(errorMessage);
    });

    requestForm.getAllGroups().then(function (data) {
        $scope.groups = data;
    },
    function (errorMessage) {
        console.log(errorMessage);
    });

    requestForm.getAllDepartments().then(function (data) {
        $scope.departments = data;
    },
    function (errorMessage) {
        console.log(errorMessage);
    });

    $scope.createMoveForm = function () {
        var move = {
            deskNumber: $scope.currentDeskNumber,
            displacedEmployee: $scope.currentDeskOccupant,
            movedEmployee: $scope.futureDeskOccupant
        }

        $scope.moves.push(move);

        var currentInDisplaced = false,
            currentInMoved = false,
            futureInMoved = false;

        for (var i = 0; i < $scope.displacedEmployees.length; i++) {
            // if futureDeskOccupant exists in displacedEmployees, remove them
            if ($scope.displacedEmployees[i] == $scope.futureDeskOccupant) {
                $scope.displacedEmployees.splice(i, 1);
            }
            // if currentDeskOccupant has already been displaced, throw error
            if ($scope.displacedEmployees[i] == $scope.currentDeskOccupant) {
                $scope.createMoveFormError = "You have already begun a move to this desk.";
                currentInDisplaced = true;
                return;
            }
        }


        for (var i = 0; i < $scope.movedEmployees.length; i++) {
            if ($scope.movedEmployees[i] == $scope.currentDeskOccupant) {
                currentInMoved = true;
            }
            if ($scope.movedEmployees[i] == $scope.futureDeskOccupant) {
                futureInMoved = true;
            }
        }
        // if currentDeskOccupant doesn't exist in displacedEmployees or movedEmployees, add them to displacedEmployees
        if (!currentInMoved && !currentInDisplaced) {
            $scope.displacedEmployees.push($scope.currentDeskOccupant);
        }

        if (!futureInMoved && !currentInMoved && !currentInDisplaced) {
            $scope.movedEmployees.push($scope.futureDeskOccupant);
        }

        // change current form tab to newest move
        $scope.currentFormTab = $scope.movedEmployees.length - 1;
    }

    $scope.sendAllForms = function () {
        for (var i = 0; i < $scope.movedEmployees.length; i++) {
            $scope.allMovesForms.push($scope.movedEmployees[i]);
        }
        requestForm.sendForm().then(function (data) {
            console.log(data);
        },
        function (errorMessage) {
            console.log(errorMessage);
        });
        console.log($scope.allMovesForms);
    }

    $scope.cancelSingleMove = function (index) {
        // remove associated displaced employee from list
        for (var i = 0; i < $scope.moves.length; i++) {
            if ($scope.moves[i].movedEmployee == $scope.movedEmployees[index]) {
                for (var j = 0; j < $scope.displacedEmployees.length; j++) {
                    if ($scope.displacedEmployees[j] == $scope.moves[i].displacedEmployee) {
                        $scope.displacedEmployees.splice(j, 1);
                    }
                }
            }
            // if moved employee is still displaced, re-add to displaced employees
            if ($scope.moves[i].displacedEmployee == $scope.movedEmployees[index]) {
                $scope.displacedEmployees.push($scope.moves[i].displacedEmployee);
            }
        }



        $scope.movedEmployees.splice(index, 1);
    }

    $scope.cancelAllMoves = function () {
        $scope.movedEmployees = [];
        $scope.displacedEmployees = [];
    }

    $scope.searchForEmployee = function () {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if ($scope.currentFloorEmployees[i].name.toLowerCase() === $scope.employeeToSearchFor.toLowerCase()) {
                $scope.showSidebar = true;

                // zoom to employee's desk on map
                $scope.currentDeskNumber = $scope.currentFloorEmployees[i].current.deskInfo.deskNumber;
                $scope.currentDeskOccupant = $scope.currentFloorEmployees[i].name;
                $scope.currentDeskOrientation = $scope.currentFloorEmployees[i].deskOrientation;

                //set current employee
                $scope.selectedDeskEmployee = $scope.currentFloorEmployees[i];
            }
        }
    }

    $scope.$watch('employeeToSearchFor', function () {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if ($scope.currentFloorEmployees[i].name.toLowerCase() === $scope.employeeToSearchFor.toLowerCase()) {
                $scope.showSidebar = true;

                // zoom to employee's desk on map
                $scope.currentDeskNumber = $scope.currentFloorEmployees[i].current.deskInfo.deskNumber;
                $scope.currentDeskOccupant = $scope.currentFloorEmployees[i].name;
                $scope.currentDeskOrientation = $scope.currentFloorEmployees[i].deskOrientation;

                //set current employee
                $scope.selectedDeskEmployee = $scope.currentFloorEmployees[i];
            }
        }
    });

    // watch for change in current floor tab. reload desks, employees, and employee names
    $scope.$watch('currentFloor', function () {
        console.log($scope.maps[$scope.currentFloor]);
        window.setTimeout(function(){                                           
            google.maps.event.trigger($scope.maps[$scope.currentFloor], 'resize');
        },100);
        desks.getDesksByFloor($scope.currentFloor).then(function (data) {
            $scope.currentFloorDesks = data;
            $scope.currentFloorEmployees = [];
            $scope.currentFloorEmployeeNames = [];
            for (var i = 0; i < $scope.currentFloorDesks.length; i++) {
                var name = $scope.currentFloorDesks[i].currentTenant.firstName + ' ' + $scope.currentFloorDesks[i].currentTenant.lastName,
                    deskNumber = $scope.currentFloorDesks[i].deskNumber,
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
                $scope.maps[$scope.currentFloor].addDesk($scope.currentFloorDesks[i].location.x, $scope.currentFloorDesks[i].location.y, $scope.currentFloorDesks[i].orientation, $scope.currentFloorDesks[i].currentTenant.id);
            }
        },
        function (errorMessage) {
            console.log(errorMessage);
        });
        window.setTimeout(function () {
            if ($scope.currentFloor === 3) {
                // resize map
            }
            else if ($scope.currentFloor === 4) {
                // resize map
            }
            else if ($scope.currentFloor === 5) {
                // resize map
            }
        }, 100);
    })
};