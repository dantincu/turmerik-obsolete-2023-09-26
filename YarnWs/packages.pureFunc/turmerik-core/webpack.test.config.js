const path = require('path');

module.exports = {
  entry: './src/test-index.ts',
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
    ],
  },
  resolve: {
    extensions: ['.tsx', '.ts', '.js'],
  },
  output: {
    filename: 'test-bundle.js',
    path: path.resolve(__dirname, 'dist'),
  },
};