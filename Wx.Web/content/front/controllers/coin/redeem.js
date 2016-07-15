define(['Vue', 'page'], function (Vue, page) {
    return function (mvc) {
        var data = {
            apiData: null,
            requireGolds: 0,
            showMask: false,
            messageform: {
                message: null
            }
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    vm.indexVm.tabIndex = 2;
                    this.refresh();
                },
                refresh: function () {
                    Vue.http.get('Coin/GetRedeemInfo').then(function (response) {
                        if (response.data.success) {
                            //console.log(response.data.data);
                            data.apiData = response.data.data;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                reddemItemClick: function (golds) {
                    data.requireGolds = golds || 0;
                },
                btnSubmitClick: function () {
                    var golds = data.requireGolds || 0;
                    if (golds <= 0) {
                        this.showMessage("请选择红包数额。");
                        return;
                    }
                    page.show('/' + mvc.context.params.gameType + '/coin/redeem_submit/' + golds);

                    return;
                    var _this = this;
                    Vue.http.post('Coin/RedeemSubmit', { golds: golds })
						.then(function (response) {
						    _this.showMessage(response.data.message);
						    _this.refresh();
						});
                },
                showMessage: function (message) {
                    data.showMask = true;
                    data.messageform.message = message;
                },
                messageOKClick: function () {
                    data.messageform.message = null;
                    data.showMask = false;
                }
            }
        };
        return vm;
    };
});