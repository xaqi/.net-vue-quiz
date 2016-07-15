/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/vue.js" />
var require_config = {
    waitSeconds: 0,
    baseUrl: "/content/front/",
    urlArgs: urlArgs,
    paths: {
        Vue: 'Scripts/Vue',
        text: 'Scripts/text',
        page: 'Scripts/page',
        vue_resource: 'Scripts/vue_resource',
        vue_focus: 'Scripts/vue_focus',
        MintLoadmore: 'Scripts/MintLoadmore',
        VueIndicator: 'Scripts/VueIndicator',
        ZoomPic: 'common/ZoomPic',
        vue_util: 'common/vue_util',
        myloadmore: 'common/myloadmore',
        rws: 'common/rws',
        wx: 'http://res.wx.qq.com/open/js/jweixin-1.0.0'
    },
    shim: {
        'vue_resource': ['Vue']
    }
};
require.config(require_config);


define('mvc', ["Vue", "text", "vue_resource", 'MintLoadmore', 'VueIndicator'], function (Vue, text, vue_resource_install, MintLoadmore, VueIndicator) {
    var vm = null;
    vue_resource_install(Vue);
    //console.log(MintLoadmore);
    Vue.component('loadmore', MintLoadmore.default);
    window.Vue = Vue;
    Vue.http.options.root = '/Api';
    Vue.http.headers.common['Authorization'] = 'Basic YXBpOnBhc3N3b3Jk';


    //VueIndicator.default.getInstance = function () { return eval('(instance)'); }.bind(VueIndicator.default);

    //console.log(VueIndicator.default.getInstance());

    var requireVm = new Vue({
        el: '#requireMsk',
        data: {
            showRequireMask: true
        }
    });
    window.requireVm = requireVm;
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


            requireVm.showRequireMask = true;
            require([jsPath, htmlPath], function (fVmOption, html) {
                requireVm.showRequireMask = false;
                if (vm && vm.onDestroy) {
                    vm.onDestroy(vm);
                }
                var vmOptions = fVmOption(this);
                vmOptions.el = options.el || vmOptions.el || '#layoutContent';
                vmOptions.template = html;
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
            Vue.nextTick(function () {
                console.log('route close');
            });
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
    //page.base('/');

    var layoutLoaded = false;
    var indexVm = null;
    var initPage = function (context, nextEnterFunc) {
        //console.log(context);
        var gameType = context.params.gameType;
        Vue.http.options.root = '/api/' + gameType;
        var controllerPath = context.params.controller;
        if (!controllerPath || controllerPath == "index") {
            controllerPath = "gamelist";
        }
        if (context.params.action) {
            controllerPath = controllerPath + "/" + context.params.action;
        }
        context.params.controllerPath = controllerPath;
        var loadLayout = function (callback) {
            //loading();
            //console.log(controllerPath);
            new mvc().render({ params: { controllerPath: 'index', gameType: gameType, subControllerPath: controllerPath } }, {
                callback: function (vm) {
                    indexVm = vm;
                    callback();
                }
            });
        };
        var loadView = function () {
            //console.log({ indexvm: indexVm });
            new mvc().render(context, { el: '#container', indexVm: indexVm });
        };
        if (!layoutLoaded) {
            loadLayout(loadView);
            layoutLoaded = true;
        }
        else {
            loadView();
        }
    };
    var loadIndex = function (context) {
        context.params.controllerPath = "login";
        new mvc().render(context, { el: '#layoutContent' });
    };
    page('/public/login', loadIndex);
    page('/:gameType', initPage)
    page('/:gameType/:controller', initPage)
    page('/:gameType/:controller/:action', initPage)
    page('/:gameType/:controller/:action/:p1', initPage)
    page('/:gameType/:controller/:action/:p1/:p2', initPage)
    page('*', initPage)
    page();
});

require(['route']);