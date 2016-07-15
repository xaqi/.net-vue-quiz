define(['Vue', 'wx'], function (Vue, wx) {
    return function (controller) {
        var data = {
            apiData: null,
            error: null
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    vm.indexVm.tabIndex = 2;
                    this.refresh();
                },
                refresh: function () {
                    Vue.http.post('invite/qrcode', { url: document.location.href }).then(function (response) {
                        if (response.data.success) {
                            this.apiData = response.data.data;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                onDestroy: function (vm) {
                },
                sendToFriends: function () {
                },
                shareToWechat: function () {
                    //wx.closeWindow();
                    //return;
                }
            },
            computed: {
            }
        };
        return vm;
    };
});