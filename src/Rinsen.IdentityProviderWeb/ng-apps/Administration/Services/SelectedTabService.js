(function () {
    'use strict';

    angular
        .module('Administration')
        .factory('SelectedTabService', SelectedTabService);

    SelectedTabService.$inject = [];

    function SelectedTabService() {
        var service = {
            selectTab: selectTab,
            selected: 'init'
        };

        return service;

        function selectTab(name) {
            service.selected = name;
        }
    }
})();