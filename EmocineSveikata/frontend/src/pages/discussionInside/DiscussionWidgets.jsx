import React, { useState, useEffect } from 'react';
import FavoriteIcon from '@mui/icons-material/Favorite';
import CommentIcon from '@mui/icons-material/Comment';
import './DiscussionWidgets.css'
import Skeleton from '@mui/material/Skeleton';

const DiscussionWidget = ({count, discussionId, handleReply}) => {
  
  const [likes, setLikes] = useState(count);
  const [liked, setLiked] = useState(false);

  // TO DO: associate with account when they get implemented
  const handleLike = () => {
    const likeState = liked;
    setLiked(!likeState);
    setLikes(likeState ? likes - 1 : likes + 1)
  } 

  return (
    <div className='container'>
      <>{likes}</>
      < FavoriteIcon onClick={handleLike} className={(liked) ? 'like-button-active' : 'like-button'} />
      < CommentIcon onClick={handleReply} className='comment-button'/>
    </div>     
  )
}

export default DiscussionWidget;