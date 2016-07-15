define(['Vue', 'page'], function (Vue, page) {
    return function (mvc) {
        var data = {
            apiData: null,
            showMask: false,
            messageform: {
                message: null
            },
            submit: {
                alipay: '',
                phone: '',
                detail: '',
                golds: 0
            },
            submit_done: false
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    vm.indexVm.tabIndex = 2;
                    this.refresh();
                },
                refresh: function () {
                    Vue.http.get('Coin/InitRedeemFormData').then(function (response) {
                        if (response.data.success) {
                            data.submit = response.data.data.submit;
                            data.apiData = response.data.data;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                btnSubmitClick: function () {
                    var _this = this;
                    data.submit.golds = mvc.context.params.p1;
                    Vue.http.post('Coin/RedeemSubmit', data.submit)
						.then(function (response) {
						    _this.showMessage(response.data.message);
						    data.submit_done = true;
						});
                },
                showMessage: function (message) {
                    data.showMask = true;
                    data.messageform.message = message;
                },
                messageOKClick: function () {
                    data.messageform.message = null;
                    data.showMask = false;
                    if (data.submit_done) {
                        page.show('/' + mvc.context.params.gameType + '/coin/record');
                    }
                }
            }
        };
        return vm;
    };
});