(function () {

    'use strict';

    angular
        .module('AuthApp.components.auth', [])
        .controller('authLoginController', authLoginController)
        .controller('authStatusController', authStatusController);


    authLoginController.$inject = ['$location', 'authService'];
    authStatusController.$inject = ['$location', 'authService'];


    function authLoginController($location, authService) {

        /*jshint validthis: true */
        const vm = this;
        vm.user = {};
        vm.onLogin = function () {
            authService.login(vm.user)
                .then(/*(user) =>*/ function(){
                    //localStorage.setItem('isLogged', user.data.token);
                    $location.path('/status');
                    alert("You have been successfully logged in...");
                })
                .catch(function (error) {
                    alert('Incorrect email or password');
                    console.log(error);
                });
        };
        //vm.onLogout = function () {
        //    const token = localStorage.getItem('token');
        //    if (token) {
        //        authService.logout(token)
        //            .then(function () {        
        //                //localStorage.removeItem('token');
        //                $location.path('/logout');                       
        //            });
        //    }         
        //}             

    }
    function authStatusController($location, authService) {
        /*jshint validthis: true */
        const vm = this;
        vm.isLoggedIn = false;
        authService.ensureAuthenticated()
            .then((user) => {
                if (user.data.status === 'success');
                vm.isLoggedIn = true;
            })
            .catch((err) => {
                console.log(err);
            });
    }
})();