define(['Vue'], function (Vue) {
    return function (controller) {
        var data = {
            selectedGame: "Dota2",
            selectedGameHref: '/dota'
        };
        var vm = {
            data: data,
            methods: {
                afterRender: function (vm) {
                },
                onChClick: function (gameName, href) {
                    data.selectedGame = gameName;
                    data.selectedGameHref = href;


                },
                onDestroy: function (vm) {
                },
                btnEnterClick: function () {
                    document.location.href = data.selectedGameHref;
                },
                btnBackClick: function () {
                    history.go(-1);
                }
            }
        };
        return vm;
    };
});