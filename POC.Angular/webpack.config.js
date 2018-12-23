const isDevBuild = true;
const path = require('path');
const webpack = require('webpack');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const CleanWebpackPlugin = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
module.exports = {
    mode: isDevBuild ? 'development' : 'production',
    devtool: isDevBuild ? 'inline-source-map' : false,
    resolve: {
        extensions: ['.js', '.jsx', '.ts',  '.json', '.css', '.scss', '.html']
    },
    entry: {
        'polyfills': './ClientApp/app/polyfills.ts',
        'vendor': './ClientApp/app/vendor.ts',
        'main-client': isDevBuild ? './ClientApp/main.ts' : './ClientApp/main.ts'
    },
    output: {
        filename: '[name].js',
        publicPath: '/dist/',
        path: path.join(__dirname, './wwwroot/dist')
    },
    optimization: {
        minimizer: [
            new UglifyJsPlugin({
                cache: true,
                parallel: true,
                sourceMap: false // set to true if you want JS source maps
            }),
            new OptimizeCSSAssetsPlugin({})
        ],
        splitChunks: {
            cacheGroups: {
                commons: {
                    test: /[\\/]node_modules[\\/]/,
                    name: 'vendor',
                    chunks: 'all'
                }
            }
        },
        sideEffects: false 
    },
    module: {
        rules: [{
            test: isDevBuild ? /\.ts$/ : /(?:\.ngfactory\.js|\.ngstyle\.js|\.ts)$/,
            use: isDevBuild ? [{
                loader: 'ts-loader',
                options: {
                    configFile: 'ClientApp/tsconfig.json'
                }
            },
                'angular-router-loader',
                'angular2-template-loader'
            ] : ['@ngtools/webpack']
        },
        {
            test: /\.html$/,
            loader: 'raw-loader'
        },
        {
            test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|eot|ttf|otf)$/,
            use: [{
                loader: 'url-loader',
                options: { limit: 100000 }
            }]
        },
        { test: /jquery\.flot\.resize\.js$/, loader: 'imports-loader?this=>window' },
        { test: /\.css$/, loaders: ['to-string-loader', 'css-loader'] },
        { test: /\.scss$/, loaders: ['to-string-loader', 'css-loader', 'sass-loader'] },
        { test: /\.json$/, loader: 'json-loader', exclude: [/node_modules/] }
        ]
    },
    plugins: []
        .concat([
            new CleanWebpackPlugin(['./wwwroot/dist/']),
            new MiniCssExtractPlugin({
                filename: 'style.[contenthash].css'
            }),
            new webpack.ProvidePlugin({
                $: 'jquery',
                jQuery: 'jquery',
                'window.jQuery': 'jquery',
                Popper: ['popper.js', 'default']

            })
        ])
};