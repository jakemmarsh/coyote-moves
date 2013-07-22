angular.module('coyoteMoves', []).
  config(['$routeProvider', function ($routeProvider) {
      $routeProvider.
          when('/', { templateUrl: 'public/partials/index.html', controller: IndexCtrl }).
          otherwise({ redirectTo: '/' });
  }]);