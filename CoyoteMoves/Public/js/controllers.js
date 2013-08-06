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

    var fetchEmployee = function (employeeName) {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if (employeeName.toLowerCase() == $scope.currentFloorEmployees[i].name.toLowerCase()) {
                return $scope.currentFloorEmployees[i];
            }
        }
    };

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




        //for (var i = 0; i < $scope.displacedEmployees.length; i++) {
        //    // if futureDeskOccupant exists in displacedEmployees, remove them
        //    if ($scope.displacedEmployees[i] == $scope.futureDeskOccupant) {
        //        $scope.displacedEmployees.splice(i, 1);
        //    }
        //    // if currentDeskOccupant has already been displaced, throw error
        //    if ($scope.displacedEmployees[i] == $scope.currentDeskOccupant) {
        //        $scope.createMoveFormError = "You have already begun a move to this desk.";
        //        currentInDisplaced = true;
        //        return;
        //    }
        //}

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
            $scope.displacedEmployees.push(fetchEmployee($scope.currentDeskOccupant));
        }

        if (!futureInMoved && !currentInMoved && !currentInDisplaced) {
            $scope.movedEmployees.push(fetchEmployee($scope.futureDeskOccupant));


            // setting future desk number on move form
            var newEmployIndex = ($scope.movedEmployees.length - 1);
            $scope.movedEmployees[newEmployIndex].future = {};
            $scope.movedEmployees[newEmployIndex].future.deskInfo = {};
            $scope.movedEmployees[newEmployIndex].future.deskInfo.deskNumber = {};
            $scope.movedEmployees[newEmployIndex].future.deskInfo.deskNumber = move.deskNumber;
        }

        // change current form tab to newest move
        $scope.currentFormTab = $scope.movedEmployees.length - 1;
    }

    $scope.sendAllForms = function () {
        $scope.allMovesForms = [];

        for (var i = 0; i < $scope.movedEmployees.length; i++) {
            $scope.allMovesForms.push($scope.movedEmployees[i]);
        }

        requestForm.sendForm($scope.allMovesForms).then(function (data) {
            console.log(data);
        },
        function (errorMessage) {
            console.log(errorMessage);
        });
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
        if ($scope.currentFloorEmployees) {
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
    });

    // watch for change in current floor tab. reload desks, employees, and employee names
    $scope.$watch('currentFloor', function () {
        window.setTimeout(function(){                                           
            google.maps.event.trigger($scope.maps[$scope.currentFloor], 'resize');
        },100);
        desks.getDesksByFloor($scope.currentFloor).then(function (data) {
            $scope.currentFloorDesks = data;
            $scope.currentFloorEmployees = [];
            $scope.currentFloorEmployeeNames = [];
            for (var i = 0; i < $scope.currentFloorDesks.length; i++) {
                var currentDesk = $scope.currentFloorDesks[i],
                    currentPerson = currentDesk.currentTenant,
                    name = currentPerson.firstName + ' ' + currentPerson.lastName,

                    employee = {
                        name: name,
                        id: currentPerson.id,
                        location: currentDesk.location,
                        current: {
                            bazookaInfo: {
                                jobTitle: currentPerson.jobTitle,
                                department: currentPerson.department,
                                group: currentPerson.group,
                                managerId: currentPerson.managerName,
                                jobTemplate: currentPerson.department + " " + currentPerson.jobTitle,
                            },
                            ultiproInfo: {
                                jobTitle: currentPerson.jobTitle,
                                department: currentPerson.department,
                                group: currentPerson.group,
                                supervisor: currentPerson.managerName,
                                other: ""
                            },
                            deskInfo: {
                                deskNumber: currentDesk.deskNumber
                            },
                            phoneInfo: {
                                phoneNumber: currentPerson.phoneNumber
                            }
                        }
                    };
                $scope.currentFloorEmployeeNames.push(name);
                $scope.currentFloorEmployees.push(employee);
                $scope.maps[currentDesk.location.floor].addDesk(currentDesk.location.topLeft.xCoordinate, currentDesk.location.topLeft.yCoordinate, currentDesk.location.orientation, currentDesk.id);
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