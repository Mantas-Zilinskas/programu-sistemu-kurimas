import React, { useState, useEffect } from 'react';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ReplyIcon from '@mui/icons-material/Reply';
import { likeComment } from '../../api/commentApi';
import styles from './CommentWidget.module.css';

const CommentWidget = ({ count, DiscussionId, CommentId, handleReply, renderFunction = () => null, initialLiked = false,
}) => {
  const [likes, setLikes] = useState(count);
  const [liked, setLiked] = useState(initialLiked);

  useEffect(() => {
    setLiked(initialLiked);
  }, [initialLiked]);

  const handleLike = async () => {
    try {
      const data = await likeComment(DiscussionId, CommentId);
      setLikes(data.likes);
      setLiked(data.likedByUser);
    } catch (error) {
      alert("You must be logged in to like a comment.");
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
