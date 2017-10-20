angular.
    module('employeeList').
    component('employeeList', {
        templateUrl: "/app/employees/employee-list/employee-list.html",
        controller: function ($http) {
            var vm = this;          
            $http.get('http://localhost:63237/api/employees/getall').then(function (response) {
                vm.employees = response.data;
            });
        }
    });