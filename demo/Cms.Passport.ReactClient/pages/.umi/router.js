import React from 'react';
import { Router as DefaultRouter, Route, Switch } from 'react-router-dom';
import dynamic from 'umi/dynamic';
import renderRoutes from 'umi/_renderRoutes';


let Router = DefaultRouter;


const routes = [
  {
    "component": require('../../../../../../Users/xiejian.CD/AppData/Roaming/npm/node_modules/umi/node_modules/umi-build-dev/lib/DefaultLayout.js').default,
    "routes": [
      {
        "path": "/index.html",
        "exact": true,
        "component": () => React.createElement(require('C:/Users/xiejian.CD/AppData/Roaming/npm/node_modules/umi/node_modules/umi-build-dev/lib/Compiling.js').default, { route: '/index.html' })
      },
      {
        "path": "/",
        "exact": true,
        "component": require('../index.js').default
      }
    ]
  }
];

export default function() {
  return (
<Router history={window.g_history}>
  { renderRoutes(routes) }
</Router>
  );
}
