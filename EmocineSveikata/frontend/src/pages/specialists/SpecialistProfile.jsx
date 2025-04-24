import React, { useState, useEffect } from 'react';
import './SpecialistProfile.css';

const SpecialistProfile = () => {
    const [profile, setProfile] = useState(null);

    useEffect(() => {
        const savedProfile = localStorage.getItem('userProfile');
        if (savedProfile) {
            setProfile(JSON.parse(savedProfile));
        }
    }, []);

    return (
        <div className="profile-view-page">
            {profile ? (
                <div>
                    <h2>Your Specialist Profile</h2>
                    <div>
                        <strong>Name: </strong> {profile.name}
                    </div>
                    <div>
                        <strong>Bio: </strong> {profile.bio}
                    </div>
                    <div>
                        <strong>Email: </strong> {profile.email}
                    </div>
                    <div>
                        <strong>Phone: </strong> {profile.phone}
                    </div>
                    {profile.profilePicture && (
                        <div>
                            <img
                                src={profile.profilePicture}
                                alt="Profile"
                                width="100"
                            />
                        </div>
                    )}
                </div>
            ) : (
                <p>No profile data available.</p>
            )}
        </div>
    );
};


export default SpecialistProfile;
