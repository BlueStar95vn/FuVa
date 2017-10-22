angular.
    module('profileEdit').
    component('profileEdit', {
        templateUrl: '/app/profiles/profile-edit/profile-edit.html',
        controller: function ($http,$location) {            
            var vm = this;            
            $http.get('http://localhost:63237/api/profiles/myId').then(function (response) {
                vm.employees = response.data;
            });
            vm.save = function () {
                $http.post('http://localhost:63237/api/profiles/update', vm.employees).then(function () {
                    alert("Save successfully");
                    $location.path('/profile');
                }).catch(function(error){
                    console.log(error);
                });

            }
        }
    });
