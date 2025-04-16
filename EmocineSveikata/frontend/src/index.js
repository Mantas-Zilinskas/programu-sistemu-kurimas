import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { DiscussionProvider } from './contexts/DiscussionContext';
import { DiscussionNewProvider } from './contexts/DiscussionNewContext';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <DiscussionProvider>
      <DiscussionNewProvider>
        <App />
      </DiscussionNewProvider>
    </DiscussionProvider>
  </React.StrictMode>
);
