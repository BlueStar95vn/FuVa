angular.
    module('teamList').
    component('teamList', {
        templateUrl: "/app/teams/team-list/team-list.html",
        controller: function ($http, $location) {
            var vm = this;
            vm.teams = {};
            $http.get('http://localhost:63237/api/teams/getallteam').then(function (response) {
                vm.teams = response.data;
            }).catch(function (err) {
                console.log(err);
                });
          
            vm.remove = function (id, index) {
                $http.delete('http://localhost:63237/api/teams/delete/' + id).then(function () {
                    alert("Delete team successfully!");
                    vm.teams.splice(index, 1);
                }).catch(function (err) {
                    console.log(err);
                });
            }
            vm.detailId = function (teamId) {
                $location.path('/teams/detail/' + teamId);
            }
            vm.editId = function (teamId) {
                $location.path('/teams/edit/' + teamId);
            }
        }

    });