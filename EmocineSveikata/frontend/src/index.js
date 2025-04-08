import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { DiscussionProvider } from './contexts/DiscussionContext';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <DiscussionProvider>
      <App />
    </DiscussionProvider>
  </React.StrictMode>
);
