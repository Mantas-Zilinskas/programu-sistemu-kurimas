import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './components/Home';
import Discussions from './components/Discussions';
import DiscussionInside from './pages/DiscussionInside/DiscussionInside';
import DiscussionsNew from './components/DiscussionsNew';
import './App.css';

function App() {
  return (
    <Router>
      <div className="app-container">
        <Navbar />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/discussions" element={<Discussions />} />
            <Route path="/discussions/:id" element={<DiscussionInside />} />
            <Route path="/discussions/new" element={<DiscussionsNew />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
