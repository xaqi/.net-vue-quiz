define(['Vue'], function (Vue) {
	return function (controller) {
		var data = {
		};
		var vm = {
			data: data,
			methods: {
				afterRender: function (vm) {
					vm.indexVm.tabIndex = 2;
				},
				onDestroy: function (vm) {
				}
			}
		};
		return vm;
	};
});