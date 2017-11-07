(function () {

    'use strict';

    angular
        .module('AuthApp.components.auth', [])      
        .controller('authStatusController', authStatusController);

    authStatusController.$inject = ['$location', 'authService'];

    function authStatusController($location, authService) {
        /*jshint validthis: true */
        const vm = this;
        vm.isLoggedIn = false;
        vm.roles = {};
        authService.ensureAuthenticated()
            .then(function (response) {
                vm.roles = response.data;
                vm.isLoggedIn = true;
            })
            .catch(function(err){
                console.log(err);
            });
    }
})();