import React, { useState, useEffect } from 'react';
import FavoriteIcon from '@mui/icons-material/Favorite';
import CommentIcon from '@mui/icons-material/Comment';
import styles from './DiscussionWidgets.module.css'
import Skeleton from '@mui/material/Skeleton';

const DiscussionWidget = ({ count, discussionId, handleReply, initialLiked }) => {
	
    const [likes, setLikes] = useState(count);
    const [liked, setLiked] = useState(initialLiked);

    useEffect(() => {
      setLiked(initialLiked);
    }, [initialLiked]);
  
    const handleLike = async () => {
        const token = JSON.parse(localStorage.getItem("user"))?.token;
      
        if (!token) {
          alert("You must be logged in to like a discussion.");
          return;
        }
      
        try {
          const response = await fetch(`/api/discussions/${discussionId}/like`, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${token}`,
            }
          });
      
          if (!response.ok) {
            throw new Error('Failed to like discussion');
          }
      
          const data = await response.json();
      
          setLikes(data.likes);
          setLiked(data.likedByUser);
      
        } catch (error) {
          console.error('Like error:', error);
        }
      };
      
  
    return (
      <div className={styles.container}>
        <>{likes}</>
        <FavoriteIcon
          onClick={handleLike}
          className={liked ? styles.likeButtonActive : styles.likeButton}
        />
        <CommentIcon onClick={handleReply} className={styles.commentButton} />
      </div>
    );
  };  

export default DiscussionWidget;