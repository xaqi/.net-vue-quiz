define('directives', ['Vue'], function (Vue) {
    Vue.directive('register', {
        update: function (newValue) {
            var html = this.el.innerHTML;
            Vue.component(newValue, {
                template: html,
                props: ['rows', 'pagination'],
                methods: {
                    dispatch: function (name, args) {
                        this.$dispatch(name, args);
                        //console.log(this.$dispatch);
                        //this.$dispatch.apply(this, Array.prototype.slice.call(arguments, 0));
                        //this.$dispatch.apply(name, Array.prototype.slice.call(arguments, 1));
                        //

                    }
                }
            });
            this.vm.$set('registered.' + newValue, true);
        }
    });
});

define('table_layout', ['text!/front/common/html/table_layout.html', 'directives', 'vue_pagination'], function (html, ds, vue_pagination) {
    Vue.component('pagination', vue_pagination);
    Vue.component('table-layout', {
        template: html,
        props: ['table_template', 'url', 'table_name'],
        data: function () {
            var data = {
                rows: [],
                pagination: {
                    total: 0,
                    per_page: 20,
                    current_page: 1,
                    total_pages: 0,
                    start: 0
                },
                loading: false
            };
            return data;
        },
        methods: {
            refresh: function () {
                var data = this._data;
                //data.url = '/api/home/getc';
                console.log(this);

                data.loading = true;
                Vue.http.get(this._props.url.raw, { p: data.pagination.current_page, ps: data.pagination.per_page }).then(function (response) {
                    data.rows = response.data.rows;
                    data.pagination.total_pages = Math.ceil(response.data.total / data.pagination.per_page);
                    data.pagination.start = (data.pagination.current_page - 1) * data.pagination.per_page;
                    data.loading = false;
                }.bind(this));
            },
            page_callback: function () {
                this.refresh();
                this.$dispatch('page_callback', this.msg);
            }
        },
        events: {
            'hook:created': function () {
                this.refresh();
            }
        }
    });
    return html;
});