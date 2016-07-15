define(['Vue'], function (Vue) {
    function onBridgeReady() {

    }
    if (typeof WeixinJSBridge == "undefined") {
        if (document.addEventListener) {
            document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
        } else if (document.attachEvent) {
            document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
            document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
        }
    } else {
        onBridgeReady();
    }

    return function (controller) {
        var data = {
            apiData: null,
            rmb: 0,
            golds: 0,
            giveGolds: 0,
            showMask: false,
            other_golds: null,
            messageform: {
                message: null
            },
            showTempPay: true
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    vm.indexVm.tabIndex = 2;
                    this.refresh();
                },
                refresh: function () {
                    Vue.http.get('Coin/GetPayInfo').then(function (response) {
                        if (response.data.success) {
                            //console.log(response.data.data);
                            data.apiData = response.data.data;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                payItemClick: function (rmb, golds) {
                    //console.log(rmb);
                    data.rmb = rmb || 0;
                    data.golds = golds || 0;
                    data.giveGolds = data.golds - data.rmb * 100;
                    data.other_golds = null;
                },
                other_golds_change: function () {
                    data.rmb = data.other_golds;
                    data.golds = data.other_golds * 100;
                    data.giveGolds = data.other_golds * 1;
                },
                btnSubmitClick: function () {
                    data.showTempPay = true;
                    return;
                    var rmb = data.rmb || 0;
                    if (rmb <= 0) {
                        this.showMessage("请选择充值金额。");
                        return;
                    }
                    var _this = this;
                    new Promise(function (resolve, reject) {
                        if (!WeixinJSBridge) { reject("浏览器不支持!"); }
                        resolve();
                    }).then(function () {
                        data.showMask = true;
                        return Vue.http.get('Coin/GetWeixinPayOption')
                    }).then(function (response) {
                        return new Promise(function (resolve, reject) {
                            //console.log(response);
                            var payConfig = response.data;
                            var result = WeixinJSBridge.invoke('getBrandWCPayRequest', payConfig,
							   function (res) {
							       if (res.err_msg == "get_brand_wcpay_request:ok") {
							           resolve("支付成功！");
							       } else {
							           //console.log({ "reject": res });
							           reject("支付失败:" + (res.err_desc || JSON.stringify(res) || "未知错误。"));
							       }
							   }
							);
                        });
                    }).then(function (message) {
                        _this.showMessage(message);
                        _this.refresh();
                    }).catch(function (error) {
                        _this.showMessage(error);
                        //console.log(error);
                    });
                },
                showMessage: function (message) {
                    data.showMask = true;
                    data.messageform.message = message;
                },
                messageOKClick: function () {
                    data.messageform.message = null;
                    data.showMask = false;
                },
                closeTempPay: function () {
                    data.showTempPay = false;
                }
            }
        };
        return vm;
    };
});