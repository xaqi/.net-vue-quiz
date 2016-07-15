define(['Vue', 'vue_pagination', 'table_layout'], function (Vue, vue_pagination, table_layout) {
    Vue.filter('money', function (value, fix) {
        if (value != 0 && !value) return 0;
        if (value > 100 && fix == 2) return value.toFixed(0);
        return value.toFixed(fix == 0 ? 0 : (fix || 2)).replace(".00", "");
    });
    return function (controller) {
        var data = {
            registered: {},
            modal_edit_golds: null
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
                    data.modal_edit_golds = null;
                },
                save: function () {
                    alert('save');
                },
                save_edit_golds: function () {
                    Vue.http.post('/api/user/EditUserGolds', data.modal_edit_golds).then(function (response) {
                        if (response.data.message) {
                            alert(response.data.message);
                        }
                        if (response.data.success) {
                            data.modal_edit_golds = null;
                            this.refresh();
                        }
                    }.bind(this));
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
                },
                row_edit_golds: function (row) {
                    data.modal_edit_golds = { openId: row.openId, header: row.header, name: row.name, changeAmount: 0, mark: '' };
                }
            }
        };
        //setInterval(function () { data.page++; },1000);
        return vm;
    };
});