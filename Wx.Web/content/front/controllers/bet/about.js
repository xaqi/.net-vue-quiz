define(['Vue'], function (Vue) {
    return function (controller) {
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
                    Vue.http.get('art/getbetabout').then(function (response) {
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