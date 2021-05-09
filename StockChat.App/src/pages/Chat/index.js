import React, { Component } from 'react';
import * as signalR from "@aspnet/signalr";
import api, { apiURL} from '../../services/Api'
import { getToken, getUserId } from "../../services/Auth";

class Chat extends Component {
  constructor(props) {
    super(props);

    this.state = {
      id: '',
      message: '',
      messages: [],
      hubConnection: null,
    };
  }

  componentDidMount = async () => {
    const response = await api.get('/chat/messages');

    for (let item of response.data)
    {
        const text = `${item.user.userName}: ${item.text}`;
        const messages = this.state.messages.concat([text]);
        this.setState({ messages })
        window.scrollTo(0,document.body.scrollHeight);
    }

    const id = getUserId();
    const token = getToken();
    const hubConnection = new signalR.HubConnectionBuilder()

    .withUrl(apiURL + '/chatHub?token=' + token)
    .build();
    this.setState({ hubConnection, id }, () => {
      this.state.hubConnection
        .start()
        .then(() => console.log('Connection started!'))
        .catch(err => console.log('Error while establishing connection :('));

      this.state.hubConnection.on('ReceiveMessage', (user, receivedMessage) => {
        const text = `${user.userName}: ${receivedMessage}`;
        const messages = this.state.messages.concat([text]);
        this.setState({ messages });
        window.scrollTo(0,document.body.scrollHeight);
      });
    });
  };

  sendMessage = (e) => {
    if (e.type === 'keydown' && e.key !== 'Enter')
      return;
    console.log(e)
    this.state.hubConnection
      .invoke('SendMessage', this.state.id, this.state.message)
      .catch(err => console.error(err));

      this.setState({message: ''});      
  };

  render() {
    return (
      <div style={{width: '50%', margin: '0 auto'}}>
        <br />

        <div>
          {this.state.messages.map((message, index) => {
            var liStyles = {
              backgroundColor: ( index % 2 === 1 ) ? '#ddd' : '#efefef',
              padding: '1rem',
              borderBottom: '1px solid #ddd',
              display: 'block'
           };
            return (
              <span style={liStyles} key={index}> {message} </span>
            );
          })}
        </div >
            <input
              style={{width: '90%', height: '30px', marginBottom: '30px'}}
              type="text"
              value={this.state.message}
              onChange={e => this.setState({ message: e.target.value })}
              onKeyDown={this.sendMessage}
            />
    
            <button style={{width: '10%', height: '30px', backgroundColor: '#0db8d6', color: 'white', marginBottom: '30px'}}onClick={this.sendMessage}>Send</button>
      </div>
    );
  }
}

export default Chat;