var number = 0;

$(function () {

    $("#dob").blur(function () {
        var alertMessageDob = "";
        let date = new Date("01/01/2004");
        let Dob = $("#dob").val();
        let SpecifiedDate = new Date(Dob);

        if (SpecifiedDate.getFullYear() > date.getFullYear()) {
            alertMessageDob += "You must be at least 18 year old<br>";
        }

        if (alertMessageDob != "") {
            document.getElementById("errDob").innerHTML = alertMessageDob;
            document.getElementById("errDob").focus();
        }
    });

    let form = document.querySelector('form');

    form.addEventListener('submit', (e) => {
        e.preventDefault();
        return false;
    });

    $("#Register").click(function () {

        $.fn.ClearAllErrorsRegister();

        if ($.fn.IsRegisterValid()) {
            var nid = $("#nid").val();
            var name = $("#name").val();
            var surname = $("#surname").val();
            var age = $("#age").val()
            var address = $("#address").val();
            var guardian = $("#guardian").val();
            var country = $("#country").val();
            var phone = $("#phone").val();
            var dob = $("#dob").val();
            var emailAddress = $("#email").val();
            var password = $("#passwd").val();
            var roleID = 3;

            var authObj = {
                StudentIdentityNumber: nid,
                Name: name,
                Surname: surname,
                Age: age,
                Address: address,
                PhoneNumber: phone,
                GuardianName: guardian,
                DateOfBirth: dob,
                Country: country,
                EmailAddress: emailAddress,
                Password: password,
                RoleID: roleID
            };

            $.ajax({
                type: "POST",
                url: "/Login/RegisterStudent",
                data: authObj,
                dataType: "json",
                success: function (response) {
                    if (response.result) {
                        toastr.success("Authentication Succeed. Redirecting to relevent page.....");
                        window.location = response.url;
                    }
                    else {
                        if (response.studentIDExists) {
                            toastr.error('Looks like the ID you inserted is not unique...Please verify your ID and try again');
                            $("#nid").focus();
                            return false
                        }
                        else if (response.studentExists) {
                            toastr.error('The Email you entered already exists in the dababase...Please choose another one');
                            $("#email").focus();
                            return false
                        }
                        toastr.error('Unable to Authenticate user');
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
    });

    $("#sign-in").click(function () {

        $.fn.ClearAllErrorsSignIn();

        if ($.fn.IsSignInValid()) {
            var emailAddress = $("#emailAddress").val();
            var password = $("#passwd").val();

            var authObj = { EmailAddress: emailAddress, Password: password };
            $.ajax({
                type: "POST",
                url: "/Login/Authenticate",
                data: authObj,
                dataType: "json",
                success: function (response) {
                    if (response.result) {
                        toastr.success("Authentication Succeed. Redirecting to relevent page.....");
                        window.location = response.url;
                    }
                    else {
                        toastr.error('Unable to Authenticate user');
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
    });
});


$.fn.IsSignInValid = function () {
    let formData = document.forms["sign-in"];

    if (formData) {

        var alertMessageEmail = "";
        var alertMessagePasswd = "";
        var newLine = "\r\n"

        let Email = formData.emailAddress.value;

        var regexemail = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;

        if (Email == "") {
            alertMessageEmail += "Please Enter an Email Address<br>";
            alertMessageEmail += newLine;
        }
        else if (!regexemail.test(Email)) {

            alertMessageEmail += "Email is not valid<br>";
            alertMessageEmail += newLine;

        }

        let Passwd = formData.passwd.value;

        if (Passwd == "") {
            alertMessagePasswd += "Please Enter a Password<br>";
            alertMessagePasswd += newLine;
        }
        

        if (alertMessageEmail != "" || alertMessagePasswd != "") {

            if (alertMessageEmail != "") {
                document.getElementById("errEmail").innerHTML = alertMessageEmail;
                // document.getElementById("alertBlock").setAttribute('class', 'display');
                document.getElementById("emailAddress").focus();
            }

            if (alertMessagePasswd != "") {
                document.getElementById("errPasswd").innerHTML = alertMessagePasswd;
                document.getElementById("passwd").focus();
            }

            if (alertMessageEmail != "" && alertMessagePasswd != "") {
                $.fn.ClearAllErrors();
            }

            return false;
        }

        return true;

    }

}

$.fn.IsRegisterValid = function () {
    let formData = document.forms["register"];

    if (formData) {

        var alertMessageNid = "";
        var alertMessageName = "";
        var alertMessageSurname = "";
        var alertMessageAge = "";
        var alertMessageDob = "";
        var alertMessageGuardian = "";
        var alertMessageCountry = "";
        var alertMessageAddress = "";
        var alertMessagePhone = "";
        var alertMessageEmail = "";
        var alertMessagePasswd = "";
        var alertMessagePasswdConfirm = "";
        var newLine = "\r\n"

        let Nid = formData.nid.value;

        if (Nid == "") {
            alertMessageNid += "Please Enter your identity number<br>";
            alertMessageNid += newLine;
            this.number++;
        }

        let Name = formData.name.value;

        if (Name == "") {
            alertMessageName += "Please Enter your Name<br>";
            alertMessageName += newLine;
            this.number++;
        }

        let Surname = formData.surname.value;

        if (Surname == "") {
            alertMessageSurname += "Please Enter your Surname<br>";
            alertMessageSurname += newLine;
            this.number++;
        }

        let Age = formData.age.value;

        if (Age == "") {
            alertMessageAge += "Please Enter your age<br>";
            alertMessageAge += newLine;
            this.number++;
        }

        let Dob = formData.dob.value;

        if (Dob == "") {
            alertMessageDob += "Please Enter your Date of Birth<br>";
            alertMessageDob += newLine;
            this.number++;
        }

        let Guardian = formData.guardian.value;

        if (Guardian == "") {
            alertMessageGuardian += "Please Enter your Guardian's name<br>";
            alertMessageGuardian += newLine;
            this.number++;
        }

        let Country = formData.country.value;

        if (Country == "") {
            alertMessageCountry += "Please Enter your country<br>";
            alertMessageCountry += newLine;
            this.number++;
        }

        let Address = formData.address.value;

        if (Address == "") {
            alertMessageAddress += "Please Enter your Address<br>";
            alertMessageAddress += newLine;
            this.number++;
        }

        let Phone = formData.phone.value;

        if (Phone == "") {
            alertMessagePhone += "Please Enter your Phone number<br>";
            alertMessagePhone += newLine;
            this.number++;
        }

        let Email = formData.email.value;

        var regexemail = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;

        if (Email == "") {
            alertMessageEmail += "Please Enter an Email Address<br>";
            alertMessageEmail += newLine;
            this.number++;
        }
        else if (!regexemail.test(Email)) {
            alertMessageEmail += "Email is not valid<br>";
            alertMessageEmail += newLine;
            this.number++;
        }

        let Passwd = formData.passwd.value;

        if (Passwd == "") {
            alertMessagePasswd += "Please Enter a Password<br>";
            alertMessagePasswd += newLine;
            this.number++;
        }

        let PasswdConfirm = formData.passwdConfirm.value;

        if (Passwd != PasswdConfirm) {
            alertMessagePasswdConfirm += "Your Confirmed Password is not the same as the provided Password<br>";
            alertMessagePasswdConfirm += newLine;
            this.number++;
        }


        if (alertMessageNid != "" || alertMessageName != "" || alertMessageSurname != "" || alertMessageAge != "" || alertMessageDob != "" || alertMessageGuardian != "" || alertMessageCountry != "" || alertMessageAddress != "" || alertMessagePhone != ""  || alertMessageEmail != "" || alertMessagePasswd != "" || alertMessagePasswdConfirm != "") {

            if (alertMessageNid != "") {
                document.getElementById("errNid").innerHTML = alertMessageNid;
                document.getElementById("errNid").focus();
            }

            if (alertMessageName != "") {
                document.getElementById("errName").innerHTML = alertMessageName;
                document.getElementById("errName").focus();
            }

            if (alertMessageSurname != "") {
                document.getElementById("errSurname").innerHTML = alertMessageSurname;
                document.getElementById("errSurname").focus();
            }

            if (alertMessageAge != "") {
                document.getElementById("errAge").innerHTML = alertMessageAge;
                document.getElementById("errAge").focus();
            }

            if (alertMessageDob != "") {
                document.getElementById("errDob").innerHTML = alertMessageDob;
                document.getElementById("errDob").focus();
            }

            if (alertMessageGuardian != "") {
                document.getElementById("errGuardian").innerHTML = alertMessageGuardian;
                document.getElementById("errGuardian").focus();
            }

            if (alertMessageCountry != "") {
                document.getElementById("errCountry").innerHTML = alertMessageCountry;
                document.getElementById("errCountry").focus();
            }

            if (alertMessageAddress != "") {
                document.getElementById("errAddress").innerHTML = alertMessageAddress;
                document.getElementById("errAddress").focus();
            }

            if (alertMessagePhone != "") {
                document.getElementById("errPhone").innerHTML = alertMessagePhone;
                document.getElementById("errPhone").focus();
            }


            if (alertMessageEmail != "") {
                document.getElementById("errEmail").innerHTML = alertMessageEmail;
                // document.getElementById("alertBlock").setAttribute('class', 'display');
                document.getElementById("email").focus();
            }

            if (alertMessagePasswd != "") {
                document.getElementById("errPasswd").innerHTML = alertMessagePasswd;
                document.getElementById("passwd").focus();
            }

            if (alertMessagePasswdConfirm != "") {
                document.getElementById("errPasswdConfirm").innerHTML = alertMessagePasswdConfirm;
                document.getElementById("errPasswdConfirm").focus();
            }

            if (this.number > 1) {
                $.fn.ClearAllErrors();
            }

            return false;
        }

        return true;

    }

}

$.fn.ClearAllErrorsRegister = function () {
    document.getElementById("errNid").innerHTML = "";
    document.getElementById("errName").innerHTML = "";
    document.getElementById("errSurname").innerHTML = "";
    document.getElementById("errAge").innerHTML = "";
    document.getElementById("errDob").innerHTML = "";
    document.getElementById("errGuardian").innerHTML = "";
    document.getElementById("errCountry").innerHTML = "";
    document.getElementById("errAddress").innerHTML = "";
    document.getElementById("errPhone").innerHTML = "";
    document.getElementById("errEmail").innerHTML = "";
    document.getElementById("errPasswd").innerHTML = "";
    document.getElementById("errPasswdConfirm").innerHTML = "";
}

$.fn.ClearAllErrorsSignIn = function () {
    document.getElementById("errEmail").innerHTML = "";
    document.getElementById("errPasswd").innerHTML = "";
}