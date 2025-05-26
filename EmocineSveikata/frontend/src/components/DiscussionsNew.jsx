import React, { useState, useEffect } from 'react';
import { useDiscussionsNew } from '../contexts/DiscussionNewContext';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import './DiscussionsNew.css';

const DiscussionsNew = () => {
  const { tagsArray, setTagsArray, error, createDiscussion } = useDiscussionsNew();
  const { currentUser } = useAuth();
  const navigate = useNavigate();
  const [newDiscussion, setNewDiscussion] = useState({ title: '', content: '', tags: [] });
  const [selectedTags, setSelectedTags] = useState([]);
  const [currentTag, setCurrentTag] = useState("");

  useEffect(() => {
    if (!currentUser) {
      navigate('/login');
    }
  }, [currentUser, navigate]);

  const handleAddTag = () => {
    if (currentTag && tagsArray.includes(currentTag)) {
      setSelectedTags([...selectedTags, currentTag]);
      setTagsArray(tagsArray.filter((tag) => tag !== currentTag));
      setCurrentTag("");
      setNewDiscussion({ ...newDiscussion, tags: [...selectedTags, currentTag] });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (newDiscussion.title && newDiscussion.content) {
      await createDiscussion(newDiscussion);
      window.location.href = "/discussions";
    }
  };

  if (!currentUser) {
    return (
      <div className="error-message">
        You must be logged in to create a discussion.
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
          <div className="form-group">
            <label htmlFor="tags" className="form-label">Žymos</label>
            <div className="tag-select-container">
              <select
                id="tags"
                className="tag-select"
                value={currentTag}
                onChange={(e) => setCurrentTag(e.target.value)}
              >
                <option value="" disabled>Pasirinkti žymą</option>
                {tagsArray.map((tag) => (
                  <option key={tag} value={tag}>{tag}</option>
                ))}
              </select>
              <button type="button" className="add-tag-button" onClick={handleAddTag}>+</button>
            </div>
            <div className="selected-tags-wrapper">
              <label htmlFor="selected-tags-label" className="form-label">Pasirinktos žymos:</label>
              <div className="selected-tags">
                {selectedTags.map((tag) => (
                  <span key={tag} className="tag-chip">{tag}</span>
                ))}
              </div>
            </div>
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
