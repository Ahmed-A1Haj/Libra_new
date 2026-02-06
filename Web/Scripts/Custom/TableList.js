var tableList = {
    usersTable: () => ({
        ajax: {
            url: '/Admin/GetUsers',
            type: 'POST'
        },
        processing: true,
        serverSide: true,
        "dataType": "json",
        columns: [
            { visible: false, data: 'Id' },
            { data: 'Name', title: 'Name' },
            { data: 'Email', title: 'Email' },
            { data: 'Login', title: 'Login' },
            { data: 'Telephone', title: 'Telephone' },
            { data: 'UserRole', title: 'UserRole' }
        ],
        columnDefs: [
            {
                targets: '_all',
                orderSequence: ['asc', 'desc']
            }
        ],
        language: {
            search: "Filter records:", // Custom search placeholder
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ users",
            paginate: {
                firstLast: false,
                previous: "Prev",
                next: "Next"
            }
        },
        layout: {
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        }
    }),
    PosTable: () => ({
        ajax: {
            url: '/Pos/GetPoses',
            type: 'POST',
        },
        processing: true,
        serverSide: true,
        "dataType": "json",
        columns: [
            { visible: false, data: 'Id' },
            { data: 'Name', title: 'Pos Name' },
            { data: 'Telephone', title: 'Telephone' },
            { data: 'Address', title: 'Address' },
            { data: 'City', title: 'City' },
            { data: 'IssueCount', title: 'Status' }
        ],
        columnDefs: [
            {
                targets: '_all',
                orderSequence: ['asc', 'desc']
            }
        ],
        language: {
            search: "Filter records:", // Custom search placeholder
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ Positions",
            paginate: {
                firstLast: false,
                previous: "Prev",
                next: "Next"
            }
        },
        layout: {
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        }
    }),
    IssueByPosIdTable: (data) => ({
        data: data,
        columns: [
            { data: 'Id', title: "Issue#" },
            { data: 'PosName', title: 'Pos Name' },
            { data: 'CreatedBy', title: 'Created By' },
            { data: 'Date', title: 'Date' },
            { data: 'IssueType', title: 'Issue Type' },
            { data: 'AssignedTo', title: 'Assigned To' },
            { data: 'Memo', title: 'Memo' }
        ],
        columnDefs: [
            {
                targets: '_all',
                orderSequence: ['asc', 'desc']
            }
        ],
        language: {
            search: "Filter records:", // Custom search placeholder
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ Issues",
            paginate: {
                firstLast: false,
                previous: "Prev",
                next: "Next"
            }
        },
        layout: {
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        }
    }),
    IssuesTable: (s) => ({
        ajax: {
            url: '/Issue/GetIssues',
            type: 'POST',
            /*dataSrc: 'Data',*/
            "dataType": "json",
            data: function (d) {
                d.extrasearch = s;
            },
        },
        processing: true,
        serverSide: true,
        "dataType": "json",
        columns: [
            { data: 'Id', title: "Issue#" , name: 'userName' },
            { data: 'PosName', title: 'Pos Name', name: 'userName2' },
            { data: 'CreatedBy', title: 'Created By', name: 'userName3' },
            { data: 'Date', title: 'Date', name: 'userName4' },
            { data: 'IssueType', title: 'Issue Type', name: 'userName5' },
            { data: 'AssignedTo', title: 'Assigned To', name: 'userName6' },
            { data: 'Memo', title: 'Memo', name: 'userName7' }
        ],
        columnDefs: [
            {
                targets: '_all',
                orderSequence: ['asc', 'desc']
            }
        ],
        order: [
            [5, 'asc'],
        ],
        language: {
            search: "Filter records:", // Custom search placeholder
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ Issues",
            paginate: {
                firstLast: false,
                previous: "Prev",
                next: "Next"
            }
        },
        layout: {
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        }
    }),
    PosTable_Issue: () => ({
        ajax: {
            url: '/Pos/GetPoses',
            type: 'POST',
            data: function (d) {
                d.posName = posName;
                d.posTelephone = posTelephone;
                d.posAddress = posAddress;
            },
        },
        dom: 'lrtip',
        processing: true,
        serverSide: true,
        "dataType": "json",
        columns: [
            { visible: false, data: 'Id' },
            { data: 'Name', title: 'Pos Name' },
            { data: 'Telephone', title: 'Telephone' },
            { data: 'Address', title: 'Address' },
            { data: 'City', title: 'City' }
        ],
        columnDefs: [
            {
                targets: '_all',
                orderSequence: ['asc', 'desc']
            }
        ],
        language: {
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ Positions",
            paginate: {
                firstLast: false,
                previous: "Prev",
                next: "Next"
            }
        },
        layout: {
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        }
    }),
    AddIssuePosItem: (data) => ({
        data: data.pos,
        dom: 'rt',
        columns: [
            { visible: false, data: 'Id' },
            { data: 'Name', title: 'Pos Name' },
            { data: 'Telephone', title: 'Telephone' },
            { data: 'Address', title: 'Address' },
            { data: 'City', title: 'City' },
            { data: 'IssueCount', visible: false}
        ],
        columnDefs: [
            {
                targets: '_all',
                orderSequence: ['asc', 'desc']
            }
        ],
    }),
    IssueLogs: (data) => ({
        data: data.logs,
        dom: 'lrtip',
        columns: [
            { data: 'Date', title: 'Date' },
            { data: 'Action', title: 'Action' },
            { data: 'User', title: 'User' },
            { data: 'Notes', title: 'Notes' }
        ],
        columnDefs: [
            {
                targets: '_all',
                orderSequence: ['asc', 'desc']
            }
        ],
        language: {
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ Logs",
            paginate: {
                firstLast: false,
                previous: "Prev",
                next: "Next"
            }
        },
        layout: {
            bottomEnd: {
                paging: {
                    firstLast: false
                }
            }
        }
    }),
};