define(['Vue', 'VueIndicator', 'myloadmore'], function (Vue, VueIndicator, ml) {
    return function (mvc) {
        var baseUrl = 'bet/history/';
        var tabIndex = parseInt(mvc.context.params["p1"] || "0");
        //console.log(tabIndex);
        var data = {
            apiData: null,
            pages: [],
            total: 0,
            ps: 10,
            tabIndex: tabIndex,
            tabs: [
                { index: 0, name: '主播竞猜', href: baseUrl + '0' },
                { index: 1, name: '赛事竞猜', href: baseUrl + '1' },
                { index: 2, name: '英雄猜', href: baseUrl + '2' }
            ]
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    vm.indexVm.tabIndex = 2;
                    this.refresh();
                },
                refresh: function () {
                    this.pages = [];
                    Vue.http.get('Bet/getBetHistory', { p: 1, ps: data.ps, tabIndex: data.tabIndex }).then(function (response) {
                        if (response.data.success) {
                            data.apiData = response.data.data;
                            var page = response.data.data.page;
                            data.pages.push({ index: 1, rows: page.rows });
                            data.total = page.total;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                }
            }
        };
        return vm;
    };
});