function IndexCtrl($scope, $routeParams) {
    $scope.thirdFloor = true;
    $scope.fourthFloor = false;
    $scope.fifthFloor = false;

    $scope.activateThirdFloor = function () {
        $scope.thirdFloor = true;
        $scope.fourthFloor = false;
        $scope.fifthFloor = false;
    }

    $scope.activateFourthFloor = function () {
        $scope.thirdFloor = false;
        $scope.fourthFloor = true;
        $scope.fifthFloor = false;
    }

    $scope.activateFifthFloor = function () {
        $scope.thirdFloor = false;
        $scope.fourthFloor = false;
        $scope.fifthFloor = true;
    }

}