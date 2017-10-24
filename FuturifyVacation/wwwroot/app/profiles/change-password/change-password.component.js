angular.module('changePassword').
    component('changePassword', {
        templateUrl: '/app/profiles/change-password/change-password.html',
        controller: function ($location, $http) {
            var vm = this;
            vm.profiles = {};
            vm.changepass = function () {
                $http.post('http://localhost:63237/api/profiles/changepassword', vm.profiles).then(function () {
                    alert("Change password successfully!");
                    $location.path('/profile')
                }).catch(function (error) {
                    console.log(error);
                });
            }
        }

    });
angular.module('changePassword').directive("matchPassword", function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=matchPassword"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.matchPassword = function (modelValue) {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
});