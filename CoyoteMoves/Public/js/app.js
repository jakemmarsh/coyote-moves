angular.module('coyoteMoves', []).
  config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

      $routeProvider.
          when('/', { templateUrl: 'public/partials/index.html', controller: IndexCtrl }).
          otherwise({ redirectTo: '/' });
  }]);