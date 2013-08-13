function IndexCtrl($scope, $routeParams, desks, userRoles, requestForm) {
    // initialize map for each tab/floor
    $scope.maps = {
        3: mapModule.initializeMap(3),
        4: mapModule.initializeMap(4),
        5: mapModule.initializeMap(5)
    };

    // initialize all starting variables
    $scope.currentFloor = 3;
    $scope.currentFormTab = 0;
    $scope.isAdmin = false;
    $scope.movedEmployees = [];
    $scope.displacedEmployees = [];
    $scope.moves = [];
    $scope.showSidebar = false;

    $scope.userRoles = userRoles;
    console.log($scope.userRoles);
    for (var i = 0; i < $scope.userRoles.length; i++) {
        // if user is an admin, set variable to true (TODO)
        if ($scope.userRoles[i].toLowerCase() == 'users') {
            $scope.isAdmin = true;
        }
    }

    // disable search bar until desks are loaded
    $scope.loadingFloorData = true;

    // function to return an employee object after searching current floor by employee name
    var fetchEmployeeByName = function (employeeName) {
        if (employeeName) {
            for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
                if (employeeName.toLowerCase() == $scope.currentFloorEmployees[i].name.toLowerCase()) {
                    return $scope.currentFloorEmployees[i];
                }
            }
        }
    };

    // function to return an employee object after searching current floor by employee ID
    var fetchEmployeeById = function (employeeId) {
        for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
            if (employeeId === $scope.currentFloorEmployees[i].id) {
                return $scope.currentFloorEmployees[i];
            }
        }
    };

    // get all data for autocompleting form inputs
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

    // create a new move form upon "Start Move" submit
    $scope.createMoveForm = function () {
        var addToDisplaced = true,
            // create a move object for later reference
            move = {
                deskNumber: $scope.currentDeskNumber,
                displacedEmployee: fetchEmployeeByName($scope.currentDeskOccupant),
                movedEmployee: fetchEmployeeByName($scope.futureDeskOccupant)
            };

        if ($scope.moves.length == 0) {

            if (!move.movedEmployee || (move.displacedEmployee == move.movedEmployee)) {
                move.movedEmployee = move.displacedEmployee;
            }
            else {
                // Setting future desk number property on moved employee
                move.movedEmployee.future.deskInfo.deskNumber = move.deskNumber;
                $scope.displacedEmployees.push(move.displacedEmployee);
            }

            $scope.movedEmployees.push(move.movedEmployee);
            $scope.createMoveFormError = "";
            $scope.moves.push(move);
        }
        else {
            var blankMove = false;
            // If moving a single employee (to a new desk)
            if (!move.movedEmployee) {
                move.movedEmployee = move.displacedEmployee;

                for (var i = 0; i < $scope.movedEmployees.length; i++) {
                    // Check if tab is already open for this employee
                    if ($scope.movedEmployees[i].name == move.movedEmployee.name) {
                        var name = $scope.movedEmployees[i].name;
                        $scope.createMoveFormError = name + " has a move open!";
                        return;
                    }
                }

                blankMove = true;
                $scope.movedEmployees.push(move.movedEmployee);
                $scope.createMoveFormError = "";
            }


            for (var i = 0; i < $scope.movedEmployees.length; i++) {
                // Prevents multiple people moving to one desk
                if (!blankMove && ($scope.movedEmployees[i].future.deskInfo.deskNumber == move.deskNumber)) {
                    var name = $scope.movedEmployees[i].name;
                    $scope.createMoveFormError = " Close the tab for " + name + " before creating this move!";
                    return;
                }

                // Check if tab is already open for this employee
                if (!blankMove && ($scope.movedEmployees[i].name == move.movedEmployee.name)) {
                    var name = $scope.movedEmployees[i].name;
                    $scope.createMoveFormError = "A move has already been started for " + name + ".";
                    return;
                }

                if ($scope.movedEmployees[i].name == move.displacedEmployee.name) {
                    addToDisplaced = false;
                }
            }

            for (var i = 0; i < $scope.displacedEmployees.length; i++) {
                // remove name from displaced employee list if move is created for this employee
                if ($scope.displacedEmployees[i].name == move.movedEmployee.name) {
                    $scope.displacedEmployees.splice(i, 1);
                }
            }

            if (!blankMove && !$scope.createMoveFormError.length) {
                move.movedEmployee.future.deskInfo.deskNumber = move.deskNumber;
                $scope.movedEmployees.push(move.movedEmployee);
                if (addToDisplaced && !($scope.movedEmployees[i].name == move.displacedEmployee.name)) {
                    $scope.displacedEmployees.push(move.displacedEmployee);
                }
                $scope.moves.push(move);
                $scope.createMoveFormError = "";
            }
        }

        $scope.futureDeskOccupant = "";
        $scope.currentFormTab = $scope.movedEmployees.length - 1;
    }

    $scope.sendAllForms = function () {
        $scope.movesProcessed = 0;
        $scope.sendFormError = "";
        // show error if there are still displaced employees
        if ($scope.displacedEmployees.length) {
            $scope.sendFormError = "All displaced employees must be given a new desk before moves may be submitted.";
            $scope.sendFormSuccess = "";
            return;
        }

        for (var i = 0; i < $scope.movedEmployees.length; i++) {
            currentEmployee = $scope.movedEmployees[i].current;

            $scope.movedEmployees[i].future.ultiproInfo.supervisor = ($scope.movedEmployees[i].future.ultiproInfo.supervisor) ? $scope.movedEmployees[i].future.ultiproInfo.supervisor : currentEmployee.ultiproInfo.supervisor;
            $scope.movedEmployees[i].future.bazookaInfo.managerId = ($scope.movedEmployees[i].future.bazookaInfo.managerId) ? $scope.movedEmployees[i].future.bazookaInfo.managerId : currentEmployee.bazookaInfo.managerId;
            $scope.movedEmployees[i].future.deskInfo.deskNumber = ($scope.movedEmployees[i].future.deskInfo.deskNumber) ? $scope.movedEmployees[i].future.deskInfo.deskNumber : $scope.movedEmployees[i].current.deskInfo.deskNumber;


            requestForm.sendForm($scope.movedEmployees[i]).then(function (data) {
                console.log(data);
                $scope.movesProcessed += 1;
            },
            function (errorMessage) {
                $scope.sendFormError = "The following error occurred while requesting the above move(s): " + errorMessage + ". Please try again or contact the help desk.";
                $scope.sendFormSuccess = "";
                console.log(errorMessage);
                return;
            });
        }
        $scope.$watch('movesProcessed', function () {
            if ($scope.movedEmployees.length == $scope.movesProcessed) {
                $scope.sendFormError = "";
                $scope.sendFormSuccess = "Your moves have been successfully requested. You will receive notification once they have been approved.";
                // remove all moves after processing them
                $scope.cancelAllMoves();
            }
        });
    }

    $scope.cancelSingleMove = function (index) {
        // store the move to be cancelled
        var removedForm = $scope.movedEmployees[index];

        // remove from list of moved employees
        $scope.movedEmployees.splice(index, 1);

        // change active form tab
        $scope.currentFormTab = $scope.movedEmployees.length - 1;

        // remove object from list of moves
        $scope.moves.splice(index, 1);

        // if a displaced employee results from move cancel, store them
        for (var i = 0; i < $scope.movedEmployees.length; i++) {
            if ($scope.movedEmployees[i].future.deskInfo.deskNumber == removedForm.current.deskInfo.deskNumber) {
                $scope.displacedEmployees.push(removedForm);
            }
        }

        // if a displaced employee is re-placed at a desk from move cancel, remove them from list
        for (var i = 0; i < $scope.displacedEmployees.length; i++) {
            if (removedForm.future.deskInfo.deskNumber == $scope.displacedEmployees[i].current.deskInfo.deskNumber) {
                $scope.displacedEmployees.splice(i, 1);
            }
        }

        // if there are no more displaced employees and no desk is selected, hide sidebar
        if ($scope.displacedEmployees.length === 0 && !$scope.selectedDeskEmployee) {
            $scope.showSidebar = false;
        }
    }

    $scope.cancelAllMoves = function () {
        // empty all lists and/or errors
        $scope.movedEmployees = [];
        $scope.displacedEmployees = [];
        $scope.createMoveFormError = "";
        $scope.moves = [];
    }

    $scope.searchForEmployee = function () {
        var employeeId;

        $scope.futureDeskOccupant = "";
        // auto-populate future occupant if one exists
        for (var i = 0; i < $scope.moves.length; i++) {
            if ($scope.moves[i].displacedEmployee.name.toLowerCase() == $scope.employeeToSearchFor.toLowerCase()) {
                $scope.futureDeskOccupant = $scope.moves[i].movedEmployee.name;
            }
        }

        employeeId = fetchEmployeeByName($scope.employeeToSearchFor.toLowerCase()).id;
        for (var i = 0; i < $scope.maps[$scope.currentFloor].desks.length; i++) {
            if ($scope.maps[$scope.currentFloor].desks[i].id === employeeId) {
                // highlight desk and show it in sidebar
                $scope.selectDesk($scope.maps[$scope.currentFloor].desks[i]);

                // zoom and pan to desk
                $scope.maps[$scope.currentFloor].setZoom(7);
                $scope.maps[$scope.currentFloor].panTo($scope.focusedDesk.getPosition());
                break;
            }
        }

    }

    $scope.selectDisplacedEmployee = function (displacedEmployee) {
        $scope.futureDeskOccupant = "";

        // auto-populate future occupant if it exists
        for (var i = 0; i < $scope.moves.length; i++) {
            if ($scope.moves[i].displacedEmployee.name.toLowerCase() == displacedEmployee.toLowerCase()) {
                $scope.futureDeskOccupant = $scope.moves[i].movedEmployee.name;
            }
        }

        var employeeId = fetchEmployeeByName(displacedEmployee.toLowerCase()).id;
        for (var i = 0; i < $scope.maps[$scope.currentFloor].desks.length; i++) {
            if ($scope.maps[$scope.currentFloor].desks[i].id === employeeId) {
                // highlight desk and show it in sidebar
                $scope.selectDesk($scope.maps[$scope.currentFloor].desks[i]);

                // zoom and pan to desk
                $scope.maps[$scope.currentFloor].setZoom(7);
                $scope.maps[$scope.currentFloor].panTo($scope.focusedDesk.getPosition());
                break;
            }
        }
    }

    // set future desk occupant upon click of "move" button in displaced employees list
    $scope.setFutureDeskOccupant = function (futureOccupant) {
        $scope.futureDeskOccupant = futureOccupant;
    }

    $scope.selectDesk = function (desk) {
        // erase previous messages
        $scope.sendFormSuccess = "";
        $scope.sendFormError = "";

        // remove highlight color if previous desk selected
        if ($scope.focusedDesk) {
            $scope.focusedDesk.modColor('#f7f7f7');
            $scope.focusedDesk = null;
        }
        // select and highlight new desk
        $scope.focusedDesk = desk;
        $scope.focusedDesk.modColor('#0592fa');
        if (!$scope.$$phase) {
            $scope.$apply();
        }

        // get employee from desk
        var employee = fetchEmployeeById(desk.id);

        // get desk number from employee
        $scope.focusedDeskNumber = employee.current.deskInfo.deskNumber;

        $scope.showSidebar = true;
        // set data for sidebar
        $scope.currentDeskNumber = employee.current.deskInfo.deskNumber;
        $scope.currentDeskOccupant = employee.name;
        $scope.currentDeskOrientation = employee.location.orientation;
        //set current employee
        $scope.selectedDeskEmployee = employee;
    }

    $scope.doNothing = function () {
    }

    $scope.changeOrientation = function () {
        // only make changes to desk if user has rights
        if ($scope.isAdmin) {
            var deskData, updatedDeskInfo;

            // get all info for desk based on focusedDeskNumber
            if ($scope.currentFloorDesks) {
                for (var i = 0; i < $scope.currentFloorDesks.length; i++) {
                    if ($scope.currentFloorDesks[i].deskNumber == $scope.focusedDeskNumber) {
                        deskData = $scope.currentFloorDesks[i];
                    }
                }

                // only make changes if new orientation is different from saved orientation
                if (deskData.location.orientation !== $scope.currentDeskOrientation) {
                    // rotate on map
                    $scope.focusedDesk.modPath(deskData.location.topLeft.xCoordinate, deskData.location.topLeft.yCoordinate, $scope.currentDeskOrientation);

                    // get desk info for changed desk
                    var deskRep = $scope.maps[$scope.currentFloor].getDesk($scope.focusedDeskNumber);

                    // define new data to be saved to database
                    updatedDeskInfo = {
                        deskNumber: $scope.focusedDeskNumber,
                        x: deskData.location.topLeft.xCoordinate,
                        y: deskData.location.topLeft.yCoordinate,
                        orientation: $scope.currentDeskOrientation
                    };

                    deskData.location.orientation = $scope.currentDeskOrientation;
                    deskData.location.topLeft.xCoordinate = deskRep.getPoint().x;
                    deskData.location.topLeft.yCoordinate = deskRep.getPoint().y;

                    desks.updateDesk($scope.focusedDeskNumber, updatedDeskInfo).then(function (data) {
                        // do something with success data
                        console.log(data);
                    },
                    function (errorMessage) {
                        console.log(errorMessage);
                    });
                }
            }
        }
    }

    $scope.checkSearch = function () {
        if ($scope.currentFloorEmployees) {
            for (var i = 0; i < $scope.currentFloorEmployees.length; i++) {
                if ($scope.currentFloorEmployees[i].name.toLowerCase() === $scope.employeeToSearchFor.toLowerCase()) {
                    // search for employee if a match is found before user submits form
                    $scope.searchForEmployee();
                    break;
                }
            }
        }
    }

    // watch for change in current floor tab. reload desks, employees, and employee names
    $scope.$watch('currentFloor', function () {
        var center;

        // erase previous messages
        $scope.sendFormSuccess = "";
        $scope.sendFormError = "";

        // erase previous desks
        for (var i = 0; i < $scope.maps[$scope.currentFloor].desks.length; i++) {
            $scope.maps[$scope.currentFloor].desks[i].setMap(null);
        }
        $scope.maps[$scope.currentFloor].desks = [];

        // erase search bar
        $scope.employeeToSearchFor = "";

        // disable search bar until desks are loaded
        $scope.loadingFloorData = true;

        // unfocus any desk
        if ($scope.focusedDesk) {
            $scope.focusedDesk.modColor('#f7f7f7');
            $scope.focusedDesk = null;
        }

        // determine whether or not to show sidebar
        if ($scope.displacedEmployees.length === 0) {
            $scope.showSidebar = false;
        }
        else {
            $scope.selectedDeskEmployee = null;
        }

        // resize map on tab change
        window.setTimeout(function () {
            google.maps.event.trigger($scope.maps[$scope.currentFloor], 'resize');

            if ($scope.currentFloor === 3) {
                center = new google.maps.LatLng(67, -173);
            }
            else if ($scope.currentFloor === 4) {
                center = new google.maps.LatLng(67, -173);
            }
            else if ($scope.currentFloor === 5) {
                center = new google.maps.LatLng(67, -177);
            }


            $scope.maps[$scope.currentFloor].panTo(center);

        }, 100);

        // load all desks for new floor
        desks.getDesksByFloor($scope.currentFloor).then(function (data) {
            $scope.loadingFloorData = false;
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
                        emailInfo: {
                            groupsToBeAddedTo: "N/A",
                            groupsToBeRemovedFrom: "N/A"
                        },
                        reviewInfo: {
                            groupsToBeAddedTo: "N/A",
                            groupsToBeRemovedFrom: "N/A"
                        },
                        future: {
                            bazookaInfo: {
                                jobTitle: "N/A",
                                department: "N/A",
                                group: "N/A",
                                managerId: "N/A",
                                jobTemplate: "N/A",
                                securityItemRights: "N/A"
                            },
                            ultiproInfo: {
                                jobTitle: "N/A",
                                department: "N/A",
                                group: "N/A",
                                supervisor: "N/A",
                                other: "N/A"
                            },
                            deskInfo: {
                                deskNumber: "N/A",
                                office: "GX"
                            },
                            phoneInfo: {
                                phoneNumber: "N/A"
                            }
                        },
                        current: {
                            bazookaInfo: {
                                jobTitle: currentPerson.jobTitle,
                                department: currentPerson.department,
                                group: currentPerson.group,
                                managerId: currentPerson.managerName,
                                jobTemplate: currentPerson.department + " " + currentPerson.jobTitle,
                                securityItemRights: ""
                            },
                            ultiproInfo: {
                                jobTitle: currentPerson.jobTitle,
                                department: currentPerson.department,
                                group: currentPerson.group,
                                supervisor: currentPerson.managerName,
                                other: ""
                            },
                            deskInfo: {
                                deskNumber: currentDesk.deskNumber,
                                office: "GX"
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
                newDesk = $scope.maps[currentDesk.location.floor].addDesk(currentDesk.location.topLeft.xCoordinate, currentDesk.location.topLeft.yCoordinate, currentDesk.location.orientation, employee, currentDesk.deskNumber, $scope.isAdmin);
                // add click listener to desk to highlight it and show it in sidebar
                google.maps.event.addListener(newDesk, 'click', function (event) {
                    $scope.selectDesk(this);
                    $scope.employeeToSearchFor = "";
                    $scope.futureDeskOccupant = "";
                    $scope.$apply();
                });

                // add dragend listener to update desk position in database
                google.maps.event.addListener(newDesk, "dragend", function (evt) {
                    // only store changes if user has rights
                    if ($scope.isAdmin) {
                        var deskNumber = fetchEmployeeById(this.id).current.deskInfo.deskNumber,
                            deskData,
                            updatedDeskInfo,
                            point = $scope.maps[$scope.currentFloor].fromLatLngToPoint(evt.latLng),
                            deskRep;

                        // get all info for desk based on dragged desk number
                        for (var i = 0; i < $scope.currentFloorDesks.length; i++) {
                            if ($scope.currentFloorDesks[i].deskNumber == deskNumber) {
                                deskData = $scope.currentFloorDesks[i];
                            }
                        }
                        deskRep = $scope.maps[$scope.currentFloor].getDesk(deskNumber);

                        updatedDeskInfo = {
                            deskNumber: deskNumber,
                            x: deskRep.getPoint().x,
                            y: deskRep.getPoint().y,
                            orientation: deskData.location.orientation
                        };

                        deskData.location.topLeft.xCoordinate = deskRep.getPoint().x;
                        deskData.location.topLeft.yCoordinate = deskRep.getPoint().y;

                        $scope.$apply(function () {
                            desks.updateDesk(deskNumber, updatedDeskInfo).then(function (data) {
                                // do something with success data
                                console.log(data);
                            },
                            function (errorMessage) {
                                console.log(errorMessage);
                            });
                        });
                    }
                });
            }
        },
        function (errorMessage) {
            console.log(errorMessage);
        });
    })
};

IndexCtrl.resolve = {
    userRoles: function (user, $q) {
        var deferred = $q.defer();

        user.getUserRoles().then(function (data) {
            deferred.resolve(data);
        }, function (errorData) {
            deferred.reject();
        });
        return deferred.promise;
    }
}