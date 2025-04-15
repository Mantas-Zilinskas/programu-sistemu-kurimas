import React, { useState, useEffect } from 'react';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ReplyIcon from '@mui/icons-material/Reply';
import './CommentWidget.css'

const CommentWidget = ({ count, CommentId }) => {

  const [likes, setLikes] = useState(count);
  const [liked, setLiked] = useState(false);

  // TO DO: associate with account when they get implemented
  const handleLike = () => {
    const likeState = liked;
    setLiked(!likeState);
    setLikes(likeState ? likes - 1 : likes + 1)
  }

  const handleReply = () => {
    console.log('implement me!') // TO DO
  }

  return (
    <>
      <div className='container'>
        <>{likes}</>
        <FavoriteIcon onClick={handleLike} className={(liked) ? 'like-button-active' : 'like-button'} />
        <ReplyIcon className='reply-button' onClick={handleReply} />
      </div>
    </>
  )
}

export default CommentWidget;