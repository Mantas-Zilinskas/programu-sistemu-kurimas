import React from 'react';
import { Link } from 'react-router-dom';
import { useDiscussions } from '../contexts/DiscussionContext';
import './Discussions.css';

const Discussions = () => {
    const { discussions, loading, error } = useDiscussions();

    if (loading) {
        return (
            <div className="loading-container">
                <div className="loading-spinner">Loading...</div>
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
        <div className="discussions-container">
            <h1 className="discussions-title">Diskusijos</h1>

            <div className="create-button-wrapper">
                <Link to="/creatediscussion" className="create-button">
                    Sukurti naują diskusiją
                </Link>
            </div>

            <div className="discussions-list">
                {discussions.length === 0 ? (
                    <p>Šiuo metu nėra diskusijų. Prisijunkite ir sukurkite pirmąją!</p>
                ) : (
                    discussions.map((discussion) => (
                        <div key={discussion.id} className="discussion-card">
                            <h3 className="discussion-title">{discussion.title}</h3>
                            <p className="discussion-content">{discussion.content}</p>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default Discussions;
