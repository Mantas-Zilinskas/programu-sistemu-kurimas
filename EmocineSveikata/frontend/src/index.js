import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { DiscussionProvider } from './contexts/DiscussionContext';
import { DiscussionNewProvider } from './contexts/DiscussionNewContext';
import { RoomsProvider } from './contexts/RoomsContext';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <DiscussionProvider>
      <DiscussionNewProvider>
        <RoomsProvider>
          <App />
        </RoomsProvider>
      </DiscussionNewProvider>
    </DiscussionProvider>
  </React.StrictMode>
);
