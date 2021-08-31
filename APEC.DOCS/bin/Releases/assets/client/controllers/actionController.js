UITree.init();

var pItems = [];

$(document).ready(function () {

    CKEDITOR.replace("BriefDescription");

    var table = $("#tbl-doc-name").DataTable({
        language: {
            "sZeroRecords": "Không tìm thấy tài liệu phù hợp"
        },
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false
    });

    table.on('click',
        '.deleteDocName',
        function (e) {
            e.preventDefault();

            if (confirm("Bạn có muốn xóa không ?") == false) {
                return;
            }

            var nRow = $(this).parents('tr')[0];
            var data = table.rows(nRow).data()[0];
            var docName = data[0];
            try {
                $.ajax({
                    url: "/Home/ChangeDocumentUpload",
                    data: {
                        docName: docName
                    },
                    dataType: "json",
                    type: "POST",
                    success: function (result) {
                        if (result.Status > 0) {
                            table.row(nRow).remove().draw();
                            toastr['success']("Xóa thành công ", "Xóa!");
                        }
                        if (table.data().length === 0) {
                            $('#lblFiles').html("Tài liệu (*)");
                            $('#lblFiles').addClass("text-danger");
                            $("#Files").prop('required', true);
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

                $("#DocumentTypeId").select2ToTree({ treeData: { dataArr: list } });
                if (docTypeId !== 0) {
                    $("#DocumentTypeId").val(docTypeId).trigger('change');
                }
            },
            error: function(result) {
                alert(result.ListMenu);
            }
        });
//        Metronic.unblockUI('body');
    } catch (err) {
        alert(err);
    }

});

function BindMenu(listAll) {
    var listTree = [];
    var parentItems = listAll.filter(x => x.ParentId === 0);
    for (var i = 0; i < parentItems.length; i++) {
        var pItem = parentItems[i];
        var treeItem = {
            id: pItem.Id,
            text: pItem.Name,
            parentId: pItem.ParentId,
            inc: BindSubMenu(listAll, pItem)
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
            inc: BindSubMenu(listAll, cItem)
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