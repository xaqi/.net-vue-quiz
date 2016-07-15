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
                    Vue.http.get('Invite/GetMyFriends').then(function (response) {
                        if (response.data.success) {
                            data.apiData = response.data.data;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                onDestroy: function (vm) {
                }
            }
        };
        return vm;
    };
});