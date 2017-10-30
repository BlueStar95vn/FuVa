angular.module('vacationRequest')
    .component('vacationRequest', {
        templateUrl: '/app/vacation/vacation-request/vacation-request.html',
        controller: function ($http, $uibModal, $log, $document) {
            var vm = this;
            vm.vacations = {};
            $http.get('http://localhost:63237/api/vacations/getrequestvacation').then(function (response) {
                vm.vacations = response.data;

            })
            vm.approve = function (id, index) {
                $http.post('http://localhost:63237/api/vacations/approve/' + id).then(function () {
                    alert('Approved');
                    vm.vacations.splice(index, 1);
                }).catch(function (err) {
                    console.log(err);
                })

            }
            vm.disaprrove = function (id) {
                console.log(id);
            };

            vm.animationsEnabled = true;

            vm.disaprrove = function (id, index) {
                var modalInstance = $uibModal.open({
                    animation: vm.animationsEnabled,
                    component: 'modalComponent',
                    resolve: {
                        vacationId: function () {
                            return id;
                        },
                        index: function () {
                            return index;
                        },
                        vacations: function () {
                            return vm.vacations;
                        }
                    }
                });

                modalInstance.result.then(function (selectedItem) {
                    vm.selected = selectedItem;
                }, function () {
                    $log.info('modal-component dismissed at: ' + new Date());
                });
            };
            vm.toggleAnimation = function () {
                vm.animationsEnabled = !vm.animationsEnabled;
            };

        }
    });

angular.module('vacationRequest').component('modalComponent', {
    templateUrl: 'myModalContent.html',
    bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
    },
    controller: function ($http) {
        var vm = this;
        vm.$onInit = function () {
            vm.vacations = vm.resolve.vacations;
            vm.index = vm.resolve.index;
        };
        vm.reasons = {};
        vm.ok = function () {
            $http.post('http://localhost:63237/api/vacations/disapprove/' + vm.resolve.vacationId, vm.reasons).then(function () {
                vm.vacations.splice(vm.index, 1);
                console.log(vm.index);
                vm.close({ $value: 'cancel' });
            }).catch(function (err) {
                console.log(err);
            });
        };
        vm.cancel = function () {
            vm.dismiss({ $value: 'cancel' });
        };

    }
});