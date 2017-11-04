angular.
    module('employeeList').
    component('employeeList', {
        templateUrl: "/app/employees/employee-list/employee-list.html",
        controller: function ($http, $routeParams, $location) {
            var vm = this;
            
            $http.get('http://localhost:63237/api/employees/getall').then(function (response) {
                vm.employees = response.data;
            });
            vm.detailId = function (userId) {
                $location.path('/employees/detail/' + userId);
            }
            vm.editId = function (userId) {
                $location.path('/employees/edit/' + userId);
            }
            vm.removeUser = function (userId, index) {
                $http.delete('http://localhost:63237/api/employees/delete/'+userId)
                    .then(function () {
                        alert("Delete Successfully!");
                        vm.employees.splice(index, 1);
                        
                    }).catch(function (error) {
                        console.log(error)
                    });
            }
        }
    });

