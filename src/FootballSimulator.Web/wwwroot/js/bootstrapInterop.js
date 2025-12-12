window.bootstrapInterop = {
    showModal: function (id) {
        var modal = new bootstrap.Modal(document.querySelector(id));
        modal.show();
    },
    hideModal: function (id) {
        var modalEl = document.querySelector(id);
        var modal = bootstrap.Modal.getInstance(modalEl);
        if (modal) {
            modal.hide();
        }
    }
};