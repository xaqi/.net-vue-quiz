/// <reference path="../../Scripts/vue.js" />
define('rws', ['Vue'], function (Vue) {
	var connectionToken = null;
	var ws = null;
	var rws = {
		processMessage: function (M) {
			for (var i = 0; i < M.length; i++) {
				var m = M[i];
				var hub = m.H;
				var method = m.A[0];
				var args = m.A[1];
				var func = rws["on" + method] || function () { };
				console.log({ "on": method, args: args });
				func.apply(this, args);
			}
		},
		invoke: function (hubName, method, args) {
			if (!ws) return;
			var str = JSON.stringify({ "H": hubName, "M": method, "A": args, "I": 0 });
			ws.send(str);
		}
	};
	var timeout = 500;
	var retryTime = 0;
	var maxRetry = 10;
	var forceClose = false;
	var closed = true;
	rws.open = function () {
		if (!closed) return;
		rws.close();
		Vue.http.get('/signalr/negotiate?clientProtocol=1.4&connectionData=%5B%7B"name"%3A"roulette"%7D%5D')
			.then(function (response) {
				if (response.data && response.data.ConnectionToken)
					connectionToken = window.encodeURIComponent(response.data.ConnectionToken);
				return Vue.http.get('/signalr/start?transport=webSockets&clientProtocol=1.4&' +
					'connectionToken=' + connectionToken +
					'&connectionData=%5B%7B"name"%3A"roulette"%7D%5D')
			})
			.then(function (response) {
				if (response.data && response.data.ConnectionToken)
					connectionToken = window.encodeURIComponent(response.data.ConnectionToken);
				var url = "ws://" + location.host + '/signalr/connect?transport=webSockets&clientProtocol=1.4&connectionToken=' +
				connectionToken
				+ '&connectionData=%5B%7B%22name%22%3A%22roulette%22%7D%5D&tid=3';
				ws = new WebSocket(url, []);
				forceClose = false;
				ws.onmessage = function (customEvent) {
					var data = eval("(" + customEvent.data + ")");
					if (data.M) {
						//console.log(data);
						rws.processMessage(data.M);
					}
				};
				ws.onerror = function () {
					console.log({ error: arguments });
				}
				ws.onconnecting = function () {
					console.log({ onconnecting: arguments });
				}
				ws.onclose = function () {
					console.log({ onclose: arguments, fc: forceClose });
					closed = true;
					if (forceClose) return;
					console.log('re conn');
					retryTime++;
					if (retryTime > maxRetry) return;
					setTimeout(function () { rws.open() }, timeout * Math.pow(2, retryTime));
				}
				ws.onopen = function () {
					retryTime = 0;
					console.log({ onopen: arguments });
				}
				rws.ws = ws;
			});
	};
	rws.close = function () {
		if (!ws) return;
		forceClose = true;
		ws.close();
		rws.ws = null;
	};
	return rws;
});