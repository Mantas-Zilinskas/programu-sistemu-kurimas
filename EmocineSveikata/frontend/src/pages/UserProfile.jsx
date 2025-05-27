import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import axios from 'axios';
import './UserProfile.css';

const topicsMap = {
  "Depresija": "Depresija",
  "PsichinėSveikata": "Psichinė sveikata",
  "ADHD": "ADHD",
  "Terapija": "Terapija",
  "Santykiai": "Santykiai",
  "FizinėSveikata": "Fizinė sveikata"
};

const topics = Object.entries(topicsMap).map(([key, value]) => ({ key, value }));

const UserProfile = () => {
    const { currentUser } = useAuth();
    const [selectedTopics, setSelectedTopics] = useState([]);
    const [imagePreview, setImagePreview] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [smsNotificationsEnabled, setSmsNotificationsEnabled] = useState(false);
    const [phoneNumber, setPhoneNumber] = useState('');
    const [smsReminderTopic, setSmsReminderTopic] = useState('');
    const [sendingTestSms, setSendingTestSms] = useState(false);

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
                setSmsNotificationsEnabled(response.data.smsNotificationsEnabled || false);
                setPhoneNumber(response.data.phoneNumber || '');
                setSmsReminderTopic(response.data.smsReminderTopic || '');
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
            setSuccess('');

            const profileData = {
                userId: currentUser.user.id,
                profilePicture: imagePreview,
                selectedTopics: selectedTopics,
                smsNotificationsEnabled: smsNotificationsEnabled,
                phoneNumber: phoneNumber,
                smsReminderTopic: smsReminderTopic
            };

            await axios.post('/api/Profile/user', profileData, {
                headers: {
                    'Authorization': `Bearer ${currentUser.token}`,
                    'Content-Type': 'application/json'
                }
            });

            setSuccess('Profilis išsaugotas!');
        } catch (err) {
            console.error('Error saving profile:', err);
            setError('Nepavyko išsaugoti profilio. Bandykite dar kartą.');
        } finally {
            setLoading(false);
        }
    };
    
    const handleTestSms = async () => {
        if (!currentUser || !currentUser.user || !phoneNumber) return;
        
        try {
            setSendingTestSms(true);
            setError('');
            setSuccess('');
            
            const testSmsData = {
                phoneNumber: phoneNumber
            };
            
            await axios.post('/api/Profile/user/test-sms', testSmsData, {
                headers: {
                    'Authorization': `Bearer ${currentUser.token}`,
                    'Content-Type': 'application/json'
                }
            });
            
            setSuccess('Bandomoji SMS sėkmingai išsiųsta!');
        } catch (err) {
            console.error('Error sending test SMS:', err);
            setError('Nepavyko išsiųsti bandomosios SMS. Patikrinkite telefono numerį ir bandykite dar kartą.');
        } finally {
            setSendingTestSms(false);
        }
    };

    if (!currentUser || !currentUser.user) {
        return <div className="profile-container">Kraunama...</div>;
    }

    return (
        <div className="profile-container">
            <h1>Naudotojo profilis</h1>

            {error && <div className="error-message">{error}</div>}
            {success && <div className="success-message">{success}</div>}

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

            <div className="sms-notifications-section">
                <h2>SMS Priminimų Nustatymai</h2>
                <div className="sms-toggle">
                    <label className="toggle-container">
                        <input
                            type="checkbox"
                            checked={smsNotificationsEnabled}
                            onChange={() => setSmsNotificationsEnabled(!smsNotificationsEnabled)}
                        />
                        <span className="toggle-slider"></span>
                        <span className="toggle-label">Įjungti SMS priminimus</span>
                    </label>
                </div>

                {smsNotificationsEnabled && (
                    <div className="sms-settings">
                        <div className="form-group">
                            <label htmlFor="phone-number">Telefono numeris:</label>
                            <input
                                id="phone-number"
                                type="tel"
                                value={phoneNumber}
                                onChange={(e) => setPhoneNumber(e.target.value)}
                                placeholder="+370xxxxxxxx"
                                className="phone-input"
                            />
                            <small>Įveskite telefono numerį su šalies kodu, pvz., +370xxxxxxxx</small>
                        </div>

                        <div className="form-group">
                            <label htmlFor="sms-topic">Pasirinkite priminimų temą:</label>
                            <select
                                id="sms-topic"
                                value={smsReminderTopic}
                                onChange={(e) => setSmsReminderTopic(e.target.value)}
                                className="topic-select"
                            >
                                <option value="">Pasirinkite temą...</option>
                                {topics.map(({ key, value }) => (
                                    <option key={key} value={key}>
                                        {value}
                                    </option>
                                ))}
                            </select>
                            <small>Gausite kasdienius priminimus pagal pasirinktą temą</small>
                        </div>
                        
                        <div className="test-sms-section">
                            <button 
                                className="test-sms-button"
                                onClick={handleTestSms} 
                                disabled={sendingTestSms || !phoneNumber}
                            >
                                {sendingTestSms ? 'Siunčiama...' : 'Siųsti bandomąją SMS'}
                            </button>
                            <small>Siųsti bandomąją SMS žinutę, kad įsitikintumėte, jog numeris teisingas</small>
                        </div>
                    </div>
                )}
            </div>

            <button onClick={handleSave} disabled={loading}>
                {loading ? 'Saugoma...' : 'Išsaugoti'}
            </button>
        </div>
    );
};

export default UserProfile;