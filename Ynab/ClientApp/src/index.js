import 'core-js';
import Vue from 'vue';
import App from 'src/App.vue';

import 'materialize-css/dist/css/materialize.css';
import 'material-design-icons/iconfont/material-icons.css';

import M from 'materialize-css';

new Vue({
	el      : '#app',
	mounted : () => M.AutoInit(),
	render  : (h) => h(App)
});
