(function () {
    'use strict';

    angular
        .module('Administration')
        .controller('AdministrationController', AdministrationController);

    AdministrationController.$inject = ['$scope', 'SelectedTabService'];

    function AdministrationController($scope, selectedTabService) {
        /* jshint validthis:true */
        var avm = this;
        avm.title = 'AdministrationController';

        activate();

        function activate() {
            $scope.$watch(function () { return selectedTabService.selected }, function (data) {
                avm.activeTab = data;
            }, true);

            $scope.$on('$routeChangeStart', function (event, next, current) {
                alert('change');
            });
        }
    }
})();
