$(document).ready(function () {
    $('#company-table').DataTable({
        ajax: {
            url: '/Admin/Company/GetAllInJSON'
        },
        columns: [
            { data: "name" },
            { data: "phoneNumber" },
            { data: "streetAddress" },
            { data: "city" },
            { data: "state" },
            { data: "postalCode" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/Admin/Company/Details/Edit/${data}" class="btn btn-info mx-2 rounded-2">
                            <i class="bi bi-pencil-square"></i> Edit Company
                        </a>
                        <a onclick="DeleteConfirmation('/Admin/Company/Delete/${data}')"
                           class="btn btn-danger mx-2 rounded-2">
                            <i class="bi bi-trash-fill"></i> Delete Company
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
                    $('#company-table').DataTable().ajax.reload(null, false);
                    toastr.success(data);
                },
                error: function (xhr) {
                    toastr.error(xhr.responseText || "Delete failed");
                }
            });
        }
    });
}
