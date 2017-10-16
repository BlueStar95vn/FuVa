angular.
    module('report').
    component('report', {
        templateUrl: "/app/report/report.html",
        controller: function ($compile, $timeout, uiCalendarConfig) {
            var vm = this;

            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();
            /* config object */

            vm.events = [
                { id: 929, title: 'All Day Event', start: new Date(y, m, 1) },
                { id: 939, title: 'Long Event', start: new Date(y, m, d - 5), end: new Date(y, m, d - 2) },
                { id: 949, title: 'Repeating Event', start: new Date(y, m, d - 3, 16, 0), allDay: true },
                { id: 959, title: 'Repeating Event', start: new Date(y, m, d + 4, 16, 0), allDay: true },
                { id: 969, title: 'Birthday Party', start: new Date(y, m, d + 1, 19, 0), end: new Date(y, m, d + 1, 22, 30), allDay: false },
                { id: 979, title: 'Click for Google', start: new Date(y, m, 28), end: new Date(y, m, 29), url: 'http://google.com/' }
            ];
            vm.eventSources = [vm.events];
            /* event source that calls a function on every view switch */
            vm.eventsF = function (start, end, timezone, callback) {
                var s = new Date(start).getTime() / 1000;
                var e = new Date(end).getTime() / 1000;
                var m = new Date(start).getMonth();
                var events = [{ title: 'Feed Me ' + m, start: s + (50000), end: s + (100000), allDay: false, className: ['customFeed'] }];
                callback(events);
            };
            vm.filterEvent = function () {

            }
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
                        right: 'month,listMonth'
                    },
                    eventClick: vm.alertOnEventClick,
                    eventDrop: vm.alertOnDrop,
                    eventResize: vm.alertOnResize,
                    eventRender: vm.eventRender,

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

            /* event sources array*/
            //vm.eventSources = [vm.events, vm.eventSource, vm.eventsF];
            //vm.eventSources = [vm.calEventsExt, vm.eventsF, vm.events];

        }
        
    });