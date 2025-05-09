import React, { useState, useEffect } from 'react';
import './SpecialistProfile.css';
import { getUserProfile, updateUserProfile } from '../api/userApi';  // Import the API functions

const SpecialistProfile = () => {
    const [specialist, setSpecialist] = useState({
        name: '',
        email: '',
        description: '',
    });

    const [imagePreview, setImagePreview] = useState(null);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            getUserProfile(token)
                .then((data) => {
                    setSpecialist({
                        name: data.username,  
                        email: data.email,    
                        description: data.description || '',
                    });
                })
                .catch((error) => {
                    console.error('Error fetching user data:', error);
                });
        }
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setSpecialist((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    const handleSaveProfile = () => {
        const token = localStorage.getItem('token');
        if (token) {
            updateUserProfile(token, specialist)
                .then((updatedData) => {
                    console.log('Profile updated successfully:', updatedData);
                })
                .catch((error) => {
                    console.error('Error updating profile:', error);
                });
        }
    };

    return (
        <div className="specialist-profile">
            <h1>Specialisto profilis</h1>

            <div className="profile-input">
                <label>Vartotojo vardas:</label>
                <input
                    type="text"
                    name="name"
                    value={specialist.name}
                    onChange={handleChange}
                    placeholder="Įrašykite vardą"
                />
            </div>

            <div className="profile-input">
                <label>Vartotojo paštas:</label>
                <input
                    type="email"
                    name="email"
                    value={specialist.email}
                    onChange={handleChange}
                    placeholder="Įrašykite el. paštą"
                />
            </div>

            <div className="profile-input">
                <label>Aprašymas:</label>
                <textarea
                    name="description"
                    value={specialist.description}
                    onChange={handleChange}
                    placeholder="Aprašykite save"
                />
            </div>

            <button className="save-button" onClick={handleSaveProfile}>Išsaugoti profilį</button>
        </div>
    );
};

export default SpecialistProfile;
