import {
	MdApp,
	MdToolbar,
	MdContent,
	MdButton,
	MdField,
	MdLayout,
	MdMenu,
	MdList,
	MdSubheader,
	MdProgress
} from 'vue-material/dist/components';

const MaterialPlugin = {
	install : function(Vue) {
		Vue.use(MdApp);
		Vue.use(MdToolbar);
		Vue.use(MdContent);
		Vue.use(MdButton);
		Vue.use(MdField);
		Vue.use(MdLayout);
		Vue.use(MdMenu);
		Vue.use(MdList);
		Vue.use(MdSubheader);
		Vue.use(MdProgress);
	}
};

export default MaterialPlugin;
