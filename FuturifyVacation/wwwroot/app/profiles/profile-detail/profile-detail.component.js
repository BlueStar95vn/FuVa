angular.
    module('profileDetail').
    component('profileDetail', {
        templateUrl: "/app/profiles/profile-detail/profile-detail.html",
        controller: function ($http,$location) {
            var vm = this;       
            $http.get('http://localhost:63237/api/profiles/myId').then(function (response) {
                vm.employees = response.data;                
            });
            $http.get('http://localhost:63237/api/profiles/getmyteam').then(function (response) {
                vm.myTeams = response.data;
            });

            vm.goToTeam = function (teamId) {
                $location.path('/teams/detail/' + teamId)
            };
        }
    });
