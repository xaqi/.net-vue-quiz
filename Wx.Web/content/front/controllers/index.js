/// <reference path="../scripts/vue.js" />
define(['vue_util', 'mvc', 'wx', 'Vue', 'VueIndicator'], function (vue_util, mvc, wx, Vue, VueIndicator) {
    return function (mvc) {
        var data = {
            navs: [
				{ cls: 'menu01', name: '竞猜', view: 'gamelist', tabIndex: 0 },
				{ cls: 'menu02', name: '英雄猜', view: 'roulette', tabIndex: 1 },
				{ cls: 'menu03', name: '个人', view: 'user', tabIndex: 2 }
            ],
            tabIndex: null,
            apiData: null,
            showLoadingMask: false
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function () {
                    Vue.http.interceptors.push({
                        request: function (request) {
                            VueIndicator.default.open();
                            data.showLoadingMask = true;
                            return request;
                        },
                        response: function (response) {
                            VueIndicator.default.close();
                            data.showLoadingMask = false;
                            return response;
                        }
                    });
                    Vue.http.post('invite/qrcode', { url: document.location.href }).then(function (response) {
                        if (response.data.success) {
                            //console.log(response.data.data);
                            var apiData = response.data.data;
                            var configData = response.data.data.jssdkUiPackage;
                            configData.debug = false;
                            configData.jsApiList = ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage'];
                            wx.config(configData);
                            wx.error(function (res) {
                                data.error = JSON.stringify(res);
                            });
                            wx.ready(function () {
                                wx.checkJsApi({
                                    jsApiList: configData.jsApiList,
                                    success: function (res) {
                                        data.error = "checkJsApi " + JSON.stringify(res);;
                                    },
                                    fail: function (res) {
                                        //alert('checkJsApi fail:' + JSON.stringify(res));
                                    }
                                });
                                wx.onMenuShareAppMessage({
                                    title: '全民菠菜share',
                                    desc: '一个专注竞猜的平台',
                                    link: 'http://domain/invite/' + apiData.userId,
                                    imgUrl: apiData.inviteQrCode,
                                    type: '',
                                    dataUrl: '',
                                    trigger: function (res) {
                                        //alert(JSON.stringify(res));
                                    },
                                    success: function () {
                                        //alert('success');
                                    },
                                    cancel: function () {
                                        //alert('cancel');
                                    },
                                    fail: function (res) {
                                        alert('fail:' + JSON.stringify(res));
                                    }
                                });
                                wx.onMenuShareTimeline({
                                    title: '全民菠菜share',
                                    link: 'http://domain/' + apiData.userId,
                                    imgUrl: apiData.inviteQrCode,
                                    trigger: function (res) {
                                        //alert(JSON.stringify(res));
                                    },
                                    success: function () {
                                        //alert('success');
                                    },
                                    cancel: function () {
                                        //alert('cancel');
                                    },
                                    fail: function (res) {
                                        alert('fail:' + JSON.stringify(res));
                                    }
                                });
                            });
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                registerShare: function () {

                },
                navItemClick: function (item) {
                    data.tabIndex = item.tabIndex;
                    return false;
                },
                closeNewUserGift: function () {
                    data.showNewUserGift = false;
                },
                closeEverydayLoginGift: function () {
                    data.showEverydayLoginGift = false;
                }
            }
        };
        return vm;
    };

});