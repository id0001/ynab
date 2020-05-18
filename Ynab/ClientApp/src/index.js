import 'core-js';
import Vue from 'vue';
import App from '@/App.vue';

import 'material-design-icons/iconfont/material-icons.css';

import M from 'materialize-css';

new Vue({
	el      : '#app',
	mounted : () => M.AutoInit(),
	render  : (h) => h(App)
});
