$(document).ready(function () {
    $('#product-table').DataTable({
        ajax: {
            url: '/Admin/Product/GetAllInJSON'
        },
        columns: [
            { data: "title" },
            { data: "isbn" },
            { data: "author" },
            { data: "listPrice" },
            { data: "category.name" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/Admin/Product/Details/Edit/${data}" class="btn btn-info mx-2 rounded-2">
                            <i class="bi bi-pencil-square"></i> Edit Product
                        </a>
                        <a onclick="DeleteConfirmation('/Admin/Product/Delete/${data}')"
                           class="btn btn-danger mx-2 rounded-2">
                            <i class="bi bi-trash-fill"></i> Delete Product
                        </a>`;
                }
            }
        ]
    });
});


function DeleteConfirmation(url) {
    console.log("Triggered Swal");
    Swal.fire({
        title: "Are you sure want to delete?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    console.log("AJAX success", data);
                    $('#product-table').DataTable().ajax.reload(null, false);
                    toastr.success(data);
                },
                error: function (xhr) {
                    toastr.error(xhr.responseText || "Delete failed");
                }
            });
        }
    });
}
