angular.
    module('teamAdd').
    component('teamAdd', {
        templateUrl: '/app/teams/team-add/team-add.html',
        controller: function ($timeout, $http, $location) {
            var vm = this;
            vm.employees = [];

            $http.get('http://localhost:63237/api/employees/getall').then(function (response) {
                vm.employees = response.data;
                vm.newTeams = vm.employees[0];
            });
            vm.newTeams = {};          
            //vm.newTeams = vm.selected;
            vm.save = function () {
                $http.post('http://localhost:63237/api/teams/addteam', vm.newTeams).then(function () {
                    alert("Add new team successfully!");
                    $location.path("/teams")
                }).catch(function (err) {
                    console.log(err)
                });
            }
        }

    });
