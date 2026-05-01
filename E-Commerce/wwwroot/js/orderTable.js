$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const orderStatus = urlParams.get('orderStatus');

    $('#order-table').DataTable({
        ajax: {
            url: `/Admin/Order/GetAllInJSON?orderStatus=${orderStatus}`
        },
        columns: [
            { data: "id" },
            { data: "name" },
            { data: "phoneNumber" },
            { data: "applicationUser.email" },
            { data: "orderStatus" },
            { data: "orderTotal" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-info mx-2 rounded-2">
                            <i class="bi bi-pencil-square"></i> Summary Details
                        </a>`;
                }
            }
        ]
    });
});