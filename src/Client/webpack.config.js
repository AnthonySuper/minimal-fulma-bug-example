var path = require("path");
var webpack = require("webpack");
var fableUtils = require("fable-utils");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyWebpackPlugin = require('copy-webpack-plugin');

function resolve(filePath) {
    return path.join(__dirname, filePath)
}

var babelOptions = fableUtils.resolveBabelOptions({
    presets: [
        ["env", {
            "targets": {
                "browsers": ["last 2 versions"]
            },
            "modules": false
        }]
    ],
    plugins: ["transform-runtime"]
});


var isProduction = process.argv.indexOf("-p") >= 0;
var port = process.env.SUAVE_FABLE_PORT || "8085";
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

var commonPlugins = [
    new HtmlWebpackPlugin({
        filename: './index.html',
        template: './public/index.html'
    })
];
module.exports = {
    devtool: "source-map",
    entry : [ "whatwg-fetch", "babel-polyfill", resolve('./Client.fsproj'), './scss/main.scss' ],
    //entry: [ resolve('./Client.fsproj'), './scss/main.scss' ],
    mode: isProduction ? "production" : "development",
    // output: {
    //     path: resolve('./public/js'),
    //     publicPath: "/js",
    //     filename: "bundle.js"
    // },
    output: {
        path: resolve('./public/js'),
        publicPath: "/js",
        filename: "bundle.js"
    },
    resolve: {
        symlinks: false,
        modules: [resolve("../../node_modules/")]
    },
    
    devServer: {
        proxy: {
            '/api/*': {
                target: 'http://localhost:' + port,
                changeOrigin: true
            }
        },
        contentBase: "./public",
        hot: true,
        inline: true
    },
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        babel: babelOptions,
                        define: isProduction ? [] : ["DEBUG"]
                    }
                }
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: babelOptions
                },
            },
            {
                test: /\.(sass|scss|css)$/,
                use: [
                    isProduction
                        ? MiniCssExtractPlugin.loader
                        : 'style-loader',
                    'css-loader',
                    'sass-loader',
                ],
            }
            // ,            
            // {
            //     test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)(\?.*$|$)/,
            //     use: ["file-loader"]
            // }
        ]
    },
    plugins: isProduction ? [new MiniCssExtractPlugin({
        filename: '[name].css',
        chunkFilename: '[name]-[id].css'
    })] : [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin(),
        
    ]
};
