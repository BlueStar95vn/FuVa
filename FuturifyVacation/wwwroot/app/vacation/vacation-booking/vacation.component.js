angular.
    module('vacation').
    component('vacation', {
        templateUrl: "/app/vacation/vacation-booking/vacation.html",
        controller: function ($compile, $timeout, uiCalendarConfig, $http, $location, $uibModal, $log, $document) {
            var vm = this;
            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();           
           
            vm.eventSources = [];
            vm.vacations = {};
            $http.get("http://localhost:63237/api/vacations/getuservacation").then(function (response) {
               
                vm.vacations = response.data;               
                uiCalendarConfig.calendars['myCalendar'].fullCalendar('addEventSource', vm.vacations);                
            }).catch(function (err) {
                console.log(err);
            });
           
            vm.eventSources = [vm.vacations];

            vm.animationsEnabled = true;

            /*eventClick */
            vm.detailVacation = function (event, date, jsEvent, view) {
                var modalInstance = $uibModal.open({
                    animation: vm.animationsEnabled,
                    component: 'vacationModalComponent',
                    resolve: {  
                        //vacationDetails = function () {
                        //    return vm.vacations;
                        //},
                        id: function () {
                            return event.id;
                        },
                        title: function () {
                            return event.title;
                        },
                        start: function () {
                            return event.start;                            
                        },
                        end: function () {
                            return event.end;
                        }
                    }
                });

                modalInstance.result.then(function () {                  
                }, function () {
                    $log.info('modal-component dismissed at: ' + new Date());
                    uiCalendarConfig.calendars['myCalendar'].fullCalendar('removeEvents');
                    uiCalendarConfig.calendars['myCalendar'].fullCalendar('addEventSource', vm.vacations);
                });
            };
            /* alert on Drop */
            vm.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
                vm.alertMessage = ('Event Dropped to make dayDelta ' + delta);
            };
            /* alert on Resize */
            vm.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
                vm.alertMessage = ('Event Resized to make dayDelta ' + delta);
            };
            /* add and removes an event source of choice */
            vm.addRemoveEventSource = function (sources, source) {
                var canAdd = 0;
                angular.forEach(sources, function (value, key) {
                    if (sources[key] === source) {
                        sources.splice(key, 1);
                        canAdd = 1;
                    }
                });
                if (canAdd === 0) {
                    sources.push(source);
                }
            };
            /* add custom event*/
            vm.addEvent = function () {
                var id = 1000;
                vm.events.push({
                    id: id++,
                    title: 'New Event',
                    start: new Date(y, m, 27),
                    end: new Date(y, m, 28),
                });
            };
            /* remove event */
            vm.remove = function (index) {
                vm.events.splice(index, 1);
                uiCalendarConfig.calendars['myCalendar'].fullCalendar('removeEvents');
                uiCalendarConfig.calendars['myCalendar'].fullCalendar('addEventSource', vm.events);
            };
            vm.update = function () {
                uiCalendarConfig.calendars['myCalendar'].fullCalendar('removeEvents');
                uiCalendarConfig.calendars['myCalendar'].fullCalendar('addEventSource', vm.events);
            };
            /* Change View */
            vm.changeView = function (view, calendar) {
                uiCalendarConfig.calendars[calendar].fullCalendar('changeView', view);
            };
            /* Change View */
            vm.renderCalendar = function (calendar) {
                $timeout(function () {
                    if (uiCalendarConfig.calendars[calendar]) {
                        uiCalendarConfig.calendars[calendar].fullCalendar('render');
                    }
                });
            };
            /* Render Tooltip */
            vm.eventRender = function (event, element, view) {
                element.attr({
                    'tooltip': event.title,
                    'tooltip-append-to-body': true
                });
                $compile(element)(vm);
            };
            /* config object */
            vm.uiConfig = {
                calendar: {
                    height: 700,
                    editable: true,
                    navLinks: true,
                    selectable: true,
                    selectHelper: true,
                    eventLimit: true,

                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,agendaWeek,agendaDay'
                    },
                    eventClick: vm.detailVacation,
                    eventDrop: vm.alertOnDrop,
                    eventResize: vm.alertOnResize,
                    eventRender: vm.eventRender,
                    eventSources: vm.eventSources,
                    eventColor: '#009966',
                    eventTextColor: 'FFFF99',
                    businessHours: {
                        dow: [1, 2, 3, 4, 5], // Monday - Friday
                        start: '09:00', // a start time 
                        end: '18:00', // an end time 
                    },
                    themeSystem: 'jquery-ui'
                }
            };

            //Add date form
            vm.format = 'dd/MM/yyyy hh:mm a';
            vm.open1 = function () {
                vm.popup1.opened = true;
            };
            vm.open2 = function () {
                vm.popup2.opened = true;
            };
            vm.popup1 = {
                opened: false
            };
            vm.popup2 = {
                opened: false
            };
            vm.dateOptions = {
                dateDisabled: disabled,
                formatYear: 'yy',
                maxDate: new Date(2050, 5, 22),
                minDate: new Date(),
                startingDay: 1
            };
            vm.toDateOptions = {
                dateDisabled: disabled,
                formatYear: 'yy',
                maxDate: new Date(2050, 5, 22),
                minDate: new Date(),
                startingDay: 1
            };
            function disabled(data) {
                var date = data.date,
                    mode = data.mode;
                return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
            }          
            vm.altInputFormats = ['M!/d!/yyyy'];
            
            /* config object */
            vm.newVacations = {};
            //add time
            vm.hstep = 1;
            vm.mstep = 60;
         

            var fromTime = new Date();
            fromTime.setHours(9);
            fromTime.setMinutes(0);

            vm.newVacations.start = fromTime;

            var toTime = new Date();
            toTime.setHours(18);
            toTime.setMinutes(0);
            vm.newVacations.end = toTime;
 
            vm.bookVacation = function () {
                $http.post("http://localhost:63237/api/vacations/bookvacation", vm.newVacations).then(function () {
                    uiCalendarConfig.calendars['myCalendar'].fullCalendar('removeEvents');
                    uiCalendarConfig.calendars['myCalendar'].fullCalendar('addEventSource', vm.vacations);
                    alert("Success");
                }).catch(function (err) {
                    console.log(err);
                });
            }
        }
    });


angular.module('vacation').component('vacationModalComponent', {
    templateUrl: 'vacationDetailModal.html',
    bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
    },
    controller: function ($http) {
        var vm = this;
        vm.formatModal = 'dd/MM/yyyy hh:mm a';
        vm.vacationDetail = {};
        vm.getDate = new Date();
        vm.$onInit = function () {
           
            vm.vacationDetail.id = vm.resolve.id;
            vm.vacationDetail.title = vm.resolve.title;
            vm.vacationDetail.start = new Date(vm.resolve.start);
            vm.vacationDetail.end = new Date(vm.resolve.end);
        };
        vm.hstep = 1;
        vm.mstep = 60;
     
        vm.open1 = function () {
            vm.popup1.opened = true;
        };
        vm.open2 = function () {
            vm.popup2.opened = true;
        };
        vm.popup1 = {
            opened: false
        };
        vm.popup2 = {
            opened: false
        };
        vm.dateOptions = {
            dateDisabled: disabled,
            formatYear: 'yy',
            maxDate: new Date(2050, 5, 22),
            minDate: new Date(),
            startingDay: 1
        };
        vm.toDateOptions = {
            dateDisabled: disabled,
            formatYear: 'yy',
            maxDate: new Date(2050, 5, 22),
            minDate: new Date(),
            startingDay: 1
        };
        function disabled(data) {
            var date = data.date,
                mode = data.mode;
            return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
        }

        vm.altInputFormats = ['M!/d!/yyyy'];

        vm.update = function () {
            $http.post('http://localhost:63237/api/vacations/updateuservacation', vm.vacationDetail).then(function () {
                alert("Update successfully");
            }).catch(function () {

            });
        };
        vm.close = function () {
            vm.dismiss({ $value: 'cancel' });
            
        };

    }
});