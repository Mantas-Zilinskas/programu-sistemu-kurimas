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
import SpecialistProfile from './pages/SpecialistProfile';
import UserProfile from './pages/UserProfile';
import PrivateRoute from './components/PrivateRoute';
import SpecialistCalendar from './pages/SpecialistCalendar';
import EditDiscussion from './pages/editDiscussion/EditDiscussions'
import './App.css';

function App() {
    return (
        <AuthProvider>
            <Router>
                <div className="app-container">
                    <Navbar />
                    <main className="main-content">
                        <Routes>
                            <Route path="/" element={<Home />} />
                            <Route path="/discussions" element={<Discussions />} />
                            <Route path="/discussions/:id" element={<DiscussionInside />} />
                            <Route path="/discussions/new" element={<DiscussionsNew />} />
                            <Route path="/editDiscussion/:id" element={<EditDiscussion />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/register" element={<Register />} />

                            {/* Protected Routes */}
                            <Route
                                path="/specialistprofile"
                                element={
                                    <PrivateRoute roleRequired="specialistas">
                                        <SpecialistProfile />
                                    </PrivateRoute>
                                }
                            />
                            <Route
                                path="/userprofile"
                                element={
                                    <PrivateRoute roleRequired="naudotojas">
                                        <UserProfile />
                                    </PrivateRoute>
                                }
                            />
                            <Route
                                path="/specialist-calendar"
                                element={
                                    <PrivateRoute roleRequired="specialistas">
                                        <SpecialistCalendar />
                                    </PrivateRoute>
                                }
                            />
                        </Routes>

                    </main>
                </div>
            </Router>
        </AuthProvider>
    );
}

export default App;
