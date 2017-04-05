(function () {
    'use strict';

    var administrationApp = angular.module('Administration', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
        
    ]);

    administrationApp.config(['$routeProvider', '$locationProvider', '$httpProvider',
        function ($routeProvider, $locationProvider, $httpProvider) {

            $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

            $locationProvider.html5Mode(true);

            $routeProvider.
                when('Administration', {
                    templateUrl: 'Administration/ExternalApplications',
                    controller: 'ExternalApplicationsController',
                    controllerAs: 'vm',
                }).
                when('Administration/ExternalApplications', {
                    templateUrl: 'Administration/ExternalApplications',
                    controller: 'ExternalApplicationsController',
                    controllerAs: 'vm',
                }).
                when('Administration/Users', {
                    templateUrl: 'Administration/Users',
                    controller: 'UsersController',
                    controllerAs: 'vm',
                }).
                otherwise({
                    redirectTo: 'Administration/ExternalApplications'
                });
        }]);
})();