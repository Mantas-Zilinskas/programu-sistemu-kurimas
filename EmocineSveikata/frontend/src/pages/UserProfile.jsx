import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import axios from 'axios';
import './UserProfile.css';

const topicsMap = {
    "Depression": "Depresija",
    "MentalHealth": "Psichinė sveikata",
    "ADHD": "ADHD",
    "Therapy": "Terapija",
    "Relationships": "Santykiai",
    "PhysicalHealth": "Fizinė sveikata"
};

const topics = Object.entries(topicsMap).map(([key, value]) => ({ key, value }));

const UserProfile = () => {
    const { currentUser } = useAuth();
    const [selectedTopics, setSelectedTopics] = useState([]);
    const [imagePreview, setImagePreview] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [receiveSmsMessages, setReceiveSmsMessages] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [smsSent, setSmsSent] = useState(false);

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
                setPhoneNumber(response.data.phoneNumber || '');
                setReceiveSmsMessages(response.data.receiveSmsMessages || false);
            }
        } catch (err) {
            console.log('Profile not found, will create on save:', err);
        } finally {
            setLoading(false);
        }
    };

    const handleCheckboxChange = (topicKey, topicValue) => {
        setSelectedTopics(prevTopics =>
            prevTopics.includes(topicKey)
                ? prevTopics.filter(t => t !== topicKey)
                : [...prevTopics, topicKey]
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
            setSmsSent(false);

            const profileData = {
                userId: currentUser.user.id,
                profilePicture: imagePreview,
                selectedTopics: selectedTopics,
                phoneNumber: phoneNumber,
                receiveSmsMessages: receiveSmsMessages
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
    
    const sendTestSms = async () => {
        if (!currentUser || !currentUser.user) return;
        if (!phoneNumber) {
            setError('Įveskite telefono numerį prieš siunčiant bandomąją žinutę');
            return;
        }
        
        try {
            setLoading(true);
            setError('');
            setSmsSent(false);
            
            await axios.post(`/api/Profile/user/${currentUser.user.id}/send-test-sms`, {}, {
                headers: {
                    'Authorization': `Bearer ${currentUser.token}`,
                    'Content-Type': 'application/json'
                }
            });
            
            setSmsSent(true);
        } catch (err) {
            console.error('Error sending test SMS:', err);
            setError(err.response?.data?.message || 'Nepavyko išsiųsti bandomosios žinutės');
        } finally {
            setLoading(false);
        }
    };

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
                    
                    <div className="sms-settings">
                        <h3>SMS pranešimų nustatymai</h3>
                        <div className="form-group">
                            <label htmlFor="phone-number">Telefono numeris:</label>
                            <input 
                                type="tel" 
                                id="phone-number" 
                                value={phoneNumber} 
                                onChange={(e) => setPhoneNumber(e.target.value)} 
                                placeholder="+370XXXXXXXX" 
                            />
                        </div>
                        
                        <div className="form-group">
                            <label className="checkbox-container">
                                <input 
                                    type="checkbox" 
                                    checked={receiveSmsMessages} 
                                    onChange={() => setReceiveSmsMessages(!receiveSmsMessages)} 
                                />
                                <span className="checkmark"></span>
                                Gauti pozityvias žinutes SMS formatu
                            </label>
                        </div>
                        
                        <button 
                            onClick={sendTestSms} 
                            disabled={loading || !phoneNumber} 
                            className="test-sms-btn"
                        >
                            {loading ? 'Siunčiama...' : 'Išsiųsti bandomąją žinutę'}
                        </button>
                        
                        {smsSent && <p className="success-message">Bandomoji žinutė sėkmingai išsiųsta!</p>}
                    </div>
                </div>
            </div>

            <div className="topics-selection">
                <label>Pageidaujamos pozityvių žinučių temos:</label>
                <div className="topics-checkboxes">
                    {topics.map(({ key, value }) => (
                        <label key={key} className="checkbox-container">
                            <input
                                type="checkbox"
                                checked={selectedTopics.includes(key)}
                                onChange={() => handleCheckboxChange(key, value)}
                            />
                            <span className="checkmark"></span>
                            {value}
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