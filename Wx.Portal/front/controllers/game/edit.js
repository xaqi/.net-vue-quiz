/// <reference path="../../scripts/linq.js" />
define(['Vue', 'vue_pagination', 'table_layout'], function (Vue, vue_pagination, table_layout) {
    return function (controller) {
        console.log(controller);
        var data = {
            id: controller.context.params.id,
            apiData: null,
            modal_bet: null,
            modal_quiz: null
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                    if (data.id > 0) {
                        this.refresh();
                    }
                },
                refresh: function () {
                    Vue.http.get('/api/game/getdetail', { gameid: data.id }).then(function (response) {
                        console.log(response.data);
                        data.apiData = response.data;
                        //data.bets = response.data;

                    }.bind(this));
                },
                btnAddBetClick: function () {
                    data.modal_bet = {
                        name: '第' + (data.apiData.bets.length + 1) + '局',
                        startTime: '2016-05-23 00:00:00',
                        endTime: '2016-05-23 00:00:00',
                        displayOrder: 0
                    };
                    //var newBet = {
                    //    name: '第' + (data.apiData.bets.length + 1) + '回合',
                    //    quizs: []
                    //};
                    //data.apiData.bets.push(newBet);
                },
                btnEditBetClick: function (row) {
                    data.modal_bet = row;
                },
                btnRemoveBetClick: function (bet) {
                    if (!confirm('确认删除?')) return;

                    Vue.http.post('/api/bet/delete', { id: bet.betId }).then(function (response) {
                        this.refresh();
                    }.bind(this));

                    //var bets = data.apiData.bets;
                    //data.apiData.bets = bets.where(function (x) { return x != bet; });
                    //console.log(bets);
                },
                btnAddQuizClick: function (bet) {
                    var modal_quiz = { betId: bet.betId, displayOrder: 0 };
                    data.modal_quiz = modal_quiz;
                },
                display: function (options) {
                    return options.select(function (r) { return r.subject; });
                    return 1;
                },
                close_modal: function () {
                    data.modal_bet = null;
                    data.modal_quiz = null;
                },
                saveBet: function () {
                    var postData = JSON.parse(JSON.stringify(data.modal_bet));
                    postData.gameId = data.id;
                    Vue.http.post('/api/bet/save', postData).then(function (response) {
                        this.refresh();
                        data.modal_bet = null;
                    }.bind(this));
                },
                saveQuiz: function () {
                    var postData = JSON.parse(JSON.stringify(data.modal_quiz));
                    Vue.http.post('/api/quiz/save', postData).then(function (response) {
                        this.refresh();
                        data.modal_quiz = null;
                    }.bind(this));
                },
                quizEditClick: function (quiz) {
                    data.modal_quiz = JSON.parse(JSON.stringify(quiz));
                },
                quizDeleteClick: function (quiz, bet) {
                    if (!confirm('确认删除?')) return;
                    Vue.http.post('/api/quiz/delete', { id: quiz.quizId }).then(function (response) {
                        this.refresh();
                    }.bind(this));

                },
                btnEditOptionClick: function (option) {
                    var subject = prompt("请输入选项", option.subject);
                    if (!subject) return;
                    Vue.http.post('/api/option/save', { subject: subject, optionId: option.optionId }).then(function (response) {
                        this.refresh();
                    }.bind(this));

                },
                btnDeleteOptionClick: function (option) {
                    if (!confirm('确认删除?')) return;
                    Vue.http.post('/api/option/delete', { id: option.optionId }).then(function (response) {
                        this.refresh();
                    }.bind(this));
                },
                btnAddOptionClick: function (quiz) {
                    var subject = prompt("请输入选项", "");
                    if (!subject) return;
                    Vue.http.post('/api/option/save', { subject: subject, quizId: quiz.quizId }).then(function (response) {
                        this.refresh();
                    }.bind(this));
                },
                btnResolveOptionClick: function (option) {
                    if (!confirm('确认结算?')) return;
                    Vue.http.post('/api/option/resolve', { optionId: option.optionId }).then(function (response) {
                        this.refresh();
                        alert(response.data.message);
                    }.bind(this));
                }

            }
        };
        return vm;
    };
});