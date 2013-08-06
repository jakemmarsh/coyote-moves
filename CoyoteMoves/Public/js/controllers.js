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

    var fetchEmployeeByName = function (employeeName) {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if (employeeName.toLowerCase() == $scope.currentFloorEmployees[i].name.toLowerCase()) {
                return $scope.currentFloorEmployees[i];
            }
        }
    };

    var fetchEmployeeById = function (employeeId) {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if (employeeId === $scope.currentFloorEmployees[i].id) {
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
            futureInMoved = false,
            newEmployeeIndex;



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
            $scope.displacedEmployees.push(fetchEmployeeByName($scope.currentDeskOccupant));
        }

        if (!futureInMoved && !currentInMoved && !currentInDisplaced) {
            $scope.movedEmployees.push(fetchEmployeeByName($scope.futureDeskOccupant));


            // setting future desk number on move form
            newEmployeeIndex = ($scope.movedEmployees.length - 1);
            $scope.movedEmployees[newEmployeeIndex].future = {};
            $scope.movedEmployees[newEmployeeIndex].future.deskInfo = {};
            $scope.movedEmployees[newEmployeeIndex].future.deskInfo.deskNumber = {};
            $scope.movedEmployees[newEmployeeIndex].future.deskInfo.deskNumber = move.deskNumber;
        }

        console.log(newEmployeeIndex);
        
        for (var i = 0; i < newEmployeeIndex+1; i++) {
            console.log($scope.movedEmployees[i].future.deskInfo.deskNumber);
        }

        // change current form tab to newest move
        $scope.currentFormTab = $scope.movedEmployees.length - 1;
    }

    // aggregate all move forms and send to back end
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
        var employeeId = fetchEmployeeByName($scope.employeeToSearchFor.toLowerCase()).id;
        for (var i = 0; i < $scope.maps[$scope.currentFloor].desks.length; i++) {
            if ($scope.maps[$scope.currentFloor].desks[i].id === employeeId) {
                // zoom and pan to desk
                $scope.maps[$scope.currentFloor].panTo($scope.maps[$scope.currentFloor].desks[i].getPosition());
                $scope.maps[$scope.currentFloor].setZoom(7);

                // highlight desk and show it in sidebar
                $scope.selectDesk($scope.maps[$scope.currentFloor].desks[i]);
                break;
            }
        }
    }

    $scope.selectDesk = function (desk) {
        // remove highlight color if previous desk selected
        if ($scope.focusedDesk) {
            $scope.focusedDesk.modColor('5C4033');
        }
        // select and highlight new desk
        $scope.focusedDesk = desk;
        $scope.focusedDesk.modColor('#41FF23');

        // get employee from desk
        var employee = fetchEmployeeById(desk.id);
        $scope.showSidebar = true;
        // set data for sidebar
        $scope.currentDeskNumber = employee.current.deskInfo.deskNumber;
        $scope.currentDeskOccupant = employee.name;
        $scope.currentDeskOrientation = employee.location.orientation;
        //set current employee
        $scope.selectedDeskEmployee = employee;
    }

    $scope.$watch('employeeToSearchFor', function () {
        if ($scope.currentFloorEmployees) {
            for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
                if ($scope.currentFloorEmployees[i].name.toLowerCase() === $scope.employeeToSearchFor.toLowerCase()) {
                    // search for employee, in case a match is found before user submits form
                    $scope.searchForEmployee();
                    break;
                }
            }
        }
    });

    // watch for change in current floor tab. reload desks, employees, and employee names
    $scope.$watch('currentFloor', function () {
        // resize map on tab switch to show whole thing
        window.setTimeout(function(){                                           
            google.maps.event.trigger($scope.maps[$scope.currentFloor], 'resize');
        }, 100);

        // populate desks for current floor
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
                    },
                    newDesk;
                $scope.currentFloorEmployeeNames.push(name);
                $scope.currentFloorEmployees.push(employee);
                // create desk and place it on map
                newDesk = $scope.maps[currentDesk.location.floor].addDesk(currentDesk.location.topLeft.xCoordinate + i, currentDesk.location.topLeft.yCoordinate, currentDesk.location.orientation, employee.id);
                // add click listener to desk to highlight it and show it in sidebar
                google.maps.event.addListener(newDesk, 'click', function (evt) {
                    $scope.selectDesk(newDesk);
                    console.log(newDesk);
                });
            }
        },
        function (errorMessage) {
            console.log(errorMessage);
        });
    })
};