UITree.init();
var docTypeId = null;
var docTypeName = null;
var actionType = null;

var pItems = [];

var cItems = [];

var exportData = {
    "options": {
        "fileName": "doc"
    },
    "tableData": [
        {
            "sheetName": "Sheet1",
            "data": [
            ]
        }
    ]
};

$(window).load(function () {
    $(".dt-button").unwrap();
    $('<i class="fa fa-file-excel-o"></i>').prependTo(".buttons-excel");

    $('.group-action').prop('disabled', true);

    if (localStorage.getItem("StatusDocType") === "1") {
        toastr['success']("Thành công!", "Loại tài liệu.");
        localStorage.clear();
    }
});

$(document).ready(function () {

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
                "targets": [10, 11, 12],
                "visible": false
            },
            { "width": "1%", "targets": [0, 1, 2, 4, 6, 7, 8, 9] },
            { "width": "20%", "targets": [3, 5] }
        ]
    });


    new $.fn.dataTable.Buttons(table, {

        buttons: [{
            extend: "excel",
            className: "btn btn-circle yellow",
            titleAttr: 'Kết xuất excel',
            text: ' Kết xuất excel',
        }]
    }).container().prependTo($('.actions'));
    
    var columnFilter =
        "<tr><th>STT</th><th>Mã văn bản</th><th>Số hiệu văn bản</th><th>Tên tài liệu</th><th>Danh mục TL</th><th>Mô tả</th><th>Cơ quan ban hành</th><th>Ngày hiệu lực</th><th>Trạng thái</th><th></th></tr >";
    $(columnFilter).appendTo("#tbl-docs thead");
    $("#tbl-docs thead tr:eq(1) th").each(function (i) {
        var title = $(this).text();
        $(this).html('<input type="text" class="form-control" placeholder="' + title + '" />');

        $("input", this).on("keyup change",
            function () {
                if (table.column(i).search() !== this.value) {
                    table
                        .column(i)
                        .search(this.value)
                        .draw();
                }
            });
    });

    $('#doctype-tree').on('select_node.jstree',
        function (e, data) {
            docTypeId = data.node.id;
            docTypeName = data.node.text;
            $('.group-action').prop('disabled', false);
            var evt = window.event || e;
            var button = evt.which || evt.button;

            if (button !== 1 && (typeof button != "undefined")) return false;
            try {
                Metronic.blockUI('.body');
                $.ajax({
                    url: "/Home/GetListDocs",
                    data: {
                        id: docTypeId
                    },
                    dataType: "json",
                    type: "POST",
                    success: function (result) {
                        var table = $('#tbl-docs').DataTable();
                        var rs = result.ListDocs;
                        exportData.tableData[0].data = rs;
                        table.clear().draw();
                        for (var i = 0; i < rs.length; i++) {
                            var actionEdit = "";
                            var actionDelete = "";
                            var displayName = rs[i].DisplayName;
                            var actionDown = "";
                            if (roleTypeStr.includes("2")) {
                                actionEdit = "<a href='/Home/Edit/" + sessionKey + '/' + rs[i].DocumentId + "'><i class='fa fa-edit'></i></a>";
                            }
                            if (roleTypeStr.includes("3")) {
                                actionDelete = "<a href='#' class='deleteDoc'><i class='fa fa-trash'></i></a>";
                            }
                            if (roleTypeStr.includes("4")) {
                                var docName = rs[i].DocumentName == null ? "" : rs[i].DocumentName.split(",");
                                if (docName.length === 1) {
                                    if (docName[0].includes('.ppt') ||
                                        docName[0].includes('.pptx') ||
                                        docName[0].includes('.doc') ||
                                        docName[0].includes('.docx') ||
                                        docName[0].includes('.xls') ||
                                        docName[0].includes('.xlsx')) {
                                        displayName = "<a target='_blank' href='https://view.officeapps.live.com/op/embed.aspx?src=http://docs.apec.com.vn/UploadedFiles/" +
                                            encodeURI(docName[0]) + "'><i class='fa fa-eye'></i> " + rs[i].DisplayName + "</a>";
                                    } else if (docName[0].includes('.pdf') || docName[0].includes('.txt')) {
                                        displayName = "<a target='_blank' href='http://docs.apec.com.vn/UploadedFiles/" +
                                            docName[0] + "#toolbar=0'><i class='fa fa-eye'></i> " + rs[i].DisplayName + "</a>";
                                    } else if (docName[0].includes('.mp4') || docName[0].includes('.mov') || docName[0].includes('.wmv')) {
                                        displayName = "<a href = '#' class = 'detailVid'><i class='fa fa-eye'></i>" + rs[i].DisplayName + "</a>";
                                    }
                                }
                            }
                            if (roleTypeStr.includes("5")) {
                                actionDown = "<a href='/Files/DownloadDoc?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                                var docName = rs[i].DocumentName == null ? "" : rs[i].DocumentName.split(",");
                                if (docName.length > 1) {
                                    actionDown = "<a href='/Files/DownloadDocZip?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                                }
                            }

                            table.row.add([
                                "",
                                rs[i].DocumentCode,
                                rs[i].DocumentNo,
                                displayName,
                                rs[i].DocTypeName,
                                rs[i].BriefDescription != null ? "<a href='#' class='detailDoc'><i class='fa fa-info-circle'></i> " + rs[i].BriefDescription + "</a>" : '',
                                rs[i].OrgPublish,
                                formatDateJson(rs[i].ActiveDate),
                                rs[i].Status === 1 ? '<span class="label label-sm label-success">Còn hiệu lực</span >' : '<span class="label label-sm label-danger">Hết hiệu lực</span >',
                                actionEdit + " " + actionDelete + " " + actionDown,
                                rs[i].BriefDescription,
                                rs[i].DocumentId,
                                rs[i].DocumentName == null ? "" : rs[i].DocumentName.split(",")[0]
                            ]);
                        }
                        table.draw(false);
                        $('html, body').animate({
                            scrollTop: $("#portlet-lst-docs").offset().top
                        }, 1000);
                        setTimeout(function () {
                            Metronic.unblockUI('body');
                        }, 1000);
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

    table.on('click', '.deleteDoc',
        function (e) {
            e.preventDefault();

            if (confirm("Bạn có muốn xóa không ?") == false) {
                return;
            }
            var nRow = $(this).parents('tr')[0];
            var data = table.rows(nRow).data()[0];
            try {
                $.ajax({
                    url: "/Home/Delete",
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
            } catch (err) {
                alert(err);
            }
        });

    table.on('click','.detailDoc',
        function (e) {
            e.preventDefault();
            /* Get the row as a parent of the link that was clicked on */
            let nRow = $(this).parents('tr')[0];
            let data = table.rows(nRow).data()[0];
            $('#detail-modal-title').html(data[4]);
            $('#detail-modal-body').html(data[10]);
            $('#exampleModal').modal('show');
        });

    table.on('click', '.detailVid',
        function (e) {
            e.preventDefault();

            let nRow = $(this).parents('tr')[0];
            let data = table.rows(nRow).data()[0];
            let content = "<video oncontextmenu='return false;' id='myVideo' width= '100%' height ='auto' autoplay controls controlsList='nodownload'>" +
                "<source src = 'http://docs.apec.com.vn/UploadedFiles/" + data[12] +"'type = 'video/mp4' ></video >";

            $('#detail-modal-title').html(data[4]);
            $('#detail-modal-body').append(content);
            $('#exampleModal').modal('show');
        });

    $("#exampleModal").on('hidden.bs.modal', function () {
        $('#detail-modal-body').empty();
    });

    table.on("order.dt search.dt",
        function () {
            table.column(0,
                {
                    search: "applied",
                    order: "applied"
                }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
        }).draw();

    try {
        $.ajax({
            url: "/Home/GetListMenu",
            data: {
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                var listMenu = result.ListMenu;
                var listAll = result.ListAllMenu;

                BindParentMenu(listAll, listMenu);

                var unionList = pItems.concat(listMenu).filter(function (o) {
                    return this.has(o.Id) ? false : this.add(o.Id);
                }, new Set());

                var list = BindMenu(unionList);
                var listSelect = BindMenuSelect(unionList);

                $("#group-name").select2ToTree({ treeData: { dataArr: listSelect } });
                $('#doctype-tree').jstree({
                    'core': {
                        "themes": {
                            "responsive": false
                        },
                        // so that create works
                        "check_callback": true,
                        'data': list
                    },
                    'contextmenu': {
                        'items': function (n) {
                            var tmp = $.jstree.defaults.contextmenu.items();
                            tmp.create.label = 'Thêm mới';
                            tmp.rename.label = 'Đổi tên';
                            tmp.remove.label = 'Xóa';
                            tmp.ccp.label = 'Chỉnh sửa';
                            tmp.ccp.submenu.cut.label = 'Cắt';
                            tmp.ccp.submenu.copy.label = 'Sao chép';
                            tmp.ccp.submenu.paste.label = 'Dán';
                            tmp.create._disabled = true;
                            tmp.ccp._disable = true;
                            tmp.rename._disable = true;
                            tmp.remove._disable = true;
                            if (roleTypeStr.includes("1")) {
                                tmp.create._disabled = false;
                            }
                            if (roleTypeStr.includes("2")) {
                                tmp.ccp._disable = false;
                                tmp.rename._disable = false;
                            }
                            if (roleTypeStr.includes("3")) {
                                tmp.remove._disable = false;
                            }
                            return tmp;
                        }
                    },
                    "plugins": ["contextmenu", "dnd", "types"]
                });
            },
            error: function (result) {
                alert(result.ListMenu);
            }
        });
    } catch (err) {
        Metronic.unblockUI('body');
        alert(err);
    }

    $('#doctype-tree').bind('create_node.jstree', function (e, data) {

    });
    $('#doctype-tree').bind('rename_node.jstree', function (e, data) {
        var id = data.node.id;
        var parentId = data.node.parent;
        var name = data.node.text;
        var model = {
            Id: id,
            ParentId: parentId,
            Name: name,
            Action: id.includes('j') ? 'Create' : 'Edit'
        };

        SaveDocType(model).then(res => {
            $('#doctype-tree').jstree(true).set_id(data.node, res);
        }).catch((err) => {
            console.log(err);
        });
    });
    $('#doctype-tree').bind('delete_node.jstree', function (e, data) {
        var id = data.node.id;
        var parentId = data.node.parent;
        var name = data.node.text;
        var model = {
            Id: id,
            ParentId: parentId,
            Name: name,
            Action: 'Delete'
        };
        SaveDocType(model);
    });
    $('#doctype-tree').bind('move_node.jstree', function (e, data) {
        var id = data.node.id;
        var parentId = data.node.parent;
        var name = data.node.text;
        var model = {
            Id: id,
            ParentId: parentId,
            Name: name,
            Action: 'Edit'
        };
        SaveDocType(model);
    });

    $('#doctype-tree').bind('copy_node.jstree', function (e, data) {
        var parentId = data.node.parent;
        var name = data.node.text;
        var model = {
            ParentId: parentId,
            Name: name,
            Action: 'Create'
        };
        SaveDocType(model);
    });

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
            url: "/Home/SearchListDocs",
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
                exportData.tableData[0].data = rs;
                table.clear().draw();
                for (var i = 0; i < rs.length; i++) {
                    var actionEdit = "";
                    var actionDelete = "";
                    var displayName = rs[i].DisplayName;
                    var actionDown = "";
                    if (roleTypeStr.includes("2")) {
                        actionEdit = "<a href='/Home/Edit/" + sessionKey + '/' + rs[i].DocumentId + "'><i class='fa fa-edit'></i></a>";
                    }
                    if (roleTypeStr.includes("3")) {
                        actionDelete = "<a href='#' class='deleteDoc'><i class='fa fa-trash'></i></a>";
                    }
                    if (roleTypeStr.includes("4")) {
                        var docName = rs[i].DocumentName == null ? "" : rs[i].DocumentName.split(",");
                        if (docName.length === 1) {
                            if (docName[0].includes('.ppt') ||
                                docName[0].includes('.pptx')||
                                docName[0].includes('.doc') ||
                                docName[0].includes('.docx')||
                                docName[0].includes('.xls') ||
                                docName[0].includes('.xlsx')) {
                                displayName = "<a target='_blank' href='https://view.officeapps.live.com/op/embed.aspx?src=http://docs.apec.com.vn/UploadedFiles/" +
                                    encodeURI(docName[0]) + "'><i class='fa fa-eye'></i> " + rs[i].DisplayName + "</a>";
                            } else if (docName[0].includes('.pdf') || docName[0].includes('.txt')) {
                                displayName = "<a target='_blank' href='http://docs.apec.com.vn/UploadedFiles/" +
                                    docName[0] + "#toolbar=0'><i class='fa fa-eye'></i> " + rs[i].DisplayName + "</a>";
                            } else if (docName[0].includes('.mp4') || docName[0].includes('.mov') || docName[0].includes('.wmw')) {
                                displayName = "<a href = '#' class = 'detailVid'><i class='fa fa-eye'></i>" + rs[i].DisplayName + "</a>";
                            }
                        }
                    }
                    if (roleTypeStr.includes("5")) {
                        actionDown = "<a href='/Files/DownloadDoc?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                        var docName = rs[i].DocumentName == null ? "" : rs[i].DocumentName.split(",");
                        if (docName.length > 1) {
                            actionDown = "<a href='/Files/DownloadDocZip?fileName=" + rs[i].DocumentName + "'><i class='fa fa-download'></i></a>";
                        }
                    }
                    table.row.add([
                        "",
                        rs[i].DocumentCode,
                        rs[i].DocumentNo,
                        displayName,
                        rs[i].DocTypeName,
                        rs[i].BriefDescription != null ? "<a href='#' class='detailDoc'><i class='fa fa-info-circle'></i> " + rs[i].BriefDescription + "</a>" : '',
                        rs[i].OrgPublish,
                        formatDateJson(rs[i].ActiveDate),
                        rs[i].Status === 1 ? '<span class="label label-sm label-success">Còn hiệu lực</span >' : '<span class="label label-sm label-danger">Hết hiệu lực</span >',
                        actionEdit + " " + actionDelete + " " + actionDown,
                        rs[i].BriefDescription,
                        rs[i].DocumentId,
                        rs[i].DocumentName == null ? "" : rs[i].DocumentName.split(",")[0]
                    ]);
                }
                table.draw(false);
                $('html, body').animate({
                    scrollTop: $("#portlet-lst-docs").offset().top
                }, 1000);
                setTimeout(function () {
                    Metronic.unblockUI('body');
                }, 1000);
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

function BindMenu(listAll) {
    var listTree = [];
    var parentItems = listAll.filter(x => x.ParentId === 0);
    for (var i = 0; i < parentItems.length; i++) {
        var pItem = parentItems[i];
        var treeItem = {
            id: pItem.Id,
            text: pItem.Name,
            state: {
                opened: true,
                selected: true
            },
            parentId: pItem.ParentId,
            children: BindSubMenu(listAll, pItem)
        };
        listTree.push(treeItem);
    };
    return listTree;
}

function BindSubMenu(listAll, pItem) {
    var treeItems = [];
    var childItems = listAll.filter(x => x.ParentId === pItem.Id);
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

function BindParentMenu(listAll, list) {
    for (var i = 0; i < list.length; i++) {
        var parentItems = listAll.filter(x => x.Id === list[i].ParentId);
        if (parentItems.length > 0) {
            pItems.push(parentItems[0]);
            BindParentMenu(listAll, parentItems);
        }
    }
}

function BindMenuSelect(listAll) {
    var listTree = [];
    var parentItems = listAll.filter(x => x.ParentId === 0);
    for (var i = 0; i < parentItems.length; i++) {
        var pItem = parentItems[i];
        var treeItem = {
            id: pItem.Id,
            text: pItem.Name,
            parentId: pItem.ParentId,
            inc: BindSubMenuSelect(listAll, pItem)
        };
        listTree.push(treeItem);
    };
    return listTree;
}

function BindSubMenuSelect(listAll, pItem) {
    var treeItems = [];
    var childItems = listAll.filter(x => x.ParentId == pItem.Id);
    for (var i = 0; i < childItems.length; i++) {
        var cItem = childItems[i];
        var treeItem = {
            id: cItem.Id,
            text: cItem.Name,
            parentId: cItem.ParentId,
            inc: BindSubMenuSelect(listAll, cItem)
        };

        treeItems.push(treeItem);
    };
    return treeItems;
}

function SaveDocType(model) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/Home/SaveDocType",
            data: model,
            type: "POST",
            success: function (result) {
                toastr['success']("Thành công!", "Loại tài liệu.");
                resolve(result);
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

}

function SaveDocTypeCallBack(model, callback) {
    $.ajax({
        url: "/Home/SaveDocType",
        data: model,
        type: "POST",
        success: function (result) {
            toastr['success']("Thành công!", "Loại tài liệu.");
            callback(result);
        },
        error: function (result) {
            console.log(result);
        }
    });
}

function formatDateJson(datetime) {
    var date = new Date(parseFloat(datetime.replace("/Date(", "").replace(")/", ""), 10));
    return moment(date).format('DD/MM/YYYY');
}

function DoExport() {
    console.log(exportData);
    if (typeof XLSX == 'undefined') XLSX = require('xlsx');

    /* make the worksheet */
    var ws = XLSX.utils.json_to_sheet(exportData.tableData[0].data);

    /* add to workbook */
    var wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "People");

    /* generate an XLSX file */
    XLSX.writeFile(wb, "sheetjs.xlsx");
}
