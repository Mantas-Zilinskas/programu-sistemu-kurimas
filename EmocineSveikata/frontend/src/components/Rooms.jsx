import React, { useState } from 'react';
import { useRooms } from '../contexts/RoomsContext';
import './Rooms.css';

const Rooms = () => {
  const { rooms, loading, error } = useRooms();

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
      // TODO: specialists' cards here
    </div>
  );
};

export default Rooms;
