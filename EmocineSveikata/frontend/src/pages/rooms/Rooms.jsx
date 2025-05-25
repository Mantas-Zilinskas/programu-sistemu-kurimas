import React, { useState, useEffect } from 'react';
import { fetchAvailableRooms, bookRoom } from '../../api/roomApi.js';
import './Rooms.css';

const Rooms = () => {
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [bookingStatus, setBookingStatus] = useState({});

  useEffect(() => {
    loadRooms();
  }, []);

  const loadRooms = async () => {
    setLoading(true);
    try {
      const data = await fetchAvailableRooms();
      setRooms(data);
    } catch (err) {
      setError('Failed to fetch rooms');
    } finally {
      setLoading(false);
    }
  };

  const handleBookRoom = async (roomId) => {
    setBookingStatus((prev) => ({ ...prev, [roomId]: 'loading' }));
    
    try {
      const result = await bookRoom(roomId);
      setBookingStatus((prev) => ({ 
        ...prev, 
        [roomId]: { 
          status: 'success', 
          message: 'Kambarys rezervuotas!',
          meetLink: result.meetLink 
        } 
      }));
      
      await loadRooms();
      
    } catch (err) {
      let errorMessage = 'Rezervacija nepavyko.';
      
      if (err.response?.data?.message) {
        errorMessage = err.response.data.message;
      } else if (err.response?.data?.error) {
        errorMessage = err.response.data.error;
      } else if (err.message) {
        errorMessage = err.message;
      }
      
      setBookingStatus((prev) => ({ 
        ...prev, 
        [roomId]: { 
          status: 'error', 
          message: errorMessage 
        } 
      }));
    }
  };

  const formatDate = (date) => date.split('T')[0];
  const formatTime = (time) => time.substring(0, 5);

  const getBookingStatusDisplay = (roomId) => {
    const status = bookingStatus[roomId];
    
    if (!status) return null;
    
    if (status === 'loading') {
      return <p className="booking-loading">⏳ Booking...</p>;
    }
    
    if (status.status === 'success') {
      return (
        <div className="booking-success">
          <p>✅ {status.message}</p>
          {status.meetLink && (
            <a 
              href={status.meetLink} 
              target="_blank" 
              rel="noopener noreferrer"
              className="meet-link"
            >
              Join Meeting
            </a>
          )}
        </div>
      );
    }
    
    if (status.status === 'error') {
      return <p className="booking-error">❌ {status.message}</p>;
    }
    
    return null;
  };

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loading-spinner"></div>
      </div>
    );
  }

  if (error) {
    return <div className="error-message">Error: {error}</div>;
  }

  return (
    <div className="rooms-container">
      <div className="rooms-top">
        <h1 className="rooms-title">Galimi kambariai</h1>
      </div>
      {rooms.map((room) => (
        <div key={room.id} className="room-card" onClick={() => handleBookRoom(room.id)}>
          <div className="room-header">
            <h2 className="room-creator-name">{room.specialistName}</h2>
            <div className="profile-picture">
              <img src={room.profilePicture || 'default-pic.jpg'} alt="Profilio nuotrauka" />
            </div>
            <h4 className="room-time">
              {formatDate(room.date)}, {formatTime(room.startTime)} – {formatTime(room.endTime)}
            </h4>
          </div>
          <p className="room-creator-bio">{room.bio}</p>
          {getBookingStatusDisplay(room.id)}
        </div>
      ))}
    </div>
  );
};

export default Rooms;
