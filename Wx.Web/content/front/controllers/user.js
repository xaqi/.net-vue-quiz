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
                    Vue.http.get('User/getUserInfo').then(function (response) {
                        if (vm.destroyed) return;
                        if (response.data.success) {
                            data.apiData = response.data.data;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                btnChangeGameClick: function (href) {
                    document.location.href = href;
                },
                onDestroy: function (vm) {
                    vm.destroyed = true;
                }
            }
        };
        return vm;
    };
});