import React, { Component } from 'react';
import './App.css';
import Oidc from "oidc-client";

const config = {
  authority: "http://localhost:5000",
  client_id: "js",
  redirect_uri: `${window.location.origin}/callback.html`,
  response_type: "id_token token",
  scope: "openid profile default-api",
  post_logout_redirect_uri: `${window.location.origin}/`,
};

const mgr = new Oidc.UserManager(config);

class App extends Component {
  constructor() {
    super();
    this.state = { loginStatus: "未登录" };
  }

  componentDidMount() {
    let self = this;
    mgr.getUser()
      .then(function (user) {
        if (user) {
          self.setState({
            loginStatus: "User logged in",
            userProfile: user.profile
          });
        }
        else {
          self.setState({
            loginStatus: "User not logged in",
            userProfile: null
          });
        }
      });
  }

  login() {
    mgr.signinRedirect();
  }

  api() {
    let self = this;
    self.get("http://localhost:5001/api/values")
      .then(function (json) {
        self.setState({ apiResult: json });
      });

  }

  getRoles() {
    let self = this;
    self.get("http://localhost:5000/api/services/app/Role/GetAll")
      .then(function (json) {
        self.setState({ apiResult: JSON.stringify(json.result.items) });
      });
  }

  logout() {
    mgr.signoutRedirect();
  }

  get(url) {
    let self = this;

    return mgr.getUser()
      .then(function (user) {

        var access_token = "";
        if (user)
          access_token = user.access_token;

        return fetch(url, {
          headers: {
            "Authorization": "Bearer " + access_token
          },
        })
          .then(function (response) {
            if (response.status !== 200) {
              self.setState({ apiResult: `${response.status} ${response.statusText}` });
              return Promise.reject();
            }
            else {
              return response;
            }
          })
          .then(function (response) {
            return response.json();
          })
      });
  }

  render() {
    return (
      <div className="App">
        <button id="login" onClick={this.login.bind(this)}>Login</button>
        <button id="roles" onClick={this.getRoles.bind(this)}>获取角色</button>
        <button id="api" onClick={this.api.bind(this)}>Call API</button>
        <button id="logout" onClick={this.logout.bind(this)}>Logout</button>

        <pre id="results">{this.state.loginStatus}
        </pre>

        <pre id="apiResult">{this.state.apiResult}
        </pre>
      </div>
    );
  }
}

export default App;
