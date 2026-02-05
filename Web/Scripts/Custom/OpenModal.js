function openModal(parameters) {
    const id = parameters.data;
    const url = parameters.url;
    const modal = $('#modal');

    if (!parameters.hasOwnProperty('data') || !parameters.hasOwnProperty('url')) {
        alert('Error: Missing required parameters "id" and "url".');
        return;
    }

    const data = { id: id };
    if (parameters.hasOwnProperty('additionalData')) {
        Object.assign(data, parameters.additionalData);
    }

    $.ajax(
        {
            type: 'GET',
            url: url,
            data: data,
            success: function (response) {
                $('.modal-dialog');
                modal.find(".modal-content").html(response);
                modal.modal('show')
            },
            failure: function () {
                modal.modal('hide')
            },
            error: function (response) {
                alert(response.responseText)
            }
        });
};

$(document).on("submit", "#userForm", function (e) {
    e.preventDefault();

    const form = $(this);
    const url = form.attr("action");
    const data = form.serialize();

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        success: function (response) {

            // If server returned the form again -> validation errors
            if ($(response).find(".validation-summary-errors").length ||
                $(response).find(".field-validation-error").length) {

                $("#modal .modal-content").html(response);
                $.validator.unobtrusive.parse("#modal"); // VERY IMPORTANT
            }
            else {
                // success case
                $("#modal").modal('hide');
                myTable.ajax.reload();
            }
        },
        error: function (err) {
            alert(err.responseText);
        }
    });
});


function closeModal() {
    $("#modal").modal('hide');
    myTable.ajax.reload();
};