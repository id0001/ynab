const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const VueLoaderPlugin = require('vue-loader/lib/plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

const resolve = (...pathSegments) => path.resolve(__dirname, ...pathSegments);

module.exports = {
	mode      : 'development',
	entry     : {
		app : resolve('src', 'index.js')
	},
	output    : {
		path     : resolve('dist'),
		filename : '[name].bundle.js'
	},
	resolve   : {
		alias : {
			src  : resolve('src'),
			vue$ : 'vue/dist/vue.esm.js'
		}
	},
	module    : {
		rules : [
			{
				test   : /\.vue$/,
				loader : 'vue-loader'
			},
			{
				test    : /\.js$/,
				exclude : /node_modules/,
				use     : {
					loader : 'babel-loader'
				}
			},
			{
				test : /\.css$/,
				use  : [
					'vue-style-loader',
					'css-loader'
				]
			},
			{
				test : /\.s[ac]ss$/,
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
	devServer : {
		hot  : true,
		port : 8080
	},
	plugins   : [
		new HtmlWebpackPlugin({
			template : resolve('index.html')
		}),
		new VueLoaderPlugin(),
		new CleanWebpackPlugin()
	]
};
