import Vue from 'vue';

const _store = Vue.observable({
	authenticated : false,
	id            : null,
	initialized   : false
});

function setAuthenticated(user) {
	_store.authenticated = true;
	_store.id = user.id;
}

function unsetAuthenticated() {
	_store.authenticated = false;
	_store.id = null;
}

export default {
	get authenticated() {
		return _store.authenticated;
	},
	get id() {
		return _store.id;
	},
	async init() {
		if (_store.initialized) return;

		let response = await fetch('/user');
		switch (response.status) {
			case 200:
				setAuthenticated(await response.json());
				break;
			case 201:
				unsetAuthenticated();
				break;
			default:
				throw Error('WTF');
		}

		_store.initialized = true;
	},
	login() {
		Window.location = '/Login';
	}
};
