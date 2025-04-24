import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import './Navbar.css';

const Navbar = () => {
    const location = useLocation();

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
                    <Link
                        to="/specialist/profile"
                        className="profile-link"
                    >
                        <div className="profile-icon">
                            <img
                                src="https://img.icons8.com/ios/452/user.png"
                                alt="Profile"
                                className="profile-image"
                            />

                        </div>
                    </Link>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;
