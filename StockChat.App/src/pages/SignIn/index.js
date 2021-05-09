import React, { Component } from "react";
import { Link, withRouter } from "react-router-dom";
import { login } from "../../services/Auth";
import api from "../../services/Api";

import { Form, Container } from "./styles";

class SignIn extends Component {
  state = {
    email: "",
    password: "",
    error: ""
  };

  handleSignIn = async e => {
    e.preventDefault();
    const { email, password } = this.state;
    if (!email || !password) {
      this.setState({ error: "Fill email and password to continue" });
    } else {
      try {
        const response = await api.post("/auth/Login", { email, password });
        login(response.data);
        this.props.history.push("/chat");
      } catch (err) {
        this.setState({
          error:
            "There was a poblem login in, check you credentials"
        });
      }
    }
  };

  render() {
    return (
      <Container>
        <Form onSubmit={this.handleSignIn}>
          <h2>StockChat</h2>
          {this.state.error && <p>{this.state.error}</p>}
          <input
            type="email"
            placeholder="Email"
            onChange={e => this.setState({ email: e.target.value })}
          />
          <input
            type="password"
            placeholder="Password"
            onChange={e => this.setState({ password: e.target.value })}
          />
          <button type="submit">Login</button>
          <hr />
          <Link to="/signup">Register</Link>
        </Form>
      </Container>
    );
  }
}

export default withRouter(SignIn);