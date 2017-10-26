angular.
    module('vacation').
    component('vacation', {
        templateUrl: "/app/vacation/vacation.html",
        controller: function ($compile, $timeout, uiCalendarConfig, $http, $location) {
            var vm = this;

            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();
            
            
           
            vm.events = [ 
                { id: 929, title: 'All Day Event', start: new Date(y, m, 1) },
                { id: 939, title: 'Long Event', start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) },
                { id: 949, title: 'Repeating Event', start: "2017-10-25T02:00:08.624Z", end: "2017-10-25 11:00:08.6250000", allDay: false, userId: "12121" },
                { id: 959, title: 'Repeating Event', start: new Date(y, m, d + 4, 16, 0), allDay: true },
                { "id": 6, "userId": "85d0a97e-4bd0-437b-b469-815f0b5ee8a7", "user": null, "title": null, "start": "0001-01-01T00:00:00Z", "end": "0001-01-01T00:00:00Z" },
                { "id": 7, "userId": "85d0a97e-4bd0-437b-b469-815f0b5ee8a7", "user": null, "title": "ABC", "start": "2017-10-26T02:00:39.408Z", "end": "2017-10-26T11:00:39.408Z" },
                { "id": 8, "userId": "85d0a97e-4bd0-437b-b469-815f0b5ee8a7", "user": null, "title": "XYZ", "start": "2017-10-27T02:00:16.605Z", "end": "2017-10-30T11:00:16.605Z" },
                { id: 969, title: 'Birthday Party', start: new Date(y, m, d + 1, 19, 0), end: new Date(y, m, d + 1, 22, 30), allDay: false },
                { id: 979, title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
            ];
            vm.eventSources = [];
            vm.vacations = {};
            $http.get("http://localhost:63237/api/vacations/getallvacation").then(function (response) {
                vm.vacations = response.data;
                uiCalendarConfig.calendars['myCalendar'].fullCalendar('addEventSource', vm.vacations);
                
            }).catch(function (err) {
                console.log(err);
            });
           
            vm.eventSources = [vm.vacations];
           
            /* event source that calls a function on every view switch */
            vm.eventsF = function (start, end, timezone, callback) {
                var s = new Date(start).getTime() / 1000;
                var e = new Date(end).getTime() / 1000;
                var m = new Date(start).getMonth();
                var events = [{ title: 'Feed Me ' + m, start: s + (50000), end: s + (100000), allDay: false, className: ['customFeed'] }];
                callback(events);
            };

            //vm.calEventsExt = {
            //    color: '#f00',
            //    textColor: 'yellow',
            //    events: [
            //        { type: 'party', title: 'Lunch', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
            //        { type: 'party', title: 'Lunch 2', start: new Date(y, m, d, 12, 0), end: new Date(y, m, d, 14, 0), allDay: false },
            //        { type: 'party', title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
            //    ]
            //};

            /* alert on eventClick */
            vm.alertOnEventClick = function (date, jsEvent, view) {
                vm.alertMessage = (date.title + ' was clicked ');
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
                    height: 600,
                    editable: true,
                    navLinks: true,
                    selectable: true,
                    selectHelper: true,
                    eventLimit: true,

                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,agendaWeek,agendaDay,listMonth'
                    },
                    eventClick: vm.alertOnEventClick,
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
            vm.mstep = 30;
         

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