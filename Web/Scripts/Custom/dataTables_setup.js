function initDataTables(data) {
    $('[data-table]').each(function () {
        if ($.fn.DataTable.isDataTable(this)) return;

        const key = this.dataset.table;

        const configFactory = tableList[key];

        if (!configFactory) return;

        const config = configFactory(data);

        myTable = $(this).DataTable(config);
    });
}

function initTableInteractions (tableId, controller, entity) {

    let selectedUser = null;

    const table = $(`#${tableId}`).DataTable();

    $(`#edit${entity}Btn`).prop('disabled', !selectedUser);
    $(`#${entity}DetailsBtn`).prop('disabled', !selectedUser);
    $(`#Delete${entity}Btn`).prop('disabled', !selectedUser);

    $(`#${tableId} tbody`).off('click').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            selectedUser = null;
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            selectedUser = table.row(this).data();
        }
        $(`#edit${entity}Btn`).prop('disabled', !selectedUser);
        $(`#${entity}DetailsBtn`).prop('disabled', !selectedUser);
        $(`#Delete${entity}Btn`).prop('disabled', !selectedUser);

    });

    $(`#add${entity}Btn`).off('click').on('click', function () {
        loadPage(`/${controller}/GetAdd${entity}`);
    });

    $(`#edit${entity}Btn`).off('click').on('click', function () {
        loadPage(`/${controller}/GetEdit${entity}/${selectedUser.Id}`);
    });

    $(`#${entity}DetailsBtn`).off('click').on('click', function () {
        loadPage(`/${controller}/Get${entity}Details/${selectedUser.Id}`);
    });

    $(`#Delete${entity}Btn`).off('click').on('click', function () {
        loadPage(`/${controller}/GetDelete${entity}/${selectedUser.Id}`);
    });

    
};

