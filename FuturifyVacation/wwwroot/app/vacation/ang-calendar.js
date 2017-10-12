$(document).ready(function () {
    var date = new Date();
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,listMonth'
        },
        defaultDate: date,
        navLinks: true, // can click day/week names to navigate views
        selectable: true,
        selectHelper: true,
        select: function (start, end) {
            var title = prompt('Reason:');
            var eventData;
            if (title) {
                var id = title + start;
                eventData = {
                    id: id,
                    title: title,
                    start: start,
                    end: end
                };
                $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true
            }
            $('#calendar').fullCalendar('unselect');
        },
        editable: true,
        eventLimit: true, // allow "more" link when too many events
        eventRender: function (event, element, view) {
            if (view.name == 'listDay') {
                element.find(".fc-list-item-time").append("<span class='closeon'>X</span>");
            } else {
                element.find(".fc-content").prepend("<span class='closeon'>X</span>");
            }
            element.find(".closeon").on('click', function () {
                $('#calendar').fullCalendar('removeEvents', event.id);
                console.log('delete');
            });
        },
        events: [
            {
                id: 412,
                title: 'All Day Event',
                start: '2017-10-01'
            },
            {
                id: 544,
                title: 'Long Event',
                start: '2017-09-07',
                end: '2017-10-10'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: '2017-10-09T16:00:00'
            },
            {
                id: 9929,
                title: 'Repeating Event',
                start: '2017-10-16T16:00:00'
            },
            {
                id: 463,
                title: 'Conference',
                start: '2017-10-11',
                end: '2017-10-13'
            },
            {
                id: 643,
                title: 'Meeting',
                start: '2017-10-12T10:30:00',
                end: '2017-10-12T12:30:00'
            },
            {
                id: 135,
                title: 'Click for Google',
                //url: 'http://google.com/',
                start: '2017-09-28'
            }
        ],

        //event Color CSS
        eventColor: '#009966',
        eventTextColor: 'FFFF99',
        businessHours: {
            dow: [1, 2, 3, 4, 5], // Monday - Friday

            start: '09:00', // a start time 
            end: '18:00', // an end time 
        },
        themeSystem: 'jquery-ui'


    });


});