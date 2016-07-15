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
                refresh: function () {
                    this.$children[0].refresh();
                },
                close_modal: function () {
                    data.modal_data = null;
                },
                save: function () {
                    Vue.http.post('/api/channel/save', data.modal_data).then(function (response) {
                        this.refresh();
                        data.modal_data = null;
                    }.bind(this));
                }
            },
            events: {
                row_edit: function (row) {
                    console.log(row.openId);
                    data.modal_data = row;
                },
                row_delete: function (row) {
                    console.log(row.openId);
                },
                btnAddlick: function () {
                    data.modal_data = {};
                }
            }
        };
        return vm;
    };
});