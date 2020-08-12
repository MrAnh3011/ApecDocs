UITree.init();

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
                    url: "/Admin/Home/ChangeDocumentUpload",
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
        
        $.ajax({
            url: "/Admin/Home/GetListMenu",
            data: {
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                
                var listAll = result.ListMenu;
                var list = BindMenu(listAll);
                $('#DocumentTypeId').select2({
                    placeholder: 'Mời bạn chọn',
                    data: { results: list, text: "name" },
                    formatSelection: function (item) {
                        return item.text;
                    },
                    formatResult: function (item) {
                        return item.text;
                    }
                });
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


