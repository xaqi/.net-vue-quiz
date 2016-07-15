define('vue_pagination', [], function () {
    var vp = {
        template: '<nav class=" {{navClass}} " v-show="visible">' +
            '<ul class="pagination {{size}} " >' +
                '<li v-if="pagination.current_page > 1">' +
                    '<a href="#" aria-label="Previous" @click.prevent="changePage(1)">' +
                        '<span aria-hidden="true">First</span>' +
                    '</a>' +
                '</li>' +
                '<li v-if="pagination.current_page > 1">' +
                    '<a href="#" aria-label="Previous" @click.prevent="changePage(pagination.current_page - 1)">' +
                        '<span aria-hidden="true">&laquo;</span>' +
                    '</a>' +
                '</li>' +
                '<li v-if="pagination.current_page > 1">' +
                    '<a href="#" aria-label="Next" @click.prevent="changePage(from)">' +
                        '<span aria-hidden="true">...</span>' +
                    '</a>' +
                '</li>' +
                '<li v-for="num in data" :class="{\'active\': num == pagination.current_page}">' +
                    '<a href="#" @click.prevent="changePage(num)">{{ num }}</a>' +
                '</li>' +
                '<li v-if="pagination.current_page < pagination.total_pages">' +
                    '<a href="#" aria-label="Next" @click.prevent="changePage(to)">' +
                        '<span aria-hidden="true">...</span>' +
                    '</a>' +
                '</li>' +
                '<li v-if="pagination.current_page < pagination.total_pages">' +
                    '<a href="#" aria-label="Next" @click.prevent="changePage(pagination.current_page + 1)">' +
                        '<span aria-hidden="true">&raquo;</span>' +
                    '</a>' +
                '</li>' +
                '<li v-if="pagination.current_page < pagination.total_pages">' +
                    '<a href="#" aria-label="Next" @click.prevent="changePage(pagination.total_pages)">' +
                        '<span aria-hidden="true">Last</span>' +
                    '</a>' +
                '</li>' +
            '</ul>' +
        '</nav>',


        props: {
            pagination: {
                type: Object,
                required: true
            },
            callback: {
                type: Function,
                required: true
            },
            size: {
                type: String,
                default: ""
            },
            navClass: {
                type: String,
                default: ""
            },
            offset: {
                type: Number,
                default: 4
            },
            visible: {
                type: Number,
                default: 1
            }

        },
        computed: {
            data: function () {

                this.visible = 1;

                var from = this.pagination.current_page - this.offset;
                if (from < 1) {
                    from = 1;
                }

                var to = from + (this.offset * 2);
                if (to >= this.pagination.total_pages) {
                    to = this.pagination.total_pages;
                }
                this.from = from;
                this.to = to;

                var arr = [];
                while (from <= to) {
                    arr.push(from);
                    from++;
                }

                console.log(arr);

                if (arr.length == 1)
                    this.visible = 0

                return arr;
            }
        },
        watch: {
            'pagination.per_page': function () {
                this.callback();
            }
        },
        methods: {
            changePage: function (page) {
                this.$set('pagination.current_page', page);
                this.callback();
            }
        }
    };
    return vp;

});
