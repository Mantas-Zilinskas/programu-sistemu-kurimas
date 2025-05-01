import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import axios from 'axios';
import './UserProfile.css';

const topics = ["Depresija", "Psichinė sveikata", "ADHD", "Terapija", "Santykiai", "Fizinė sveikata"];

const UserProfile = () => {
    const { currentUser } = useAuth();
    const [selectedTopics, setSelectedTopics] = useState([]);
    const [imagePreview, setImagePreview] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        if (currentUser && currentUser.user) {
            fetchUserProfile();
        }
    }, [currentUser]);

    const fetchUserProfile = async () => {
        if (!currentUser || !currentUser.user) return;
        
        try {
            setLoading(true);
            const response = await axios.get(`/api/Profile/user/${currentUser.user.id}`, {
                headers: {
                    'Authorization': `Bearer ${currentUser.token}`
                }
            });
            
            if (response.data) {
                setSelectedTopics(response.data.selectedTopics || []);
                setImagePreview(response.data.profilePicture || '');
            }
        } catch (err) {
            console.log('Profile not found, will create on save:', err);
            // If profile doesn't exist yet, that's okay - we'll create it when they save
        } finally {
            setLoading(false);
        }
    };

    const handleCheckboxChange = (topic) => {
        setSelectedTopics(prevTopics =>
            prevTopics.includes(topic)
                ? prevTopics.filter(t => t !== topic)
                : [...prevTopics, topic]
        );
    };

    const handleImageUpload = (e) => {
        if (!currentUser || !currentUser.user) return;
        
        const file = e.target.files[0];
        const reader = new FileReader();
        reader.onloadend = () => {
            setImagePreview(reader.result);
        };
        reader.readAsDataURL(file);
    };

    const handleSave = async () => {
        if (!currentUser || !currentUser.user) return;
        
        try {
            setLoading(true);
            setError('');
            
            const profileData = {
                userId: currentUser.user.id,
                profilePicture: imagePreview,
                selectedTopics: selectedTopics
            };
            
            await axios.post('/api/Profile/user', profileData, {
                headers: {
                    'Authorization': `Bearer ${currentUser.token}`,
                    'Content-Type': 'application/json'
                }
            });
            
            alert('Profilis išsaugotas!');
        } catch (err) {
            console.error('Error saving profile:', err);
            setError('Nepavyko išsaugoti profilio. Bandykite dar kartą.');
        } finally {
            setLoading(false);
        }
    };

    // If currentUser is not available, show loading
    if (!currentUser || !currentUser.user) {
        return <div className="profile-container">Kraunama...</div>;
    }

    return (
        <div className="profile-container">
            <h1>Naudotojo profilis</h1>

            {error && <div className="error-message">{error}</div>}

            <div className="profile-content">
                <div className="profile-left">
                    <div className="profile-pic">
                        <img src={imagePreview || 'default-pic.jpg'} alt="Profile" />
                    </div>
                    <label htmlFor="file-upload" className="upload-btn">Įkelti nuotrauką</label>
                    <input id="file-upload" type="file" onChange={handleImageUpload} />
                </div>

                <div className="profile-right">
                    <p><strong>Vartotojo vardas:</strong> {currentUser?.user?.username || 'No username available'}</p>
                    <p><strong>El. paštas:</strong> {currentUser?.user?.email || 'No email available'}</p>
                </div>
            </div>

            <div className="topics-selection">
                <label>Pageidaujamos pozityvių žinučių temos:</label>
                <div className="topics-checkboxes">
                    {topics.map(topic => (
                        <label key={topic} className="checkbox-container">
                            <input
                                type="checkbox"
                                checked={selectedTopics.includes(topic)}
                                onChange={() => handleCheckboxChange(topic)}
                            />
                            <span className="checkmark"></span>
                            {topic}
                        </label>
                    ))}
                </div>
            </div>

            <button onClick={handleSave} disabled={loading}>
                {loading ? 'Saugoma...' : 'Išsaugoti'}
            </button>
        </div>
    );
};

export default UserProfile;