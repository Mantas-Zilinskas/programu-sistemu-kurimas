import React, { useState } from 'react';
import { useDiscussions } from '../contexts/DiscussionContext';
import './Discussions.css';

const Discussions = () => {
  const { discussions, loading, error, createDiscussion } = useDiscussions();
  const [newDiscussion, setNewDiscussion] = useState({ title: '', content: '' });

  const handleSubmit = (e) => {
    e.preventDefault();
    if (newDiscussion.title && newDiscussion.content) {
      createDiscussion(newDiscussion);
      setNewDiscussion({ title: '', content: '' });
    }
  };

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loading-spinner"></div>
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

      <div className="discussions-form-card">
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
          <button
            type="submit"
            className="submit-button"
          >
            Sukurti diskusiją
          </button>
        </form>
      </div>

      {discussions.map((discussion) => (
        <div key={discussion.id} className="discussion-card">
          <h2 className="discussion-title">
            {discussion.title}
          </h2>
          <p className="discussion-content">
            {discussion.content}
          </p>
        </div>
      ))}
    </div>
  );
};

export default Discussions;
