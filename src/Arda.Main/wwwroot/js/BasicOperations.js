// Functions with automatic initialization
$(function ($) {
    // Loading datatable to fiscal years.
    $("#table-fiscalyears").DataTable({
        "sAjaxSource": "/FiscalYear/ListAllFiscalYears",
        "columnDefs": [
            {
                "width": "33%", "targets": 2,
                "orderable": false
            }
        ],
        "ordering": false
    });

    //Loading datatable to all appointments.
    $("#table-appointments-all").DataTable({
        "sAjaxSource": "/Appointment/ListAllAppointments",
        "columns": [
            { "width": "30%" },
            { "width": "10%" },
            { "width": "10%" },
            { "width": "5%" },
            { "width": "30%" }
        ],
        "columnDefs": [
            {
                "orderable": false
            }
        ],
        "ordering": false
    });

    //Loading datatable to my appointments.
    $("#table-appointments-my").DataTable({
        "sAjaxSource": "/Appointment/ListMyAppointments",
        "columns": [
            { "width": "30%", "type":"string" },
            { "width": "20%", "type": "date" },
            { "width": "15%", "type": "integer" },
            { "width": "35%" }
        ],
        "columnDefs": [
            {
                "orderable": true
            }
        ],
        "ordering": false
    });

    // Send the new account request to specific controller/action in Arda.Main.
    $("#loginform").submit(function (e) {
        e.preventDefault();

        $("#email").attr("disabled", "disabled");
        $("#password").attr("disabled", "disabled");
        $("#signin").attr("disabled", "disabled");

        $("#signin").text("Validating user data...");

        var email = $("#email").val();
        var password = $("#password").val();

        $.ajax({
            url: "http://localhost:2787/api/authentication/userauthentication",
            type: "POST",
            data: { Email: email, Password: password },
            dataType: "json"
        }).done(function (data) {
            if (data.Status == "Ok") {
                ClearModalForm();
                RedirectTo("http://localhost:2168/Dashboard/Index");
            }
            else if (data.Status == "Inactive") {
                $("#MessagePanelLogin").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> The requested user is here but is inactive. Please, consult the system admin.</div>");
                $("#signin").html("Sign in");
                ClearModalForm();
                $("#email").removeAttr("disabled");
                $("#password").removeAttr("disabled");
                $("#signin").removeAttr("disabled");

            }
            else {
                $("#MessagePanelLogin").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Ops!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#signin").html("Sign in");
                ClearModalForm();
                $("#email").removeAttr("disabled");
                $("#password").removeAttr("disabled");
                $("#signin").removeAttr("disabled");
            }
        }).fail(function (e, f) {
            $("#MessagePanelLogin").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Ops!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
            $("#signin").html("Sign in");
            ClearModalForm();
            $("#email").removeAttr("disabled");
            $("#password").removeAttr("disabled");
            $("#signin").removeAttr("disabled");
        });

    });

    // Send the new account request to specific controller/action in Arda.Main.
    $("#NewAccountRequest").submit(function (e) {
        e.preventDefault();

        $("#RequestAccountButton").attr("disabled", "disabled");
        $("#RequestAccountButton").text("Requesting...");

        var pName = $("#Name").val();
        var pEmail = $("#Email").val();
        var pPhone = $("#Phone").val();
        var pJustification = $("#Justification").val();

        $.ajax({
            url: "http://localhost:2787/api/accountoperations/requestnewaccount",
            type: "POST",
            data: { Name: pName, Email: pEmail, Phone: pPhone, Justification: pJustification },
            dataType: "json"
        }).done(function (data) {
            if (data.Status == "Ok") {
                $("#MessagePanel").html("<div class='alert alert-success'><strong>Success!</strong> Your request was sent. Thank you.</div>");
                $("#RequestAccountButton").removeAttr("disabled");
                $("#RequestAccountButton").html("<span class='glyphicon glyphicon-ok'></span>&nbsp;Request account");
                ClearModalForm();
            }
        }).fail(function (e, f) {
            $("#MessagePanel").html("<div class='alert alert-danger'><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
            $("#RequestAccountButton").html("<span class='glyphicon glyphicon-ok'></span>&nbsp;Request account");
        });

    });

    // Send the help request to specific controller/action in Arda.Main.
    $("#RadioGroup").submit(function (e) {
        e.preventDefault();

        $("#SendHelpRequest").attr("disabled", "disabled");
        $("#SendHelpRequest").text("Requesting...");

        var Value;

        if ($("input[name='radiooption']:checked").val() == "1") {
            Value = $("#YourCompleteName").val();
        }
        else if ($("input[name='radiooption']:checked").val() == "2") {
            Value = $("#YourEmail").val();
        }
        else {
            Value = $("#YourDescription").val();
        }

        // Sending the help request
        $.ajax({
            url: "http://localhost:2787/api/accountoperations/requesthelp",
            type: "POST",
            data: { RequestType: Value },
            dataType: "json"
        }).done(function (data) {
            if (data.Status == "Ok") {
                $("#MessagePanel").html("<div class='alert alert-success'><strong>Success!</strong> Your help request was sent. Thank you.</div>");
                $("#SendHelpRequest").removeAttr("disabled");
                $("#SendHelpRequest").html("<span class='glyphicon glyphicon-ok'></span>&nbsp;Send help request");
                ClearModalForm();
            }
            else {
                $("#MessagePanel").html("<div class='alert alert-danger'><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#SendHelpRequest").removeAttr("disabled");
                $("#SendHelpRequest").html("<span class='glyphicon glyphicon-ok'></span>&nbsp;Send help request");
            }
        }).fail(function () {
            $("#MessagePanel").html("<div class='alert alert-danger'><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
            $("#RequestAccountButton").html("<span class='glyphicon glyphicon-ok'></span>&nbsp;Send help request");
        });
    });

    $("#form-edit-fiscal-year").validate({
        rules: {
            FiscalYearID: "required",
            TextualFiscalYearMain: "required",
            FullNumericFiscalYearMain: "required"
        },
        messages: {
            FiscalYearID: "Please, inform the fiscal year code.",
            TextualFiscalYearMain: "Please, type the textual form of fiscal year.",
            FullNumericFiscalYearMain: "Please, inform the numeric form of fiscal year."
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            DisableFiscalYearFields();
            $("#btnUpdate").text("Updating fiscal year data...");

            $.ajax({
                url: "/FiscalYear/EditFiscalYear",
                type: "POST",
                data: $(form).serialize()
            }).done(function (data) {
                if (data.Status == "Ok") {
                    ClearModalForm();
                    $("#message").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> The fiscal year was updated succefully.</div>");
                    $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableFiscalYearFields();
                    RedirectIn(3000, "/FiscalYear/Index");
                }
                else {
                    $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Ops!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                    $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableFiscalYearFields();
                }
            }).fail(function (e, f) {
                $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Ops!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                EnableFiscalYearFields();
            });
        }

    });

    $("#form-add-fiscal-year").validate({
        rules: {
            FiscalYearID: "required",
            TextualFiscalYearMain: "required",
            FullNumericFiscalYearMain: "required"
        },
        messages: {
            FiscalYearID: "Please, inform the fiscal year code.",
            TextualFiscalYearMain: "Please, type the textual form of fiscal year.",
            FullNumericFiscalYearMain: "Please, inform the numeric form of fiscal year."
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            DisableFiscalYearFields();
            $("#btnAdd").text("Saving the new fiscal year...");

            $.ajax({
                url: "/FiscalYear/AddFiscalYear",
                type: "POST",
                data: $(form).serialize()
            }).done(function (data) {
                if (data.Status == "Ok") {
                    ClearModalForm();
                    $("#message").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> Fiscal year has been added into Arda.</div>");
                    $("#btnAdd").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableFiscalYearFields();

                    RedirectIn(3000, "/FiscalYear/Index");
                }
                else {
                    $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                    $("#btnAdd").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableFiscalYearFields();
                }
            }).fail(function (e, f) {
                $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#btnAdd").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                EnableFiscalYearFields();
            });
        }
    });

    $("#form-edit-metric").validate({
        rules: {
            MetricID: "required",
            FiscalYearID: "required",
            MetricCategory: "required",
            MetricName: "required",
            Description: "required"
        },
        messages: {
            MetricID: "Please, inform the Metric code.",
            FiscalYearID: "Please, select a fiscal year.",
            MetricCategory: "Please, type or choose a category.",
            MetricName: "Please, type a name.",
            Description: "Please, type a description."
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            DisableMetricFields();
            $("#btnUpdate").text("Updating metric data...");

            $.ajax({
                url: "/Metric/EditMetric",
                type: "PUT",
                data: $(form).serialize()
            }).done(function (data) {
                if (data.IsSuccessStatusCode) {
                    $("#message").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> The metric was updated succefully.</div>");
                    $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    //EnableFiscalYearFields();

                    RedirectIn(3000, "/Metric/Index");
                }
                else {
                    $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Ops!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                    $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableFiscalYearFields();
                }
            }).fail(function (e, f) {
                $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Ops!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                EnableMetricFields();
            });
        }
    });

    $("#form-add-metric").validate({
        rules: {
            MetricID: "required",
            FiscalYearID: "required",
            MetricCategory: "required",
            MetricName: "required",
            Description: "required"
        },
        messages: {
            MetricID: "Please, inform the Metric code.",
            FiscalYearID: "Please, select a fiscal year.",
            MetricCategory: "Please, type or choose a category.",
            MetricName: "Please, type a name.",
            Description: "Please, type a description."
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            DisableMetricFields();
            $("#btnAdd").text("Saving the new metric...");

            $.ajax({
                url: "/Metric/AddMetric",
                type: "POST",
                data: $(form).serialize()
            }).done(function (data) {
                if (data.IsSuccessStatusCode) {
                    $("#message").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> The metric has been added into Arda.</div>");
                    $("#btnAdd").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    //EnableFiscalYearFields();

                    RedirectIn(3000, "/Metric/Index");
                }
                else {
                    $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                    $("#btnAdd").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableFiscalYearFields();
                }
            }).fail(function (e, f) {
                $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#btnAdd").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                EnableMetricFields();
            });
        }
    });

    $("#form-add-appointment").validate({
        rules: {
            _AppointmentID: "required",
            _AppointmentUserName: "required",
            _WorkloadTitle: "required",
            _AppointmentDate: "required",
            _AppointmentHoursDispensed: "required"
        },
        messages: {
            _AppointmentID: "Sorry but, we need appointment code.",
            _AppointmentUserName: "Ops! Who is doind this appointment? Mandatory info.",
            _WorkloadTitle: "Ops! You must type the workload. The system will find the occurrency in database.",
            _AppointmentDate: "Ops! Date is mandatory.",
            _AppointmentHoursDispensed: "Ops! Hours dispensed is mandatory."
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            UpdateCKEditor();
            DisableAppointmentFields();
            $("#btnAddAppointment").text("Saving appointment...");

            var data = $(form).serialize();
            console.log(data);
            //data.set('', 123);

            $.ajax({
                url: "/Appointment/AddAppointment",
                type: "POST",
                data: data,
            }).done(function (data) {
                if (data.IsSuccessStatusCode) {
                    $("#message").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> The appointment has been added into Arda.</div>");
                    $("#btnAddAppointment").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    RedirectIn(3000, "/Appointment/My");
                }
                else {
                    $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                    $("#btnAddAppointment").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableAppointmentFields();
                }
            }).fail(function (e, f) {
                $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#btnAddAppointment").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                EnableAppointmentFields();
            });
        }
    });


    $("#form-simpleadd-appointment").validate({
        rules: {
            _AppointmentID: "required",
            _AppointmentUserName: "required",
            _WorkloadTitle: "required",
            _AppointmentDate: "required",
            _AppointmentHoursDispensed: "required"
        },
        messages: {
            _AppointmentID: "Sorry but, we need appointment code.",
            _AppointmentUserName: "Ops! Who is doind this appointment? Mandatory info.",
            _WorkloadTitle: "Ops! You must type the workload. The system will find the occurrency in database.",
            _AppointmentDate: "Ops! Date is mandatory.",
            _AppointmentHoursDispensed: "Ops! Hours dispensed is mandatory."
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            UpdateCKEditor();
            DisableAppointmentFields();
            $("#btnAddAppointment").text("Saving appointment...");

            var data = $(form).serialize();
            console.log(data);
            //data.set('', 123);

            $.ajax({
                url: "/Appointment/AddAppointment",
                type: "POST",
                data: data,
            }).done(function (data) {
                if (data.IsSuccessStatusCode) {
                    $("#message").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> The appointment has been added into Arda.</div>");
                    // $("#btnAddAppointment").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    // RedirectIn(3000, "/Appointment/My");
                    window.close();
                }
                else {
                    $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                    $("#btnAddAppointment").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableAppointmentFields();
                }
            }).fail(function (e, f) {
                $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#btnAddAppointment").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                EnableAppointmentFields();
            });
        }
    });

    $("#form-edit-appointment").validate({
        rules: {
            _AppointmentID: "required",
            _AppointmentUserName: "required",
            _WorkloadTitle: "required",
            _AppointmentDate: "required",
            _AppointmentHoursDispensed: "required"
        },
        messages: {
            _AppointmentID: "Sorry but, we need appointment code.",
            _AppointmentUserName: "Ops! Who is doind this appointment? Mandatory info.",
            _WorkloadTitle: "Ops! You must type the workload. The system will find the occurrency in database.",
            _AppointmentDate: "Ops! Date is mandatory.",
            _AppointmentHoursDispensed: "Ops! Hours dispensed is mandatory."
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        submitHandler: function (form) {
            UpdateCKEditor();
            DisableAppointmentFields();
            $("#btnUpdate").text("Updating appointment...");

            var data = $(form).serialize();

            $.ajax({
                url: "/Appointment/EditAppointment",
                type: "PUT",
                data: data,
            }).done(function (data) {
                if (data.IsSuccessStatusCode) {
                    $("#message").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> The appointment has been updated in Arda.</div>");
                    $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    RedirectIn(3000, "/Appointment/My");
                }
                else {
                    $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                    $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                    EnableAppointmentFields();
                }
            }).fail(function (e, f) {
                $("#message").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> Something wrong happened with your request. Try again in few minutes.</div>");
                $("#btnUpdate").html("<i class='fa fa-floppy-o' aria-hidden='true'></i> Save");
                EnableAppointmentFields();
            });
        }
    });

    // Calling workloads by current user
    GetWorkloadsByUser();

    // Apply mask money to appointment screen
    LoadMaskMoney();

    // Load CKEditor
    LoadCKEditor();

    // Load DatePicker
    LoadDatePicker();
});
// General functions

// Modais

function MountNeedHelpModal() {
    //Defining values
    var ModalTitle = "How can we help?";
    var ModalBody = "<p style='margin-bottom:20px;' class='p-modal-body'>What's happening? Please, select the best option below.</p>";
    ModalBody += "<p class='p-modal-body'>";
    ModalBody += "<div>";
    ModalBody += "<div class='radio'>";
    ModalBody += "<label><input type='radio' name='radiooption' id='RadioEmail' value='1' onchange='CheckRadio();'>I'm having problems with my email (manual process)</label>";
    ModalBody += "</div>";
    ModalBody += "<div id='YourCompleteNameField'></div>"
    ModalBody += "<div class='radio'>";
    ModalBody += "<label><input type='radio' name='radiooption' id='RadioPassword' value='2' onchange='CheckRadio();'>I'm having problems with my password (automatic process)</label>";
    ModalBody += "</div>";
    ModalBody += "<div id='YourEmailField'></div>"
    ModalBody += "<div class='radio'>";
    ModalBody += "<label><input type='radio' name='radiooption' id='RadioAnother' value='3' onchange='CheckRadio();'>I'm having another kind of problem (manual process)</label>";
    ModalBody += "</div>";
    ModalBody += "<div id='DescriptionField'></div>"
    ModalBody += "</p>";
    ModalBody += "</div>";
    ModalBody += "<div id='MessagePanel'></div>";

    //Injecting contents
    $("#GenericModal2 .modal-title").html("<strong>" + ModalTitle + "</strong>");
    $("#GenericModal2 .modal-body").html("<strong>" + ModalBody + "</strong>");
    $("#GenericModal2 .modal-footer").html("<button type='submit' class='btn btn-success' id='SendHelpRequest' disabled='disabled'><span class='glyphicon glyphicon-ok'></span>&nbsp;Send help request</button>");
}

function MountRequestNewAccountModal() {
    var ModalTitle = "Request a new account";
    var ModalBody = "<p style='margin-bottom:20px;' class='p-modal-body'>In order to get a new system account please, fill all requested informations at form below and click in 'Request account'.</p>";

    ModalBody += "<p>";
            ModalBody += "<fieldset class='form-group'>";
            ModalBody += "<label for='Name'>Name</label>";
            ModalBody += "<input type='text' class='form-control' id='Name' placeholder='Your complete name' required>";
            ModalBody += "</fieldset>";
            ModalBody += "<fieldset class='form-group'>";
            ModalBody += "<label for='Email'>Email</label>";
            ModalBody += "<input type='email' class='form-control' id='Email' placeholder='alias@yourdomain.com' required>";
            ModalBody += "</fieldset>";
            ModalBody += "<fieldset class='form-group'>";
            ModalBody += "<label for='Phone'>Phone</label>";
            ModalBody += "<input type='tel' class='form-control' id='Phone' placeholder='(11) 01234-5678'>";
            ModalBody += "</fieldset>";
            ModalBody += "<fieldset class='form-group'>";
            ModalBody += "<label for='Justification'>Justification</label>";
            ModalBody += "<textarea class='form-control' id='Justification' placeholder='Tell us: why you need this account?' required></textarea>";
            ModalBody += "</fieldset>";
     ModalBody += "</p>";
     ModalBody += "<div id='MessagePanel'></div>";

    //Injecting contents
    $("#GenericModal .modal-title").html("<strong>" + ModalTitle + "</strong>");
    $("#GenericModal .modal-body").html(ModalBody);
    $("#GenericModal .modal-footer").html("<button type='submit' class='btn btn-success' id='RequestAccountButton'><span class='glyphicon glyphicon-ok'></span>&nbsp;Request account</button>");
}

function ModalDelete_FiscalYear(FiscalYearID, TextualFiscalYear) {
    //Defining values
    var ModalTitle = "Deleting '" + TextualFiscalYear + "' record";
    var ModalBody = "<p style='margin-bottom:20px; font-weight: 400;' class='p-modal-body'>This operation will be permanent. Are you sure?</p>";
    ModalBody += "<p><ul>";
    ModalBody += "<li>Fiscal year ID: " + FiscalYearID + "</li>";
    ModalBody += "<li>Fiscal year (textual mode): " + TextualFiscalYear + "</li>";
    ModalBody += "</ul></p>";
    ModalBody += "<div id='message-panel' style='margin-top: 10px;'></div>"

    //Injecting contents
    $("#generic-modal .modal-title").html("<strong>" + ModalTitle + "</strong>");
    $("#generic-modal .modal-body").html(ModalBody);
    $("#generic-modal .modal-footer").html("<button type='button' class='btn btn-danger' id='btnDelete' onclick=\"DeleteFiscalYear('" + FiscalYearID + "');\"><i class='fa fa-trash' aria-hidden='true'></span>&nbsp;Delete</button>");
}

function ModalDelete_Metric(MetricID, MetricCategory, MetricName) {
    //Defining values
    var ModalTitle = "Deleting '" + MetricName + "' record";
    var ModalBody = "<p style='margin-bottom:20px; font-weight: 400;' class='p-modal-body'>This operation will be permanent. Are you sure?</p>";
    ModalBody += "<p><ul>";
    ModalBody += "<li>Metric ID: " + MetricID + "</li>";
    ModalBody += "<li>Metric Category: " + MetricCategory + "</li>";
    ModalBody += "<li>Metric Name: " + MetricName + "</li>";
    ModalBody += "</ul></p>";
    ModalBody += "<div id='message-panel' style='margin-top: 10px;'></div>"

    //Injecting contents
    $("#generic-modal .modal-title").html("<strong>" + ModalTitle + "</strong>");
    $("#generic-modal .modal-body").html(ModalBody);
    $("#generic-modal .modal-footer").html("<button type='button' class='btn btn-danger' id='btnDelete' onclick=\"DeleteMetric('" + MetricID + "');\"><i class='fa fa-trash' aria-hidden='true'></span>&nbsp;Delete</button>");
}

function ModalDelete_Appointment(AppointmentID, WorkloadTitle, AppointmentDate, AppointmentHoursDispensed, AppointmentUserName) {
    //Defining values
    var ModalTitle = "Deleting appointment to '" + WorkloadTitle + "' workload";
    var ModalBody = "<p style='margin-bottom:20px; font-weight: 400;' class='p-modal-body'>This operation will be permanent. Are you sure?</p>";
    ModalBody += "<p><ul>";
    ModalBody += "<li>Appointment ID: " + AppointmentID + "</li>";
    ModalBody += "<li>Workload: " + WorkloadTitle + "</li>";
    ModalBody += "<li>Registered in: " + AppointmentDate + "</li>";
    ModalBody += "<li>Hours dispensed: " + AppointmentHoursDispensed + "</li>";
    ModalBody += "<li>User: " + AppointmentUserName + "</li>";
    ModalBody += "</ul></p>";
    ModalBody += "<div id='message-panel' style='margin-top: 10px;'></div>"

    //Injecting contents
    $("#generic-modal .modal-title").html("<strong>" + ModalTitle + "</strong>");
    $("#generic-modal .modal-body").html(ModalBody);
    $("#generic-modal .modal-footer").html("<button type='button' class='btn btn-danger' id='btnDelete' onclick=\"DeleteAppointment('" + AppointmentID + "');\"><i class='fa fa-trash' aria-hidden='true'></span>&nbsp;Delete</button>");
}


// Another functions

function ClearModalForm() {
    $("form").trigger("reset");
}

function CheckRadio() {
    $("#SendHelpRequest").removeAttr("disabled");

    if ($("input[name='radiooption']:checked").val() == "1") {
        $("#YourCompleteNameField").html("<fieldset class='form-group'><input type='text' class='form-control' id='YourCompleteName' placeholder='Your complete name here' required></fieldset>");
        $("#YourEmailField").html("");
        $("#DescriptionField").html("");
    }
    else if ($("input[name='radiooption']:checked").val() == "2") {
        $("#YourEmailField").html("<fieldset class='form-group'><input type='text' class='form-control' id='YourEmail' placeholder='Your email here' required></fieldset>");
        $("#YourCompleteNameField").html("");
        $("#DescriptionField").html("");
    }
    else {
        $("#DescriptionField").html("<fieldset class='form-group'><textarea class='form-control' id='YourDescription' placeholder='Describes your problem here' required></textarea></fieldset>");
        $("#YourCompleteNameField").html("");
        $("#YourEmailField").html("");
    }
}

function RedirectTo(url)
{
    window.location = url;
}

function DisableFiscalYearFields()
{
    $("#fyid").attr("disabled", "disabled");
    $("#fytext").attr("disabled", "disabled");
    $("#fynumber").attr("disabled", "disabled");
    $("#btnUpdate").attr("disabled", "disabled");
}

function EnableFiscalYearFields()
{
    $("#fyid").removeAttr("disabled");
    $("#fytext").removeAttr("disabled");
    $("#fynumber").removeAttr("disabled");
    $("#btnUpdate").removeAttr("disabled", "disabled");
}

function DisableMetricFields() {
    $("#FiscalYearID").attr("readonly", "readonly");
    $("#MetricCategory").attr("readonly", "readonly");
    $("#MetricName").attr("readonly", "readonly");
    $("#Description").attr("readonly", "readonly");
    $("#btnAdd").attr("disabled", "disabled");
    $("#btnUpdate").attr("disabled", "disabled");
}

function DisableAppointmentFields()
{
    $("#_WorkloadTitle").attr("readonly", "readonly");
    $("#_AppointmentDate").attr("readonly", "readonly");
    $("#_AppointmentHoursDispensed").attr("readonly", "readonly");
    $("#_AppointmentTE").attr("readonly", "readonly");
    $("#_AppointmentComment").attr("readonly", "readonly");
}

function EnableMetricFields() {
    $(".FiscalYearID").removeAttr("readonly");
    $(".MetricCategory").removeAttr("readonly");
    $(".MetricName").removeAttr("readonly");
    $(".Description").removeAttr("readonly");
    $("#btnAdd").removeAttr("disabled");
    $("#btnUpdate").removeAttr("disabled");
}

function EnableAppointmentFields()
{
    $("#_WorkloadTitle").removeAttr("readonly", "readonly");
    $("#_AppointmentDate").removeAttr("readonly", "readonly");
    $("#_AppointmentHoursDispensed").removeAttr("readonly", "readonly");
    $("#_AppointmentTE").removeAttr("readonly", "readonly");
    $("#_AppointmentComment").removeAttr("readonly", "readonly");
}

function RedirectIn(delay, url)
{
    setTimeout(function () {
        window.location = url;
    }, delay);
}

function DeleteFiscalYear(fiscalYearID) {
    $("#btnDelete").attr("disabled", "disabled");

    $.ajax({
        url: "/FiscalYear/Delete",
        type: "POST",
        data: { id: fiscalYearID },
        success: function (data) {
            if (data.Status) {
                $("#message-panel").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> Fiscal year successful deleted.</div>");
                RedirectIn(4000, "/FiscalYear/Index");
            } else {
                $("#message-panel").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> We found an error in this request. Try again in a few minutes.</div>");
            }
        }
    });
}

function DeleteAppointment(appointmentID) {
    $("#btnDelete").attr("disabled", "disabled");

    $.ajax({
        url: "/Appointment/DeleteAppointment",
        type: "DELETE",
        data: { id: appointmentID },
        success: function (data) {
            if (data.IsSuccessStatusCode) {
                $("#message-panel").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> Appointment successful deleted.</div>");
                RedirectIn(3000, "/Appointment/My");
            } else {
                $("#message-panel").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> We found an error in this request. Try again in a few minutes.</div>");
            }
        }
    });
}



$(function () {
    $('a[href*="#"]:not([href="#"])').click(function () {
        if (location.pathname.replace(/^\//, "") == this.pathname.replace(/^\//, "") && location.hostname == this.hostname) {
            var e = $(this.hash);
            if (e = e.length ? e : $("[name=" + this.hash.slice(1) + "]"), e.length) return $("html, body").animate({
                scrollTop: e.offset().top
            }, 1e3), !1
        }
    })
});

$(window).scroll(function () {
    var height = $(window).scrollTop();
    var some_number = 70;
    header = $(".navbar-landing");

    if (header.hasClass("navbar-landing-scrolled")) {
        if (height < some_number) {
            header.removeClass("navbar-landing-scrolled")
        }
    } else {
        if (height > some_number) {
            header.addClass("navbar-landing-scrolled")
        }
    }
});

function DeleteMetric(metricID) {
    $("#btnDelete").attr("disabled", "disabled");

    $.ajax({
        url: "/Metric/DeleteMetric",
        type: "DELETE",
        data: { id: metricID },
        success: function (data) {
            if (data.IsSuccessStatusCode) {
                $("#message-panel").html("<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Success!</strong> Metric successful deleted.</div>");
                RedirectIn(3000, "/Metric/Index");
            } else {
                $("#message-panel").html("<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a><strong>Error!</strong> We found an error in this request. Try again in a few minutes.</div>");
            }
        }
    });
}

// Search workloads
function GetWorkloadsByUser() {
    var url = '/Workload/ListWorkloadsByUser';
    $.ajax({
        url: url,
        type: "GET",
        cache: false,
        success: function (data, textStatus, jqXHR) {
            CallbackGetWorkloadsByUser(data);
        }
    });
}

function CallbackGetWorkloadsByUser(data) {
    if (data.length > 0) {
        var str = JSON.stringify(data);
        str = str.replace(/id/g, 'data');
        str = str.replace(/textual/g, 'value');

        object = JSON.parse(str);

        $('#_WorkloadTitle').autocomplete({
            lookup: object,
            minLength: 2,
            onSelect: function (suggestion) {
                $("#_AppointmentWorkloadWBID").val(suggestion.data);
            }
        });
    }
    else
    {
        $('#_WorkloadTitle').attr('disabled', 'disabled');
        $('#btnAddWorkload').attr('disabled', 'disabled');
    }
}

// Mask money

function LoadMaskMoney()
{
    $("#_AppointmentTE").maskMoney({ prefix: 'R$ ', allowNegative: false, allowZero: true, thousands: '.', decimal: ',', affixesStay: false });
}

// CKEditor

function LoadCKEditor()
{
    CKEDITOR.replace('_AppointmentComment');
}

function UpdateCKEditor()
{
    for (instance in CKEDITOR.instances) {
        CKEDITOR.instances[instance].updateElement();
    }
}

// Datepicker

function LoadDatePicker()
{
    $('#_AppointmentDate').datepicker({
        format: "mm/dd/yyyy",
        autoclose: true,
        todayHighlight: true
    });
}