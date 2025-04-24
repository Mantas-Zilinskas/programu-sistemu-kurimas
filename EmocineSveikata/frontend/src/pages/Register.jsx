import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { register } from '../api/authApi';
import { useAuth } from '../contexts/AuthContext';
import '../styles/Auth.css';

const Register = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [role, setRole] = useState('Naudotojas');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  
  const navigate = useNavigate();
  const { setCurrentUser } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    
    if (password !== confirmPassword) {
      return setError('Slaptažodžiai nesutampa');
    }
    
    setLoading(true);
    
    try {
      const data = await register(username, email, password, confirmPassword, role);
      setCurrentUser(data);
      navigate('/');
    } catch (error) {
      setError(error.toString());
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-form-container">
        <h2>Registracija</h2>
        {error && <div className="auth-error">{error}</div>}
        
        <form onSubmit={handleSubmit} className="auth-form">
          <div className="form-group">
            <label htmlFor="username">Naudotojo vardas</label>
            <input
              type="text"
              id="username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="email">El. paštas</label>
            <input
              type="email"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="password">Slaptažodis</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="confirmPassword">Pakartokite slaptažodį</label>
            <input
              type="password"
              id="confirmPassword"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          
          <div className="form-group">
            <label>Rolė</label>
            <div className="role-selection">
              <div className="role-option">
                <input
                  type="radio"
                  id="roleUser"
                  name="role"
                  value="Naudotojas"
                  checked={role === 'Naudotojas'}
                  onChange={() => setRole('Naudotojas')}
                />
                <label htmlFor="roleUser">Naudotojas</label>
              </div>
              
              <div className="role-option">
                <input
                  type="radio"
                  id="roleSpecialist"
                  name="role"
                  value="Specialistas"
                  checked={role === 'Specialistas'}
                  onChange={() => setRole('Specialistas')}
                />
                <label htmlFor="roleSpecialist">Specialistas</label>
              </div>
            </div>
          </div>
          
          <button type="submit" className="auth-button" disabled={loading}>
            {loading ? 'Registruojama...' : 'Registruotis'}
          </button>
        </form>
        
        <div className="auth-links">
          <p>
            Jau turite paskyrą? <Link to="/login">Prisijungti</Link>
          </p>
        </div>
      </div>
    </div>
  );
};

export default Register;
