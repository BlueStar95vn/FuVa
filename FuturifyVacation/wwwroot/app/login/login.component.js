
angular.
    module('login').
    component('login', {
        templateUrl: '/app/login/login.html',
        controller: function ($location, authService, $http) {
            const vm = this;
            vm.user = {};
            vm.onLogin = function () {
                authService.login(vm.user)
                    .then(/*(user) =>*/ function () {
                        $location.path('/status');
                        alert("You have been successfully logged in...");
                    })
                    .catch(function (error) {
                        alert('Incorrect email or password');
                        console.log(error);
                    });
            };
            vm.google = function () {
                $http.post('http://localhost:63237/api/account/externallogin').then(function () {

                }).catch(function (err) {
                    console.log(err);
                });
            }
        }
    });

