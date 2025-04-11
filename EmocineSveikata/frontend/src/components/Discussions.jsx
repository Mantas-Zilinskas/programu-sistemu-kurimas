import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useDiscussions } from '../contexts/DiscussionContext';
import './Discussions.css';

const Discussions = () => {
  const { discussions, tagsArray, selectedTag, setSelectedTag, loading, error } = useDiscussions();
  const [newDiscussion, setNewDiscussion] = useState({ title: '', content: '' });

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
      <div className="discussions-top">
        <h1 className="discussions-title">Diskusijos</h1>
        <Link to="/discussions/new" className="new-discussion-button">
          Nauja diskusija
        </Link>
      </div>

      <select
        value={selectedTag}
        onChange={(e) => setSelectedTag(e.target.value)}
        className="tag-select"
      >
        <option value="">Visos diskusijos</option>
        {tagsArray.map((tag, index) => (
          <option key={index} value={tag}>
            {tag}
          </option>
        ))}
      </select>

      {discussions.map((discussion) => (
        <div key={discussion.id} className="discussion-card">
          <div className="discussion-header">
            <h2 className="discussion-title">{discussion.title}</h2>
            <div className="discussion-tags">
              {discussion.tags.map((tag, index) => (
                <span key={index} className="discussion-tag">
                  {tag}
                </span>
              ))}
            </div>
          </div>
          <p className="discussion-content">
            {discussion.content}
          </p>
        </div>
      ))}
    </div>
  );
};

export default Discussions;
