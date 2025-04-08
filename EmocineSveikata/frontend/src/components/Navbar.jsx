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
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
