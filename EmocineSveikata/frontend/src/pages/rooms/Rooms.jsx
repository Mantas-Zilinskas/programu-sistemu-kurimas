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
  
  // Format date as YYYY-MM-DD
  const formatDate = (date) => {
    return date.split('T')[0];
  };

  // Format time as HH:00
  const formatTime = (time) => {
    return time.substring(0, 5);
  };

  return (
    <div className="rooms-container">
      <div className="rooms-top">
        <h1 className="rooms-title">Galimi kambariai</h1>
      </div>
      {rooms.map((room) => (
        <div key={room.id} className="room-card">
          <div className="room-header">
            <div className="room-header">
              <h2 className="room-creator-name">{room.specialistName}</h2>
              <div className="profile-picture">
                <img src={room.profilePicture || 'default-pic.jpg'} alt="Profilio nuotrauka" />
              </div>
            </div>
            <h4 className="room-time">{formatDate(room.date) + ', ' + formatTime(room.startTime) + ' – ' + formatTime(room.endTime)}</h4>
          </div>
          <p className="room-creator-bio">
            {room.bio}
          </p>
        </div>
      ))}
    </div>
  );
};

export default Rooms;
