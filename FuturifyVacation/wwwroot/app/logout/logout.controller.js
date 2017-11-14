(function () {

    'use strict';
    angular.module('logout').
        controller('authLogoutController', authLogoutController);

    authLogoutController.$inject = ['$location', 'authService'];
    function authLogoutController($location, authService) {
        const vm = this;
        vm.onLogout = function () {
            $location.path('/logout');
            authService.logout()
                .then(function () {                   
                    $location.path('/login');
                })
                .catch(function (err) {
                    console.log(err)
                });
        };
    }
})();