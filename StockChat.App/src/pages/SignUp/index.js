import React, { Component } from "react";
import { Link, withRouter } from "react-router-dom";
import { Form, Container } from "./styles";
import { login } from "../../services/Auth";
import api from "../../services/Api";

class SignUp extends Component {
  state = {
    username: "",
    email: "",
    password: "",
    error: ""
  };

  handleSignUp = async e => {
    e.preventDefault();
    const { username, email, password } = this.state;
    if (!username || !email || !password) {
      this.setState({ error: "Fill all data to register" });
    } else {
      try {
        const response = await api.post("/auth/Register", { username, email, password });
        login(response.data);
        this.props.history.push("/chat");
      } catch (err) {
        console.log(err);
        this.setState({ error: "Error occurred while registering you account" });
      }
    }
  };

  render() {
    return (
      <Container>
        <Form onSubmit={this.handleSignUp}>
          <h2>StockChat</h2>
          {this.state.error && <p>{this.state.error}</p>}
          <input
            type="text"
            placeholder="User Name"
            onChange={e => this.setState({ username: e.target.value })}
          />
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
          <button type="submit">Register</button>
          <hr />
          <Link to="/">Login</Link>
        </Form>
      </Container>
    );
  }
}

export default withRouter(SignUp);