(function () {
    'use strict';
    angular
        .module('AuthApp.services', [])
        .service('authService', authService);
    authService.$inject = ['$http'];
    function authService($http) {
        /*jshint validthis: true */
        const baseURL = 'http://localhost:63237/';
        this.login = function (user) {
            return $http({
                method: 'POST',
                url: baseURL + 'api/account/login',
                data: user               
            });
        };
        this.logout = function () {
            return $http({
                method: 'POST',
                url: baseURL + 'api/account/logout'          
            });
        };
        this.ensureAuthenticated = function () {
            return $http({
                method: 'GET',
                url: baseURL + 'api/account/check-auth'               
            });
        };
    }   
})();