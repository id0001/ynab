// import 'core-js';
// import Vue from 'vue';
// import App from 'src/App.vue';
// import toastr from 'toastr';

// // import 'materialize-css/dist/css/materialize.css';
// // import 'material-design-icons/iconfont/material-icons.css';
// // import 'toastr/build/toastr.css';
// // import 'src/styles/global.scss';

// import M from 'materialize-css';

// toastr.options.positionClass = 'toast-bottom-center';

// new Vue({
// 	el      : '#app',
// 	mounted : () => M.AutoInit(),
// 	render  : (h) => h(App)
// });

import 'core-js';
import Vue from 'vue';
import App from '@/App.vue';

// import 'materialize-css/dist/css/materialize.css';
import 'material-design-icons/iconfont/material-icons.css';

import M from 'materialize-css';

new Vue({
	el      : '#app',
	mounted : () => M.AutoInit(),
	render  : (h) => h(App)
});
