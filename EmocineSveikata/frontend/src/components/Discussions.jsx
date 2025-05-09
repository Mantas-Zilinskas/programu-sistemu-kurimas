import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useDiscussions } from '../contexts/DiscussionContext';
import { useNavigate } from 'react-router-dom';
import './Discussions.css';

const Discussions = () => {
  const { discussions, tagsArray, selectedTag, setSelectedTag, isPopular, setIsPopular, loading, error } = useDiscussions();
  const [newDiscussion, setNewDiscussion] = useState({ title: '', content: '' });
  const navigate = useNavigate();

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
        <h1 className="discussions-title">Naujausios diskusijos</h1>
        <Link to="/discussions/new" className="new-discussion-button">
          Nauja diskusija
        </Link>
      </div>

      <div className="discussions-filters">
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
        <label className="popular-discussions-checkbox">
          <input
            type="checkbox"
            checked={isPopular}
            onChange={(e) => setIsPopular(e.target.checked)}
          />
          Populiariausios diskusijos
        </label>
      </div>

      {discussions.map((discussion) => (
        <div key={discussion.id} className="discussion-card" onClick={() => navigate(`/discussions/${discussion.id}`)}>
          <div style={{display: 'flex', justifyContent: 'space-between'}}>
            <span style={{ display: 'flex', alignItems: 'center' }}>
              <img className='author-picture' src={discussion.authorPicture} />
              {discussion.authorName}
            </span>
            <div className="discussion-tags">
              {discussion.tags.map((tag, index) => (
                <span key={index} className="discussion-tag">
                  {tag}
                </span>
              ))}
            </div>
          </div>
          <div className="discussion-header">
            <h2 className="discussion-title">{discussion.title}</h2>
          </div>
          <p className="discussion-content">
            {discussion.content}
          </p>
          <div className="discussion-likes">
            <span>{discussion.likes} Likes</span>
          </div>
        </div>
      ))}
    </div>
  );
};

export default Discussions;
