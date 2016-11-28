(function () {
    'use strict';

    angular
        .module('Administration')
        .controller('ExternalApplicationsController', ExternalApplicationsController);

    ExternalApplicationsController.$inject = ['$location', 'ExternalApplicationsService'];

    function ExternalApplicationsController($location, externalApplicationsService) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'ExternalApplicationsController';
        vm.externalApplications = [];

        activate();

        function activate() {
            externalApplicationsService.getExternalApplications()
                .then(function (response) {
                    response.data.externalApplications.forEach(function (externalApplication) {
                        vm.externalApplications.push(externalApplication);
                    });
                });
        }
    }
})();
