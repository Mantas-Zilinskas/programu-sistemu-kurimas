import React, { useState, useEffect } from 'react';
import FavoriteIcon from '@mui/icons-material/Favorite';
import CommentIcon from '@mui/icons-material/Comment';
import styles from './DiscussionWidgets.module.css'
import { likeDiscussion } from '../../api/discussionApi';
import { useAuth } from '../../contexts/AuthContext';
import Skeleton from '@mui/material/Skeleton';

const DiscussionWidget = ({ count, discussionId, handleReply, initialLiked }) => {
    
    const [likes, setLikes] = useState(count);
    const [liked, setLiked] = useState(initialLiked);
    const { currentUser } = useAuth();

    useEffect(() => {
      setLiked(initialLiked);
    }, [initialLiked]);
  
    const handleLike = async () => {
        if (!currentUser) {
          alert("You must be logged in to like a discussion.");
          return;
        }
        
        try {
          const data = await likeDiscussion(discussionId);
          setLikes(data.likes);
          setLiked(data.likedByUser);
        } catch (error) {
          alert("You must be logged in to like a discussion.");
        }
      };
  
    return (
      <div className={styles.container}>
        <>{likes}</>
        <FavoriteIcon
          onClick={currentUser ? handleLike : undefined}
          className={
            currentUser 
              ? (liked ? styles.likeButtonActive : styles.likeButton)
              : styles.likeButtonDisabled
          }
        />
        {currentUser && (
          <CommentIcon onClick={handleReply} className={styles.commentButton} />
        )}
      </div>
    );
  };  

export default DiscussionWidget;
