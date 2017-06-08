var openWifiApp = angular.module('openWifiApp', ['ngRoute', 'ZoneControllers']);

openWifiApp.config(['$routeProvider', function($routeProvider) {
  $routeProvider.
  when('/', {
    templateUrl: 'partials/login.html',
    controller: 'LoginController'
  }).
  when('/dashboard', {
    templateUrl: 'partials/dashboard.html',
    controller: 'DashboardController'
  }).
  otherwise({
    redirectTo: '/list'
  });
}]);

var ZoneControllers = angular.module("ZoneControllers", []);

ZoneControllers.controller("LoginController", ['$scope', '$http']);

ZoneControllers.controller("DashboardController", ['$scope', '$http']);
