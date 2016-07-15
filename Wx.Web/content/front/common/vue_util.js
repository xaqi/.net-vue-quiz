/// <reference path="../../Scripts/vue.js" />
define('vue_util', ['Vue'], function (Vue) {
    var emptyFn = function () { };
    Vue.filter('enum', function (value, enumTypeName) {
        var enums = {
            rouletteStatus: { "0": '竞猜中', "1": '正在开奖', "2": '已开奖', "3": '流局' },
            betStatus: { "0": '竞猜中', "1": '正在开奖', "2": '已结束', "3": '流局' }

        };
        return enums[enumTypeName][value];
    });
    Vue.filter('datetime', function (value) {
        return value.replace('T', ' ');
    });
    Vue.filter('time', function (value) {
        return value.substr(value.indexOf('T') + 1);
    });
    Vue.filter('process', function (quiz, index) {
        if (quiz.options[0].userCount == quiz.options[1].userCount) {
            return 50;
        }
        var percent = quiz.options[index].userCount * 100 / (quiz.options[0].userCount + quiz.options[1].userCount);
        return percent;
    });
    Vue.filter('persentWidth', function (value, styleName) {
        var obj = {};
        obj[styleName] = value + "%";
        return obj;
    });
    Vue.filter('money', function (value, fix) {
        if (value != 0 && !value) return 0;
        if (value > 100 && fix == 2) return value.toFixed(0);
        return value.toFixed(fix == 0 ? 0 : (fix || 2)).replace(".00", "");
    });
    //var putil = {
    //	require: function (arr) {
    //		return new Promise(function (resolve, reject) {
    //			require(arr, function () {
    //				console.log(arguments);
    //				resolve.call(this, arguments);
    //			}, function () { reject(arguments); });
    //		});

    //	},
    //	loadPopups: function (config) {
    //		var promise = this.require(['!text!' + require_config.baseUrl + 'views/util/roulette_popups.html', 'vue_focus'])
    //			.then(function (arr) {
    //				return new Promise(function (resolve, reject) {
    //					var html = arr[0];
    //					var vue_focus = arr[1];
    //					var vm = new Vue({
    //						template: html,
    //						el: '#util_container',
    //						mixins: [vue_focus.mixin],
    //						directives: { focusAuto: vue_focus.focusAuto },
    //						data: config.data,
    //						methods: {
    //							//submitClick: function () {
    //							//	resolve();
    //							//},
    //							//cancelClick: function () {
    //							//	reject();
    //							//	data.submitform.data = null;
    //							//},
    //							//messageOKClick: function () {
    //							//	data.messageform.message = null;
    //							//}
    //						},
    //						computed: {
    //							//showMask: function () {
    //							//	return data.submitform.data || data.messageform.message
    //							//},
    //							//expect_income: function () {
    //							//	if (data.submitform.fixedOdds) {
    //							//		return data.submitform.golds * data.submitform.fixedOdds;
    //							//	}
    //							//	else if (data.submitform.option0_golds != null || data.submitform.option1_golds != null) {
    //							//		return data.submitform.option1_golds / (data.submitform.option0_golds + data.submitform.golds) * data.submitform.golds;
    //							//	}
    //							//}
    //						}
    //					});

    //				});
    //			});
    //		return promise;
    //	}
    //};
    //return putil;
    //change the callbacks to promise
    var util = {
        submitOption: function (data) {
            return Vue.http.post('quiz/submit', data)
				.then(function (response) {
				    console.log("submit success", response);
				    return response;
				});;
        },
        loadPopups: function (config, callback) {
            require(['!text!' + require_config.baseUrl + 'views/util/popups.html', 'vue_focus'], function (html, vue_focus) {
                var data = {
                    submitform: {
                        data: null,
                        golds: null,
                        user_balance: null,
                        fixedOdds: null,
                        option0_golds: null,
                        option1_golds: null
                    },
                    messageform: {
                        message: null
                    }
                };
                var vm = new Vue({
                    template: html,
                    el: '#util_container',
                    mixins: [vue_focus.mixin],
                    directives: { focusAuto: vue_focus.focusAuto },
                    data: data,
                    methods: {
                        submitClick: (config.submitClick || emptyFn),
                        cancelClick: function () {
                            data.submitform.data = null;
                        },
                        messageOKClick: (config.messageOKClick || function () {
                            data.messageform.message = null;
                        })
                    },
                    computed: {
                        showMask: function () {
                            return data.submitform.data || data.messageform.message
                        },
                        expect_income: function () {
                            if (data.submitform.fixedOdds) {
                                return data.submitform.golds * data.submitform.fixedOdds;
                            }
                            else if (data.submitform.option0_golds != null || data.submitform.option1_golds != null) {
                                return data.submitform.option1_golds / (data.submitform.option0_golds + data.submitform.golds) * data.submitform.golds;
                            }
                        }
                    }
                });
                (config.callback || emptyFn).apply(vm);
            });

        },
        showSubmitForm: function (quiz, option, another_option, user_balance, callback, configs) {
            console.log(arguments);
            this.loadPopups({
                submitClick: function () {
                    var vm = this;
                    if (!(vm.submitform.golds > 0)) {
                        vm.messageform.message = "请至少投注1金币";
                        return;
                    }
                    util.submitOption({ optionId: option.optionId, golds: vm.submitform.golds, fixedOdds: option.fixedOdds }).then(function (response) {
                        vm.submitform.data = null;
                        vm.messageform.message = response.data.message;
                        console.log(response);
                    });
                },
                messageOKClick: function () {
                    var vm = this;
                    vm.messageform.message = null;
                    (callback || emptyFn)();
                },
                callback: function () {
                    var vm = this;
                    vm.submitform.user_balance = user_balance;
                    vm.submitform.fixedOdds = option.fixedOdds;
                    vm.submitform.option0_golds = option.totalGolds;
                    vm.submitform.option1_golds = another_option.totalGolds;
                    vm.submitform.data = { quiz: quiz, option: option };

                }
            });
        },
        showMessage: function (message, callback) {
            this.loadPopups({
                callback: function () {
                    var vm = this;
                    vm.messageform.message = message;
                    callback(vm);
                }
            }, callback);
        }
    };
    return util;
});