import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import axios from 'axios';
import './SpecialistCalendar.css';

const SpecialistCalendar = () => {
    const { currentUser } = useAuth();

    const [selectedDate, setSelectedDate] = useState(() => {
        const savedYear = localStorage.getItem('specialistCalendarYear');
        const savedMonth = localStorage.getItem('specialistCalendarMonth');
        const savedDay = localStorage.getItem('specialistCalendarDay');

        if (savedYear && savedMonth && savedDay) {
            const year = parseInt(savedYear, 10);
            const month = parseInt(savedMonth, 10) - 1;
            const day = parseInt(savedDay, 10);

            const date = new Date();
            date.setFullYear(year);
            date.setMonth(month);
            date.setDate(day);
            date.setHours(12, 0, 0, 0); 

            return date;
        } else {
            const today = new Date();
            today.setHours(12, 0, 0, 0);
            return today;
        }
    });


    const [timeSlots, setTimeSlots] = useState([]);
    const [view, setView] = useState('week');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const hours = Array.from({ length: 13 }, (_, i) => i + 8);

    useEffect(() => {
        localStorage.setItem('specialistCalendarYear', selectedDate.getFullYear());
        localStorage.setItem('specialistCalendarMonth', selectedDate.getMonth() + 1);
        localStorage.setItem('specialistCalendarDay', selectedDate.getDate());
    }, [selectedDate]);

    useEffect(() => {
        if (currentUser && currentUser.user) {
            fetchTimeSlots();
        }
    }, [currentUser]);

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
                const formattedSlots = response.data.map(slot => {
                    const dbDateString = slot.date.split(' ')[0];
                    const [year, month, day] = dbDateString.split('-').map(Number);

                    const localDate = new Date(year, month - 1, day);

                    return {
                        id: slot.id,
                        date: formatDate(localDate),
                        time: slot.startTime.substring(0, 5),
                        endTime: slot.endTime.substring(0, 5),
                        isBooked: slot.isBooked,
                        bookedByUserId: slot.bookedByUserId
                    };
                });

                setTimeSlots(formattedSlots);
            }
        } catch (err) {
            console.error('Error fetching time slots:', err);
            setError('Nepavyko gauti laisvų laikų. Bandykite dar kartą.');
        } finally {
            setLoading(false);
        }
    };

    const getDatesForWeek = () => {
        const dates = [];
        const startOfWeek = new Date(selectedDate);
        const day = startOfWeek.getDay();
        const diff = startOfWeek.getDate() - day + (day === 0 ? -6 : 1);
        startOfWeek.setDate(diff);

        for (let i = 0; i < 7; i++) {
            const date = new Date(startOfWeek);
            date.setDate(date.getDate() + i);
            dates.push(date);
        }

        return dates;
    };

    const formatDate = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `<span class="math-inline">\{year\}\-</span>{month}-${day}`;
    };

    const formatTime = (hour) => {
        return `${hour.toString().padStart(2, '0')}:00`;
    };

    const isTimeSlotSelected = (date, hour) => {
        const dateStr = formatDate(date);
        const timeStr = formatTime(hour);
        return timeSlots.some(slot => slot.date === dateStr && slot.time === timeStr);
    };

    const isTimeSlotBooked = (date, hour) => {
        const dateStr = formatDate(date);
        const timeStr = formatTime(hour);
        const slot = timeSlots.find(slot => slot.date === dateStr && slot.time === timeStr);
        return slot ? slot.isBooked : false;
    };

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
                if (existingSlot.isBooked) {
                    setError('Negalima ištrinti užrezervuoto laiko.');
                    return;
                }

                await axios.delete(`/api/Profile/specialist/timeslot/${existingSlot.id}`, {
                    headers: {
                        'Authorization': `Bearer ${currentUser.token}`
                    }
                });

                setTimeSlots(timeSlots.filter(slot => slot.id !== existingSlot.id));
            } else {
                const newSlot = {
                    userId: currentUser.user.id,
                    date: formatDate(date),
                    startTime: timeStr,
                    endTime: endTimeStr,
                    isBooked: false,
                    bookedByUserId: null
                };


                try {
                    const response = await axios.post('/api/Profile/specialist/timeslot', newSlot, {
                        headers: {
                            'Authorization': `Bearer ${currentUser.token}`,
                            'Content-Type': 'application/json'
                        }
                    });

                    setTimeSlots([...timeSlots, {
                        id: response.data.id,
                        date: dateStr,
                        time: timeStr,
                        endTime: endTimeStr,
                        isBooked: false,
                        bookedByUserId: null
                    }]);
                } catch (error) {
                    console.error('Error creating time slot:', error);

                    if (error.response &&
                        error.response.data &&
                        error.response.data.message &&
                        error.response.data.message.includes('Specialist profile not found')) {
                        setError(
                            <div className="custom-alert">
                                <div className="custom-alert-content">
                                    <div className="custom-alert-icon">
                                        <i className="bi bi-exclamation-triangle-fill text-warning"></i>
                                    </div>
                                    <div className="custom-alert-message">
                                        <h5>Profilis neužpildytas</h5>
                                        <p>
                                            Prieš pridedant laisvus laikus, reikia užpildyti specialisto profilį.
                                        </p>
                                        <a href="/specialistprofile" className="btn btn-outline-primary btn-sm mt-2">
                                            <i className="bi bi-pencil-square me-1"></i>
                                            Užpildyti profilį
                                        </a>
                                    </div>
                                </div>
                            </div>
                        );
                        setLoading(false);
                        return;
                    } else {
                        setError('Nepavyko pridėti laisvo laiko. Bandykite dar kartą.');
                    }
                }
            }
        } catch (err) {
            console.error('Error updating time slot:', err);
            setError('Nepavyko atnaujinti laisvo laiko. Bandykite dar kartą.');
        } finally {
            setLoading(false);
        }
    };

    const previousWeek = () => {
        const newDate = new Date(selectedDate);
        newDate.setDate(newDate.getDate() - 7);
        newDate.setHours(12, 0, 0, 0);
        setSelectedDate(newDate);
    };

    const nextWeek = () => {
        const newDate = new Date(selectedDate);
        newDate.setDate(newDate.getDate() + 7);
        newDate.setHours(12, 0, 0, 0);
        setSelectedDate(newDate);
    };

    const formatDateForDisplay = (date) => {
        const options = { weekday: 'short', month: 'numeric', day: 'numeric' };
        return date.toLocaleDateString('lt-LT', options);
    };

    if (!currentUser || !currentUser.user) {
        return <div className="calendar-container">Kraunama...</div>;
    }

    return (
        <div className="calendar-container">
            <h2>Jūsų laisvi laikai</h2>

            {error && (typeof error === 'string' ? <div className="alert alert-danger">{error}</div> : error)}

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