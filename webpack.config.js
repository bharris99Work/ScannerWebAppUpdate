const path = require('path');

module.exports = {
    entry: {
        capture: './wwwroot/js/capture.js',
        testCapture: './wwwroot/js/testCapture.js',
        codeTable: './wwwroot/js/codeTable.js'
    },
    output: {
        filename: '[name].bundle.js',
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
