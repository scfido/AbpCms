export default {
    "publicPath": "/dist/",
    "proxy": {
      "/api": {
        "target": "http://localhost:5000/",
        "changeOrigin": true,
      }
    },
  }