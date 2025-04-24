import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './SpecialistProfileCreation.css';

const SpecialistProfileCreation = () => {
    const [profile, setProfile] = useState({
        name: '',
        bio: '',
        email: '',
        phone: '',
        profilePicture: ''
    });

    const navigate = useNavigate();

    useEffect(() => {
        const savedProfile = localStorage.getItem('userProfile');
        if (savedProfile) {
            setProfile(JSON.parse(savedProfile));
        }
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setProfile((prevProfile) => ({
            ...prevProfile,
            [name]: value
        }));
    };

    const handleFileChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onloadend = () => {
                setProfile((prevProfile) => ({
                    ...prevProfile,
                    profilePicture: reader.result
                }));
            };
            reader.readAsDataURL(file);
        }
    };

    const saveProfile = () => {
        localStorage.setItem('userProfile', JSON.stringify(profile));
        navigate('/specialist/profile');
    };

    return (
        <div className="specialist-profile-page">
            <h2>Your Specialist Profile</h2>
            <form>
                <div>
                    <label htmlFor="name">Name</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        value={profile.name}
                        onChange={handleChange}
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label htmlFor="bio">Bio</label>
                    <textarea
                        id="bio"
                        name="bio"
                        value={profile.bio}
                        onChange={handleChange}
                        placeholder="Enter your bio"
                    />
                </div>
                <div>
                    <label htmlFor="email">Email</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={profile.email}
                        onChange={handleChange}
                        placeholder="Enter your email"
                    />
                </div>
                <div>
                    <label htmlFor="phone">Phone</label>
                    <input
                        type="text"
                        id="phone"
                        name="phone"
                        value={profile.phone}
                        onChange={handleChange}
                        placeholder="Enter your phone"
                    />
                </div>
                <div>
                    <label htmlFor="profilePicture">Profile Picture</label>
                    <input
                        type="file"
                        id="profilePicture"
                        onChange={handleFileChange}
                    />
                    {profile.profilePicture && (
                        <img
                            src={profile.profilePicture}
                            alt="Profile Preview"
                            width="100"
                        />
                    )}
                </div>
                <button type="button" onClick={saveProfile}>
                    Save Changes
                </button>
            </form>
        </div>
    );
};

export default SpecialistProfileCreation;
