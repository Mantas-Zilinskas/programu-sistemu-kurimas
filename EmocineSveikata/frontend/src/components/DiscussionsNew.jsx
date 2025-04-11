import React, { useState } from 'react';
import { useDiscussionsNew } from '../contexts/DiscussionNewContext';
import './DiscussionsNew.css';

const DiscussionsNew = () => {
  const { error, createDiscussion } = useDiscussionsNew();
  const [newDiscussion, setNewDiscussion] = useState({ title: '', content: '' });

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (newDiscussion.title && newDiscussion.content) {
      await createDiscussion(newDiscussion);
      window.location.href = "/discussions";
    }
  };

  if (error) {
    return (
      <div className="error-message">
        Error: {error}
      </div>
    );
  }

  return (
    <div className="new-discussion-container">
      <h1 className="new-discussion-title">Nauja diskusija</h1>

      <div className="new-discussion-form-card">
        <form className="new-discussion-form" onSubmit={handleSubmit}>
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
    </div>
  );
};

export default DiscussionsNew;
