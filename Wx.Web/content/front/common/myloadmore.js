/// <reference path="../scripts/linq.js" />
define('myloadmore', ['Vue', 'VueIndicator'], function (Vue, VueIndicator) {
    Vue.component('myloadmore', {
        template: '<div class="myloadmore" @click="onclick"><loadmore :top-method="loadTop" :bottom-method="loadBottom" :bottom-all-loaded="allLoaded"><slot></slot></loadmore></div>',
        data: function () {
            return {
                startPageIndex: 0,
                endPageIndex: 0,
                totalPages: 0,
                allLoaded: false
            };
        },
        props: ['url', 'list', 'ps'],
        methods: {
            getStartPageIndex: function () {
                return this.getPages().min(function (r) { return r.index; });
            },
            getEndPageIndex: function () {
                return this.getPages().max(function (r) { return r.index; });
            },
            getPages: function () {
                var parent = this.$parent;
                var pages = parent[this.list];
                return pages;
            },
            setPages: function (arr) {
                var parent = this.$parent;
                parent[this.list] = arr;
            },
            getData: function (pageIndex, id, eventName, callback) {
                var parent = this.$parent;
                if (parent.getData) {
                    return parent.getData.apply(parent, arguments);
                }
                var url = this.url;
                //VueIndicator.default.open();
                Vue.http.get(url, { p: pageIndex, ps: this.ps }).then(function (response) {
                    //VueIndicator.default.close();
                    if (response.data.success) {
                        this.totalPages = response.data.data.totalPages;
                        var page = { index: pageIndex, rows: response.data.data.rows };
                        this.allLoaded = this.totalPages <= page.index;
                        callback(page);
                    } else {
                        alert(response.data.message);
                    }
                    this.$broadcast(eventName, id);
                }.bind(this));
            },
            loadTop: function loadTop(id) {
                var startPageIndex = this.getStartPageIndex();
                var prePageIndex = startPageIndex > 1 ? startPageIndex - 1 : 1;

                this.getData(prePageIndex, id, 'onTopLoaded', function (page) {
                    this.setPages([page]);
                }.bind(this));

            },
            loadBottom: function loadBottom(id) {
                var endPageIndex = this.getEndPageIndex();
                var nextPageIndex = endPageIndex + 1;
                this.getData(nextPageIndex, id, 'onBottomLoaded', function (page) {
                    var pages = this.getPages();
                    pages.push(page);

                }.bind(this));
            },
            onclick: function compiled() {
                //this.loadTop();
                //console.log({ s: this.getStartPageIndex(), e: this.getEndPageIndex() });
            }
        }

    });

});