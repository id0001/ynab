const path = require('path');
const HtmlWebPackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const { VueLoaderPlugin } = require('vue-loader');

module.exports = {
	mode      : 'development',
	entry     : path.resolve(__dirname, 'src', 'index.js'),
	output    : {
		path     : path.resolve(__dirname, 'build'),
		filename : 'bundle.js'
	},
	resolve   : {
		alias : {
			src : path.resolve(__dirname, 'src')
		}
	},
	devServer : {
		contentBase : path.resolve(__dirname),
		hot         : true,
		port        : 8080
	},
	module    : {
		rules : [
			{
				test    : /\.vue$/i,
				include : path.resolve(__dirname, 'src'),
				use     : 'vue-loader'
			},
			{
				test    : /\.js$/i,
				include : path.resolve(__dirname, 'src'),
				use     : 'babel-loader'
			},
			{
				test : /\.css/i,
				use  : [
					'style-loader',
					'css-loader'
				]
			},
			{
				test : /\.s[ac]ss$/i,
				use  : [
					'vue-style-loader',
					'css-loader',
					'sass-loader'
				]
			},
			{
				test : /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
				use  : [
					{
						loader  : 'file-loader',
						options : {
							name       : '[name].[ext]',
							outputPath : 'fonts/'
						}
					}
				]
			}
		]
	},
	plugins   : [
		new HtmlWebPackPlugin({
			template : path.resolve(__dirname, 'index.html')
		}),
		new CleanWebpackPlugin(),
		new VueLoaderPlugin()
	]
};
