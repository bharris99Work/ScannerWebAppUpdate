const path = require('path');

module.exports = {
    entry: './wwwroot/js/capture.js',
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'wwwroot/dist')
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            }
        ]
    },
    resolve: {
        extensions: ['.js']
    },
    mode: 'development'
};
