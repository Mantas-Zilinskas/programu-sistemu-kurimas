import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './components/Home';
import Discussions from './components/Discussions';
import DiscussionInside from './pages/DiscussionInside/DiscussionInside';
import DiscussionsNew from './components/DiscussionsNew';
import Login from './pages/Login';
import Register from './pages/Register';
import { AuthProvider } from './contexts/AuthContext';
import PositiveMessages from './components/PositiveMessages';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="app-container">
          <Navbar />
          <PositiveMessages />
          <main className="main-content">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/discussions" element={<Discussions />} />
              <Route path="/discussions/:id" element={<DiscussionInside />} />
              <Route path="/discussions/new" element={<DiscussionsNew />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
            </Routes>
          </main>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
