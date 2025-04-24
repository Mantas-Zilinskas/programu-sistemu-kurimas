import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './components/Home';
import Discussions from './components/Discussions';
import DiscussionInside from './pages/DiscussionInside/DiscussionInside';
import DiscussionsNew from './components/DiscussionsNew';
import SpecialistProfile from './pages/specialists/SpecialistProfile';
import SpecialistProfileCreation from './pages/specialists/SpecialistProfileCreation';
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
                        <Route path="/specialist/profile" element={<SpecialistProfile />} />
                        <Route path="/specialist/profilecreation" element={<SpecialistProfileCreation />} />
                    </Routes>
                </main>
            </div>
        </Router>
    );
}

export default App;
