import React, { useState, useEffect } from 'react';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ReplyIcon from '@mui/icons-material/Reply';
import styles from './CommentWidget.module.css';

const CommentWidget = ({ count, DiscussionId, CommentId, handleReply, renderFunction = () => null, initialLiked = false,
}) => {
  const [likes, setLikes] = useState(count);
  const [liked, setLiked] = useState(initialLiked);

  useEffect(() => {
    setLiked(initialLiked);
  }, [initialLiked]);

  const handleLike = async () => {
    const token = JSON.parse(localStorage.getItem("user"))?.token;

    if (!token) {
      alert("You must be logged in to like a comment.");
      return;
    }

    try {
      const response = await fetch(`/api/discussions/${DiscussionId}/comments/${CommentId}/like`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        }
      });

      if (!response.ok) {
        throw new Error('Failed to like comment');
      }

      const data = await response.json();
      setLikes(data.likes);
      setLiked(data.likedByUser);

    } catch (error) {
      console.error('Like error:', error);
    }
  };

  return (
    <>
      <div className={styles.container}>
        <>{likes}</>
        <FavoriteIcon
          onClick={handleLike}
          className={liked ? styles.likeButtonActive : styles.likeButton}
        />
        <ReplyIcon className={styles.replyButton} onClick={handleReply} />
      </div>
      {renderFunction()}
    </>
  );
};

export default CommentWidget;
