angular.
    module('employeeEdit').
    component('employeeEdit', {
        templateUrl: '/app/employees/employee-edit/employee-edit.html',
        controller: function ($http,$location,$routeParams) {
            //$scope.data = {
            //    model: null,
            //    availableOptions:
            //    [
            //        { id: "1", firstName: "ABC", lastName: "BCD", gender: "Male", phoneNumber: "0122121212", position: "DEF", dob: "1/1/1990", email: "123@gmail.com", dayoff: "12" },
            //        { id: "2", firstName: "OKA", lastName: "UHS", gender: "Female", phoneNumber: "65765765", position: "BOO", dob: "1/2/1990", email: "343@gmail.com", dayoff: "12" },
            //        { id: "3", firstName: "AKO", lastName: "HSU", gender: "Male", phoneNumber: "0324234212", position: "DEF", dob: "1/3/1990", email: "153@gmail.com", dayoff: "12" },
            //        { id: "4", firstName: "OAK", lastName: "SHU", gender: "Female", phoneNumber: "014543543", position: "DEF", dob: "1/4/1990", email: "2343@gmail.com", dayoff: "12" }
            //    ]
            //}
            //$scope.selectedOption = $scope.data.availableOptions[0];
            var vm = this;            
            $http.get('http://localhost:63237/api/employees/'+ $routeParams.userId).then(function (response) {
                vm.employees = response.data;
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
                maxDate: new Date(2050, 5, 22),
                minDate: new Date(1950, 1, 1),
                startingDay: 1
            };
            vm.popup1 = {
                opened: false
            };
            vm.altInputFormats = ['M!/d!/yyyy'];
        }
    });
