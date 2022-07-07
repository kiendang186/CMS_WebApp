$(document).ready(function () {

    var events = [];
    var selectedEvent = null;

    FetchEventAndRenderCalendar();

    function FetchEventAndRenderCalendar() {
        
        $.ajax({
            type: "GET",
            url: "/ad/calendar/GetEvents",
            success: function (result) {
                
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
            events: events,
            eventClick: function (calEvent, jsEvent, view) {                
                selectedEvent = calEvent;
                $('#myModal #eventTitle').text(calEvent.title);
                var $description = $('<div/>');
                $description.append($('<p/>').html('<b>Bắt đầu: </b>' + calEvent.start.format("DD/MM/YYYY HH:mm")));
                if (calEvent.end !== null) {
                    $description.append($('<p/>').html('<b>Kết thúc: </b>' + calEvent.end.format("DD/MM/YYYY HH:mm")));
                }
                $description.append($('<p/>').html('<b>Phòng: </b>' + calEvent.room));
                $description.append($('<p/>').html('<b>Địa điểm: </b>' + calEvent.location));
                $description.append($('<p/>').html('<b>Ghi chú: </b>' + calEvent.description));
                $('#myModal #pDetails').empty().html($description);

                $('#myModal').modal('toggle');
            }
        });
    }
    
});

function closeModal() {
    $('#myModal').modal('hide');
}