angular.module('setting').component('setting', {
    templateUrl: '/app/setting/setting.html',
    controller: function ($http) {
        var vm = this;
       
        vm.changeDayOff = false;
        vm.dayOffClicked = function () {
            vm.changeDayOff = true;
        };
        vm.dayOffCanceled = function () {
            vm.changeDayOff = false;
        };
        
        $http.get('http://localhost:63237/api/settings/allsetting').then(function (response) {
            vm.settings = response.data;
        }).catch(function (err) {
            console.log(err);
        });

        vm.setDayOff = function () {
            $http.post('http://localhost:63237/api/employees/setdayoff', vm.settings).then(function () {
                alert('Day off is updated');
                vm.saveDayoff();
            }).catch(function (err) {
                console.log(err);
            });
        };

        vm.saveDayoff = function () {
            $http.post('http://localhost:63237/api/settings/setdayoff', vm.settings).then(function () {
              
            }).catch(function (err) {
                console.log(err);
            });
        }
    }
})