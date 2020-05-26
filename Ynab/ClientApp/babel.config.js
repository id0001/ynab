module.exports = {
  presets: [
    [
      "@babel/preset-env",
      {
        useBuiltIns: "entry",
        corejs: 3
      }
    ]
  ],
  plugins: [
    [
      "@babel/transform-runtime",
      {
        helpers: false,
        regenerator: true
      }
    ]
  ]
};
