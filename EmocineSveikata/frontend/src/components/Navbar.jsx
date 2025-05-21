import React, { useEffect, useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { logout } from '../api/authApi';
import { fetchNotifications } from '../api/notificationApi';
import './Navbar.css';

const Navbar = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { currentUser, setCurrentUser } = useAuth();
	const [unreadCount, setUnreadCount] = useState(0);

	useEffect(() => {
        const loadNotifications = async () => {
            try {
                if (currentUser) {
                    const data = await fetchNotifications();
                    const unread = data.filter(n => !n.isRead).length;
                    setUnreadCount(unread);
                }
            } catch (err) {
                console.error('Failed to fetch notifications', err);
            }
        };

        loadNotifications();
    }, [location.pathname, currentUser]);

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
                    <Link
                        to="/rooms" 
                        className={`navbar-link ${location.pathname === '/rooms' ? 'navbar-link-active' : ''}`}
                    >
                        Kambariai
                   </Link>
                </div>
                <div className="navbar-auth">
                    {currentUser && currentUser.user ? (
                        <>
							<Link
								to="/notifications"
								className={`navbar-link ${location.pathname === '/notifications' ? 'navbar-link-active' : ''}`}
							>
								Pranešimai
								{unreadCount > 0 && <span className="notification-dot" />}
							</Link>
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
