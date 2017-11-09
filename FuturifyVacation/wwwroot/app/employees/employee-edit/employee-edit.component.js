angular.
    module('employeeEdit').
    component('employeeEdit', {
        templateUrl: '/app/employees/employee-edit/employee-edit.html',
        controller: function ($http,$location,$routeParams) {           
            var vm = this;            
            $http.get('http://localhost:63237/api/employees/'+ $routeParams.userId).then(function (response) {
                vm.employees = response.data;
                vm.employees.doB = new Date(response.data.doB);
            });
            vm.save = function () {
                $http.post('http://localhost:63237/api/employees/update/' + $routeParams.userId, vm.employees).then(function () {
                    alert("Save successfully");
                    //$location.path('/employees/detail/' + $routeParams.userId);
                    $location.path('/employees');
                }).catch(function(error){
                    console.log(error);
                });
            }
            vm.detailId = function (userId) {
                $location.path('/employees');
                //$location.path('/employees/detail/' + userId);
            };
            vm.format = 'dd/MM/yyyy';
            vm.open1 = function () {
                vm.popup1.opened = true;
            };
            vm.dateOptions = {
                dateDisabled: false,               
                startingDay: 1
            };
            vm.popup1 = {
                opened: false
            };
            vm.altInputFormats = ['M!/d!/yyyy'];
        }
    });
