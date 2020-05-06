import 'core-js';
import Vue from 'vue';
import App from 'src/App.vue';
import toastr from 'toastr';

import 'materialize-css/dist/css/materialize.css';
import 'material-design-icons/iconfont/material-icons.css';
import 'toastr/build/toastr.css';

import M from 'materialize-css';

toastr.options.positionClass = 'toast-bottom-center';

new Vue({
	el      : '#app',
	mounted : () => M.AutoInit(),
	render  : (h) => h(App)
});
