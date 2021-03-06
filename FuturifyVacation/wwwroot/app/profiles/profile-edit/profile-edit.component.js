﻿angular.
    module('profileEdit').
    component('profileEdit', {
        templateUrl: '/app/profiles/profile-edit/profile-edit.html',
        controller: function ($http,$location) {            
            var vm = this;
            vm.employees = {};
            $http.get('http://localhost:63237/api/profiles/myId').then(function (response) {
                vm.employees = response.data;
                vm.employees.doB = new Date(response.data.doB);
            });
            vm.save = function () {
                $http.post('http://localhost:63237/api/profiles/update', vm.employees).then(function () {
                    alert("Save successfully");
                    $location.path('/profile');
                }).catch(function (error) {
                    console.log(error);
                });
            };
            var date = new Date();
            var y = date.getFullYear();

            vm.currentDate = new Date();
            vm.minAge = new Date(y - 18, 12, 31);
            vm.maxAge = new Date(y - 60, 1, 1);
           

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
