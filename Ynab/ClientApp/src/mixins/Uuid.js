let uuid = 1;
export default {
	beforeCreate() {
		this.$uuid = uuid.toString();
		uuid++;
	}
};
