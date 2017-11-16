angular.module('vacationRequest')
    .component('vacationRequest', {
        templateUrl: '/app/vacation/vacation-request/vacation-request.html',
        controller: function ($http, $uibModal, $log, $document) {
            var vm = this;
            vm.vacations = {};
            $http.get('http://localhost:63237/api/vacations/getrequestvacation').then(function (response) {
                vm.vacations = response.data;

            });
            vm.approveClicked = false;

            vm.approve = function (vacation, index) {
                vm.approveClicked = true;
                vm.indexClicked = index;
                $http.post('http://localhost:63237/api/vacations/approve/' + vacation.id).then(function () {
                    vm.approveClicked = false;
                    alert('Approved');
                    vm.sendApproveEmail(vacation);
                    $http.post("http://localhost:63237/api/googlecalendars/addevent", vacation).then(function (response) {
                        var googleId = response.data.googleCalendarId;
                        vm.saveEventGoogleId(googleId, vacation.id);
                    }).catch(function (err) {
                        console.log('Add event to google calendar err');
                        console.log(err);
                    });
                    vm.vacations.splice(index, 1);
                }).catch(function (err) {
                    console.log(err);
                });

            }

            vm.sendApproveEmail = function (vacation) {
                $http.post("http://localhost:63237/api/emailsenders/approvevacation", vacation).then(function () {

                }).catch(function (err) {

                    console.log(err);
                });
            }
            //google approve
            vm.saveEventGoogleId = function (googleId, fullId) {
                $http.post("http://localhost:63237/api/googlecalendars/saveeventid/" + googleId + '/' + fullId).then(function () {

                }).catch(function (err) {
                    console.log(err);
                });
            }


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

            vm.detail = function (vacation, index) {
                var modalInstance = $uibModal.open({
                    animation: vm.animationsEnabled,
                    component: 'detailRequestComponent',
                    resolve: {
                        index: function () {
                            return index;
                        },
                        vacation: function () {
                            return vacation;
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
        vm.vacations = {};
        vm.thisVacation = {};
        vm.$onInit = function () {
            vm.index = vm.resolve.index;
            vm.vacations = vm.resolve.vacations;

        };
        vm.reasons = {};

        vm.disapproveClicked = false;
        vm.ok = function () {
            vm.disapproveClicked = true;
            $http.post('http://localhost:63237/api/emailsenders/disapprovevacation/' + vm.resolve.vacationId, vm.reasons).then(function () {

            }).catch(function (err) {
                console.log(err);
            });

            $http.post('http://localhost:63237/api/vacations/disapprove/' + vm.resolve.vacationId, vm.reasons).then(function () {
                alert("Disapproved!");
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


        //google calendar


        vm.deleteGoogleCalendarEvent = function (googleEventId) {
            $http.delete('http://localhost:63237/api/googleCalendars/deletegoogleevent/' + googleEventId).then(function () {

            }).catch(function (err) {
                console.log(err);
            });
        };
    }
});

angular.module('vacationRequest').component('detailRequestComponent', {
    templateUrl: 'detailRequestModal.html',
    bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
    },
    controller: function ($http) {
        var vm = this;

        vm.$onInit = function () {
            vm.index = vm.resolve.index;

            vm.vacation = vm.resolve.vacation;
            $http.get('http://localhost:63237/api/vacations/approvedvacationinmonth/' + vm.vacation.userId + '/' + vm.vacation.id).then(function (response) {
                vm.numberVacation = response.data;
            });

            $http.get('http://localhost:63237/api/vacations/checkteamondate/' + vm.vacation.userId + '/' + vm.vacation.id).then(function (response) {
                vm.teams = response.data;
               
            })
        };


        vm.cancel = function () {
            vm.dismiss({ $value: 'cancel' });
        };



    }
});