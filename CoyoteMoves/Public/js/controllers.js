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
        if (employeeName) {
            for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
                if (employeeName.toLowerCase() == $scope.currentFloorEmployees[i].name.toLowerCase()) {
                    return $scope.currentFloorEmployees[i];
                }
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
        var errorMessage = "",
            move = {
            deskNumber: $scope.currentDeskNumber,
            displacedEmployee: fetchEmployeeByName($scope.currentDeskOccupant),
            movedEmployee: fetchEmployeeByName($scope.futureDeskOccupant)
            };

        if ($scope.moves.length == 0) {

            if (!move.movedEmployee) {
                move.movedEmployee = move.displacedEmployee;
                $scope.movedEmployees.push(move.movedEmployee);
            }
            else {
                // Setting future desk number property on moved employee
                move.movedEmployee.future.deskInfo.deskNumber = move.deskNumber;
                $scope.movedEmployees.push(move.movedEmployee);
                $scope.displacedEmployees.push(move.displacedEmployee);
            }

            $scope.moves.push(move);
        }
        else {
            // If moving a single employee (to a new desk)
            if (!move.movedEmployee) {
                move.movedEmployee = move.displacedEmployee;
                $scope.movedEmployees.push(move.movedEmployee);
                $scope.createMoveFormError = "";
            }
           
            for (var i = 0; i < $scope.displacedEmployees.length; i++) {
                // remove name from displaced employee list if move is created for this employee
                if ($scope.displacedEmployees[i].name == move.movedEmployee.name) {
                    $scope.displacedEmployees.splice(i, 1);
                }
            }

            
            for (var i = 0; i < $scope.movedEmployees.length; i++) {
                // Prevents multiple people moving to one desk
                if ($scope.movedEmployees[i].future.deskInfo.deskNumber == move.deskNumber) {
                    var name = $scope.movedEmployees[i].name;
                    $scope.createMoveFormError = name + " is moving to this desk!";
                    return;
                }

                // Check if tab is already open for this employee
                if ($scope.movedEmployees[i].name == move.movedEmployee.name) {
                    var name = $scope.movedEmployees[i].name;
                    $scope.createMoveFormError = name + " has a move open!";
                    return;
                }
            }

            move.movedEmployee.future.deskInfo.deskNumber = move.deskNumber;
            $scope.movedEmployees.push(move.movedEmployee);
            if (!($scope.movedEmployees[i].name == move.displacedEmployee.name)) {
                $scope.displacedEmployees.push(move.displacedEmployee);
            }
            $scope.moves.push(move);
            $scope.createMoveFormError = "";
        }

        $scope.futureDeskOccupant = "";
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
        var removedForm = $scope.movedEmployees[index];
        $scope.movedEmployees.splice(index, 1);
        $scope.currentFormTab = $scope.movedEmployees.length - 1;
        $scope.moves.splice(index, 1);
        
        for (var i = 0; i < $scope.movedEmployees.length; i++) {
            if ($scope.movedEmployees[i].future.deskInfo.deskNumber == removedForm.current.deskInfo.deskNumber) {
                $scope.displacedEmployees.push(removedForm);
            }
        }

        for (var i = 0; i < $scope.displacedEmployees.length; i++) {
            if (removedForm.future.deskInfo.deskNumber == $scope.displacedEmployees[i].current.deskInfo.deskNumber) {
                $scope.displacedEmployees.splice(i, 1);
            }
        }

        if ($scope.displacedEmployees.length === 0 && !$scope.selectedDeskEmployee) {
            $scope.showSidebar = false;
        }
    }

    $scope.cancelAllMoves = function () {
        $scope.movedEmployees = [];
        $scope.displacedEmployees = [];
    }

    $scope.searchForEmployee = function () {
        var employeeId = fetchEmployeeByName($scope.employeeToSearchFor.toLowerCase()).id;
        for (var i = 0; i < $scope.maps[$scope.currentFloor].desks.length; i++) {
            if ($scope.maps[$scope.currentFloor].desks[i].id === employeeId) {
                if ($scope.focusedDesk) {
                    $scope.focusedDesk.modColor('#41FF23');
                }
                $scope.focusedDesk = $scope.maps[$scope.currentFloor].desks[i];
                $scope.focusedDesk.modColor('blue');
                $scope.maps[$scope.currentFloor].setZoom(7);
                $scope.maps[$scope.currentFloor].panTo($scope.focusedDesk.getPosition());
                break;
            }
        }
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
                    // search for employee if a match is found before user submits form
                    $scope.searchForEmployee();
                    break;
                }
            }
        }
    });

    // watch for change in current floor tab. reload desks, employees, and employee names
    $scope.$watch('currentFloor', function () {
        $scope.employeeToSearchFor = "";
        if ($scope.displacedEmployees.length === 0) {
            $scope.showSidebar = false;
        }
        else {
            $scope.selectedDeskEmployee = null;
        }
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
                        future: {
                            deskInfo: {}
                        },
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
                $scope.maps[currentDesk.location.floor].addDesk(currentDesk.location.topLeft.xCoordinate+i, currentDesk.location.topLeft.yCoordinate, currentDesk.location.orientation, employee.id);
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