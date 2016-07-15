define(['Vue'], function (Vue) {
    return function (mvc) {
        var data = {
            apiData: null
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    vm.indexVm.tabIndex = 2;
                    this.refresh();
                },
                refresh: function () {
                    Vue.http.get('art/getart', { url: 'about/' + mvc.context.params.gameType }).then(function (response) {
                        data.apiData = response.data.data;
                    }.bind(this));
                },
                onDestroy: function (vm) {
                }
            }
        };
        return vm;
    };
});