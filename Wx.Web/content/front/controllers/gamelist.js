/// <reference path="../scripts/vue.js" />
define(['vue_util'], function (vue_util) {
    if (!window.WebSocket) {
        //alert('no ws');
    } else {
        //alert('ws');
    }
    return function (controller) {
        var data = {
            submitform: {
                data: null,
                golds: null
            },
            successform: {
                show: false
            },
            data: null
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    vm.indexVm.tabIndex = 0;
                    this.refresh();
                },
                refresh: function () {
                    Vue.http.get('quiz/getList').then(function (response) {
                        if (response.data.success) {
                            this.data = response.data.data;
                        } else {
                            alert(response.data.message);
                        }
                    }.bind(this));
                },
                optionClick: function (quiz, option, another_option) {
                    if (quiz.quizstatus != 0) return;
                    vue_util.showSubmitForm(quiz, option, another_option, data.data.user.user_balance, this.refresh.bind(this));
                },
                avatarImageClick: function () {
                }
            },
            computed: {
                showMask: function () {
                    return this.showSubmitForm || data.successform.show;
                },
                showSubmitForm: function () {
                    return data.submitform.data;
                }
            }
        };
        return vm;

    };
});