(function () {

    'use strict';

    angular
        .module('AuthApp.components.auth', [])
        //.controller('authLoginController', authLoginController)
        .controller('authStatusController', authStatusController);

    //authLoginController.$inject = ['$location', 'authService'];
    authStatusController.$inject = ['$location', 'authService'];

    //function authLoginController($location, authService) {
    //    /*jshint validthis: true */
    //    const vm = this;
    //    vm.user = {};
    //    vm.onLogin = function () {
    //        authService.login(vm.user)
    //            .then(/*(user) =>*/ function () {                 
    //                $location.path('/status');
    //                alert("You have been successfully logged in...");
    //            })
    //            .catch(function (error) {
    //                alert('Incorrect email or password');
    //                console.log(error);
    //            });
    //    };
    //}
    function authStatusController($location, authService) {
        /*jshint validthis: true */
        const vm = this;
        vm.isLoggedIn = false;
        vm.roles = {};
        authService.ensureAuthenticated()
            .then(function (response) {
                vm.roles = response.data;
                //vm.getId = response.data;
                //localStorage.setItem("userId", vm.getId.userId);
                vm.isLoggedIn = true;
            })
            .catch(function(err){
                console.log(err);
            });
    }
})();