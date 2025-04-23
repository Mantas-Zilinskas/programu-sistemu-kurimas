import React, { useState, useEffect } from 'react';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ReplyIcon from '@mui/icons-material/Reply';
import styles from'./CommentWidget.module.css'

const CommentWidget = ({ count, CommentId, handleReply, renderFunction = ()=>null}) => {

  const [likes, setLikes] = useState(count);
  const [liked, setLiked] = useState(false);

  // TO DO: associate with account when they get implemented
  const handleLike = () => {
    const likeState = liked;
    setLiked(!likeState);
    setLikes(likeState ? likes - 1 : likes + 1)
  }

  return (
    <>
      <div className={styles.container}>
        <>{likes}</>
        <FavoriteIcon onClick={handleLike} className={(liked) ? styles.likeButtonActive : styles.likeButton} />
        <ReplyIcon className={styles.replyButton} onClick={handleReply} />
      </div>
      {renderFunction()}
    </>
  )
}

export default CommentWidget;