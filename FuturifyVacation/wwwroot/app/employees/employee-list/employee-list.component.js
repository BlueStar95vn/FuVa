angular.
    module('employeeList').
    component('employeeList', {
        templateUrl: "/app/employees/employee-list/employee-list.html",
        controller: function ($http, $routeParams, $location, $log) {
            var vm = this;
            vm.employees = [];
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
                        alert("Change team lead before deteting this employee!")
                        console.log(error)
                    });
            }

            vm.searchChange = function () {
                vm.currentPage = 1;
            }
            vm.currentPage = 1;
            vm.viewby = 10;                    
            vm.itemsPerPage = vm.viewby;
            vm.maxSize = 5;
            vm.pageChanged = function () {
                $log.log('Page changed to: ' + vm.currentPage);
            };
            vm.setItemsPerPage = function (num) {
                vm.itemsPerPage = num;
                vm.currentPage = 1;
            }
        }
    });

