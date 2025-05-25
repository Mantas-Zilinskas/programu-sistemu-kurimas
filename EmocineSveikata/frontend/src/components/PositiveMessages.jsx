import React, { useState, useEffect, useRef } from 'react';
import { useAuth } from '../contexts/AuthContext';
import './PositiveMessages.css';
import { fetchPositiveMessage } from '../api/positiveMessagesApi.js';
import { v4 as uuid } from 'uuid';

const MESSAGE_INTERVAL = 10 * 60 * 1000; // 10 min delay between messages

const PositiveMessages = () => {
  const { currentUser } = useAuth();
  const [messages, setMessages] = useState([]);
  const intervalRef = useRef(null);

  useEffect(() => {
    if (currentUser && currentUser.user) {
      intervalRef.current = setInterval(fetchMessage, MESSAGE_INTERVAL);
      return () => clearInterval(intervalRef.current);
    }
  }, [currentUser]);

  const fetchMessage = async () => {
    try {
      const { message } = await fetchPositiveMessage(currentUser);
      if (message && message.length !== 0) {
        const newMsg = { id: uuid(), text: message };
        setMessages((prev) => [...prev, newMsg]);
      }
    } catch (err) {
      console.error('Failed to fetch positive message:', err);
    }
  };

  const closeOne = (id) => {
    setMessages((prev) => prev.filter((m) => m.id !== id));
  };

  return (
    <>
      {messages.map((m, idx) => (
        <div
          key={m.id}
          className="message-popup"
          style={{ bottom: 20 + idx * 80 }}>
          <div className="message-content">
            {m.text}
            <button className="close-button" onClick={() => closeOne(m.id)}>✖</button>
          </div>
        </div>
      ))}
    </>
  );
};

export default PositiveMessages;
