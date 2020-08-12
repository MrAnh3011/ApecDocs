UITree.init();
//$(window).load(function () {
//    // Animate loader off screen
//    $(".se-pre-con").fadeOut("slow");;
//});
$(document).ready(function() {
//    Metronic.blockUI('.body');
//    $('#DocumentNo').number()
//    $('#DocumentNo').number(true, 0, decimalSeparator, thousandsSeparator);
    var table = $("#tbl-docs").DataTable({
        language: {
            "sProcessing": "Đang xử lý...",
            "sLengthMenu": "Hiển thị _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
        rowId: 'DocumentId',
        "columnDefs": [
            {
                "targets": [10,11],
                "visible": false
            },
            { "width": "1%", "targets": [0,1,2,4,6,7,8,9] },
            { "width": "20%", "targets": [3,5]}
        ]
//        orderCellsTop: false,
//        fixedHeader: true
        //            "scrollX": true
    });

    var columnFilter =
        "<tr><th>STT</th><th>Mã văn bản</th><th>Số hiệu văn bản</th><th>Tên tài liệu</th><th>Danh mục TL</th><th>Mô tả</th><th>Cơ quan ban hành</th><th>Ngày hiệu lực</th><th>Trạng thái</th><th></th></tr >";
    $(columnFilter).appendTo("#tbl-docs thead");
    $("#tbl-docs thead tr:eq(1) th").each(function(i) {
        var title = $(this).text();
        $(this).html('<input type="text" class="form-control" placeholder="' + title + '" />');

        $("input", this).on("keyup change",
            function() {
                if (table.column(i).search() !== this.value) {
                    table
                        .column(i)
                        .search(this.value)
                        .draw();
                }
            });
    });

    $('#doctype-tree').on('select_node.jstree',
        function(e, data) {
            var id = data.node.id;
            try {
                Metronic.blockUI('.body');
                $.ajax({
                    url: "/Admin/Home/GetListDocs",
                    data: {
                        id: id
                    },
                    dataType: "json",
                    type: "POST",
                    success: function (result) {
                        var table = $('#tbl-docs').DataTable();
                        var rs = result.ListDocs;
                        table.clear().draw();
                        for (var i = 0; i < rs.length; i++) {
                            var actionEdit = "";
                            var actionDelete = "";
                            var detail = rs[i].BriefDescription;
                            var actionDown = "";
                            if (roleTypeStr.includes("2")) {
                                actionEdit = "<a href='../../Admin/Home/Edit/" + rs[i].DocumentId + "'><i class='fa fa-edit'></i></a>";
                            }
                            if (roleTypeStr.includes("3")) {
                                actionDelete = "<a href='#' class='deleteDoc'><i class='fa fa-trash'></i></a>";
                            }
                            if (roleTypeStr.includes("4")) {
                                detail = "<a href='#' class='detailDoc'>" + rs[i].BriefDescription + "</a>";
                            }
                            if (roleTypeStr.includes("5")) {
                                actionDown = "<a href='../../Admin/Files/DownloadDoc?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                                var docName = rs[i].DocumentName.split(",");
                                if (docName.length > 1) {
                                    actionDown = "<a href='../../Admin/Files/DownloadDocZip?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                                }
                            }

                            table.row.add([
                                "",
                                rs[i].DocumentCode,
                                rs[i].DocumentNo,
                                rs[i].DisplayName,
                                rs[i].DocTypeName,
                                detail,
                                rs[i].OrgPublish,
                                formatDateJson(rs[i].ActiveDate),
                                rs[i].Status === 1 ? "Còn hiệu lực" : "Hết hiệu lực",
                                actionEdit + " " + actionDelete + " " + actionDown,
                                rs[i].BriefDescription,
                                rs[i].DocumentId
                            ]);
                        }
                        table.draw(false);
                        $('html, body').animate({
                            scrollTop: $("#tbl-docs").offset().top
                        }, 1000);
                        Metronic.unblockUI('body');
                    },
                    error: function (result) {
                        Metronic.unblockUI('body');
                        alert(result.ListDocs);
                    }
                });
            } catch (err) {
                Metronic.unblockUI('body');
                alert(err);
            }
        });

    table.on('click',
        '.deleteDoc',
        function (e) {
            e.preventDefault();

            if (confirm("Bạn có muốn xóa không ?") == false) {
                return;
            }

            var nRow = $(this).parents('tr')[0];
            var data = table.rows(nRow).data()[0];
            try {
                $.ajax({
                    url: "/Admin/Home/Delete",
                    data: {
                        id: data[11]
                    },
                    dataType: "json",
                    type: "POST",
                    success: function (result) {
                        if (result > 0) {
                            table.row(nRow).remove().draw();
                            toastr['success']("Xóa thành công ", "Xóa!");
                        }
                    },
                    error: function (result) {
                        toastr['error']("Xóa thất bại ", "Xóa!");
                        alert(result);
                    }
                });
                //        Metronic.unblockUI('body');
            } catch (err) {
                alert(err);
            }
        });

    table.on('click',
        '.detailDoc',
        function (e) {
            e.preventDefault();

            /* Get the row as a parent of the link that was clicked on */
            var nRow = $(this).parents('tr')[0];
            var data = table.rows(nRow).data()[0];
            $('#detail-modal-title').html(data[4]);
            $('#detail-modal-body').html(data[10]);
            $('#exampleModal').modal('show');
        });

    table.on("order.dt search.dt",
        function() {
            table.column(0,
                {
                    search: "applied",
                    order: "applied"
                }).nodes().each(function(cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

    function BindMenu(listAll) {
        var listTree = [];
        var parentItems = listAll.filter(x => x.ParentId === 0);
        for (var i = 0; i < parentItems.length; i++) {
            var pItem = parentItems[i];
            var treeItem = {
                id: pItem.Id,
                text: pItem.Name,
                parentId: pItem.ParentId,
                children: BindSubMenu(listAll, pItem)
            };
            listTree.push(treeItem);
        };
        return listTree;
    }

    function BindSubMenu(listAll, pItem) {
        var treeItems = [];
        var childItems = listAll.filter(x => x.ParentId == pItem.Id);
        for (var i = 0; i < childItems.length; i++) {
            var cItem = childItems[i];
            var treeItem = {
                id: cItem.Id,
                text: cItem.Name,
                parentId: cItem.ParentId,
                children: BindSubMenu(listAll, cItem)
            };

            treeItems.push(treeItem);
        };
        return treeItems;
    }

    try {
        Metronic.blockUI('.body');
        $.ajax({
            url: "/Admin/Home/GetListMenu",
            data: {
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                var listAll = result.ListMenu;
                var list = BindMenu(listAll);
                $('#group-name').select2({
                    placeholder: 'Search for an option',
                    data: { results: list, text: "text" },
                    formatSelection: function (item) {
                        return item.text;
                    },
                    formatResult: function (item) {
                        return item.text;
                    }
                });
                $('#doctype-tree').jstree({
                    'core': {
                        'data': list
                    }
                });
                Metronic.unblockUI('body');
            },
            error: function (result) {
                Metronic.unblockUI('body');
                alert(result.ListMenu);
            }
        });
    } catch (err) {
        Metronic.unblockUI('body');
        alert(err);
    }

    searchListDocs();

});

function searchListDocs() {
    try {
        Metronic.blockUI('.body');
        var groupId = $('#group-name').val();
        var docName = $('#doc-name').val();
        var docType = $('#doc-type').val();
        var orgPublish = $('#org-publish').val();
        var docContent = $('#doc-content').val();
        $.ajax({
            url: "/Admin/Home/SearchListDocs",
            data: {
                groupId: groupId,
                docName: docName,
                docType: docType,
                orgPublish: orgPublish,
                docContent: docContent
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                var table = $('#tbl-docs').DataTable();
                var rs = result.ListDocs;
                table.clear().draw();
                for (var i = 0; i < rs.length; i++) {
                    var actionEdit = "";
                    var actionDelete = "";
                    var detail = rs[i].BriefDescription;
                    var actionDown = "";
                    if (roleTypeStr.includes("2")) {
                        actionEdit = "<a href='../../Admin/Home/Edit/" + rs[i].DocumentId + "'><i class='fa fa-edit'></i></a>";
                    }
                    if (roleTypeStr.includes("3")) {
                        actionDelete = "<a href='#' class='deleteDoc'><i class='fa fa-trash'></i></a>";
                    }
                    if (roleTypeStr.includes("4")) {
                        detail = "<a href='#' class='detailDoc'>" + rs[i].BriefDescription + "</a>";
                    }
                    if (roleTypeStr.includes("5")) {
                        actionDown = "<a href='../../Admin/Files/DownloadDoc?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                        var docName = rs[i].DocumentName.split(",");
                        if (docName.length > 1) {
                            actionDown = "<a href='../../Admin/Files/DownloadDocZip?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                        }
                    }
                    table.row.add([
                        "",
                        rs[i].DocumentCode,
                        rs[i].DocumentNo,
                        rs[i].DisplayName,
                        rs[i].DocTypeName,
                        detail,
                        rs[i].OrgPublish,
                        formatDateJson(rs[i].ActiveDate),
                        rs[i].Status === 1 ? "Còn hiệu lực" : "Hết hiệu lực",
                        actionEdit + " " + actionDelete + " " + actionDown,
                        rs[i].BriefDescription,
                        rs[i].DocumentId
                    ]);
                }
                table.draw(false);
                $('html, body').animate({
                    scrollTop: $("#tbl-docs").offset().top
                }, 1000);
                Metronic.unblockUI('body');
            },
            error: function (result) {
                Metronic.unblockUI('body');
                alert(result.ListDocs);
            }
        });
    } catch (err) {
        Metronic.unblockUI('body');
        alert(err);
    }
}

function formatDateJson(datetime) {
    var date = new Date(parseFloat(datetime.replace("/Date(", "").replace(")/", ""), 10));
    return moment(date).format('DD/MM/YYYY');
}

