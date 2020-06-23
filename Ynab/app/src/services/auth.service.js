class AuthService {
	constructor() {}
}

export default {
	install(Vue, options = {}) {
		Vue.auth = new AuthService();
	}
};
