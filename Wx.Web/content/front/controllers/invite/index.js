define(['Vue'], function (Vue) {
	return function (controller) {
		var data = {
			apiData: null
		};
		var vm = {
			data: data,
			methods: {
				afterRender: function (vm) {
					vm.indexVm.tabIndex = 2;
					this.refresh();
				},
				refresh: function () {
					Vue.http.get('User/getUserInfo').then(function (response) {
						if (response.data.success) {
							//console.log(response.data.data);
							data.apiData = response.data.data;
						} else {
							alert(response.data.message);
						}
					}.bind(this));
				},
				onDestroy: function (vm) {
				}
			}
		};
		return vm;
	};
});