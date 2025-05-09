import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

// PrivateRoute Component: Conditional Rendering Based on Role
const PrivateRoute = ({ roleRequired = 'none', children }) => {
    const { currentUser } = useAuth();

    if (!currentUser) {
        return <Navigate to="/login" />; // Redirect to login if no user is logged in
    }

    if (!currentUser.user) {
        return <div>Kraunama...</div>; // Show loading while user data is being fetched
    }

    if (currentUser.user.role.toLowerCase() !== 'none') {
      return children;
    }

    if (currentUser.user.role.toLowerCase() !== roleRequired.toLowerCase()) {
        return <Navigate to="/" />; // Redirect to home if the user doesn't have the required role
    }

    return children; // If user has correct role, render the page
};

export default PrivateRoute;
