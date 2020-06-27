import Vue from "vue";

const state = Vue.observable({
  authenticated: false,
  initialized: false,
  id: null
});

const actions = {
  login() {
    window.location = "/login";
  },
  logout() {
    window.location = "/logout";
  },
  async init() {
    if (state.initialized) return;

    let response = await fetch("/user");
    switch (response.status) {
      case 200:
        setAuthenticated(await response.json());
        break;
      case 401:
        setAuthenticated(null);
    }
  }
};

export default {
  get authenticated() {
    return state.authenticated;
  },
  ...actions
};

function setAuthenticated(user) {
  if (!user) {
    state.authenticated = false;
    state.id = null;
    return;
  }

  state.authenticated = true;
  state.id = user.id;
}
