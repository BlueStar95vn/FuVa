angular.
    module('employeeAdd').
    component('employeeAdd', {
        templateUrl: 'app/employees/employee-add/employee-add.html',
        controller: function ($http, $location) {
            var vm = this;
            vm.employees = {};
            vm.addClicked = false;
            vm.register = function () {
                vm.addClicked = true;
                $http.post("http://localhost:63237/api/employees/register", vm.employees).then(function () {                   
                    alert("Register successfully!");
                    vm.sendLoginInfoEmail();
                    $location.path("/employees");
                }).catch(function (error) {
                    console.log(error);
                });
            }


            vm.sendLoginInfoEmail = function () {
                $http.post("http://localhost:63237/api/emailsenders/addemployee", vm.employees).then(function () {
                   
                }).catch(function (error) {
                    console.log(error);
                });
            } 
           
           
        }
    });