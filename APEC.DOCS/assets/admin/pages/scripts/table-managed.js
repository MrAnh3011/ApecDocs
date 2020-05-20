var TableManaged = function () {

    var initTable1 = function () {

        var table = $('#sample_1');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.

            "columns": [{
                "orderable": false
            }, {
                "orderable": true
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": true
            }, {
                "orderable": false
            }],
            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 5,            
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm kiếm: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous":"Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            },
            "columnDefs": [{  // set default column settings
                'orderable': false,
                'targets': [0]
            }, {
                "searchable": false,
                "targets": [0]
            }],
            "order": [
                [3, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = jQuery('#sample_1_wrapper');

        table.find('.group-checkable').change(function () {
            var set = jQuery(this).attr("data-set");
            var checked = jQuery(this).is(":checked");
            jQuery(set).each(function () {
                if (checked) {
                    $(this).attr("checked", true);
                    $(this).parents('tr').addClass("active");
                } else {
                    $(this).attr("checked", false);
                    $(this).parents('tr').removeClass("active");
                }
            });
            jQuery.uniform.update(set);
        });

        table.on('change', 'tbody tr .checkboxes', function () {
            $(this).parents('tr').toggleClass("active");
        });

        tableWrapper.find('.dataTables_length select').addClass("form-control input-xsmall input-inline"); // modify table per page dropdown
    }

    var inittblUnit = function () {

        var table = $('#tblUnit');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            //"bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
            "ordering": false,
            "columns": [{
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }],
            "lengthMenu": [
                [10, 20, 50, -1],
                [10, 20, 50, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm kiếm: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            }
            //,
            //"order": [
            //     [4, "desc"]
            //]
        });

    }

    var inittblOrganization = function () {

        var table = $('#tblOrganization');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            //"bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
            "ordering": false,
            "columns": [{
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }],
            "lengthMenu": [
                [10, 20, 50, -1],
                [10, 20, 50, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm kiếm: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            }
            //,
            //"order": [
            //     [4, "desc"]
            //]
        });

    }

    var inittblCrop = function () {

        var table = $('#tblCrop');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            //"bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
            "ordering": false,
            "columns": [{
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }],
            // set the initial value
            //"pageLength": 10,
            "bLengthChange": false,
            "lengthChange": false,
            "bPaginate": false,
            //"pagingType": "bootstrap_full_number",
            "language": {
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            },
            "searching": false
            //,
            //"order": [
            //    [4, "desc"]
            //] // set first column as a default sort by asc
        });
    }

    var inittblSeason = function () {

        var table = $('#tblSeason');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            //"bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
            "ordering": false,
            "columns": [{
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }
            , {
                "orderable": false
            }],
            "lengthMenu": [
                [10, 20, 50, -1],
                [10, 20, 50, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm kiếm: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            }
            //,
            //"order": [
            //    [0, "desc"]
            //] // set first column as a default sort by asc
        });
    }

    var inittblProduct = function () {

        var table = $('#tblProduct');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            //"bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
            "ordering": false,
            "columns": [{
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }],
            "lengthMenu": [
                 [10, 20, 50, -1],
                 [10, 20, 50, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm kiếm: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            }
            ,
            //"order": [
            //    [5, "desc"]
            //] // set first column as a default sort by asc
        });
    }

    var inittblPackUnit = function () {

        var table = $('#tblPackUnit');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "ordering": false,
            "columns": [{
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }],
            "lengthMenu": [
                [10, 20, 50, -1],
                [10, 20, 50, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm kiếm: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            }
            //,
            //"order": [
            //    [3, "desc"]
            //] // set first column as a default sort by asc
        });

    }

    var inittblDivision = function () {

        var table = $('#tblDivision');

        // begin first table
        table.dataTable({

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            //"bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
            "ordering": false,
            "columns": [{
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }],
            "lengthMenu": [
                [10, 20, 50, -1],
                [10, 20, 50, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm kiếm: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            }
            //,
            //"order": [
            //    [4, "desc"]
            //] // set first column as a default sort by asc
        });
    }

    var initTable5 = function () {

        var table = $('#sample_5');

        // begin first table
        table.dataTable({
            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
            "ordering": false,
            "columns": [{
                "orderable": true
            }, {
                "orderable": false
            }, {
                "orderable": false
            }, {
                "orderable": false
            }],
            "lengthMenu": [
                [10, 20, 50, -1],
                [10, 20, 50, "Tất cả"] // change per page values here
            ],
            // set the initial value
            "pageLength": 10,
            "pagingType": "bootstrap_full_number",
            "language": {
                "search": "Tìm tài khoản: ",
                "lengthMenu": "  _MENU_ bản ghi",
                "paginate": {
                    "previous": "Prev",
                    "next": "Next",
                    "last": "Last",
                    "first": "First"
                },
                "emptyTable": "Không có dữ liệu trong bảng",
                "info": "Bản ghi _START_ tới _END_ của _TOTAL_ bản ghi",
                "infoEmpty": "Không tìm thấy bản ghi nào",
                "infoFiltered": "(được lọc từ _MAX_ bản ghi)",
                "lengthMenu": "Hiển thị _MENU_ bản ghi",
                "zeroRecords": "Không có bản ghi nào được tìm thấy"
            }
            //,
            //"order": [
            //    [0, "asc"]
            //] // set first column as a default sort by asc
        });
    }

    var initTable2 = function () {

        var table = $('#sample_2');

        table.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "Show _MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },

            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.

            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 5,
            "language": {
                "lengthMenu": " _MENU_ records",
                "paging": {
                    "previous": "Prev",
                    "next": "Next"
                }
            },
            "columnDefs": [{  // set default column settings
                'orderable': false,
                'targets': [0]
            }, {
                "searchable": false,
                "targets": [0]
            }],
            "order": [
                [1, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = jQuery('#sample_2_wrapper');

        table.find('.group-checkable').change(function () {
            var set = jQuery(this).attr("data-set");
            var checked = jQuery(this).is(":checked");
            jQuery(set).each(function () {
                if (checked) {
                    $(this).attr("checked", true);
                } else {
                    $(this).attr("checked", false);
                }
            });
            jQuery.uniform.update(set);
        });

        tableWrapper.find('.dataTables_length select').select2(); // initialize select2 dropdown
    }

    var initTable3 = function () {

        var table = $('#sample_3');

        // begin: third table
        table.dataTable({

            // Internationalisation. For more info refer to http://datatables.net/manual/i18n
            "language": {
                "aria": {
                    "sortAscending": ": activate to sort column ascending",
                    "sortDescending": ": activate to sort column descending"
                },
                "emptyTable": "No data available in table",
                "info": "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": "No entries found",
                "infoFiltered": "(filtered1 from _MAX_ total entries)",
                "lengthMenu": "Show _MENU_ entries",
                "search": "Search:",
                "zeroRecords": "No matching records found"
            },
            
            // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
            // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js). 
            // So when dropdowns used the scrollable div should be removed. 
            //"dom": "<'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",

            "bStateSave": true, // save datatable state(pagination, sort, etc) in cookie.
            
            "lengthMenu": [
                [5, 15, 20, -1],
                [5, 15, 20, "All"] // change per page values here
            ],
            // set the initial value
            "pageLength": 5,
            "language": {
                "lengthMenu": " _MENU_ records"
            },
            "columnDefs": [{  // set default column settings
                'orderable': false,
                'targets': [0]
            }, {
                "searchable": false,
                "targets": [0]
            }],
            "order": [
                [1, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = jQuery('#sample_3_wrapper');

        table.find('.group-checkable').change(function () {
            var set = jQuery(this).attr("data-set");
            var checked = jQuery(this).is(":checked");
            jQuery(set).each(function () {
                if (checked) {
                    $(this).attr("checked", true);
                } else {
                    $(this).attr("checked", false);
                }
            });
            jQuery.uniform.update(set);
        });

        tableWrapper.find('.dataTables_length select').select2(); // initialize select2 dropdown
    }

    return {

        //main function to initiate the module
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }

            initTable1();
            initTable2();
            initTable3();
            initTable5();
            inittblUnit();
            inittblPackUnit();
            inittblDivision();
            inittblSeason();
            inittblProduct();
            inittblCrop();
        }

    };

}();