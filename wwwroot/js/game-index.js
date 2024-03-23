$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);
        console.log(btn.data('id'));
        const swal = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-danger mx-2',
                cancelButton: 'btn btn-light'
            },
            buttonsStyling: false
        });
        swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, cancel!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/Games/Delete/${btn.data('id')}`,
                    method: 'DELETE',
                    success: function () {
                        swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        );
                        btn.closest('tr').remove();
                    },
                    error: function () {
                        swal.fire(
                            'Error!',
                            'There was an error deleting the game.',
                            'error'
                        );
                    }
                });
            }
        });
    })
})