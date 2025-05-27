import React, { useState, useEffect } from 'react';
import { fetchAvailableRooms, bookRoom, getMyBookedRooms } from '../../api/roomApi.js';
import './Rooms.css';
import TextFieldModal from '../../components/TextFieldModal';
import { sendNotification } from '../../api/notificationApi';
import EmailIcon from '@mui/icons-material/Email';
import { useAuth } from '../../contexts/AuthContext';

const Rooms = () => {
  const [availableRooms, setAvailableRooms] = useState([]);
  const [bookedRooms, setBookedRooms] = useState([]);
  const [activeTab, setActiveTab] = useState('available');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [bookingStatus, setBookingStatus] = useState({});
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [handleSubmit, setHandleSubmit] = useState(() => { });
  const { currentUser } = useAuth();

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [availableData, bookedData] = await Promise.all([
        fetchAvailableRooms(),
        getMyBookedRooms().catch(() => [])
      ]);
      setAvailableRooms(availableData);
      setBookedRooms(bookedData);
    } catch (err) {
      setError('Failed to fetch rooms');
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = async (id) => {
    setHandleSubmit(() =>
      (content) => {
        sendNotification(id, content);
        setModalIsOpen(false);
        alert("pranešimas išsiųstas");
      }
    );
    setModalIsOpen(true);
  }

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
      
      await loadData();
      
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

  const formatDate = (date) => new Date(date).toLocaleDateString('lt-LT');
  const formatTime = (time) => time.substring(0, 5);
  const formatDateTime = (dateTime) => new Date(dateTime).toLocaleString('lt-LT');

  const isUpcoming = (date, startTime) => {
    const roomDateTime = new Date(`${date.split('T')[0]}T${startTime}`);
    return roomDateTime > new Date();
  };

  const getBookingStatusDisplay = (roomId) => {
    const status = bookingStatus[roomId];
    
    if (!status) return null;
    
    if (status === 'loading') {
      return <p className="booking-loading">⏳ Rezervuojama...</p>;
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

  const upcomingBookedRooms = bookedRooms.filter(room => isUpcoming(room.date, room.startTime));
  const pastBookedRooms = bookedRooms.filter(room => !isUpcoming(room.date, room.startTime));

  return (
    <div className="rooms-container">
      <div className="rooms-top">
<<<<<<< HEAD
        <h1 className="rooms-title">Kambariai</h1>
        
        {/* Tab Navigation */}
        <div className="tab-navigation">
          <button 
            className={`tab-button ${activeTab === 'available' ? 'active' : ''}`}
            onClick={() => setActiveTab('available')}
          >
            Galimi kambariai ({availableRooms.length})
          </button>
          <button 
            className={`tab-button ${activeTab === 'booked' ? 'active' : ''}`}
            onClick={() => setActiveTab('booked')}
          >
            Mano kambariai ({bookedRooms.length})
          </button>
=======
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
              {currentUser && <EmailIcon className="message-button-icon" onClick={() => handleOpenModal(room.specialistId)} />}
            </div>
            <h4 className="room-time">{formatDate(room.date) + ', ' + formatTime(room.startTime) + ' – ' + formatTime(room.endTime)}</h4>
          </div>
          <p className="room-creator-bio">
            {room.bio}
          </p>
>>>>>>> f136ebb (PSK-28 small fixes)
        </div>
      </div>

      {/* Available Rooms Tab */}
      {activeTab === 'available' && (
        <div className="tab-content">
          {availableRooms.length === 0 ? (
            <div className="no-rooms">
              <p>Šiuo metu nėra galimų kambarių.</p>
            </div>
          ) : (
            availableRooms.map((room) => (
              <div key={room.id} className="room-card clickable" onClick={() => handleBookRoom(room.id)}>
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
            ))
          )}
        </div>
      )}

      {/* Booked Rooms Tab */}
      {activeTab === 'booked' && (
        <div className="tab-content">
          {bookedRooms.length === 0 ? (
            <div className="no-rooms">
              <p>Neturite rezervuotų kambarių.</p>
            </div>
          ) : (
            <>
              {/* Upcoming Meetings */}
              {upcomingBookedRooms.length > 0 && (
                <div className="booked-section">
                  <h2 className="section-title">Artėjantys susitikimai</h2>
                  {upcomingBookedRooms.map((room) => (
                    <div key={room.id} className="room-card booked-room upcoming">
                      <div className="room-header">
                        <h3 className="room-creator-name">{room.specialistName}</h3>
                        <div className="profile-picture">
                          <img src={room.profilePicture || 'default-pic.jpg'} alt="Profilio nuotrauka" />
                        </div>
                        {currentUser && <EmailIcon className="message-button-icon" onClick={() => handleOpenModal(room.specialistId)} />}
                        <h4 className="room-time">
                          {formatDate(room.date)}, {formatTime(room.startTime)} – {formatTime(room.endTime)}
                        </h4>
                      </div>
                      <p className="room-creator-bio">{room.bio}</p>
                      <div className="booking-info">
                        <p className="booked-at">Rezervuota: {formatDateTime(room.bookedAt)}</p>
                        {room.meetLink && (
                          <a 
                            href={room.meetLink} 
                            target="_blank" 
                            rel="noopener noreferrer"
                            className="meet-link-button"
                          >
                            Prisijungti prie susitikimo
                          </a>
                        )}
                      </div>
                    </div>
                  ))}
                </div>
              )}

              {/* Past Meetings */}
              {pastBookedRooms.length > 0 && (
                <div className="booked-section">
                  <h2 className="section-title">Praėję susitikimai</h2>
                  {pastBookedRooms.map((room) => (
                    <div key={room.id} className="room-card booked-room past">
                      <div className="room-header">
                        <h3 className="room-creator-name">{room.specialistName}</h3>
                        <div className="profile-picture">
                          <img src={room.profilePicture || 'default-pic.jpg'} alt="Profilio nuotrauka" />
                        </div>
                        {currentUser && <EmailIcon className="message-button-icon" onClick={() => handleOpenModal(room.specialistId)} />}
                        <h4 className="room-time">
                          {formatDate(room.date)}, {formatTime(room.startTime)} – {formatTime(room.endTime)}
                        </h4>
                      </div>
                      <p className="room-creator-bio">{room.bio}</p>
                      <div className="booking-info">
                        <p className="booked-at">Rezervuota: {formatDateTime(room.bookedAt)}</p>
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </>
          )}
        </div>
      )}
      <TextFieldModal
        isOpen={modalIsOpen}
        onRequestClose={() => setModalIsOpen(false)}
        handleSubmit={handleSubmit}
        title="Palikite atsiliepimą specialistui"
      />
    </div>
  );
};

export default Rooms;
