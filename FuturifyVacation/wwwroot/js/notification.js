$(document).ready(function () {
    $("#messageLink").click(function () {
        $("#notificationContainer").hide();
        $("#messageContainer").fadeToggle(300);
        $("#message_count").fadeOut("slow");
        return false;
    });

    $("#notificationLink").click(function () {
        $("#messageContainer").hide();
        $("#notificationContainer").fadeToggle(300);
        $("#notification_count").fadeOut("slow");
        return false;
    });
    //Document Click hiding the popup 
    $(document).click(function () {
        $("#notificationContainer").hide();
        $("#messageContainer").hide();
    });

    //Popup on click
    $("#notificationContainer").click(function () {
        return false;
    });

    $("#messageContainer").click(function () {
        return false;
    });
});