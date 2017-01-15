
// jquery validation'ın date formatını günceller.
if ($.validator)
{
    $(function () {
        $.validator.methods.date = function (value, element) {
            return this.optional(element) || Globalize.parseDate(value, "dd.MM.yyyy") !== null;
        }
    });
}

// bootstrap datepicker
$(".datetimepicker").datetimepicker({
    format: violaParameters.datepickerFormat,
    locale: violaParameters.language
});


// select2
$("select").select2({
    "language": violaParameters.language
});




function loadUsersAssignedToProject(projectId) {

    var ctrlSelect = $("#UserIdMulti");
    ctrlSelect.select2().empty();

    if (projectId) {
        $.ajax({
            type: 'GET',
            url: '/Json/GetUsersAssignedToProject/?projectId=' + projectId,
            data: '',
            dataType: 'json',
            success: function (data) {
                ctrlSelect.select2({ data: data });
            },
            error: function (data, status) {
            }
        })
    }
}