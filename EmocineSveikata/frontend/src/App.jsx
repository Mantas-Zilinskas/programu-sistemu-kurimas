import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './components/Home';
import Discussions from './components/Discussions';
import DiscussionsNew from './components/DiscussionsNew';
import Rooms from './components/Rooms';
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
            <Route path="/discussions/new" element={<DiscussionsNew />} />
            <Route path="/rooms" element={<Rooms />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
