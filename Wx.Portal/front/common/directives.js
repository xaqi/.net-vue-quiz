define('directives', ['Vue'], function (Vue) {
    Vue.directive('register', {
        update: function (newValue) {
            var html = this.el.innerHTML;
            Vue.component(newValue, {
                template: html,
                props: ['rows'],
                methods: {
                    dispatch: function (name) {
                        this.$dispatch.apply(name, arguments);
                        //this.$dispatch(name, Array.prototype.slice.call(arguments, 1));

                    }
                }
            });
            this.vm.$set('registered.' + newValue, true);
        }
    });
});