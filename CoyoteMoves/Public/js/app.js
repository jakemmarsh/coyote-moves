var myModule = angular.module('coyoteMoves', ['ui.utils', '$strap.directives', 'uiSlider']).
  config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

      $locationProvider.html5Mode(true);

      $routeProvider.
          when('/coyotemoves',
          {
              templateUrl: 'coyotemoves/public/partials/index.html',
              controller: IndexCtrl,
              resolve: IndexCtrl.resolve
          }).
          otherwise({ redirectTo: '/coyotemoves' });
  }]);
