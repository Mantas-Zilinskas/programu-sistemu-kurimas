import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Rooms.css';

const Rooms = () => {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchAvailableRooms();
  }, []);
  
  const fetchAvailableRooms = async () => {
    setLoading(true);
    try {
      const response = await fetch('/api/Room/available');
      const data = await response.json();
      setRooms(data);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch rooms');
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loading-spinner"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-message">
        Error: {error}
      </div>
    );
  }

  return (
    <div className="rooms-container">
      <div className="rooms-top">
        <h1 className="rooms-title">Naujausi kambariai</h1>
      </div>
      {rooms.map((room) => (
        <div key={room.id} className="room-card">
          <div className="room-header">
            <h2 className="room-creator-name">{room.specialistName}</h2>
            <div className="profile-picture">
              <img src={room.profilePicture || 'default-pic.jpg'} alt="Profilio nuotrauka" />
            </div>
          </div>
          <p className="room-bio">
            {room.bio}
          </p>
        </div>
      ))}
    </div>
  );
};

export default Rooms;
