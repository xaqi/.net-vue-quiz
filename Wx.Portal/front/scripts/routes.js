/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/vue.js" />
var require_config = {
    waitSeconds: 0,
    baseUrl: "/front/",
    urlArgs: urlArgs,
    paths: {
        Vue: 'Scripts/Vue',
        text: 'Scripts/text',
        page: 'Scripts/page',
        vue_resource: 'Scripts/vue_resource',
        vue_focus: 'Scripts/vue_focus',
        ZoomPic: 'common/ZoomPic',
        vue_util: 'common/vue_util',
        rws: 'common/rws',
        table_layout: 'common/table_layout'
    },
    shim: {
        'vue_resource': ['Vue']
    }
};
require.config(require_config);


define('mvc', ["Vue", "text", "vue_resource"], function (Vue, text, vue_resource_install) {
    var vm = null;
    vue_resource_install(Vue);
    window.Vue = Vue;
    Vue.http.options.root = '/Api';
    Vue.http.headers.common['Authorization'] = 'Basic YXBpOnBhc3N3b3Jk';

    var mvc = function () {
        return this;
    };
    mvc.prototype = {
        context: {},
        render: function (context, options) {
            options = options || {};
            this.context = context;
            var jsPath = require_config.baseUrl + 'controllers/' + context.params.controllerPath + '.js';
            var htmlPath = 'text!' + require_config.baseUrl + 'views/' + context.params.controllerPath + '.html';
            require([jsPath, htmlPath], function (fVmOption, html) {
                if (vm && vm.onDestroy) {
                    vm.onDestroy(vm);
                }
                var vmOptions = fVmOption(this);
                vmOptions.el = options.el || vmOptions.el || '#layoutContent';
                vmOptions.template = html;
                //console.log(vmOptions);
                vm = new Vue(vmOptions);
                for (key in options) {
                    vm[key] = options[key];
                }
                if (vm.afterRender) {
                    vm.afterRender(vm);
                }
                if (options.callback) {
                    options.callback(vm);
                }
            }.bind(this), this.errorHandler);
        },
        errorHandler: function (e) {
            //new Vue({
            //	el: '#layoutContent',
            //	template: '<div><h1>404</h1><p>{{errorMessage}}</p></div>',
            //	data: {
            //		errorMessage: e.message
            //	}
            //});
            console.log(e);
        }
    };
    return mvc;
});

define('route', ["Vue", "text", "page", "mvc"], function (Vue, text, page, mvc) {
    var loading = function () {
        new Vue({
            el: '#layoutContent',
            template: '<h1>loading...</h1>',
            data: {
                loading: true
            }
        })
    };

    var initPage = function (context, nextEnterFunc) {
        var controller = context.params.controller || "home";
        var action = context.params.action || "index";
        context.params.controllerPath = controller + "/" + action;
        new mvc().render(context, { el: '#page-content' });
    };

    page('/sample', initPage)
    page('/sample/:controller', initPage)
    page('/sample/:controller/:action', initPage)
    page('/sample/:controller/:action/:id', initPage)
    page('/:controller', initPage)
    page('/:controller/:action', initPage)
    page('/:controller/:action/:id', initPage)
    page('*', initPage)
    page();
});

require(['route']);