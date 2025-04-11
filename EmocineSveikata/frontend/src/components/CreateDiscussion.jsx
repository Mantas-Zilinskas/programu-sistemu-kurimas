import React, { useState } from 'react';
import { useDiscussions } from '../contexts/DiscussionContext';
import { useNavigate } from 'react-router-dom';
import './CreateDiscussion.css';

const CreateDiscussion = () => {
    const { createDiscussion, loading, error } = useDiscussions();
    const [newDiscussion, setNewDiscussion] = useState({ title: '', content: '' });
    const [submitting, setSubmitting] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (newDiscussion.title && newDiscussion.content) {
            setSubmitting(true);
            try {
                await createDiscussion(newDiscussion);
                setNewDiscussion({ title: '', content: '' });
                navigate('/discussions');
            } catch (err) {
            } finally {
                setSubmitting(false);
            }
        }
    };

    return (
        <div className="discussions-form-card">
            <h1>Sukurti diskusiją</h1>
            <form className="discussions-form" onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="title" className="form-label">Pavadinimas</label>
                    <input
                        id="title"
                        type="text"
                        className="form-input"
                        value={newDiscussion.title}
                        onChange={(e) =>
                            setNewDiscussion({ ...newDiscussion, title: e.target.value })
                        }
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="content" className="form-label">Turinys</label>
                    <textarea
                        id="content"
                        className="form-textarea"
                        value={newDiscussion.content}
                        onChange={(e) =>
                            setNewDiscussion({ ...newDiscussion, content: e.target.value })
                        }
                        rows={4}
                        required
                    ></textarea>
                </div>
                <button type="submit" className="submit-button" disabled={submitting || loading}>
                    {submitting ? 'Kuriama...' : 'Sukurti diskusiją'}
                </button>
            </form>
            {error && (
                <div className="error-message">
                    Error: {error}
                </div>
            )}
        </div>
    );
};

export default CreateDiscussion;
