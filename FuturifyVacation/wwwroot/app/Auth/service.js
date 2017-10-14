(function () {

    'use strict';

    angular
        .module('tokenAuthApp.services', [])
        .service('authService', authService);
    authService.$inject = ['$http'];

    function authService($http) {
        /*jshint validthis: true */
        const baseURL = 'http://localhost:63237/';
        this.login = function (user) {
            return $http({
                method: 'POST',
                url: baseURL + 'api/account/login',
                data: user,
                headers: { 'Content-Type': 'application/json' }
            });
        };
        this.logout = function (token) {
            return $http({
                method: 'POST',
                url: baseURL + 'api/account/logout',              
                headers: { 'Content-Type': 'application/json' }
            });
        };
        this.ensureAuthenticated = function (token) {
            return $http({
                method: 'GET',
                url: baseURL + 'api/account/check-auth',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: 'Bearer ' + token
                }
            });
        };
    }
    

})();