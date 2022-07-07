$(document).ready(function () {
    var events = [];
    var selectedEvent = null;
    var locations = [];
    var rooms = [];
    initEvent();
    FetchEventAndRenderCalendar();
    function FetchEventAndRenderCalendar() {
        events = [];
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
                showErrorDialog();
            }
        })
    }

    function showErrorDialog() {
        $('#modal-default-error').modal({
            backdrop: 'static',
            keyboard: false
        });
    }

    function initEvent() {
        $('#selectLocation').change(function () {            
            fillRooms(parseInt($(this).val()));
        });
    }

    function GenerateCalender(events) {
        
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            contentHeight: 700,
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

                $('#myModal').modal();
            },
            selectable: true,
            select: function (start, end) {
                selectedEvent = {
                    eventID: 0,
                    title: '',
                    description: '',
                    start: start,
                    end: end,
                    allDay: false,
                    color: '',
                    roomid: 0,
                    room: '',
                    locationid: 0,
                    location: ''
                };
                openAddEditForm();
                $('#calendar').fullCalendar('unselect');
            }
        })
    }

    $('#btnEdit').click(function () {
        //Open modal dialog for edit event
        openAddEditForm();
    })
    $('#btnDelete').click(function () {
        if (selectedEvent !== null && confirm('Bạn chắc chắn muốn xóa lịch trình này?')) {
            $.ajax({
                type: "POST",
                url: '/ad/calendar/DeleteEvent',
                data: { 'eventID': selectedEvent.eventID },
                success: function (data) {
                    if (data.status) {
                        //Refresh the calender
                        FetchEventAndRenderCalendar();
                        $('#myModal').modal('hide');
                    }
                },
                error: function () {
                    $('#myModal').modal('hide');
                    showErrorDialog();
                }
            })
        }
    })

    $('#dtp1,#dtp2').datetimepicker({
        format: 'DD/MM/YYYY HH:mm'
    });

    $('#chkIsFullDay').change(function () {
        if ($(this).is(':checked')) {
            $('#divEndDate').hide();
        }
        else {
            $('#txtEnd-error').html('');
            $('#divEndDate').show();
        }
    });

    function clearError() {
        $('span[id$="error"]').html('');
    }

    function openAddEditForm() {

        clearError();

        $.ajax({
            type: "GET",
            url: '/ad/calendar/GetLocationRoom',            
            success: function (data) {
                if (data.status) {
                    initLocAndRoom(data);
                    fillEditData();
                }
            },
            error: function () {
                $('#myModal').modal('hide');
                showErrorDialog();
            }
        })
    }

    function initLocAndRoom(data) {
        locations = [];
        rooms = [];

        $.each(data.locations, function (i, e) {           
            locations.push({
                Id: e.Id,
                Name: e.Name
            });
        });

        $.each(data.rooms, function (i, e) {
            rooms.push({
                Id: e.Id,
                Name: e.Name,
                LocationId: e.LocationId
            });
        });
    }

    function fillLocation() {
        var locOptions = '';
        $.each(locations, function (i, e) {
            locOptions += '<option value="' + e.Id + '">' + e.Name + '</option>';
            locations.push({
                Id: e.Id,
                Name: e.Name
            });
        });
        $('#selectLocation').html(locOptions);
    }

    function fillRooms(locId) {
        
        var roomOptions = '';
        $.each(rooms, function (i, e) {            
            if (e.LocationId === locId) {
                roomOptions += '<option value="' + e.Id + '">' + e.Name + '</option>';
            }
        });

        $('#selectRoom').html(roomOptions);
    }

    function fillEditData() {
        fillLocation();        
        if (selectedEvent !== null) {
            $('#hdEventID').val(selectedEvent.eventID);
            $('#txtSubject').val(selectedEvent.title);
            $('#txtStart').val(selectedEvent.start.format('DD/MM/YYYY HH:mm'));
            $('#chkIsFullDay').prop("checked", selectedEvent.allDay || false);
            $('#chkIsFullDay').change();
            $('#txtEnd').val(selectedEvent.end !== null ? selectedEvent.end.format('DD/MM/YYYY HH:mm') : '');
            $('#txtDescription').val(selectedEvent.description);
            $('#ddThemeColor').val(selectedEvent.color);            
            if (selectedEvent.eventID != 0) {
                $('#selectLocation').val(selectedEvent.locationid);
                $('#selectLocation').trigger('change');
                $('#selectRoom').val(selectedEvent.roomid);
            }
            else {
                $("#selectLocation").val($("#selectLocation option:first").val());
                $("#selectLocation").trigger('change');
            }
        }
        
        $('#myModal').modal('hide');
        $('#myModalSave').modal();
    }

    $('#btnSave').click(function () {
        
        if ($('#txtSubject').val().trim() === "") {
            clearError();
            $('#txtSubject-error').html('Vui lòng nhập tiêu đề cho lịch');
            return;
        }
        if ($('#txtStart').val().trim() === "") {
            clearError();
            $('#txtStart-error').html('Vui lòng chọn thời gian bắt đầu');
            return;
        }
        if ($('#chkIsFullDay').is(':checked') === false && $('#txtEnd').val().trim() === "") {
            clearError();
            $('#txtEnd-error').html('Vui lòng chọn thời gian kết thúc');
            return;
        }
        else {
            var startDate = moment($('#txtStart').val(), "DD/MM/YYYY HH:mm").toDate();
            var endDate = moment($('#txtEnd').val(), "DD/MM/YYYY HH:mm").toDate();
            if (startDate > endDate) {
                clearError();
                $('#txtEnd-error').html('Thời gian kết thúc và bắt đầu không phù hợp');
                return;
            }
        }

        var data = {
            Id: $('#hdEventID').val(),
            Title: $('#txtSubject').val().trim(),
            Start: $('#txtStart').val().trim(),
            End: $('#chkIsFullDay').is(':checked') ? null : $('#txtEnd').val().trim(),
            Description: $('#txtDescription').val(),
            ThemeColor: $('#ddThemeColor').val(),
            IsFullDay: $('#chkIsFullDay').is(':checked'),
            RoomId: $('#selectRoom').val()
        }

        SaveEvent(data);
        // call function for submit data to the server 
    })

    function SaveEvent(data) {
        $.ajax({
            type: "POST",
            url: '/ad/calendar/SaveEvent',
            data: data,
            success: function (data) {

                if (data.status) {
                    //Refresh the calender
                    FetchEventAndRenderCalendar();                    
                }

                $('#myModalSave').modal('hide');
                clearError();
                showNofityMessage(data.message);
            },
            error: function () {
                showErrorDialog();
            }
        })
    }
})
