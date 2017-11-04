angular.
    module('employeeAdd').
    component('employeeAdd', {
        templateUrl: 'app/employees/employee-add/employee-add.html',
        controller: function ($http, $location) {
            var vm = this;
            vm.employees = {};
            vm.register = function () {
                $http.post("http://localhost:63237/api/employees/register", vm.employees).then(function () {                   
                    alert("Register successfully!");
                    $location.path("/employees");
                }).catch(function (error) {
                    console.log(error);
                });
            }

           
        }
    });