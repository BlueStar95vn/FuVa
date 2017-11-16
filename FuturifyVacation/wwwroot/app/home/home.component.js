angular.module('home').
    component('home', {
        templateUrl: '/app/home/home.html',
        controller: function ($http) {
            var vm = this;
            $http.get('http://localhost:63237/api/settings/allsetting').then(function (response) {
                vm.settings = response.data;
                vm.time = vm.settings[0];
            }).catch(function (err) {
                console.log(err);
            });
        }
    });