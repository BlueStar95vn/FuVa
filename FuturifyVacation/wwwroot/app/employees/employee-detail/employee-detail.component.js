angular.
    module('employeeDetail').
    component('employeeDetail', {
        templateUrl: "/app/employees/employee-detail/employee-detail.html",
        controller: function ($http, $routeParams,$location) {
            var vm = this;      
            vm.employees = {};

            $http.get('http://localhost:63237/api/employees/' + $routeParams.userId).then(function (response) {
                vm.employees = response.data;                
            });
            vm.editId = function (userId) {
                $location.path('/employees/edit/' + userId);
            };

            $http.get('http://localhost:63237/api/employees/getteam/' + $routeParams.userId).then(function (response) {
                vm.teams = response.data;
            })
            vm.goToTeam = function (teamId) {
                $location.path('/teams/detail/' + teamId)
            };

        }
    });
