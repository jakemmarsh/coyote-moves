var myModule = angular.module('coyoteMoves', ['ui.utils', 'ui.map']).
  config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

      $locationProvider.html5Mode(true);

      $routeProvider.
          when('/coyotemoves', { templateUrl: 'coyotemoves/public/partials/index.html', controller: IndexCtrl }).
          otherwise({ redirectTo: '/coyotemoves' });
  }]);