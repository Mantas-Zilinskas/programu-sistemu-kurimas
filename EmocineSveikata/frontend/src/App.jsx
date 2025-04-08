import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Discussions from './components/Discussions';
import './App.css';

function App() {
  return (
    <Router>
      <div className="app-container">
        <Navbar />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Discussions />} />
            <Route path="/discussions" element={<Discussions />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
