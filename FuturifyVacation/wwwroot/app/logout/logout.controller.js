(function () {

    'use strict';
    angular.module('logout').
        controller('authLogoutController', authLogoutController);

    authLogoutController.$inject = ['$location', 'authService'];
    function authLogoutController($location, authService) {
        const vm = this;
        vm.onLogout = function () {
            $location.path('/logout');
            const token = localStorage.getItem('token');
            if (token) {
                authService.logout(token)
                    .then(function () {
                        localStorage.removeItem('token');
                        $location.path('/login');
                    })
                    .catch(function (err) {
                        console.log(err)
                    });
            }
        };
    }
})();