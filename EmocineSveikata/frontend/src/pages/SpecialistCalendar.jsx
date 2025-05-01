import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import axios from 'axios';
import './SpecialistCalendar.css';

const SpecialistCalendar = () => {
    const { currentUser } = useAuth();
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [timeSlots, setTimeSlots] = useState([]);
    const [view, setView] = useState('week');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    
    // Generate time slots from 8:00 to 20:00
    const hours = Array.from({ length: 13 }, (_, i) => i + 8);
    
    useEffect(() => {
        if (currentUser && currentUser.user) {
            fetchTimeSlots();
        }
    }, [currentUser]);
    
    // Fetch time slots from the server
    const fetchTimeSlots = async () => {
        if (!currentUser || !currentUser.user) return;
        
        try {
            setLoading(true);
            setError('');
            
            const response = await axios.get(`/api/Profile/specialist/${currentUser.user.id}/timeslots`, {
                headers: {
                    'Authorization': `Bearer ${currentUser.token}`
                }
            });
            
            if (response.data) {
                // Convert API time slots to our format
                const formattedSlots = response.data.map(slot => ({
                    id: slot.id,
                    date: new Date(slot.date).toISOString().split('T')[0],
                    time: slot.startTime.substring(0, 5), // Format HH:MM
                    endTime: slot.endTime.substring(0, 5),
                    isBooked: slot.isBooked,
                    bookedByUserId: slot.bookedByUserId
                }));
                
                setTimeSlots(formattedSlots);
            }
        } catch (err) {
            console.error('Error fetching time slots:', err);
            setError('Nepavyko gauti laisvų laikų. Bandykite dar kartą.');
        } finally {
            setLoading(false);
        }
    };
    
    // Generate dates for the current week view
    const getDatesForWeek = () => {
        const dates = [];
        const startOfWeek = new Date(selectedDate);
        const day = startOfWeek.getDay();
        const diff = startOfWeek.getDate() - day + (day === 0 ? -6 : 1); // Adjust for Sunday
        startOfWeek.setDate(diff);
        
        for (let i = 0; i < 7; i++) {
            const date = new Date(startOfWeek);
            date.setDate(date.getDate() + i);
            dates.push(date);
        }
        
        return dates;
    };
    
    // Format date as YYYY-MM-DD
    const formatDate = (date) => {
        return date.toISOString().split('T')[0];
    };
    
    // Format time as HH:00
    const formatTime = (hour) => {
        return `${hour.toString().padStart(2, '0')}:00`;
    };
    
    // Check if a time slot is selected
    const isTimeSlotSelected = (date, hour) => {
        const dateStr = formatDate(date);
        const timeStr = formatTime(hour);
        return timeSlots.some(slot => slot.date === dateStr && slot.time === timeStr);
    };
    
    // Check if a time slot is booked
    const isTimeSlotBooked = (date, hour) => {
        const dateStr = formatDate(date);
        const timeStr = formatTime(hour);
        const slot = timeSlots.find(slot => slot.date === dateStr && slot.time === timeStr);
        return slot ? slot.isBooked : false;
    };
    
    // Toggle time slot selection
    const toggleTimeSlot = async (date, hour) => {
        if (!currentUser || !currentUser.user) return;
        
        const dateStr = formatDate(date);
        const timeStr = formatTime(hour);
        const endHour = hour + 1;
        const endTimeStr = `${endHour.toString().padStart(2, '0')}:00`;
        
        const existingSlot = timeSlots.find(
            slot => slot.date === dateStr && slot.time === timeStr
        );
        
        try {
            setLoading(true);
            setError('');
            
            if (existingSlot) {
                // Delete the slot if it exists and is not booked
                if (existingSlot.isBooked) {
                    setError('Negalima ištrinti užrezervuoto laiko.');
                    return;
                }
                
                await axios.delete(`/api/Profile/specialist/timeslot/${existingSlot.id}`, {
                    headers: {
                        'Authorization': `Bearer ${currentUser.token}`
                    }
                });
                
                // Update local state
                setTimeSlots(timeSlots.filter(slot => slot.id !== existingSlot.id));
            } else {
                // Create a new time slot
                const newSlot = {
                    userId: currentUser.user.id,
                    date: new Date(dateStr),
                    startTime: timeStr,
                    endTime: endTimeStr,
                    isBooked: false,
                    bookedByUserId: null
                };
                
                const response = await axios.post('/api/Profile/specialist/timeslot', newSlot, {
                    headers: {
                        'Authorization': `Bearer ${currentUser.token}`,
                        'Content-Type': 'application/json'
                    }
                });
                
                // Add the new slot to local state
                setTimeSlots([...timeSlots, {
                    id: response.data.id,
                    date: dateStr,
                    time: timeStr,
                    endTime: endTimeStr,
                    isBooked: false,
                    bookedByUserId: null
                }]);
            }
        } catch (err) {
            console.error('Error updating time slot:', err);
            setError('Nepavyko atnaujinti laisvo laiko. Bandykite dar kartą.');
        } finally {
            setLoading(false);
        }
    };
    
    // Navigate to previous week
    const previousWeek = () => {
        const newDate = new Date(selectedDate);
        newDate.setDate(newDate.getDate() - 7);
        setSelectedDate(newDate);
    };
    
    // Navigate to next week
    const nextWeek = () => {
        const newDate = new Date(selectedDate);
        newDate.setDate(newDate.getDate() + 7);
        setSelectedDate(newDate);
    };
    
    // Format date for display
    const formatDateForDisplay = (date) => {
        const options = { weekday: 'short', month: 'numeric', day: 'numeric' };
        return date.toLocaleDateString('lt-LT', options);
    };
    
    // If currentUser is not available, show loading
    if (!currentUser || !currentUser.user) {
        return <div className="calendar-container">Kraunama...</div>;
    }
    
    return (
        <div className="calendar-container">
            <h2>Jūsų laisvi laikai</h2>
            
            {error && <div className="alert alert-danger">{error}</div>}
            
            <div className="calendar-controls mb-3">
                <button className="btn btn-outline-primary" onClick={previousWeek}>
                    &lt; Ankstesnė savaitė
                </button>
                <span className="mx-3">
                    {formatDateForDisplay(getDatesForWeek()[0])} - {formatDateForDisplay(getDatesForWeek()[6])}
                </span>
                <button className="btn btn-outline-primary" onClick={nextWeek}>
                    Kita savaitė &gt;
                </button>
            </div>
            
            <div className="table-responsive">
                <table className="table table-bordered calendar-table">
                    <thead>
                        <tr>
                            <th>Laikas</th>
                            {getDatesForWeek().map((date, index) => (
                                <th key={index}>{formatDateForDisplay(date)}</th>
                            ))}
                        </tr>
                    </thead>
                    <tbody>
                        {hours.map(hour => (
                            <tr key={hour}>
                                <td className="time-cell">{formatTime(hour)}</td>
                                {getDatesForWeek().map((date, index) => (
                                    <td 
                                        key={index} 
                                        className={`time-slot ${isTimeSlotSelected(date, hour) ? 'selected' : ''} ${isTimeSlotBooked(date, hour) ? 'booked' : ''}`}
                                        onClick={() => toggleTimeSlot(date, hour)}
                                    >
                                        {isTimeSlotSelected(date, hour) && (
                                            <span className="slot-indicator">{isTimeSlotBooked(date, hour) ? '🔒' : '✓'}</span>
                                        )}
                                    </td>
                                ))}
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
            
            <p className="instruction mt-3">
                Paspauskite ant laiko langelio, kad pažymėtumėte jį kaip laisvą laiką.
                Paspauskite dar kartą, kad pašalintumėte.
            </p>
        </div>
    );
};

export default SpecialistCalendar;
