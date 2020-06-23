import Vue from 'vue';
import App from 'src/App.vue';
import MaterialPlugin from 'src/material';

// Import the vue-material css
import 'vue-material/dist/vue-material.min.css';
import 'vue-material/dist/theme/default.css';

// Import the global scss.
import 'src/styles/global.scss';

export default async function main() {
	Vue.use(MaterialPlugin);
	return new Vue({
		render : (h) => h(App)
	}).$mount('#app');
}

main();
