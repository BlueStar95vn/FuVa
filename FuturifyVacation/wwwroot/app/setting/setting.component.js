angular.module('setting').component('setting', {
    templateUrl: '/app/setting/setting.html',
    controller: function ($http) {
        var vm = this;
        vm.dayOffModel = {};
        vm.dayOffModel.remainingDayOff = 0;
        vm.changeDayOff = false;
        vm.dayOffClicked = function () {
            vm.changeDayOff = true;
        };
        vm.dayOffCanceled = function () {
            vm.changeDayOff = false;
        }
        vm.setDayOff = function () {
            $http.post('http://localhost:63237/api/employees/setdayoff', vm.dayOffModel).then(function () {
                alert('Day off is updated');
            }).catch(function (err) {
                console.log(err);
            });
        }
    }
})