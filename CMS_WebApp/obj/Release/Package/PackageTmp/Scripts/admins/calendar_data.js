$(document).ready(function () {

    FetchEventAndRenderCalendar();

    function FetchEventAndRenderCalendar() {
        var events = [];
        $.ajax({
            type: "GET",
            url: "/ad/calendar/GetEvents",
            success: function (result) {
                console.log(result);
                $.each(result.Data, function (i, v) {
                    events.push({
                        eventID: v.Id,
                        title: v.Title,
                        description: v.Description,
                        start: moment(v.Start),
                        end: v.End !== null ? moment(v.End) : null,
                        color: v.ThemeColor,
                        allDay: v.IsFullDay,
                        roomid: v.RoomId,
                        room: v.Room,
                        locationid: v.LocationId,
                        location: v.Location
                    });
                });
                console.log(events);
                GenerateCalender(events);
            },
            error: function (xhr, status, error) {
                window.location.href = '/error';
            }
        })
    }

    function GenerateCalender(events) {
        $('#calendar').fullCalendar('destroy');
        $('#calendar').fullCalendar({
            contentHeight: 450,
            defaultDate: new Date(),
            timeFormat: 'h(:mm)a',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay,agenda'
            },
            eventLimit: true,
            eventColor: '#378006',
            events: events
        });
    }
});