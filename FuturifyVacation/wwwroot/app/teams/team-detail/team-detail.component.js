angular.
    module('teamDetail').
    component('teamDetail', {
        templateUrl: 'app/teams/team-detail/team-detail.html',
        controller: function ($http, $routeParams,$location) {
            var vm = this;
            $http.get('http://localhost:63237/api/teams/getteamdetail/' + $routeParams.teamId).then(function (response) {
                vm.teamNameModel = response.data;
                vm.teamLeadModel = response.data;
            }).catch(function (err) {
                console.log(err);
            });

            //Load member in team
            $http.get('http://localhost:63237/api/teams/getteammember/' + $routeParams.teamId).then(function (response) {
                vm.members = response.data;
            });

            vm.gotoEdit = function () {
                $location.path('/teams/edit/' + $routeParams.teamId);
            }
        }
    })