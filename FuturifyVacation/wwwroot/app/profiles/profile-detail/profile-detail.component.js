angular.
    module('profileDetail').
    component('profileDetail', {
        templateUrl: "/app/profiles/profile-detail/profile-detail.html",
        controller: function ($http) {
            var vm = this;       
            $http.get('http://localhost:63237/api/profiles/myId').then(function (response) {
                vm.employees = response.data;                
            });
        }
    });
