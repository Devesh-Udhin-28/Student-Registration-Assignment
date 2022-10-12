var number = 0;

$(function () {
    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    $("#confirm").click(function () {

        if (IsValid()) {
            $.fn.registerSubjects();
        }
        
    });

});

$.fn.registerSubjects = function () {
    var Numcheckboxes = document.querySelectorAll('input[type="checkbox"]:checked');

    var subjectsArray = new Array(3);
    var pointsArray = new Array(3);
    var userId = $("#userid").val();

    for (var i = 0; i < Numcheckboxes.length; i++) {

        var subID = Numcheckboxes.item(i).id;

        //var subName = $("#" + subID).val();
        var subPoint = $("select#" + subID + " option:selected").val();

        //alert(subName);
        //alert(subPoint);

        subjectsArray[i] = subID;
        pointsArray[i] = subPoint;

    }

    var authObj = {
        SubjectArray: subjectsArray,
        PointsArray: pointsArray,
        StudentIdentityNumber: userId
    };
    $.ajax({
        type: "POST",
        url: "/Portal/RegisterSubjects",
        data: authObj,
        dataType: "json",
        success: function (response) {
            if (response.result) {
                toastr.success("Subjects Registered Successfully...Please wait for your confirmation email");
                window.location = response.url;
            }
            else {
                if (!response.canRegisterSubjects) {
                    toastr.error('You cannot register more than 3 Subjects...you already have ' + response.numOfSubjects + ' registered in the database, please ajust your choice');
                    return false;
                }
                toastr.error('Unable to Register Subjects');
                return false;
            }
        },
        failure: function (response) {
            toastr.error('Unable to make request!!');
        },
        error: function (response) {
            toastr.error('Something happen, Please contact Administrator!!');

        }
    });
}

function IsValid() {

    let formData = document.forms["applyForm"];

    if (formData) {

        var alertMessage = "";
        var newLine = "\r\n"
        var numberOfAllowedOptions = 3;

        let Numcheckboxes = document.querySelectorAll('input[type="checkbox"]:checked');

        if (Numcheckboxes.length > 3) {
            alertMessage += concatMessage("Please do not check more than " + numberOfAllowedOptions + " options<br>");
            alertMessage += newLine;
        }
        else if (Numcheckboxes.length == 0) {
            alertMessage += concatMessage("Please select at least one option<br>");
            alertMessage += newLine;
        }

        if (alertMessage != "") {
            document.getElementById("alertMessage").innerHTML = alertMessage;
            document.getElementById("alertBlock").classList.add("display");
            document.getElementById("alertBlock").classList.remove("hide");
            // document.getElementById("alertBlock").setAttribute('class', 'display');
            return false;
        }
        else {
            return true
        }

    }

}

function closeDialogBox() {
    document.getElementById("alertMessage").innerHTML = "";
    document.getElementById("alertBlock").classList.add("hide");
    document.getElementById("alertBlock").classList.remove("display");
    this.number = 0;
    $.fn.UncheckAllCheckBoxes();

    return false;
}

function concatMessage(message) {

    this.number++;
    return this.number + ". " + message;

}

$.fn.UncheckAllCheckBoxes = function () {
    $("input[type='checkbox']").prop("checked", false);
}