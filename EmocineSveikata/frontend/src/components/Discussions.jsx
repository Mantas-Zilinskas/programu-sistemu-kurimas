import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useDiscussions } from '../contexts/DiscussionContext';
import './Discussions.css';

const Discussions = () => {
  const { discussions, loading, error } = useDiscussions();
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
