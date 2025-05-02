import React, { useState, useEffect, useRef } from 'react';
import './PositiveMessages.css';
import { v4 as uuid } from 'uuid';

const MESSAGE_INTERVAL = 1 * 10 * 1000; // For testing, only 1 min delay between messages
const STORAGE_KEY = 'lastMessageTimestamp';

const PositiveMessages = () => {
  const [messages, setMessages] = useState([]);
  const intervalRef = useRef(null);

  const fetchMessage = async () => {
    try {
      const res = await fetch('/api/positiveMessages/random');
      if (!res.ok) throw new Error('Network error');
      const { message } = await res.json();
      if (message) {
        const newMsg = { id: uuid(), text: message };
        setMessages((prev) => [...prev, newMsg]);
        localStorage.setItem(STORAGE_KEY, Date.now().toString());
      }
    } catch (err) {
      console.error('Failed to fetch:', err);
    }
  };

  useEffect(() => {
    const lastTs = parseInt(localStorage.getItem(STORAGE_KEY), 10) || 0;
    const now = Date.now();
    const delta = now - lastTs;

    intervalRef.current = setInterval(fetchMessage, MESSAGE_INTERVAL);
    return () => clearInterval(intervalRef.current);
  }, []);

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
