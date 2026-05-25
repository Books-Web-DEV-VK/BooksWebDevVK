$(document).ready(function () {
    $('#user-table').DataTable({
        ajax: {
            url: '/Admin/User/GetAllInJSON'
        },
        columns: [
            { data: "name" },
            { data: "email" },
            { data: "phoneNumber" },
            {
                data: "company",
                render: function (data) {
                    console.log(data);
                    return data ? data.name : "Individual";
                }
            },
            { data: "role" }, 
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                render: function (data) {
                    var isLocked = new Date(data.lockoutEnd).getTime() > new Date().getTime();
                    var content = "";
                    if (isLocked) {
                        content = `<a href="/Admin/User/LockUnLock?userId=${data.id}" class="btn btn-info mx-2 rounded-2">
                            <i class="bi bi-pencil-square"></i> UnLock User
                        </a>`;
                    }
                    else
                    {
                        content = `<a href="/Admin/User/LockUnLock?userId=${data.id}" class="btn btn-info mx-2 rounded-2">
                            <i class="bi bi-pencil-square"></i> Lock User
                        </a>`;
                    }

                    content += `<a href="/Admin/User/ManageUserRole?userId=${data.id}" class="btn btn-danger mx-2 rounded-2">
                        <i class="bi bi-pencil-square"></i> Manage Roles
                    </a>`;
                    return content;
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
                    $('#user-table').DataTable().ajax.reload(null, false);
                    toastr.success(data);
                },
                error: function (xhr) {
                    toastr.error(xhr.responseText || "Delete failed");
                }
            });
        }
    });
}
