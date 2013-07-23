function IndexCtrl($scope, $routeParams) {
    $scope.currentFloor = 3;

    $scope.currentEmployee = 0;

    $scope.changeCurrentForm = function (index) {
        $scope.currentEmployee = index;
        $scope.$apply();
        console.log(index);
    }

    $scope.displacedEmployees = [{
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
};
