import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { logout } from '../api/authApi';
import './Navbar.css';

const Navbar = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { currentUser, setCurrentUser } = useAuth();

    const handleLogout = () => {
        logout();
        setCurrentUser(null);
        navigate('/login');
    };

    return (
        <nav className="navbar">
            <div className="navbar-container">
                <Link to="/" className="navbar-brand">
                    Emocinė Sveikata
                </Link>
                <div className="navbar-menu">
                    <Link
                        to="/"
                        className={`navbar-link ${location.pathname === '/' ? 'navbar-link-active' : ''}`}
                    >
                        Pagrindinis
                    </Link>
                    <Link
                        to="/discussions"
                        className={`navbar-link ${location.pathname === '/discussions' ? 'navbar-link-active' : ''}`}
                    >
                        Diskusijos
                    </Link>
                </div>
                <div className="navbar-auth">
                    {currentUser && currentUser.user ? (
                        <>
                            <span className="navbar-user">
                                <Link
                                    to={currentUser.user.role.toLowerCase() === 'specialistas' ? "/specialistprofile" : "/userprofile"}
                                    className="username-link"
                                >
                                    <span className="username">{currentUser.user.username}</span>
                                </Link>
                                <span className="role-badge">{currentUser.user.role}</span>
                            </span>
                            <button onClick={handleLogout} className="navbar-button logout-button">
                                Atsijungti
                            </button>
                        </>
                    ) : (
                        <>
                            <Link to="/login" className="navbar-button login-button">
                                Prisijungti
                            </Link>
                            <Link to="/register" className="navbar-button register-button">
                                Registruotis
                            </Link>
                        </>
                    )}
                </div>
            </div>
        </nav>
    );
};

export default Navbar;
