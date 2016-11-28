(function () {
    'use strict';

    angular
        .module('Administration')
        .factory('ExternalApplicationsService', ExternalApplicationsService);

    ExternalApplicationsService.$inject = ['$http'];

    function ExternalApplicationsService($http) {
        var service = {
            getExternalApplications: getExternalApplications
        };

        return service;

        function getExternalApplications() {
            return $http.get('/webapi/ExternalApplications/GetAll');
        }
    }
})();