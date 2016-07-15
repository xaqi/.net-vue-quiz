define(['Vue', 'vue_pagination', 'table_layout'], function (Vue, vue_pagination, table_layout) {
    return function (controller) {
        var data = {
            registered: {},
            modal_data: null,
            apiData: {}
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    Vue.http.get('/api/team/getList', { ps: 100 }).then(function (response) {
                        data.apiData.teams = response.data.rows;
                    }.bind(this));
                },
                refresh: function () {
                    //console.log(this);
                    this.$children[0].refresh();
                },
                close_modal: function () {
                    data.modal_data = null;
                },
                save: function () {
                    Vue.http.post('/api/game/save', data.modal_data).then(function (response) {
                        this.refresh();
                        data.modal_data = null;
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
                btnAddGameClick: function () {
                    var newGame = { game_typeId: 1 };
                    data.modal_data = newGame;
                }
            }
        };
        //setInterval(function () { data.page++; },1000);
        return vm;
    };
});