define(['Vue', 'vue_pagination', 'table_layout'], function (Vue, vue_pagination, table_layout) {
    return function (controller) {
        var data = {
            registered: {},
            modal_data: null
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                },
                close_modal: function () {
                    data.modal_data = null;
                },
                save: function () {
                    alert('save');
                }
            },
            events: {
                row_edit: function (row) {
                    //alert('edit row');
                    console.log(row.openId);
                    data.modal_data = row;
                },
                row_delete: function (row) {
                    //alert('edit row');
                    console.log(row.openId);
                }
            }
        };
        //setInterval(function () { data.page++; },1000);
        return vm;
    };
});